extends Node

signal status_changed(status)
signal network_message(msg_data)
signal received_item(item)
signal connected(slot_data)
signal connection_lost()
signal _datapackage_received(datapackage)
signal retrieved_data(data)
signal received_location_info(locations)

onready var TimeoutTimer = Timer.new()
var secure = true
var FileManager = null
var players = null
var inventory = {"items":[]}
var checked_locations = null
var _socket := WebSocketClient.new()
var _url = null
var _port = null
var slot_name = null
var _password = null
var _status = null
var _game = null
var _seed_name = null


func _ready():
	set_process(false)
	_socket.set_buffers(2048, 1024, 2048, 1024)
	TimeoutTimer.set_wait_time(10.0)
	add_child(TimeoutTimer)
	
	TimeoutTimer.connect("timeout", self, "_on_timeout")
	_socket.connect("data_received", self, "_on_data_received")
	_socket.connect("connection_error", self, "_on_connection_error")
	_socket.connect("connection_closed", self, "_on_connection_closed")
	_socket.connect("server_close_request", self, "_on_server_close_request")
	
	_set_status("Client Ready")


func connect_to_server(url, port, slot_name, password, game):
	disconnect_from_server()
	_set_status("Attempting connection...")
	
	_url = url
	_port = port
	self.slot_name = slot_name
	_password = password
	_game = game
	
	# try secure first
	var websocket_url = "wss://" + url + ":" + port
	_connect(websocket_url)


func disconnect_from_server():
	_socket.disconnect_from_host()


func say(msg):
	send_packet('[{"cmd": "Say", "text": "' + msg + '"}]')


func send_checks(loc_ids: Array):
	for id in loc_ids:
		id = float(id)
		if not checked_locations.has(id):
			checked_locations.append(id)

	var ids_string = JSON.print(loc_ids)
	_out("Attempting to send checks: " + ids_string)
	send_packet('[{"cmd": "LocationChecks", "locations": ' + ids_string + '}]')


func request_location_hints(loc_ids):
	var ids_string = JSON.print(loc_ids)
	_out("Attempting to request hints for: " + ids_string)
	send_packet('[{"cmd": "LocationScouts", "locations": ' + ids_string + ', "create_as_hint": 2}]')


func send_victory():
	send_packet('[{"cmd": "StatusUpdate", "status": 30}]')


func send_packet(packet):
	_out("Sending message: " + packet)
	var bytes = packet.to_utf8()
	_socket.get_peer(1).put_packet(bytes)


func get_item_count(id):
	var sum = 0
	for item in inventory.items:
		print(item)
		if int(item.item) == id:
			sum += 1
	return sum


func _connect(websocket_url):
	_out("Attempting to connect to: " + websocket_url)
	TimeoutTimer.start()
	if _socket.connect_to_url(websocket_url) != OK:
		_out("Connection failed!")
		_set_status("Connection failed!")
		disconnect_from_server()
	else:
		set_process(true)
		_socket.get_peer(1).set_write_mode(WebSocketPeer.WRITE_MODE_TEXT)


func _on_timeout():
	_set_status("Connection timeout!")
	_out("Connection timeout!")
	disconnect_from_server()


func _on_connection_error():
	TimeoutTimer.stop()
	# try unsecure
	if secure:
		_out("Connection error! Trying to connect on ws protocol...")
		secure = false
		var websocket_url = "ws://" + _url + ":" + _port
		_connect(websocket_url)
	else:
		secure = true
		_set_status("Connection error! Process aborted.")
		disconnect_from_server()


func _on_connection_closed(clean):
	set_process(false)
	emit_signal("connection_lost")
	if not clean:
		_out("Disconnected from server! (unclean)")
		_set_status("Lost connection to server!")
		
		# attempt to reconnect
		# var websocket_url = "ws://" + _url + ":" + _port
		# _connect(websocket_url)
		#
		# ^ this is what I would do if a regular connection closure didn't result in unclean
		#   for whatever inconceivable reason
	else:
		_out("Disconnected from server! (clean)")
		_set_status("Disconnected from server!")


func _on_server_close_request(code, reason):
	_out("Server requested clean close with reason: " + reason + " (Code " + str(code) + ")")


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
			secure = true
			_seed_name = obj.seed_name
			FileManager.inventory_loc = "user://ap_data/seeds/inv_" + _seed_name + ".save"
			send_packet('[{"cmd": "Connect", "password": "' + _password + '", "game": "' + _game + '", "name": "' + slot_name + '", "uuid": "' + slot_name + '", "items_handling": 3, "tags": [], "version": {"build": 1, "class": "Version", "major": 5, "minor": 0}, "slot_data": true}]')
			
		"ConnectionRefused":
			_set_status("Connection was refused!")
			_out("Connection was refused:" + str(obj.errors))
			
		"Connected":
			FileManager.cache_game_settings(obj.slot_info)
			FileManager.cache_connection_settings(_url, _port, slot_name)
			send_packet('[{"cmd": "Sync"}]')
			inventory = FileManager.get_inventory()
			players = obj.players
			checked_locations = obj.checked_locations
			
			var slot_data
			if obj.has("slot_data"):
				slot_data = obj.slot_data
			else:
				slot_data = null
				
			emit_signal("connected", slot_data)
			_out("Successfully connected")
			_set_status("Successfully connected!")
			
		"ReceivedItems":
			_validate_received_items(obj)
			
		"DataPackage":
			emit_signal("_datapackage_received", obj)
			
		"PrintJSON":
			emit_signal("network_message", obj.data)
			
		"Retrieved":
			emit_signal("retrieved_data", obj)
			
		"LocationInfo":
			emit_signal("received_location_info", obj.locations)
		
		"SetReply":
			_out("Updated server storage: " + str(obj))
			
		"InvalidPacket":
			_out("Invalid packet sent: " + obj.text)


func _validate_received_items(data):
	if inventory:
		_out("Got index " + str(data.index) +", had stored " + str(inventory.index) + ".")
		if data.index == 0:
			# if equal: just set to inventory
			# if greater: assume new save. overwrite and add all items
			# if less: received new items since. overwrite and new items
			var offset
			if inventory.items.size() > data.items.size():
				offset = 0
			elif inventory.items.size() < data.items.size():
				offset = inventory.items.size()
			else:
				offset = data.items.size()
				
			_submit_items(data, true, offset)
			return
		else:
			# if equal: inventory in sync. add all the new items
			# if not equal: inventory _out of sync. resync via index = 0 case above
			if inventory.index == data.index:
				_out("Expected inventory index found")
				_submit_items(data, false, 0)
			else:
				_out("Unexpected inventory index. Requesting resync")
				send_packet('[{"cmd": "Sync"}]')
			return

	else:
		# no file or invalid json: assume new save. overwrite and add all items
		_out("Inventory save non-existent or invalid")
		_submit_items(data, true)


func _submit_items(new_items, overwrite = false, offset = 0):
	# offset: which index to start emitting signals in new_items
	# necessary as new_items may be the current inventory plus new items
	
	# add items
	for net_item in new_items.items.slice(offset, new_items.items.size()):
		emit_signal("received_item", net_item.item)
		
	if overwrite:
		inventory = new_items
	else:
		inventory.items += new_items.items
		
	inventory.index = inventory.items.size()


func _out(text):
	print("APNetwork: " + str(text))


"""
func store_data(key, data):
	send_packet('[{"cmd": "Set", "key": "' + str(key) + '", "want_reply": true, "default": {}, "operations": [{"operation": "replace", "value": ' + str(data) + '}]}]')


func request_data(key):
	_out("Attempting to request data for: " + str(key))
	send_packet('[{"cmd": "Get", "keys": ["' + key + '"]}]')
	

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
			send_packet('[{"cmd": "LocationChecks", "locations": ' + current_ids + '}]')


# retrieving save from server for playing on multiple devices
# webfishing doesn't support cloud saves and this would not support concurrent play on the same slot
# v uploading save data packet (would go after inventory saving) v
# send_packet('[{"cmd": "Set", "key": "inv_' + _slot_name + '", "want_reply": true, "default": {}, "operations": [{"operation": "update", "value": ' + current_data + '}]}]')
func _get_server_save():
	var key = "inv_" + _slot_name
	send_packet('[{"cmd": "Get", "keys": ["' + key + '"]}]')
	var data = yield(self, "_retrieved_data")
	
	var file = File.new()
	if data.keys.has(key) and data.keys[key] != null:
		_out("Retrieving latest save from server")
		file.open(_inventory_loc, File.WRITE)
		file.store_string(to_json(data.keys[key]))
		file.close()
"""
