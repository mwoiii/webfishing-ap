extends Node

const _GAME_CACHE_LOC = "user://ap_data/gamecache.save"
const _CONN_CACHE_LOC = "user://ap_data/conncache.save"
const _COLOURS_PLAYER = [
	"[color=#49ffdd]",
	"[color=#ff49da]",
	"[color=#ad49ff]",
	"[color=#49ff49]",
	"[color=#ff4949]",
	"[color=#4968ff]",
	"[color=#ffff49]"
]
const _COLOURS_ITEM = [
	"[color=#00eeee]",
	"[color=#6d8be8]",
	"[color=#af99ef]",
	"[color=#ea0101]",
	"[color=#ea0101]",
]

signal status_changed(status)
signal network_message(text, text_coloured)
signal received_item(item)
signal connected(slot_data)
signal connection_lost()
signal _datapackage_received(datapackage)
signal _retrieved_data(data)

var TimeoutTimer
var _socket := WebSocketClient.new()
var _url
var _port
var _slot_name
var _password
var _players
var _status
var _game
var _seed_name
var _inventory_loc
var _inventory


func connect_to_server(url, port, slot_name, password, game):
	_socket.disconnect_from_host()
	_set_status("Attempting connection...")
	
	_url = url
	_port = port
	_slot_name = slot_name
	_password = password
	_game = game
	
	TimeoutTimer.start()
	var websocket_url = "ws://" + url + ":" + port
	if _socket.connect_to_url(websocket_url) != OK:
		out("Connection failed!")
		_set_status("Connection failed!")
		_socket.disconnect_from_host()
		set_process(false)
	else:
		set_process(true)
		_socket.get_peer(1).set_write_mode(WebSocketPeer.WRITE_MODE_TEXT)


func disconnect_from_server():
	_socket.disconnect_from_host()


func out(text):
	print("APClient: " + str(text))


func say(msg):
	_send_packet('[{"cmd": "Say", "text": "' + msg + '"}]')


func send_checks(loc_ids: Array):
	var ids_string = JSON.print(loc_ids)
	_send_packet('[{"cmd": "LocationChecks", "locations": ' + ids_string + '}]')


func save_inventory():
	var file = File.new()
	file.open(_inventory_loc, File.WRITE)
	file.store_string(to_json(_inventory))
	file.close()


func get_connection_cache():
	var file = File.new()
	if not file.file_exists(_CONN_CACHE_LOC):
		return null
	
	file.open(_CONN_CACHE_LOC, File.READ)
	var cache_dict = parse_json(file.get_line())
	file.close()
	
	return cache_dict


func send_victory():
	_send_packet('[{"cmd": "StatusUpdate", "status": 30}]')


func _ready():
	set_process(false)
	
	TimeoutTimer = Timer.new()
	TimeoutTimer.set_wait_time(10.0)
	add_child(TimeoutTimer)
	
	TimeoutTimer.connect("timeout", self, "_on_timeout")
	_socket.connect("data_received", self, "_on_data_received")
	_socket.connect("connection_closed", self, "_on_connection_closed")
	
	_create_data_files()
	
	_set_status("APClient Ready")


func _on_timeout():
	_set_status("Connection timeout!")
	_socket.disconnect_from_host()


func _on_connection_closed(clean):
	if not clean:
		emit_signal("connection_lost")
		_set_status("Lost connection to server!")
		_socket.disconnect_from_host()


func _set_status(text):
	var time_dict = OS.get_time()
	var time = str(time_dict.hour) + ":" +  str(time_dict.minute) + ":" + str(time_dict.second)

	_status = str(text) + " (" + time + ")"
	emit_signal("status_changed", _status)


func _process(_delta):
	_socket.poll()


func _exit_tree():
	disconnect_from_server()
	_set_status("Disconnected from server")


func _on_data_received():
	var packet = _socket.get_peer(1).get_packet()
	var string = packet.get_string_from_utf8()
	var obj = JSON.parse(string).result[0]
	var cmd = obj.cmd
	
	match cmd:
		"RoomInfo":
			TimeoutTimer.stop()
			_seed_name = obj.seed_name
			_inventory_loc = "user://ap_data/seeds/inv_" + _seed_name + ".save"
			_send_packet('[{"cmd": "Connect", "password": "' + _password + '", "game": "' + _game + '", "name": "' + _slot_name + '", "uuid": "' + _slot_name + '", "items_handling": 3, "tags": [], "version": {"build": 1, "class": "Version", "major": 5, "minor": 0}, "slot_data": true}]')
			
		"ConnectionRefused":
			_set_status("Connection was refused!")
			
		"Connected":
			_cache_game_settings(obj.slot_info)
			_cache_connection_settings()
			_send_packet('[{"cmd": "Sync"}]')
			_get_inventory()
			_players = obj.players
			
			var slot_data
			if obj.has("slot_data"):
				slot_data = obj.slot_data
			else:
				slot_data = null
			emit_signal("connected", slot_data)
			
			_set_status("Successfully connected!")
			
			
		"ReceivedItems":
			_validate_received_items(obj)
			
		"DataPackage":
			emit_signal("_datapackage_received", obj)
			
		"PrintJSON":
			var text = ""
			var text_coloured = ""
			for part in obj.data:
				if part.has("type"):
					match part.type:
						"player_id":
							var id = int(part.text) - 1
							text += _players[id].name
							text_coloured += _COLOURS_PLAYER[fmod(id, 7)] + _players[id].name + "[/color]"
						"item_id":
							var name = _get_name_from_id(part.player, part.text, part.type)
							text += name
							text_coloured += _COLOURS_ITEM[fmod(int(part.flags), 3)] + name + "[/color]"
						"location_id":
							var name = _get_name_from_id(part.player, part.text, part.type)
							text += name
							text_coloured += "[color=#00ff7f]" + name + "[/color]"
						_:
							text += part.text
							text_coloured += "[color=#bffffc]" + part.text + "[/color]"
				else:
					text += part.text
					text_coloured += "[color=#bffffc]" + part.text + "[/color]"
			emit_signal("network_message", text, text_coloured)
			
		"Retrieved":
			emit_signal("_retrieved_data", obj)
			
		"InvalidPacket":
			out("Invalid packet sent: " + obj.text)


func _send_packet(packet):
	var bytes = packet.to_utf8()
	_socket.get_peer(1).put_packet(bytes)


func _cache_connection_settings():
	var cache_dict = {
		"url": _url,
		"port": _port,
		"slot_name": _slot_name 
	}
	
	var file = File.new()
	file.open(_CONN_CACHE_LOC, File.WRITE)
	file.store_string(to_json(cache_dict))
	file.close()


func _reverse_dict_str(dict):
	# Ensures that all keys are strings also (as opposed to floats??)
	var reversed_dict = {}
	for key in dict:
		var value = dict[key]
		reversed_dict[str(value)] = key
	
	return reversed_dict


func _cache_game_settings(slot_info):
	var game_to_slot = {}
	for slot in slot_info:
		game_to_slot[slot_info[slot].game] = slot
	var games_string = JSON.print(game_to_slot.keys())
	_send_packet('[{"cmd": "GetDataPackage", "games":' + games_string + '}]')
	var datapackage = yield(self, "_datapackage_received")
	
	var game_data = datapackage.data.games
	var file = File.new()
	
	# Compare checksums
	var should_write_cache = true
	if file.file_exists(_GAME_CACHE_LOC):
		file.open(_GAME_CACHE_LOC, File.READ)
		if file.get_len() > 0:
			var line_count = 0
			should_write_cache = false
			while file.get_position() < file.get_len():
				var cache_dict = JSON.parse(file.get_line())
				line_count += 1
				# v in case of corrupt data 
				if cache_dict.error != OK or cache_dict.result.checksum != game_data[cache_dict.result.game].checksum:
					should_write_cache = true
					break
					
			if line_count != game_to_slot.keys().size():
				should_write_cache = true
				
		file.close()
	
	if not should_write_cache:
		out("No need to write to AP game cache.")
		return
		
	out("Writing to AP game cache...")
	file.open(_GAME_CACHE_LOC, File.WRITE)
	for game in game_data:
		var id_to_item = _reverse_dict_str(game_data[game].item_name_to_id)
		var id_to_loc = _reverse_dict_str(game_data[game].location_name_to_id)
		var cache_dict = {
			"game": game,
			"checksum": game_data[game].checksum,
			"id_to_item": id_to_item,
			"id_to_loc": id_to_loc,
			"slot": game_to_slot[game]
		}
		file.store_string(to_json(cache_dict) + "\n")
	file.close()


func _get_name_from_id(slot, id, type):
	var unknown = "Unknown"
	
	var file = File.new()
	if not file.file_exists(_GAME_CACHE_LOC):
		return "Unknown"
	
	file.open(_GAME_CACHE_LOC, File.READ)
	while file.get_position() < file.get_len():
		var cache_dict = parse_json(file.get_line())
		if cache_dict.slot == str(slot):
			var name
			match type:
				"item_id":
					if cache_dict.id_to_item.has(str(id)):
						name = cache_dict.id_to_item[str(id)]
					else:
						name = unknown
					return name
				"location_id":
					if cache_dict.id_to_loc.has(str(id)):
						name = cache_dict.id_to_loc[str(id)]
					else:
						name = unknown
					return name
				_:
					out("Invalid type lookup!")
					name = unknown
			file.close()
			return name


func _get_inventory():
	# if no inventory to be gotten, set with _validate_received_items
	var file = File.new()
	file.open(_inventory_loc, File.READ)
	var save_data = JSON.parse(file.get_line())
	file.close()
	if save_data.error == OK:
		_inventory = save_data.result
	


func _validate_received_items(data):
	if _inventory:
		out("Got index " + str(data.index) +", had stored " + str(_inventory.index) + ".")
		if data.index == 0:
			# if equal: just set to inventory
			# if greater: assume new save. overwrite and add all items
			# if less: received new items since. overwrite and new items
			var offset
			if _inventory.items.size() > data.items.size():
				offset = 0
			elif _inventory.items.size() < data.items.size():
				offset = _inventory.items.size()
			else:
				offset = data.items.size()
				
			_submit_items(data, true, offset)
			return
		else:
			# if equal: inventory in sync. add all the new items
			# if not equal: inventory out of sync. resync via index = 0 case above
			if _inventory.index == data.index:
				out("Expected inventory index found")
				_submit_items(data, false, 0)
			else:
				out("Unexpected inventory index. Requesting resync")
				_send_packet('[{"cmd": "Sync"}]')
			return

	else:
		# no file or invalid json: assume new save. overwrite and add all items
		out("Inventory save non-existent or invalid")
		_submit_items(data, true)


func _submit_items(new_items, overwrite = false, offset = 0):
	# offset: which index to start emitting signals in new_items
	# necessary as new_items may be the current inventory plus new items
	
	# add items
	for net_item in new_items.items.slice(offset, new_items.items.size()):
		emit_signal("received_item", net_item.item)
		
	if overwrite:
		_inventory = new_items
	else:
		_inventory.items += new_items.items
		
	_inventory.index = _inventory.items.size()


func _create_data_files():
	var file = File.new()
	var dir = Directory.new()
	if not dir.dir_exists("user://ap_data/"):
		dir.make_dir("user://ap_data/")
		file.open(_GAME_CACHE_LOC, File.WRITE)
		file.close()
		file.open(_CONN_CACHE_LOC, File.WRITE)
		file.close()
		file.open(_inventory_loc, File.WRITE)
		file.close()
	if not dir.dir_exists("user://ap_data/seeds/"):
		dir.make_dir("user://ap_data/seeds/")
		file.open(_inventory_loc, File.WRITE)
		file.close()

"""
# storing each check made
# would be for offline play
func _store_checks(ids):
	var file = File.new()
	if file.file_exists(_checks_loc):
		file.open(_checks_loc, File.READ_WRITE)
		var current_ids = JSON.parse(file.get_line())
		current_ids += ids
		current_ids = to_json(current_ids)
		file.seek(0)
		file.store_string(current_ids)
		file.close()


# sending all stored checks (on connection)
# also for offline play
func _send_stored_checks():
	var file = File.new()
	if file.file_exists(_checks_loc):
		file.open(_checks_loc, File.READ)
		var current_ids = file.get_line()
		current_ids = current_ids
		file.close()
		var valid = JSON.parse(current_ids)
		if valid.error == OK:
			_send_packet('[{"cmd": "LocationChecks", "locations": ' + current_ids + '}]')


# retrieving save from server for playing on multiple devices
# webfishing doesn't support cloud saves and this would not support concurrent play on the same slot
# v uploading save data packet (would go after inventory saving) v
# _send_packet('[{"cmd": "Set", "key": "inv_' + _slot_name + '", "want_reply": true, "default": {}, "operations": [{"operation": "update", "value": ' + current_data + '}]}]')
func _get_server_save():
	var key = "inv_" + _slot_name
	_send_packet('[{"cmd": "Get", "keys": ["' + key + '"]}]')
	var data = yield(self, "_retrieved_data")
	
	var file = File.new()
	if data.keys.has(key) and data.keys[key] != null:
		out("Retrieving latest save from server")
		file.open(_inventory_loc, File.WRITE)
		file.store_string(to_json(data.keys[key]))
		file.close()
"""
