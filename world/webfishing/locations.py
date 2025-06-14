from BaseClasses import Location
import typing

class AdvData(typing.NamedTuple):
    id: typing.Optional[int]
    # region: str


class WebfishingAdvancement(Location):
    game: str = "WEBFISHING"


def populate_advancement_tables():
    counter = 0

    # 3 lots of easy generic catch quests
    no = 3
    for i, name in enumerate(gcq_easy):
        for x in range(no):
            title = f"{name} Quest ({x + 1}/{no})"
            item = AdvData(quest_offset + counter)
            standard_base[title] = item
            assign_rod(name, title, item)
            counter += 1

    # 2 lots of normal generic catch quests
    no = 2
    for i, name in enumerate(gcq_normal):
        for x in range(no):
            title = f"{name} Quest ({x + 1}/{no})"
            item = AdvData(quest_offset + counter)
            standard_base[title] = item
            assign_rod(name, title, item)
            counter += 1

    # 2 lots of hard generic catch quests
    no = 2
    for i, c in enumerate(gcq_hard):
        for x in range(no):
            hard[f"{c} Quest ({x + 1}/{no})"] = AdvData(quest_offset + counter)
            counter += 1

    # 2 lots of (all) normal specific catch quests (>1% chance according to wiki, no extra bait req.)
    no = 2
    for i, name in enumerate(scq_normal):
        for x in range(no):
            title = f"{name} Quest ({x + 1}/{no})"
            item = AdvData(quest_offset + counter)
            scqe[title] = item
            assign_rod(name, title, item)
            counter += 1

    # 1 of hard specific catch quests (so only need to catch 1, no second check for 5)
    for name in scq_hard:
        title = f"{name} Quest (1/1)"
        item = AdvData(quest_offset + counter)
        hard_specific[name] = item
        assign_rod(name, title, item)
        counter += 1

    # --- MISC --- #

    # reset counter
    counter = 0

    # one of each in misc
    for c in base_misc:
        standard_base[c] = AdvData(item_offset + counter)
        counter += 1

    # --- SHOPS --- #

    # another reset
    counter = 0

    # progression shop checks

    # t1 checks
    for i in range(8):
        standard_base[f"T1 Progression Shop Purchase ({i+1})"] = AdvData(shop_offset + counter)
        counter += 1

    # t2 checks
    for i in range(8):
        camp_2[f"T2 Progression Shop Purchase ({i+1})"] = AdvData(shop_offset + counter)
        counter += 1

    # t3 checks
    for i in range(8):
        camp_3[f"T3 Progression Shop Purchase ({i+1})"] = AdvData(shop_offset + counter)
        counter += 1

    # t4 checks
    for i in range(8):
        camp_4[f"T4 Progression Shop Purchase ({i+1})"] = AdvData(shop_offset + counter)
        counter += 1

    # spectral shop checks
    for i in range(4):
        spectral_shop[f"Spectral Shop Purchase ({i+1})"] = AdvData(shop_offset + counter)
        counter += 1

    # configuring harmonised base for gcq
    for key in standard_base:
        if key not in traveler:
            harmonised_base[key] = standard_base[key]

def assign_rod(name, title, item):
    for category, catches in rod_categories.items():
        if name in catches:
            category_mapping[category][title] = item
            break


# --- QUESTS --- #
gcq_easy = [
    "Catch Lake Fish", "Catch Ocean Fish"
]
gcq_normal = [
    "Catch Tiny/Microscopic Fish", "Catch Massive/Gigantic/Colossal Fish", "Catch Treasure Chests",
    "Catch Fish in the Rain"
]
gcq_hard = [
    "Catch High Tier Fish"
]
scq_normal = [
    "Sturgeon", "Catfish", "Koi", "Frog", "Turtle", "Toad", "Leech", "Eel", "Swordfish",
    "Hammerhead Shark", "Octopus", "Seahorse", "CREATURE"
]
scq_hard = [
    "Golden Ray", "Unidentified Fish Object", "Helicoprion", "Leedsichthys", "Golden Bass", "Bull Shark",
    "Manta Ray", "Gar", "Sawfish", "Muskellunge", "Axolotl", "Pupfish", "Mooneye", "Great White Shark", "Whale",
    "Coelacanth", "Alligator", "King Salmon", "Man O' War", "Sea Turtle", "Squid"
]
base_misc = [
    "Spectral Rib", "Spectral Skull", "Spectral Spine", "Spectral Humerus", "Spectral Femur"
]

# -- ROD CATEGORIES -- #
rod_categories = {
    "traveler": ["Catch Lake Fish", "Catch Ocean Fish", "Catch Tiny/Microscopic Fish",
                 "Catch Massive/Gigantic/Colossal Fish", "Catch Fish in the Rain"],
    "collector": ["Frog", "Seahorse", "Swordfish"],
    "shining": ["Catfish", "Koi", "Sturgeon", "Eel", "Octopus"],
    "glistening": ["Leech", "Turtle", "Toad", "Hammerhead Shark", "Sawfish"],
    "opulent": ["Gar", "King Salmon", "Muskellunge", "Man O' War", "Manta Ray", "Sea Turtle", "Squid"],
    "radiant": ["Axolotl", "Pupfish", "Mooneye", "Great White Shark"],
    "alpha": ["Whale", "Coelacanth", "Alligator", "Bull Shark"],
    "prosperous": ["Leedsichthys", "Golden Bass", "Golden Ray"],
    "spectral": ["Unidentified Fish Object", "Helicoprion", "CREATURE"]
}

# quests requiring respective rods
traveler = {}
collector = {}
shining = {}
glistening = {}
opulent = {}
radiant = {}
alpha = {}
prosperous = {}
spectral = {}

category_mapping = {
    "traveler": traveler,
    "collector": collector,
    "shining": shining,
    "glistening": glistening,
    "opulent": opulent,
    "radiant": radiant,
    "alpha": alpha,
    "prosperous": prosperous,
    "spectral": spectral
}

# sphere 1
standard_base = {}
harmonised_base = {}

scqe = {}  # specific catch quest easy - position depends on game mode
hard = {}  # soft requirement for lots of resources
hard_specific = {}  # same as hard, but toggleable
camp_2 = {}  # require 1 camp upgrade
camp_3 = {}  # require 2 camp upgrades
camp_4 = {}  # require all camp upgrades
spectral_shop = {}   # require at least one spectral bone

quest_offset: int = 94200
item_offset: int = 94700
shop_offset: int = 95200

populate_advancement_tables()

all_advancements = standard_base | scqe | hard | hard_specific | camp_2 | camp_3 | camp_4 | spectral_shop
