from BaseClasses import Location
import typing

t3_fish = [
    "Alligator",
    "Axolotl",
    "Bull Shark",
    "Gar",
    "Golden Bass",
    "King Salmon",
    "Mooneye",
    "Pupfish",
    "Coalacanth",
    "Golden Ray",
    "Great White Shark",
    "Man O' War",
    "Manta Ray",
    "Sea Turtle",
    "Squid",
    "Whale",
    "Leedsichthys",
    "Catch High Tier Fish"
]


class AdvData(typing.NamedTuple):
    id: typing.Optional[int]
    # region: str


class WebfishingAdvancement(Location):
    game: str = "WEBFISHING"


def populate_advancement_table():
    counter = 0
    gcq_count = 5

    gcq = [
        "Catch Lake Fish", "Catch Ocean Fish", "Catch Tiny/Microscopic Fish", "Catch Massive/Gigantic/Colossal Fish",
        "Catch Treasure Chests", "Catch Fish in the Rain", "Catch High Tier Fish"
    ]

    scq_normal = [
        "Sturgeon", "Catfish", "Koi", "Frog", "Turtle", "Toad", "Leech", "Muskellunge", "Axolotl",
        "Alligator", "King Salmon", "Pupfish", "Mooneye", "Eel", "Sawfish", "Swordfish",
        "Hammerhead Shark", "Octopus", "Seahorse", "Manta Ray", "Coalacanth", "Great White Shark", "Man O' War",
        "Sea Turtle", "Whale", "Squid", "CREATURE", "Gar"
    ]

    scq_hard = [
        "Golden Ray", "Unidentified Fish Object", "Helicoprion", "Leedsichthys", "Golden Bass", "Bull Shark"
    ]

    # gcq_count lots of generic catch quests
    for i, q in enumerate(gcq):
        for x in range(gcq_count):
            advancement_table[f"{q} ({x + 1})"] = AdvData(offset + counter)
            counter += 1

    # 2 lots of (all) "not too bad" specific catch quests
    for i, q in enumerate(scq_normal):
        for x in range(2):
            advancement_table[f"{q} ({x + 1})"] = AdvData(offset + counter)
            counter += 1

    # 1 of "hard" specific catch quests (so only need to catch 1, no second check for 5)
    for q in scq_hard:
        advancement_table[f"{q} (1)"] = AdvData(offset + counter)
        counter += 1


advancement_table = {}
offset: int = 94200
populate_advancement_table()

