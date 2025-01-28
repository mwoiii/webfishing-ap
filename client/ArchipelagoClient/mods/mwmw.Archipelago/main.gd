extends Node

const LOCKED_ITEMS = {
	"Crickets License": null,
	"Leeches License": null,
	"Minnows License": null,
	"Squid License": null,
	"Nautiluses License": null,
	"Gilded Worm License": null,
	"Rod Power 1": null,
	"Rod Power 2": null,
	"Rod Power 3": null,
	"Rod Power 4": null,
	"Rod Power 5": null,
	"Rod Power 6": null,
	"Rod Power 7": null,
	"Rod Power 8": null,
	"Rod Speed 1": null,
	"Rod Speed 2": null,
	"Rod Speed 3": null,
	"Rod Speed 4": null,
	"Rod Speed 5": null,
	"Rod Chance 1": null,
	"Rod Chance 2": null,
	"Rod Chance 3": null,
	"Rod Chance 4": null,
	"Rod Chance 5": null,
	"Tacklebox Upgrade 1": null,
	"Tacklebox Upgrade 2": null,
	"Tacklebox Upgrade 3": null,
	"Tacklebox Upgrade 4": null,
	"Tacklebox Upgrade 5": null,
	"Tacklebox Upgrade 6": null,
	"Tacklebox Upgrade 7": null,
	"Tacklebox Upgrade 8": null,
	"Tacklebox Upgrade 9": null,
	"Buddy Quality Upgrade 1": null,
	"Buddy Quality Upgrade 2": null,
	"Buddy Quality Upgrade 3": null,
	"Buddy Quality Upgrade 4": null,
	"Buddy Quality Upgrade 5": null,
	"Buddy Speed Upgrade 1": null,
	"Buddy Speed Upgrade 2": null,
	"Buddy Speed Upgrade 3": null,
	"Buddy Speed Upgrade 4": null,
	"Buddy Speed Upgrade 5": null,
	"Rod Luck 1": null,
	"Rod Luck 2": null,
	"Rod Luck 3": null,
	"Rod Luck 4": null,
	"Rod Luck 5": null,
	"Fly Hook": null,
	"Lucky Hook": null,
	"Patient Lure": null,
	"Quick Jig": null,
	"Salty Lure": null,
	"Fresh Lure": null,
	"Efficient Lure": null,
	"Magnet Lure": null,
	"Large Lure": null,
	"Attractive Angler": null,
	"Sparkling Lure": null,
	"Double Hook": null,
	"Golden Hook": null,
	"Challenge Lure": null,
	"Shower Lure": null
}

enum Goal {
	TOTAL_COMPLETION,
	RANK
}

var APClient
var Btn
var Menu 
var status_label
var active = false
var current_goal = null
var rank_goal
var total_completion_goal


func _ready():
	set_process(false)
	
	APClient = preload("res://mods/mwmw.Archipelago/apclient.gd").new()
	add_child(APClient)
	
	get_tree().connect("node_added", self, "_on_node_added")
	get_tree().connect("node_removed", self, "_on_node_removed")
	UserSave.connect("_slot_saved", self, "_on_slot_saved")
	APClient.connect("status_changed", self, "_on_status_changed")
	APClient.connect("network_message", self, "_on_network_message")
	APClient.connect("received_item", self, "_on_received_item")
	APClient.connect("connected", self, "_on_connected")
	APClient.connect("connection_lost", self, "_on_connection_lost")
	
	# connect_to_server("127.0.0.1", "38281", "MwoiWebfishing", "")


func say(msg):
	APClient.say(msg)


func connect_to_server(url, port, slot_name, password):
	APClient.connect_to_server(url, port, slot_name, password, "WEBFISHING")


func send_victory():
	APClient.send_victory()


func send_check(goal_id, tier, action):
	# Tier starts at 0, i.e. tier 0 means first quest completion	
	# gcq: generic catch quest
	# scqn: specifc catch quest (normal)
	# scqh: specific catch quest (hard)
	#
	# all these quests are sequential so this is easier?
	var offset = 94200
	var gcq_count = 5
	var scqn_offset = gcq_count * 7
	var scqn_count = 2
	var scqh_offset = scqn_offset + scqn_count * 28
	var scqh_count = 1
	
	match action:
		"catch_small", "catch_big", "catch_treasure", "catch_rain", "catch_hightier":
			goal_id = action
	
	match goal_id:
		# GENERIC CATCH QUESTS
		"lake":
			if tier < gcq_count:
				APClient.send_checks([offset + tier])
		"ocean":
			if tier < gcq_count:
				APClient.send_checks([offset + gcq_count + tier])
		"catch_small":
			if tier < gcq_count:
				APClient.send_checks([offset + gcq_count * 2 + tier])
		"catch_big":
			if tier < gcq_count:
				APClient.send_checks([offset + gcq_count * 3 + tier])
		"catch_treasure":
			if tier < gcq_count:
				APClient.send_checks([offset + gcq_count * 4 + tier])
		"catch_rain":
			if tier < gcq_count:
				APClient.send_checks([offset + gcq_count * 5 + tier])
		"catch_hightier":
			if tier < gcq_count:
				APClient.send_checks([offset + gcq_count * 6 + tier])
				
		# SPECIFIC CATCH QUESTS (NORMAL)
		"fish_lake_sturgeon":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + tier])
		"fish_lake_catfish":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count + tier])
		"fish_lake_koi":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 2 + tier])
		"fish_lake_frog":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 3 + tier])
		"fish_lake_turtle":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 4 + tier])
		"fish_lake_toad":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 5 + tier])
		"fish_lake_leech":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 6 + tier])
		"fish_lake_muskellunge":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 7 + tier])
		"fish_lake_axolotl":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 8 + tier])
		"fish_lake_alligator":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 9 + tier])
		"fish_lake_kingsalmon":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 10 + tier])
		"fish_lake_pupfish":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 11 + tier])
		"fish_lake_mooneye":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 12 + tier])
		"fish_ocean_eel":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 13 + tier])
		"fish_ocean_sawfish":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 14 + tier])
		"fish_ocean_swordfish":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 15 + tier])
		"fish_ocean_hammerhead_shark":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 16 + tier])
		"fish_ocean_octopus":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 17 + tier])
		"fish_ocean_seahorse":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 18 + tier])
		"fish_ocean_manta_ray":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 19 + tier])
		"fish_ocean_coalacanth":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 20 + tier])
		"fish_ocean_greatwhiteshark":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 21 + tier])
		"fish_ocean_manowar":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 22 + tier])
		"fish_ocean_sea_turtle":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 23 + tier])
		"fish_ocean_whale":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 24 + tier])
		"fish_ocean_squid":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 25 + tier])
		"fish_void_voidfish":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 26 + tier])
		"fish_lake_gar":
			if tier < scqn_count:
				APClient.send_checks([offset + scqn_offset + scqn_count * 27 + tier])
		
		# SPECIFIC CATCH QUESTS (HARD)
		"fish_ocean_golden_manta_ray":
			if tier < scqh_count:
				APClient.send_checks([offset + scqh_offset + tier])
		"fish_alien_dog":
			if tier < scqh_count:
				APClient.send_checks([offset + scqh_offset + scqh_count + tier])
		"fish_rain_heliocoprion":
			if tier < scqh_count:
				APClient.send_checks([offset + scqh_offset + scqh_count * 2 + tier])
		"fish_rain_leedsichthys":
			if tier < scqh_count:
				APClient.send_checks([offset + scqh_offset + scqh_count * 3 + tier])
		"fish_rain_golden_bass":
			if tier < scqh_count:
				APClient.send_checks([offset + scqh_offset + scqh_count * 4 + tier])
		"fish_rain_bullshark":
			if tier < scqh_count:
				APClient.send_checks([offset + scqh_offset + scqh_count * 5 + tier])
		_:
			APClient.out("Unrecognised fish id: " + goal_id + ". Couldn't send check!")


func _on_slot_saved():
	APClient.save_inventory()


func _on_node_added(node):
	if node.filename == "res://Scenes/HUD/Esc Menu/esc_menu.tscn":
		var btn_resource = preload("res://mods/mwmw.Archipelago/assets/ap_button.tscn")
		var menu_resource = preload("res://mods/mwmw.Archipelago/assets/ap_connect_menu.tscn")
		Btn = btn_resource.instance()
		Menu = menu_resource.instance()
		status_label = Menu.get_node("Panel/Panel/VBoxContainer/connection_status")
		node.add_child(Btn)
		node.add_child(Menu)
		
		APClient.out("Added UI")


func _on_node_removed(node):
	if node.filename == "res://Scenes/HUD/Esc Menu/esc_menu.tscn":
		APClient.disconnect_from_server()
		current_goal = null
		active = false


func _on_status_changed(status):
	if status_label:
		status_label.text = status


func _on_connected(slot_data):
	PlayerData._send_notification("Connected to Archipelago!")
	active = true
	current_goal = slot_data.goal
	total_completion_goal = slot_data.total_completion
	rank_goal = slot_data.rank


func _on_network_message(text, text_coloured):
	APClient.out(text)
	Network._update_chat(text_coloured, true)


func _on_received_item(item):
	var name
	var item_id
	var lure_id
	
	match str(item):
		"94000":
			name = "Rod Power Upgrade"
			PlayerData.rod_power_level += 1
		"94001":
			name = "Rod Speed Upgrade"
			PlayerData.rod_speed_level += 1
		"94002":
			name = "Rod Chance Upgrade"
			PlayerData.rod_chance_level += 1
		"94003":
			name = "Rod Luck Upgrade"
			PlayerData.rod_luck_level += 1
		"94004":
			name = "Max Bait Upgrade"
			PlayerData.max_bait += 5
		"94005":
			var new_bait
			name = "Bait Upgrade"
			
			if not("worms" in PlayerData.bait_unlocked):
				new_bait = "worms"
			elif not("cricket" in PlayerData.bait_unlocked):
				new_bait = "cricket"
			elif not("leech" in PlayerData.bait_unlocked):
				new_bait = "leech"
			elif not("minnow" in PlayerData.bait_unlocked):
				new_bait = "minnow"
			elif not("squid" in PlayerData.bait_unlocked):
				new_bait = "squid"
			elif not("nautilus" in PlayerData.bait_unlocked):
				new_bait = "nautilus"
			elif not("gildedworm" in PlayerData.bait_unlocked):
				new_bait = "gildedworm"
				
			if new_bait:
				PlayerData.bait_unlocked.append(new_bait)	
				PlayerData._refill_bait(new_bait, false)
		"94006":
			lure_id = "fly_hook"
		"94007":
			lure_id = "patient_lure"
		"94008":
			lure_id = "lucky_hook"
		"94009":
			lure_id = "challenge_lure"
		"94010":
			lure_id = "salty_lure"
		"94011":
			lure_id = "fresh_lure"
		"94012":
			lure_id = "quick_jig"
		"94013":
			lure_id = "efficient_lure"
		"94014":
			lure_id = "magnet_lure"
		"94015":
			lure_id = "large_lure"
		"94016":
			lure_id = "attractive_angler"
		"94017":
			lure_id = "sparkling_lure"
		"94018":
			lure_id = "double_hook"
		"94019":
			lure_id = "rain_lure"
		"94020":
			lure_id = "gold_hook"
		"94021":
			name = "Fishing Buddy Quality Upgrade"
			PlayerData.buddy_level += 1
		"94022":
			name = "Fishing Buddy Speed Upgrade"
			PlayerData.buddy_speed += 1
		"94023":
			item_id = "treasure_chest"
		"94024":
			item_id = "potion_catch"
		"94025":
			item_id = "potion_catch_big"
		"94026":
			item_id = "potion_catch_deluxe"
		"94027":
			item_id = "potion_beer"
		"94028":
			item_id = "potion_wine"
		"94029":
			item_id = "potion_speed"
		"94030":
			item_id = "potion_speed_burst"
		"94031":
			item_id = "potion_revert"
		"94032":
			item_id = "potion_small"
		"94033":
			item_id = "potion_grow"
		"94034":
			item_id = "potion_bounce"
		"94035":
			item_id = "potion_bouncebig"
		"94036":
			item_id = "scratch_off"
		"94037":
			item_id = "scratch_off_2"
		"94038":
			item_id = "scratch_off_3"
		"94039":
			name = "$100"
			PlayerData.money += 100
		"94040":
			name = "$500"
			PlayerData.money += 500
		"94041":
			name = "$1000"
			PlayerData.money += 1000
		"94042":
			name = "$5000"
			PlayerData.money += 5000
	
	if item_id:
		name = Globals.item_data[item_id]["file"].item_name
		PlayerData._add_item(item_id)
	elif lure_id:
		name = PlayerData.LURE_DATA[lure_id].name.to_upper()
		PlayerData.lure_unlocked.append(lure_id)
		
	PlayerData.emit_signal("_bait_update")
	PlayerData._send_notification(name + " obtained through Archipelago!")


func _on_connection_lost():
	PlayerData._send_notification("Lost connection to Archipelago! Attempting to reconnect...")

