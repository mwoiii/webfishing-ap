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
	"Shower Lure": null,
	"Collector's Fishing Rod": null,
	"Shining Collector's Fishing Rod": null,
	"Glistening Collector's Fishing Rod": null,
	"Opulent Collector's Fishing Rod": null,
	"Radiant Collector's Fishing Rod": null,
	"Alpha Collector's Fishing Rod": null,
	"Prosperous Fishing Rod": null,
	"Traveler's Fishing Rod": null
}

const SHOP_ID = {
	"t1_1": 95200,
	"t1_2": 95201,
	"t1_3": 95202,
	"t1_4": 95203,
	"t1_5": 95204,
	"t1_6": 95205,
	"t1_7": 95206,
	"t1_8": 95207,
	"t2_9": 95208,
	"t2_10": 95209,
	"t2_11": 95210,
	"t2_12": 95211,
	"t2_13": 95212,
	"t2_14": 95213,
	"t2_15": 95214,
	"t2_16": 95215,
	"t3_17": 95216,
	"t3_18": 95217,
	"t3_19": 95218,
	"t3_20": 95219,
	"t3_21": 95220,
	"t3_22": 95221,
	"t3_23": 95222,
	"t3_24": 95223,
	"t4_25": 95224,
	"t4_26": 95225,
	"t4_27": 95226,
	"t4_28": 95227,
	"t4_29": 95228,
	"t4_30": 95229,
	"t4_31": 95230,
	"t4_32": 95231,
	"s_1": 95232,
	"s_2": 95233,
	"s_3": 95234,
	"s_4": 95235
}


const ALT_FREE_ITEMS = {
	"fishing_rod_simple": null,
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
var auto_hint = null
