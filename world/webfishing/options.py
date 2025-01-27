from dataclasses import dataclass
from Options import Toggle, DefaultOnToggle, DeathLink, Range, Choice, PerGameCommonOptions  # , OptionGroup


class Goal(Choice):
    """
    Total Journal Completion: Reach a specific percentage threshold in total journal completion
    (i.e. all fish, all rarities).

    Rank: Reach a specific rank.

    Completed Camp: Unlock all camp tiers and purchase the final tier.
    """
    display_name = "Goal"
    option_total_completion = 0
    option_rank = 1
    option_camp = 2
    default = 0


class TotalCompletion(Range):
    """The required total journal completion percentage for the Total Completion goal."""
    display_name = "Required Journal Percentage"
    range_start = 1
    range_end = 100
    default = 60


class Rank(Range):
    """The required rank for the Rank goal."""
    display_name = "Required Rank"
    range_start = 2
    range_end = 50
    default = 40


class GameMode(Choice):
    """
    Classic: The classic WEBFISHING experience. Progression is unchanged;
    all fish have their normal catch rates by default.

    Streamlined: Fish are tied to specific rods (which are free from the
    shop), and have equal catch rates in their respective pools. The
    quality of fish is guaranteed depending on which bait you use.
    Based on Eszenn's Archipelago Tweaks mod. See the full loot pool
    here:
    https://github.com/Eszenn/Webfishing-Archipelago-Tweaks/tree/main
    """
    display_name = "Game Mode"
    option_classic = 0
    option_streamlined = 1
    default = 0


class RareFishChecks(Toggle):
    """Whether "rare" fish will have associated checks or not."""
    display_name = "Rare Fish Checks"
    default = 1


class FishChanceEqualizer(Toggle):
    """
    For the Classic game mode. How much the fish catch chances are equalized.
    At 0, all fish have default odds. At 1, they all have equal odds.
    """
    display_name = "Fish Chance Equalizer"
    range_start = 0
    range_end = 1
    default = 0

# class ProgressiveCampTier(Toggle):
#     """Whether camp tiers need to be received or not."""
#     display_name = "Progressive Camp Tiers"
#
#
# class LockUpgradesBehindCamp(Toggle):
#     """Whether all the following receivable upgrades need to have their corresponding camp tier unlocked or not."""
#     display_name = "Lock Receivable Upgrades Behind Camp Tier"
#
#
# class ProgressiveRodUpgrade(DefaultOnToggle):
#     """Whether rod upgrades need to be received or not."""
#     display_name = "Progressive Rod Upgrades"
#
#
# class BaitUpgrades(Choice):
#     """
#     Progressive: Bait upgrades are received in the standard ascending order.
#
#     Individual: Bait upgrades are received separate of each other.
#
#     Standard: Bait upgrades are not used in the Archipelago item pool.
#
#     """
#     display_name = "Bait Upgrades"
#     option_progressive = 0
#     option_individual = 1
#     option_standard = 2
#     default = 1
#
#
# class ProgressiveFishingBuddyUpgrades(DefaultOnToggle):
#     """Whether the fishing buddy upgrades need to be received or not."""
#     display_name = "Progressive Fishing Buddy Upgrades"
#
#
# class RequireProgressivePurchase(Toggle):
#     """Whether receivable progressive upgrades require purchasing after being unlocked or not."""
#     display_name = "Require Progressive Upgrade Purchase"


#  webfishing_option_groups = [
#     OptionGroup("Total Journal Completion Goal Options", [
#         TotalCompletion,
#     ]),
#     OptionGroup("Rank Goal Options", [
#         Rank,
#     ]),
# ]


@dataclass
class WebfishingOptions(PerGameCommonOptions):
    goal: Goal
    total_completion: TotalCompletion
    rank: Rank
    game_mode: GameMode
    rare_fish_checks: RareFishChecks
    fish_chance_equalizer: FishChanceEqualizer
    # progressive_camp_tier = ProgressiveCampTier
    # lock_upgrades_behind_camp = LockUpgradesBehindCamp
    # progressive_rod_upgrade = ProgressiveRodUpgrade
    # bait_upgrades = BaitUpgrades
    # progressive_fishing_buddy_upgrades = ProgressiveFishingBuddyUpgrades
    # require_purchase_progressive = RequireProgressivePurchase
    death_link: DeathLink
