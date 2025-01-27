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

const SHOP_ID = {
	"cheap_1": 95200,
	"cheap_2": 95201,
	"cheap_3": 95202,
	"cheap_4": 95203,
	"cheap_5": 95204,
	"cheap_6": 95205,
	"cheap_7": 95206,
	"cheap_8": 95207,
	"cheap_9": 95208,
	"cheap_10": 95209,
	"cheap_11": 95210,
	"cheap_12": 95211,
	"cheap_13": 95212,
	"cheap_14": 95213,
	"cheap_15": 95214,
	"cheap_16": 95215,
	"moderate_17": 95216,
	"moderate_18": 95217,
	"moderate_19": 95218,
	"moderate_20": 95219,
	"moderate_21": 95220,
	"moderate_22": 95221,
	"moderate_23": 95222,
	"moderate_24": 95223,
	"expensive_25": 95224,
	"expensive_26": 95225,
	"expensive_27": 95226,
	"expensive_28": 95227,
	"expensive_29": 95228,
	"expensive_30": 95229,
	"expensive_31": 95230,
	"expensive_32": 95231,
	"progression_1": 95232,
	"progression_2": 95233,
	"progression_3": 95234,
	"progression_4": 95235
}


const ALT_FREE_ITEMS = {
	"fishing_rod_collectors": null,
	"fishing_rod_collectors_alpha": null,
	"fishing_rod_collectors_glistening": null,
	"fishing_rod_collectors_opulent": null,
	"fishing_rod_collectors_radiant": null,
	"fishing_rod_collectors_shining": null,
	"fishing_rod_prosperous": null,
	"fishing_rod_simple": null,
	"fishing_rod_skeleton": null,
	"fishing_rod_travelers": null,
}

const PLAYER_COLOURS = [
	"[color=#a2ffee]",
	"[color=#ffbff2]",
	"[color=#e7caff]",
	"[color=#95ff95]",
	"[color=#ffc8c8]",
	"[color=#cdd1ff]",
	"[color=#ffff98]"
]

const ITEM_COLOURS = [
	"[color=#acfaf9]",  # common/junk
	"[color=#f5d7ff]",  # important/progression
	"[color=#d4deff]",  # rare/useful
	"[color=#d57272]",  # filler (unused by ap)
	"[color=#d57272]",  # trap
]

const TEXT_COLOUR = "[color=#effffe]"

const LOCATION_COLOUR = "[color=#a8ffd3]"

enum Goal {
	TOTAL_COMPLETION,
	RANK,
	COMPLETED_CAMP
}

enum Gamemode {
	CLASSIC,
	ALT
}

# current goal (and also mode) indicate that the ap is active if they are not null
var mode = null
var current_goal = null
var rank_goal = null
var total_completion_goal = null
var chance_eq = null

