import settings
from typing import Dict, Any
from .options import WebfishingOptions
from .items import (WebfishingItem, WebfishingItemData, item_table, fixed_quantities, filler_weights)
from .locations import (WebfishingAdvancement, base, medium, hard, hard_specific, camp_2, camp_3, camp_4,
                        all_advancements)
from worlds.AutoWorld import World
from worlds.generic.Rules import set_rule
from BaseClasses import Region, Item
from Utils import visualize_regions


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
    location_name_to_id = {name: data.id for name, data in all_advancements.items()}

    def create_regions(self) -> None:
        menu_region = Region("Menu", self.player, self.multiworld)

        # Base (no items needed)
        base_region = Region("Base", self.player, self.multiworld)
        base_region.locations += [WebfishingAdvancement(self.player, loc_name, loc_data.id, base_region)
                                  for loc_name, loc_data in base.items()]

        # Medium (some progression items needed for time)
        medium_region = Region("Medium", self.player, self.multiworld)
        medium_region.locations += [WebfishingAdvancement(self.player, loc_name, loc_data.id, medium_region)
                                    for loc_name, loc_data in medium.items()]

        # Hard (lots of progression items needed for time)
        hard_region = Region("Hard", self.player, self.multiworld)
        hard_region.locations += [WebfishingAdvancement(self.player, loc_name, loc_data.id, hard_region)
                                  for loc_name, loc_data in hard.items()]

        # HardSpecific (same as hard, but can have all checks toggled off)
        hard_specific_region = Region("Hard Specific", self.player, self.multiworld)
        if bool(self.options.rare_fish_checks.value):
            hard_specific_region.locations += [WebfishingAdvancement(self.player, loc_name, loc_data.id,
                                               hard_specific_region) for loc_name, loc_data in hard_specific.items()]

        # Camp2 (needs one progressive camp upgrade)
        camp_2_region = Region("Camp 2", self.player, self.multiworld)
        camp_2_region.locations += [WebfishingAdvancement(self.player, loc_name, loc_data.id, camp_2_region)
                                    for loc_name, loc_data in camp_2.items()]

        # Camp3 (needs two progressive camp upgrades)
        camp_3_region = Region("Camp 3", self.player, self.multiworld)
        camp_3_region.locations += [WebfishingAdvancement(self.player, loc_name, loc_data.id, camp_3_region)
                                    for loc_name, loc_data in camp_3.items()]

        # Camp4 (needs two progressive camp upgrades)
        camp_4_region = Region("Camp 4", self.player, self.multiworld)
        camp_4_region.locations += [WebfishingAdvancement(self.player, loc_name, loc_data.id, camp_4_region)
                                    for loc_name, loc_data in camp_4.items()]

        self.multiworld.regions += [menu_region, base_region, medium_region, hard_region, hard_specific_region,
                                    camp_2_region, camp_3_region, camp_4_region]

        menu_region.connect(base_region)
        base_region.add_exits({"Medium": "Some Upgrades", "Camp 2": "Progressive Camp 1"})
        medium_region.add_exits({"Hard": "Upgrade Abundance", "Camp 3": "Progressive Camp 2"})
        hard_region.add_exits({"Camp 4": "Progressive Camp 3"})
        hard_region.connect(hard_specific_region)

    def set_rules(self) -> None:
        set_rule(self.multiworld.get_entrance("Some Upgrades", self.player),
                 lambda state: state.has("Progressive Bait", self.player, 3) and
                 state.has("Progressive Rod Power", self.player, 2) and
                 state.has("Progressive Tacklebox Upgrade", self.player, 1) and
                 state.has("Progressive Rod Speed", self.player, 1) and
                 state.has("Progressive Rod Luck", self.player, 1))

        set_rule(self.multiworld.get_entrance("Upgrade Abundance", self.player),
                 lambda state: state.has("Progressive Bait", self.player, 6) and
                 state.has("Progressive Rod Power", self.player, 5) and
                 state.has("Progressive Tacklebox Upgrade", self.player, 2) and
                 state.has("Progressive Rod Speed", self.player, 3) and
                 state.has("Progressive Rod Luck", self.player, 3) and
                 state.has("Magnet Lure", self.player, 1) and
                 state.has("Golden Hook", self.player, 1) and
                 state.has("Shower Lure", self.player, 1) and
                 state.has("Sparkling Lure", self.player, 1) and
                 state.has("Large Lure", self.player, 1) and
                 state.has("Fly Hook", self.player, 1))

        set_rule(self.multiworld.get_entrance("Progressive Camp 1", self.player),
                 lambda state: state.has("Progressive Camp Tier Unlock", self.player, 1))

        set_rule(self.multiworld.get_entrance("Progressive Camp 2", self.player),
                 lambda state: state.has("Progressive Camp Tier Unlock", self.player, 2))

        set_rule(self.multiworld.get_entrance("Progressive Camp 3", self.player),
                 lambda state: state.has("Progressive Camp Tier Unlock", self.player, 3))

        visualize_regions(self.multiworld.get_region("Menu", self.player), "my_world.puml",
                          show_entrance_names=True, show_other_regions=True)

    def create_items(self) -> None:
        for name, quantity in fixed_quantities.items():
            for i in range(quantity):
                item = self.create_item(name)
                self.multiworld.itempool.append(item)
        filler_quantity = len(all_advancements) - sum(fixed_quantities.values())
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
        return self.options.as_dict("goal", "total_completion", "rank", "game_mode", "rare_fish_checks",
                                    "fish_chance_equalizer")
