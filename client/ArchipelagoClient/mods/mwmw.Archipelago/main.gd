extends Node

onready var Config = preload("res://mods/mwmw.Archipelago/config.gd").new()
onready var NetworkManager = preload("res://mods/mwmw.Archipelago/network_manager.gd").new()
onready var FileManager = preload("res://mods/mwmw.Archipelago/file_manager.gd").new()

var Btn = null
var Menu = null
var status_label = null
var shop_hints = null
var unlocked_level = 0


func _ready():
	set_process(false)
	
	add_child(NetworkManager)
	add_child(FileManager)
	
	FileManager.NetworkManager = NetworkManager
	NetworkManager.FileManager = FileManager
	FileManager.create_data_files()
	
	get_tree().connect("node_added", self, "_on_node_added")
	get_tree().connect("node_removed", self, "_on_node_removed")
	UserSave.connect("_slot_saved", self, "_on_slot_saved")
	PlayerData.connect("_save_reset", self, "_on_save_reset")
	NetworkManager.connect("status_changed", self, "_on_status_changed")
	NetworkManager.connect("network_message", self, "_on_network_message")
	NetworkManager.connect("received_item", self, "_on_received_item")
	NetworkManager.connect("connected", self, "_on_connected")
	NetworkManager.connect("connection_lost", self, "_on_connection_lost")
	NetworkManager.connect("received_location_info", self, "_on_received_location_info")
	
	# connect_to_server("127.0.0.1", "38281", "MwoiWebfishing", "")


func say(msg):
	NetworkManager.say(msg)


func connect_to_server(url, port, slot_name, password):
	if NetworkManager.inventory != null:
		FileManager.save_inventory(NetworkManager.inventory)
	NetworkManager.connect_to_server(url, port, slot_name, password, "WEBFISHING")


func send_victory():
	NetworkManager.send_victory()


func send_shop_check(item_id):
	NetworkManager.send_checks([float(Config.SHOP_ID[item_id])])


func has_shop_check(item_id):
	if NetworkManager.checked_locations.has(float(Config.SHOP_ID[item_id])):
		return true
	return false


func send_item_check(item_id):
	var offset = 94700
	
	match item_id:
		"spectral_rib":
			NetworkManager.send_checks([offset])
		"spectral_skull":
			NetworkManager.send_checks([offset + 1])
		"spectral_spine":
			NetworkManager.send_checks([offset + 2])
		"spectral_humerus":
			NetworkManager.send_checks([offset + 3])
		"spectral_femur":
			NetworkManager.send_checks([offset + 4])


func send_quest_check(goal_id, tier, action):	
	# Tier starts at 0, i.e. tier 0 means first quest completion	
	# gcqe: generic catch quest - easy
	# gcqh: generic catch quest - hard (merged with normal ig)
	# scqn: specific catch quest - normal
	# scqh: specific catch quest - hard

	var offset = 94200
	
	var gcqe_count = 3
	var gcqh_count = 2
	var scqn_count = 2
	var scqh_count = 1
	
	# multiplier must be equal to the number of fish in the previous grouping
	# this looks like a negative cook but it's been too long for me to care
	var gcqh_offset = offset + gcqe_count * 2
	var scqn_offset = gcqh_offset + gcqh_count * 5
	var scqh_offset = scqn_offset + scqn_count * 13
	
	
	match action:
		"catch_small", "catch_big", "catch_treasure", "catch_rain", "catch_hightier":
			goal_id = action
	
	match goal_id:
		# GENERIC CATCH QUESTS (EASY)
		"lake":
			if tier < gcqe_count:
				NetworkManager.send_checks([offset + tier])
		"ocean":
			if tier < gcqe_count:
				NetworkManager.send_checks([offset + gcqe_count + tier])
		
		# GENERIC CATCH QUESTS (HARD)
		"catch_small":
			if tier < gcqh_count:
				NetworkManager.send_checks([gcqh_offset+ tier])
		"catch_big":
			if tier < gcqh_count:
				NetworkManager.send_checks([gcqh_offset + gcqh_count + tier])
		"catch_treasure":
			if tier < gcqh_count:
				NetworkManager.send_checks([gcqh_offset + gcqh_count * 2 + tier])
		"catch_rain":
			if tier < gcqh_count:
				NetworkManager.send_checks([gcqh_offset + gcqh_count * 3 + tier])
		"catch_hightier":
			if tier < gcqh_count:
				NetworkManager.send_checks([gcqh_offset + gcqh_count * 4 + tier])
				
		# SPECIFIC CATCH QUESTS (NORMAL)
		"fish_lake_sturgeon":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + tier])
		"fish_lake_catfish":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count + tier])
		"fish_lake_koi":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 2 + tier])
		"fish_lake_frog":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 3 + tier])
		"fish_lake_turtle":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 4 + tier])
		"fish_lake_toad":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 5 + tier])
		"fish_lake_leech":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 6 + tier])
		"fish_ocean_eel":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 7 + tier])
		"fish_ocean_swordfish":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 8 + tier])
		"fish_ocean_hammerhead_shark":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 9 + tier])
		"fish_ocean_octopus":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 10 + tier])
		"fish_ocean_seahorse":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 11 + tier])
		"fish_void_voidfish":
			if tier < scqn_count:
				NetworkManager.send_checks([scqn_offset + scqn_count * 12 + tier])
		
		# SPECIFIC CATCH QUESTS (HARD)
		"fish_ocean_golden_manta_ray":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + tier])
		"fish_alien_dog":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count + tier])
		"fish_rain_heliocoprion":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 2 + tier])
		"fish_rain_leedsichthys":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 3 + tier])
		"fish_lake_golden_bass":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 4 + tier])
		"fish_lake_bullshark":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 5 + tier])
		"fish_ocean_manta_ray":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 6 + tier])
		"fish_lake_gar":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 7 + tier])
		"fish_ocean_sawfish":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 8 + tier])
		"fish_lake_muskellunge":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 9 + tier])
		"fish_lake_axolotl":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 10 + tier])
		"fish_lake_pupfish":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 11 + tier])
		"fish_lake_mooneye":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 12 + tier])
		"fish_ocean_greatwhiteshark":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 13 + tier])
		"fish_ocean_whale":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 14 + tier])
		"fish_ocean_coalacanth":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 15 + tier])
		"fish_lake_alligator":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 16 + tier])
		"fish_lake_kingsalmon":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 17 + tier])
		"fish_ocean_manowar":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 18 + tier])
		"fish_ocean_sea_turtle":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 19 + tier])
		"fish_ocean_squid":
			if tier < scqh_count:
				NetworkManager.send_checks([scqh_offset + scqh_count * 20 + tier])
		_:
			out("Unrecognised fish id: " + goal_id + ". Couldn't send check!")


const inactive = "[color=#d57272]Archipelago is currently inactive![/color]"
func command(cmd):	
	match cmd:
		"/help":
			Network._update_chat(Config.TEXT_COLOUR + 
			"/help: View this command panel.\n" +
			"/completion: Return your current percentage total journal completion.\n" +
			"/goal: Return information relating to your currently configured goal condition.\n" + 
			"/mode: Return your currently configured game mode." + "[/color]", "ap")
		"/completion":
			var percent = _get_journal_completion()
			Network._update_chat(Config.TEXT_COLOUR + 
			"You have completed " + str(percent) + "% of the journal." + "[/color]", "ap")
		"/goal":
			if Config.current_goal == null:
				Network._update_chat(inactive, "ap")
				return
			var goal_info = "unknown."
			match int(Config.current_goal):
				Config.Goal.TOTAL_COMPLETION:
					goal_info = "Total Completion. Your configured completion percentage is " + str(Config.total_completion_goal) + "%."
				Config.Goal.RANK:
					goal_info = "Rank. Your configured completion rank is " + str(Config.rank_goal) + "."
				Config.Goal.COMPLETED_CAMP:
					goal_info = "Completed Camp."
			Network._update_chat(Config.TEXT_COLOUR + 
			"Your current goal is " + goal_info + "[/color]", "ap")
		"/mode":
			if Config.current_goal == null:
				Network._update_chat(inactive, "ap")
				return
			var mode = "unknown."
			match int(Config.mode):
				Config.Gamemode.CLASSIC:
					mode = "Classic."
				Config.Gamemode.ALT:
					mode = "Harmonized."
			Network._update_chat(Config.TEXT_COLOUR + 
			"Your current mode is " + mode + "[/color]", "ap")


func get_connection_cache():
	return FileManager.get_connection_cache()


func get_shop_hint(item_id):
	var net_item = shop_hints[Config.SHOP_ID[item_id] - 95200]
	return {
		"name": FileManager.get_name_from_id(net_item.player, net_item.item, "item_id"),
		"player": NetworkManager.players[net_item.player - 1].name
	}

func get_harmonized_rod_desc(item_id):
	match item_id:
		"fishing_rod_simple":
			return "Catches:\n[color=#cfffd2]Bone, Boot, Branch, Drink Rings, Plastic Bag, Soda Can, Seaweed[/color]"
		"fishing_rod_travelers":
			return "Catches:\n[color=#cfffd2]Salmon, Largemouth Bass, Carp, Rainbow Trout, Bluegill, Perch, Walleye, Goldfish, Atlantic Salmon, Herring, Flounder, Clownfish, Shrimp, Angelfish, Grouper[/color]"
		"fishing_rod_collectors":
			return "Catches:\n[color=#cfffd2]Crayfish, Drum, Guppy, Snail, Frog, Crab, Krill, Oyster, Bluefish, Lobster, Tuna, Seahorse, Sunfish, Swordfish[/color]"
		"fishing_rod_collectors_shining":
			return "Catches:\n[color=#cfffd2]Catfish, Crappie, Pike, Bowfin, Koi, Sturgeon, Marlin, Octopus, Stingray, Eel, Dogfish, Lionfish[/color]"
		"fishing_rod_collectors_radiant":
			return "Catches:\n[color=#cfffd2]Pupfish, Axolotl, Mooneye, Great White Shark[/color]"
		"fishing_rod_collectors_opulent":
			return "Catches:\n[color=#cfffd2]Muskellunge, King Salmon, Gar, Sea Turtle, Squid, Man O' War, Manta Ray[/color]"
		"fishing_rod_collectors_glistening":
			return "Catches:\n[color=#cfffd2]Leech, Turtle, Toad, Sawfish, Wolffish, Hammerhead Shark[/color]"
		"fishing_rod_collectors_alpha":
			return "Catches:\n[color=#cfffd2]Alligator, Bullshark, Whale, Coelacanth[/color]"
		"fishing_rod_skeleton":
			return "Catches:\n[color=#cfffd2]Anomalocaris, Horseshoe Crab, Heliocoprion, UFO, Voidfish[/color] (under their respective conditions)"
		"fishing_rod_prosperous":
			return "Catches:\n[color=#cfffd2]Golden Bass, Diamond, Golden Manta Ray, Leedsichthys[/color]"


func _set_bait_desc_harmonized(harmonized):
	
	if harmonized:
		PlayerData.BAIT_DATA["worms"]["desc"] = "Low Quality Cheap Bait\nCatches all Tiers of fish\nCatches Low Tier fish only"
		PlayerData.BAIT_DATA["cricket"]["desc"] = "Standard Quality Cheap Bait\nCatches all Tiers of fish\nCatches SHINING fish only"
		PlayerData.BAIT_DATA["leech"]["desc"] = "Above Standard Quality Bait\nCatches all Tiers of fish\nCatches GLISTENING fish only"
		PlayerData.BAIT_DATA["minnow"]["desc"] = "High Quality Bait\nCatches all Tiers of fish\nCatches OPULENT fish only"
		PlayerData.BAIT_DATA["squid"]["desc"] = "Very High Quality Bait\nCatches all Tiers of fish\nCatches RADIANT fish only"
		PlayerData.BAIT_DATA["nautilus"]["desc"] = "Pristine Quality Bait\nCatches all Tiers of fish\nCatches ALPHA fish only"
		PlayerData.BAIT_DATA["gildedworm"]["desc"] = "Pristine Quality Bait\nCatches all Tiers of fish\nVanilla odds - high chance of finding ALPHA fish"
	else:
		PlayerData.BAIT_DATA["worms"]["desc"] = "Low Quality Cheap Bait\nCatches Low Tier fish only"
		PlayerData.BAIT_DATA["cricket"]["desc"] = "Standard Quality Cheap Bait\nCatches all Tiers of fish\nLow chance of finding SHINING fish"
		PlayerData.BAIT_DATA["leech"]["desc"] = "Above Standard Quality Bait\nCatches all Tiers of fish\nLow chance of finding GLISTENING fish"
		PlayerData.BAIT_DATA["minnow"]["desc"] = "High Quality Bait\nCatches all Tiers of fish\nLow chance of finding OPULENT fish"
		PlayerData.BAIT_DATA["squid"]["desc"] = "Very High Quality Bait\nCatches all Tiers of fish\nLow chance of finding RADIANT fish"
		PlayerData.BAIT_DATA["nautilus"]["desc"] = "Pristine Quality Bait\nCatches all Tiers of fish\nLow chance of finding ALPHA fish"
		PlayerData.BAIT_DATA["gildedworm"]["desc"] = "Pristine Quality Bait\nCatches all Tiers of fish\nBest chance of finding ALPHA fish"

# adding to the enum only as is necessary
enum Items {
	SPECTRAL_RIB = 94044,
	SPECTRAL_SKULL = 94045,
	SPECTRAL_SPINE = 94046,
	SPECTRAL_HUMERUS = 94047,
	SPECTRAL_FEMUR = 94048,
}

func reobtain_spectral_bones():
	# initiate vars
	var give_rib = false
	var give_skull = false
	var give_spine = false
	var give_humerus = false
	var give_femur = false
	
	# possibility of granting if in ap inventory
	for item in NetworkManager.inventory["items"]:
		match int(item["item"]):
			Items.SPECTRAL_RIB:
				give_rib = true
			Items.SPECTRAL_SKULL:
				give_skull = true
			Items.SPECTRAL_SPINE:
				give_spine = true
			Items.SPECTRAL_HUMERUS:
				give_humerus = true
			Items.SPECTRAL_FEMUR:
				give_femur = true
	
	# certainty of not granting if already in possession
	for item in PlayerData.inventory:
		match item["id"]:
			"spectral_rib":
				give_rib = false
			"spectral_skull":
				give_skull = false
			"spectral_spine":
				give_spine = false
			"spectral_humerus":
				give_humerus = false
			"spectral_femur":
				give_femur = false
		
	if give_rib:
		PlayerData._add_item("spectral_rib", - 1, 10)
	if give_skull:
		PlayerData._add_item("spectral_skull", - 1, 10)
	if give_spine:
		PlayerData._add_item("spectral_spine", - 1, 10)
	if give_humerus:
		PlayerData._add_item("spectral_humerus", - 1, 10)
	if give_femur:
		PlayerData._add_item("spectral_femur", - 1, 10)
		
func out(text):
	print("APMain: " + str(text))

"""
func _generate_general_shop(shop_node):
	var button_script = load("res://mods/mwmw.Archipelago/scenes/ap_shop_button.gd")
	var category = shop_node.get_node("instruments").duplicate()
	category.category_title = "archipelago"
	var button = category.get_child(0).duplicate()
	button.set_script(button_script)
	
	for child in category.get_children():
		child.queue_free()
	
	for i in range(32):
		if i + 1 <= 16:
			var cheap_button = button.duplicate()
			cheap_button.cost = 250
			cheap_button.item_id = "cheap_" + str(i + 1)
			cheap_button.slot_name = "Cheap Archipelago Check"
			cheap_button.slot_desc = "Help someone out!"
			cheap_button.icon = preload("res://icon_bronze.png")
			category.add_child(cheap_button)
		elif i + 1 <= 24:
			var moderate_button = button.duplicate()
			moderate_button.cost = 1000
			moderate_button.item_id = "moderate_" + str(i + 1)
			moderate_button.slot_name = "Moderate Archipelago Check"
			moderate_button.slot_desc = "Help someone out!"
			moderate_button.icon = preload("res://icon_silver.png")
			category.add_child(moderate_button)
		else:
			var expensive_button = button.duplicate()
			expensive_button.cost = 5000
			expensive_button.item_id = "expensive_" + str(i + 1)
			expensive_button.slot_name = "Expensive Archipelago Check"
			expensive_button.slot_desc = "Help someone out!"
			expensive_button.icon = preload("res://icon_gold.png")
			category.add_child(expensive_button)
	
	shop_node.add_child(category)
"""

func _generate_progression_shop(shop_node):
	var button_script = load("res://mods/mwmw.Archipelago/scenes/ap_shop_button.gd")
	var category = shop_node.get_node("misc").duplicate()
	category.category_title = "archipelago"
	var button = category.get_child(0).duplicate()
	button.set_script(button_script)
	
	for child in category.get_children():
		child.queue_free()
	
	for i in range(32):
		var progression_button = button.duplicate()
		if i + 1 <= 8:
			progression_button.cost = 50
			progression_button.icon = preload("res://icon.png")
			progression_button.item_id = "t1_" + str(i + 1)
			progression_button.slot_name = "Archipelago Progression Check"
			progression_button.slot_desc = "Help someone out!"
		elif i + 1 <= 16:
			progression_button.cost = 500
			progression_button.icon = preload("res://icon_bronze.png")
			progression_button.item_id = "t2_" + str(i + 1)
			progression_button.slot_name = "Archipelago Progression Check"
			progression_button.slot_desc = "Help someone out!"
			progression_button.loan_level_require = 1
		elif i + 1 <= 24:
			progression_button.cost = 1000
			progression_button.icon = preload("res://icon_silver.png")
			progression_button.item_id = "t3_" + str(i + 1)
			progression_button.slot_name = "Archipelago Progression Check"
			progression_button.slot_desc = "Help someone out!"
			progression_button.loan_level_require = 2
		elif i + 1 <= 32:
			progression_button.cost = 2500
			progression_button.icon = preload("res://icon_gold.png")
			progression_button.item_id = "t4_" + str(i + 1)
			progression_button.slot_name = "Archipelago Progression Check"
			progression_button.slot_desc = "Help someone out!"
			progression_button.loan_level_require = 3

		category.add_child(progression_button)
	
	shop_node.add_child(category)


func _generate_spectral_shop(shop_node):
	var button_script = load("res://mods/mwmw.Archipelago/scenes/ap_shop_button.gd")
	var category = shop_node.get_node("spectral").duplicate()
	category.category_title = "archipelago"
	var button = category.get_child(0).duplicate()
	button.set_script(button_script)
	
	for child in category.get_children():
		child.queue_free()
	
	for i in range(4):
		var spectral_button = button.duplicate()
		spectral_button.cost = 50
		spectral_button.item_id = "s_" + str(i + 1)
		spectral_button.slot_name = "Archipelago Spectral Check"
		spectral_button.slot_desc = "Help someone out!"
		spectral_button.icon = preload("res://icon_bronze.png")
		category.add_child(spectral_button)
		
	shop_node.add_child(category)


func _generate_alt_loot_tables():
	var lootTable = {
		# Misc
		"lake_trash": ["wtrash_bone", "wtrash_boot", "wtrash_branch", "wtrash_drink_rings", "wtrash_plastic_bag", "wtrash_sodacan", "wtrash_weed"],
		"ocean_trash": ["wtrash_bone", "wtrash_boot", "wtrash_branch", "wtrash_drink_rings", "wtrash_plastic_bag", "wtrash_sodacan", "wtrash_weed"],
		"rain_spectral": ["fish_rain_anomalocaris", "fish_rain_horseshoe_crab", "fish_rain_heliocoprion"],
		"alien_spectral": ["fish_alien_dog"],
		"void_spectral": ["fish_void_voidfish"],

		# Lake fish
		"lake_travelers": ["fish_lake_salmon", "fish_lake_bass", "fish_lake_carp", "fish_lake_rainbowtrout", "fish_lake_bluegill", "fish_lake_perch", "fish_lake_walleye", "fish_lake_goldfish"],
		"lake_collectors": ["fish_lake_crayfish", "fish_lake_drum", "fish_lake_guppy", "fish_lake_snail", "fish_lake_frog", "fish_lake_crab"],
		"lake_shining": ["fish_lake_catfish", "fish_lake_crappie", "fish_lake_pike", "fish_lake_bowfin", "fish_lake_koi", "fish_lake_sturgeon"],
		"lake_glistening": ["fish_lake_leech", "fish_lake_turtle", "fish_lake_toad"],
		"lake_opulent": ["fish_lake_muskellunge", "fish_lake_kingsalmon", "fish_lake_gar"],
		"lake_radiant": ["fish_lake_pupfish", "fish_lake_axolotl", "fish_lake_mooneye"],
		"lake_alpha": ["fish_lake_alligator", "fish_lake_bullshark"],
		"lake_prosperous": ["fish_lake_golden_bass", "wtrash_diamond"],
		"lake_prosperous_rain": ["fish_lake_golden_bass", "wtrash_diamond", "fish_rain_leedsichthys"],

		# Ocean fish
		"ocean_travelers": ["fish_ocean_atlantic_salmon", "fish_ocean_herring", "fish_ocean_flounder", "fish_ocean_clownfish", "fish_ocean_shrimp", "fish_ocean_angelfish", "fish_ocean_grouper"],
		"ocean_collectors": ["fish_ocean_krill", "fish_ocean_oyster", "fish_ocean_bluefish", "fish_ocean_lobster", "fish_ocean_tuna", "fish_ocean_seahorse", "fish_ocean_sunfish", "fish_ocean_swordfish"],
		"ocean_shining": ["fish_ocean_marlin", "fish_ocean_octopus", "fish_ocean_stingray", "fish_ocean_eel", "fish_ocean_dogfish", "fish_ocean_lionfish"],
		"ocean_glistening": ["fish_ocean_sawfish", "fish_ocean_wolffish", "fish_ocean_hammerhead_shark"],
		"ocean_opulent": ["fish_ocean_sea_turtle", "fish_ocean_squid", "fish_ocean_manowar", "fish_ocean_manta_ray"],
		"ocean_radiant": ["fish_ocean_greatwhiteshark"],
		"ocean_alpha": ["fish_ocean_whale", "fish_ocean_coalacanth"],
		"ocean_prosperous": ["fish_ocean_golden_manta_ray", "wtrash_diamond"],
		"ocean_prosperous_rain": ["fish_ocean_golden_manta_ray", "wtrash_diamond", "fish_rain_leedsichthys"],
	};
	
	for table in lootTable.keys():
		var entries = lootTable[table]
	
		var new_table = {}
		new_table["entries"] = {}
		
		var total_weight = 0.0
		for item in Globals.item_data.keys():
			var loot_weight = 1.0
			if entries.has(item):
				total_weight += loot_weight
				new_table["entries"][item] = total_weight
		
		new_table["total"] = total_weight
		
		Globals.loot_tables[table] = new_table


func _set_alt_bait_weights():
	get_node("/root/world/Viewport/main/entities/player").BAIT_DATA = {
		"": {"catch": 0.0, "max_tier": 0, "quality": []},
		"worms": {"catch": 0.06, "max_tier": 2, "quality": [1.0]},
		"cricket": {"catch": 0.06, "max_tier": 2, "quality": [0.0, 1.0]},
		"leech": {"catch": 0.06, "max_tier": 2, "quality": [0.0, 0.0, 1.0]},
		"minnow": {"catch": 0.06, "max_tier": 2, "quality": [0.0, 0.0, 0.0, 1.0]},
		"squid": {"catch": 0.06, "max_tier": 2, "quality": [0.0, 0.0, 0.0, 0.0, 1.0]},
		"nautilus": {"catch": 0.06, "max_tier": 2, "quality": [0.0, 0.0, 0.0, 0.0, 0.0, 1.0]},
		"gildedworm": {"catch": 0.06, "max_tier": 2, "quality": [1.0, 0.99, 0.85, 0.75, 0.55, 0.12]},
	}


func _on_slot_saved():
	FileManager.save_inventory(NetworkManager.inventory)
	FileManager.save_locations(NetworkManager.checked_locations)


func _on_save_reset():
	out("Resetting location data for slot " + str(UserSave.current_loaded_slot))
	FileManager.reset_locations(UserSave.current_loaded_slot)


func _on_node_added(node):
	# on loading into a game
	if node.filename == "res://Scenes/HUD/Esc Menu/esc_menu.tscn":
		
		# adding in the menus
		var btn_resource = preload("res://mods/mwmw.Archipelago/scenes/ap_button.tscn")
		var menu_resource = preload("res://mods/mwmw.Archipelago/scenes/ap_connect_menu.tscn")
		Btn = btn_resource.instance()
		Menu = menu_resource.instance()
		status_label = Menu.get_node("Panel/Panel/VBoxContainer/connection_status")
		node.add_child(Btn)
		node.add_child(Menu)
		out("Added UI")
		
		# and retrieving checked locations if they exist
		FileManager.locations_loc = "user://ap_data/slot" + str(UserSave.current_loaded_slot) + ".save"
		NetworkManager.checked_locations = FileManager.get_locations()
		if not NetworkManager.checked_locations:
			NetworkManager.checked_locations = []
	
	# removed in favour of more progression purchases to reduce sphere 1 checks
	# elif node.filename == "res://Scenes/HUD/Shop/ShopSetups/general_shop.tscn" and Config.current_goal != null:
	# 	_generate_general_shop(node)
	
	elif node.filename == "res://Scenes/HUD/Shop/ShopSetups/progression_shop.tscn" and Config.current_goal != null:
		_generate_progression_shop(node)
	
	elif node.filename == "res://Scenes/HUD/Shop/ShopSetups/spectral_shop.tscn" and Config.current_goal != null:
		_generate_spectral_shop(node)


func _on_node_removed(node):
	# on leaving a game session
	if node.filename == "res://Scenes/HUD/Esc Menu/esc_menu.tscn":
		NetworkManager.disconnect_from_server()
		Config.current_goal = null
		Config.mode = null
		_regenerate_loot_tables()


func _on_status_changed(status):
	if status_label:
		status_label.text = status


func _on_connected(slot_data):
	# initializing ap config data
	PlayerData._send_notification("Connected to Archipelago!")
	Config.current_goal = slot_data.goal
	Config.total_completion_goal = slot_data.total_completion
	Config.rank_goal = slot_data.rank
	Config.mode = slot_data.game_mode
	Config.chance_eq = float(slot_data.fish_chance_equalizer) / 100
	Config.auto_hint = slot_data.auto_hint
	
	# adjusting game state based on config options
	if Config.mode == Config.Gamemode.ALT:
		_generate_alt_loot_tables()
		_set_alt_bait_weights()
		_set_bait_desc_harmonized(true)
	
	if Config.chance_eq > 0:
		_adjust_fish_weights()
	
	NetworkManager.request_location_hints(Config.SHOP_ID.values(), Config.auto_hint)
	unlocked_level = NetworkManager.get_item_count(94043)
	out("unlocked_level: " + unlocked_level)
	
	_sync_state()


func _sync_state():
	# sending all stored locations
	# and checking if victory has been met
	
	NetworkManager.send_checks(NetworkManager.checked_locations)
	
	if (Config.current_goal == Config.Goal.RANK and PlayerData.badge_level >= Config.rank_goal) \
	or (Config.current_goal == Config.Goal.TOTAL_COMPLETION and _get_journal_completion() >= Config.total_completion_goal) \
	or (Config.current_goal == Config.Goal.COMPLETED_CAMP and PlayerData.loan_level >= 3):
		send_victory()


func _get_journal_completion():
	var total_count = 0.0
	var caught_count = 0.0
	for cat in PlayerData.journal_logs.keys():
		for key in PlayerData.journal_logs[cat].keys():
			var catch_data = PlayerData.journal_logs[cat][key]
			total_count += PlayerData.ITEM_QUALITIES.size()
			caught_count += catch_data["quality"].size()
	return (caught_count / total_count) * 100


func _adjust_fish_weights():
	# data structure snippet:
	# {water_trash:{entries:{wtrash_bone:0.25, wtrash_boot:0.75, wtrash_branch:1.75,
	# wtrash_diamond:1.93, wtrash_drink_rings:2.93, wtrash_plastic_bag:3.93, wtrash_sodacan:4.93,
	# wtrash_weed:5.93}, total:5.93}}
	for table in Globals.loot_tables.keys():
		var total = 0.0
		var last_fish_weight = 0.0
		for fish in Globals.loot_tables[table].entries.keys():
			var current_weight = Globals.loot_tables[table].entries[fish] - last_fish_weight
			total = current_weight + (1 - current_weight) * Config.chance_eq + total
			last_fish_weight = Globals.loot_tables[table].entries[fish]
			Globals.loot_tables[table].entries[fish] = total
		Globals.loot_tables[table].total = total


func _regenerate_loot_tables():
	Globals._generate_loot_tables("fish", "lake")
	Globals._generate_loot_tables("fish", "ocean")
	Globals._generate_loot_tables("fish", "deep")
	Globals._generate_loot_tables("fish", "prehistoric")
	Globals._generate_loot_tables("fish", "rain")
	Globals._generate_loot_tables("fish", "alien")
	Globals._generate_loot_tables("fish", "void")
	Globals._generate_loot_tables("fish", "water_trash")
	Globals._generate_loot_tables("bug", "bush_bug")
	Globals._generate_loot_tables("bug", "shoreline_bug")
	Globals._generate_loot_tables("bug", "tree_bug")
	Globals._generate_loot_tables("none", "seashell")
	Globals._generate_loot_tables("none", "trash")
	Globals._generate_loot_tables("fish", "metal")


func _on_network_message(msg_data):
	var text = ""
	var text_coloured = ""
	for part in msg_data:
		if part.has("type"):
			match part.type:
				"player_id":
					var id = int(part.text) - 1
					text += NetworkManager.players[id].name
					text_coloured += Config.PLAYER_COLOURS[fmod(id, 7)] + NetworkManager.players[id].name + "[/color]"
				"item_id":
					var name = FileManager.get_name_from_id(part.player, part.text, part.type)
					text += name
					text_coloured += Config.ITEM_COLOURS[fmod(int(part.flags), 3)] + name + "[/color]"
				"location_id":
					var name = FileManager.get_name_from_id(part.player, part.text, part.type)
					text += name
					text_coloured += Config.LOCATION_COLOUR + name + "[/color]"
				_:
					text += part.text
					text_coloured += Config.LOCATION_COLOUR + part.text + "[/color]"
		else:
			text += part.text
			text_coloured += Config.TEXT_COLOUR + part.text + "[/color]"
	
	out(text)
	Network._update_chat(text_coloured, "ap")


func _on_received_item(item):
	var name = null
	var item_id = null
	var lure_id = null
	var is_bone = false
	
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
		"94043":
			name = "Progressive Camp Tier Unlock"
			unlocked_level += 1
		"94044":
			name = "Spectral Rib"
			item_id = "spectral_rib"
			is_bone = true
		"94045":
			name = "Spectral Skull"
			item_id = "spectral_skull"
			is_bone = true
		"94046":
			name = "Spectral Spine"
			item_id = "spectral_spine"
			is_bone = true
		"94047":
			name = "Spectral Humerus"
			item_id = "spectral_humerus"
			is_bone = true
		"94048":
			name = "Spectral Femur"
			item_id = "spectral_femur"
			is_bone = true
		"94049":
			name = "Traveler's Rod"
			item_id = "fishing_rod_travelers"
		"94050":
			name = "Collector's Rod"
			item_id = "fishing_rod_collectors"
		"94051":
			name = "Shining Collector's Rod"
			item_id = "fishing_rod_collectors_shining"
		"94052":
			name = "Glistening Collector's Rod"
			item_id = "fishing_rod_collectors_glistening"
		"94053":
			name = "Opulent Collector's Rod"
			item_id = "fishing_rod_collectors_opulent"
		"94054":
			name = "Radiant Collector's Rod"
			item_id = "fishing_rod_collectors_radiant"
		"94055":
			name = "Alpha Collector's Rod"
			item_id = "fishing_rod_collectors_alpha"
		"94056":
			name = "Prosperous Collector's Rod"
			item_id = "fishing_rod_prosperous"
	
	if item_id:
		name = Globals.item_data[item_id]["file"].item_name
		PlayerData._add_item(item_id)
		if is_bone and not PlayerData.saved_tags.has("spectral"):
			yield (get_tree().create_timer(5.0), "timeout")
			PlayerData.saved_tags.append("spectral")
			PlayerData._send_notification("you feel an otherworldly force beckon you...")
			Network._update_chat("[color=#008583]" + "you feel an otherwordly force beckon you..." + "[/color]")
		
	elif lure_id:
		name = PlayerData.LURE_DATA[lure_id].name.to_upper()
		PlayerData.lure_unlocked.append(lure_id)
		
	PlayerData.emit_signal("_bait_update")
	PlayerData._send_notification(name + " obtained through Archipelago!")


func _on_connection_lost():
	PlayerData._send_notification("Disconnected from Archipelago!")
	_set_bait_desc_harmonized(false)
	Config.mode = null


func _on_received_location_info(locations):
	shop_hints = locations
