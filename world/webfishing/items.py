from BaseClasses import Item, ItemClassification
from typing import NamedTuple


class WebfishingItem(Item):
    game: str = "WEBFISHING"


class WebfishingItemData(NamedTuple):
    code: int
    item_type: ItemClassification = ItemClassification.filler


offset: int = 94000
item_table = {
    "Progressive Rod Power": WebfishingItemData(offset, ItemClassification.progression),
    "Progressive Rod Speed": WebfishingItemData(offset + 1, ItemClassification.progression),
    "Progressive Rod Chance": WebfishingItemData(offset + 2, ItemClassification.progression),
    "Progressive Rod Luck": WebfishingItemData(offset + 3, ItemClassification.progression),
    "Progressive Tacklebox Upgrade": WebfishingItemData(offset + 4, ItemClassification.progression),
    "Progressive Bait": WebfishingItemData(offset + 5, ItemClassification.progression),
    "Fly Hook": WebfishingItemData(offset + 6, ItemClassification.progression),
    "Patient Lure": WebfishingItemData(offset + 7, ItemClassification.useful),
    "Lucky Hook": WebfishingItemData(offset + 8, ItemClassification.useful),
    "Challenge Lure": WebfishingItemData(offset + 9, ItemClassification.useful),
    "Salty Lure": WebfishingItemData(offset + 10, ItemClassification.useful),
    "Fresh Lure": WebfishingItemData(offset + 11, ItemClassification.useful),
    "Quick Jig": WebfishingItemData(offset + 12, ItemClassification.useful),
    "Efficient Lure": WebfishingItemData(offset + 13, ItemClassification.useful),
    "Magnet Lure": WebfishingItemData(offset + 14, ItemClassification.progression),
    "Large Lure": WebfishingItemData(offset + 15, ItemClassification.progression),
    "Attractive Angler": WebfishingItemData(offset + 16, ItemClassification.useful),
    "Sparkling Lure": WebfishingItemData(offset + 17, ItemClassification.progression),
    "Double Hook": WebfishingItemData(offset + 18, ItemClassification.useful),
    "Shower Lure": WebfishingItemData(offset + 19, ItemClassification.progression),
    "Golden Hook": WebfishingItemData(offset + 20, ItemClassification.progression),
    "Progressive Buddy Quality Upgrade": WebfishingItemData(offset + 21, ItemClassification.progression),
    "Progressive Buddy Speed Upgrade": WebfishingItemData(offset + 22, ItemClassification.progression),
    "Treasure Chest": WebfishingItemData(offset + 23, ItemClassification.filler),
    "Catcher's Cola": WebfishingItemData(offset + 24, ItemClassification.filler),
    "Catcher's Cola ULTRA": WebfishingItemData(offset + 25, ItemClassification.filler),
    "Catcher's Cola DELUXE": WebfishingItemData(offset + 26, ItemClassification.filler),
    "Hook's Lite": WebfishingItemData(offset + 27, ItemClassification.filler),
    "Red Carp-ernet Suavignon Wine": WebfishingItemData(offset + 28, ItemClassification.filler),
    "Speed Slurpo": WebfishingItemData(offset + 29, ItemClassification.filler),
    "Speed Slurpo XL": WebfishingItemData(offset + 30, ItemClassification.filler),
    "Water": WebfishingItemData(offset + 31, ItemClassification.filler),
    "Shrink Soda": WebfishingItemData(offset + 32, ItemClassification.filler),
    "Big Beverage": WebfishingItemData(offset + 33, ItemClassification.filler),
    "Bounce Brew": WebfishingItemData(offset + 34, ItemClassification.filler),
    "Super Bounce Brew": WebfishingItemData(offset + 35, ItemClassification.filler),
    "Scratch Off (T1)": WebfishingItemData(offset + 36, ItemClassification.filler),
    "Scratch Off (T2)": WebfishingItemData(offset + 37, ItemClassification.filler),
    "Scratch Off (T3)": WebfishingItemData(offset + 38, ItemClassification.filler),
    "Money ($100)": WebfishingItemData(offset + 39, ItemClassification.filler),
    "Money ($500)": WebfishingItemData(offset + 40, ItemClassification.filler),
    "Money ($1000)": WebfishingItemData(offset + 41, ItemClassification.useful),
    "Money ($5000)": WebfishingItemData(offset + 42, ItemClassification.useful),
    "Progressive Camp Tier Unlock": WebfishingItemData(offset + 43, ItemClassification.progression),
    "Spectral Rib": WebfishingItemData(offset + 44, ItemClassification.progression),
    "Spectral Skull": WebfishingItemData(offset + 45, ItemClassification.progression),
    "Spectral Spine": WebfishingItemData(offset + 46, ItemClassification.progression),
    "Spectral Humerus": WebfishingItemData(offset + 47, ItemClassification.progression),
    "Spectral Femur": WebfishingItemData(offset + 48, ItemClassification.progression),
    "Traveler's Rod": WebfishingItemData(offset + 49, ItemClassification.progression),
    "Collector's Rod": WebfishingItemData(offset + 50, ItemClassification.progression),
    "Shining Collector's Rod": WebfishingItemData(offset + 51, ItemClassification.progression),
    "Glistening Collector's Rod": WebfishingItemData(offset + 52, ItemClassification.progression),
    "Opulent Collector's Rod": WebfishingItemData(offset + 53, ItemClassification.progression),
    "Radiant Collector's Rod": WebfishingItemData(offset + 54, ItemClassification.progression),
    "Alpha Collector's Rod": WebfishingItemData(offset + 55, ItemClassification.progression),
    "Prosperous Rod": WebfishingItemData(offset + 56, ItemClassification.progression),
}

# 73 guaranteed items (and hence min no. locations)
fixed_quantities = {
    "Progressive Rod Power": 8,
    "Progressive Rod Speed": 5,
    "Progressive Rod Chance": 5,
    "Progressive Rod Luck": 5,
    "Progressive Tacklebox Upgrade": 9,
    "Progressive Bait": 6,
    "Progressive Buddy Quality Upgrade": 5,
    "Progressive Buddy Speed Upgrade": 5,
    "Fly Hook": 1,
    "Patient Lure": 1,
    "Lucky Hook": 1,
    "Challenge Lure": 1,
    "Salty Lure": 1,
    "Fresh Lure": 1,
    "Quick Jig": 1,
    "Efficient Lure": 1,
    "Magnet Lure": 1,
    "Large Lure": 1,
    "Attractive Angler": 1,
    "Sparkling Lure": 1,
    "Double Hook": 1,
    "Shower Lure": 1,
    "Golden Hook": 1,
    "Money ($1000)": 5,
    "Money ($5000)": 2,
    "Progressive Camp Tier Unlock": 3,
    "Spectral Rib": 1,
    "Spectral Skull": 1,
    "Spectral Spine": 1,
    "Spectral Humerus": 1,
    "Spectral Femur": 1
}

rods = {
    "Traveler's Rod": 1,
    "Collector's Rod": 1,
    "Shining Collector's Rod": 1,
    "Glistening Collector's Rod": 1,
    "Opulent Collector's Rod": 1,
    "Radiant Collector's Rod": 1,
    "Alpha Collector's Rod": 1,
    "Prosperous Rod": 1,
}

filler_weights = {
    "Treasure Chest": 50,
    "Catcher's Cola": 15,
    "Catcher's Cola ULTRA": 13,
    "Catcher's Cola DELUXE": 10,
    "Hook's Lite": 7,
    "Red Carp-ernet Suavignon Wine": 4,
    "Speed Slurpo": 9,
    "Speed Slurpo XL": 4,
    "Water": 1,
    "Shrink Soda": 6,
    "Big Beverage": 6,
    "Bounce Brew": 4,
    "Super Bounce Brew": 3,
    "Scratch Off (T1)": 15,
    "Scratch Off (T2)": 10,
    "Scratch Off (T3)": 5,
    "Money ($100)": 15,
    "Money ($500)": 5,
}
