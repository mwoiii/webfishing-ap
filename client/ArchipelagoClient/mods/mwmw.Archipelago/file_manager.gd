extends Node

const _GAME_CACHE_LOC = "user://ap_data/gamecache.save"
const _CONN_CACHE_LOC = "user://ap_data/conncache.save"

var inventory_loc = null
var locations_loc = null
var NetworkManager = null
var _slot_info = null


func create_data_files():
	var file = File.new()
	var dir = Directory.new()
	if not dir.dir_exists("user://ap_data/"):
		dir.make_dir("user://ap_data/")
		file.open(_GAME_CACHE_LOC, File.WRITE)
		file.close()
		file.open(_CONN_CACHE_LOC, File.WRITE)
		file.close()
		file.open(inventory_loc, File.WRITE)
		file.close()
		file.open(locations_loc, File.WRITE)
		file.close()
	if not dir.dir_exists("user://ap_data/seeds/"):
		dir.make_dir("user://ap_data/seeds/")
		file.open(inventory_loc, File.WRITE)
		file.close()
		file.open(locations_loc, File.WRITE)
		file.close()


func get_inventory():
	# if no inventory to be gotten, set with _validate_received_items
	var file = File.new()
	file.open(inventory_loc, File.READ)
	var save_data = JSON.parse(file.get_line())
	file.close()
	if save_data.error == OK:
		return save_data.result
	else:
		return null


func get_locations():
	var file = File.new()
	file.open(locations_loc, File.READ)
	var save_data = JSON.parse(file.get_line())
	file.close()
	if save_data.error == OK:
		return save_data.result
	else:
		return null


func get_name_from_id(slot, id, type):
	var unknown = "Unknown"
	
	var file = File.new()
	if not file.file_exists(_GAME_CACHE_LOC):
		return "Unknown"
	
	file.open(_GAME_CACHE_LOC, File.READ)
	while file.get_position() < file.get_len():
		var cache_dict = parse_json(file.get_line())
		var game = _slot_info[str(slot)].game
		if cache_dict.game == str(game):
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
					_out("Invalid type lookup!")
					name = unknown
			file.close()
			return name


func save_inventory(inventory):
	var file = File.new()
	file.open(inventory_loc, File.WRITE)
	file.store_string(to_json(inventory))
	file.close()


func save_locations(locations):
	var file = File.new()
	file.open(locations_loc, File.WRITE)
	file.store_string(to_json(locations))
	file.close()


func reset_locations(slot):
	var path = "user://ap_data/slot" + str(slot) + ".save"
	var dir = Directory.new()
	if dir.file_exists(path):
		dir.remove(path)


func get_connection_cache():
	var file = File.new()
	if not file.file_exists(_CONN_CACHE_LOC):
		return null
	
	file.open(_CONN_CACHE_LOC, File.READ)
	var cache_dict = parse_json(file.get_line())
	file.close()
	
	return cache_dict


func cache_connection_settings(url, port, slot_name):
	var cache_dict = {
		"url": url,
		"port": port,
		"slot_name": slot_name 
	}
	
	var file = File.new()
	file.open(_CONN_CACHE_LOC, File.WRITE)
	file.store_string(to_json(cache_dict))
	file.close()


func cache_game_settings(slot_info):
	_slot_info = slot_info
	# slot part of game_to_slot is now redundant
	# as ids are retrieved by game and not id
	# for obvious reasons
	# (people playing same game)
	var game_to_slot = {}
	for slot in slot_info:
		game_to_slot[slot_info[slot].game] = slot
		
	var game_data = {}
	for key in game_to_slot.keys():
		var games_string = JSON.print([key])
		NetworkManager.send_packet('[{"cmd": "GetDataPackage", "games":' + games_string + '}]')
		var datapackage = yield(NetworkManager, "_datapackage_received")
		game_data.merge(datapackage.data.games)
		
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
		_out("No need to write to AP game cache.")
		return
		
	_out("Writing to AP game cache...")
	file.open(_GAME_CACHE_LOC, File.WRITE)
	for game in game_data:
		var id_to_item = _reverse_dict_str(game_data[game].item_name_to_id)
		var id_to_loc = _reverse_dict_str(game_data[game].location_name_to_id)
		var cache_dict = {
			"game": game,
			"checksum": game_data[game].checksum,
			"id_to_item": id_to_item,
			"id_to_loc": id_to_loc,
			# "slot": game_to_slot[game]
		}
		file.store_string(to_json(cache_dict) + "\n")
	file.close()


func _reverse_dict_str(dict):
	# Ensures that all keys are strings also (as opposed to floats??)
	var reversed_dict = {}
	for key in dict:
		var value = dict[key]
		reversed_dict[str(value)] = key
	
	return reversed_dict


func _out(text):
	print("APFiles: " + str(text))
