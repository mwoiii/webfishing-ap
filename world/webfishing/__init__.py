import settings
from typing import Dict, Any
from .options import WebfishingOptions  # the options we defined earlier
from .items import (WebfishingItem, WebfishingItemData, item_table, fixed_quantities, filler_weights)  # data used below to add items to the World
from .locations import WebfishingAdvancement, advancement_table, t3_fish  # same as above
from worlds.AutoWorld import World
from BaseClasses import Region, Location, Entrance, Item  # , RegionType, ItemClassification


# class MyGameItem(Item):  # or from Items import MyGameItem
#     game = "My Game"  # name of the game/world this item is from
#
#
# class MyGameLocation(Location):  # or from Locations import MyGameLocation
#     game = "My Game"  # name of the game/world this location is in
#
#
# class MyGameSettings(settings.Group):
#     class RomFile(settings.SNESRomPath):
#         """Insert help text for host.yaml here."""
#
#     rom_file: RomFile = RomFile("MyGame.sfc")


class WebfishingWorld(World):
    """WEBFISHING is a multiplayer chatroom-focused fishing game! Relax and fish (on the web!)"""

    game = "WEBFISHING"
    options_dataclass = WebfishingOptions  # options the player can set
    options: WebfishingOptions  # typing hints for option results
    # settings: typing.ClassVar[MyGameSettings]  # will be automatically assigned from type hint
    topology_present = False  # show path to required location checks in spoiler

    # The following two dicts are required for the generation to know which
    # items exist. They could be generated from json or something else. They can
    # include events, but don't have to since events will be placed manually.
    item_name_to_id = {name: data.code for name, data in item_table.items()}
    location_name_to_id = {name: data.id for name, data in advancement_table.items()}

    def create_regions(self) -> None:
        menu_region = Region("Menu", self.player, self.multiworld)

        tier_12 = Region("Tier12", self.player, self.multiworld)
        tier_12.locations += [WebfishingAdvancement(self.player, loc_name, loc_data.id, tier_12)
                              for loc_name, loc_data in advancement_table.items()
                              if not any(fish in loc_name for fish in t3_fish)]

        tier_3 = Region("Tier3", self.player, self.multiworld)
        tier_3.locations += [WebfishingAdvancement(self.player, loc_name, loc_data.id, tier_3)
                             for loc_name, loc_data in advancement_table.items()
                             if any(fish in loc_name for fish in t3_fish)]

        self.multiworld.regions += [menu_region, tier_3, tier_12]

        menu_region.connect(tier_12)

        tier_12.add_exits({"Tier3": "Progressive Bait (2)"},
                          {"Tier3": lambda state: state.has("Progressive Bait", self.player, 2)})

    def create_items(self) -> None:
        for name, quantity in fixed_quantities.items():
            for i in range(quantity):
                item = self.create_item(name)
                self.multiworld.itempool.append(item)

        filler_quantity = len(advancement_table) - sum(fixed_quantities.values())
        for _ in range(filler_quantity):
            name = self.random.choices(list(filler_weights.keys()), weights=list(filler_weights.values()))[0]
            item = self.create_item(name)
            self.multiworld.itempool.append(item)

        self.multiworld.push_precollected(self.create_item("Water"))

    def create_item(self, name: str) -> Item:
        item_data = item_table[name]
        item = WebfishingItem(name, item_data.item_type, item_data.code, self.player)
        return item

    def fill_slot_data(self) -> Dict[str, Any]:
        return self.options.as_dict("goal", "total_completion", "rank")
