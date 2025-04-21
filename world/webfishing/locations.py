from BaseClasses import Location
import typing

class AdvData(typing.NamedTuple):
    id: typing.Optional[int]
    # region: str


class WebfishingAdvancement(Location):
    game: str = "WEBFISHING"


def populate_advancement_tables():
    counter = 0
    gcq_count = 5

    # --- QUESTS --- #

    gcq_base = [
        "Catch Lake Fish", "Catch Ocean Fish", "Catch Tiny/Microscopic Fish", "Catch Massive/Gigantic/Colossal Fish",
        "Catch Treasure Chests", "Catch Fish in the Rain"
    ]

    gcq_hard = [
        "Catch High Tier Fish"
    ]

    scq_normal = [
        "Sturgeon", "Catfish", "Koi", "Frog", "Turtle", "Toad", "Leech", "Eel", "Swordfish",
        "Hammerhead Shark", "Octopus", "Seahorse",  "CREATURE"
    ]

    scq_hard = [
        "Golden Ray", "Unidentified Fish Object", "Helicoprion", "Leedsichthys", "Golden Bass", "Bull Shark",
        "Manta Ray", "Gar", "Sawfish", "Muskellunge", "Axolotl", "Pupfish", "Mooneye", "Great White Shark", "Whale",
        "Coelacanth", "Alligator", "King Salmon", "Man O' War", "Sea Turtle", "Squid"
    ]

    # -- MISC-- #

    base_misc = [
        "Spectral Rib", "Spectral Skull", "Spectral Spine", "Spectral Humerus", "Spectral Femur"
    ]

    # -- QUESTS -- #

    # gcq_count lots of generic catch quests
    for i, c in enumerate(gcq_base):
        for x in range(gcq_count):
            check = f"{c} Quest ({x + 1}/{gcq_count})"
            if x == 0:
                quests_1_group.append(check)
            else:
                quests_2_group.append(check)
            base[check] = AdvData(quest_offset + counter)
            counter += 1

    # gcq_count lots of generic catch quests
    for i, c in enumerate(gcq_hard):
        for x in range(gcq_count):
            check = f"{c} Quest ({x + 1}/{gcq_count})"
            if x == 0:
                quests_1_group.append(check)
            else:
                quests_2_group.append(check)
            hard[check] = AdvData(quest_offset + counter)
            counter += 1

    # 2 lots of (all) normal specific catch quests (>1% chance according to wiki, no extra bait req.)
    for i, c in enumerate(scq_normal):
        for x in range(2):
            check = f"{c} Quest ({x + 1}/2)"
            if x == 0:
                quests_1_group.append(check)
            else:
                quests_2_group.append(check)
            base[check] = AdvData(quest_offset + counter)
            counter += 1

    # 1 of hard specific catch quests (so only need to catch 1, no second check for 5)
    for c in scq_hard:
        check = f"{c} Quest (1/1)"
        quests_1_group.append(check)
        hard_specific[check] = AdvData(quest_offset + counter)
        counter += 1

    # --- MISC --- #

    # reset counter
    counter = 0

    # one of each in misc
    for c in base_misc:
        base[c] = AdvData(item_offset + counter)
        counter += 1
    spectral_group.extend(base_misc)

    # --- SHOPS --- #

    # another reset
    counter = 0

    # cheap shop checks
    for i in range(16):
        check = f"Cheap Shop Purchase ({i+1})"
        shop_1_group.append(check)
        base[check] = AdvData(shop_offset + counter)
        counter += 1

    # costly shop checks
    for i in range(8):
        check = f"Moderate Shop Purchase ({i+1})"
        shop_2_group.append(check)
        medium[check] = AdvData(shop_offset + counter)
        counter += 1

    # expensive shop checks
    for i in range(8):
        check = f"Expensive Shop Purchase ({i+1})"
        shop_3_group.append(check)
        hard[check] = AdvData(shop_offset + counter)
        counter += 1

    # progression shop checks
    base["Progression Shop Purchase (1)"] = AdvData(shop_offset + counter)
    counter += 1

    camp_2["Progression Shop Purchase (2)"] = AdvData(shop_offset + counter)
    counter += 1

    camp_3["Progression Shop Purchase (3)"] = AdvData(shop_offset + counter)
    counter += 1

    camp_4["Progression Shop Purchase (4)"] = AdvData(shop_offset + counter)
    counter += 1
    camp_shop_group.extend(["Progression Shop Purchase (1)", "Progression Shop Purchase (2)", "Progression Shop Purchase (3)", "Progression Shop Purchase (4)"])


base = {}
medium = {}
hard = {}
hard_specific = {}
camp_2 = {}
camp_3 = {}
camp_4 = {}

# Location groups
quests_1_group = []
quests_2_group = []
shop_1_group = []
shop_2_group = []
shop_3_group = []
camp_shop_group = []
spectral_group = []

quest_offset: int = 94200
item_offset: int = 94700
shop_offset: int = 95200

populate_advancement_tables()

all_advancements = base | medium | hard | hard_specific | camp_2 | camp_3 | camp_4



