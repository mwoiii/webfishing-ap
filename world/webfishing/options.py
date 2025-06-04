from dataclasses import dataclass
from Options import Toggle, DefaultOnToggle, DeathLink, Range, Choice, PerGameCommonOptions  # , OptionGroup


class Goal(Choice):
    """
    Total Journal Completion: Reach a specific percentage threshold in total journal completion
    (i.e. all fish, all rarities).

    Rank: Reach a specific rank.

    Camp: Unlock all camp tiers and purchase the final tier.
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

    Harmonized: Fish are tied to specific rods (which are free from the
    shop), and have equal catch rates in their respective pools. The
    quality of fish is guaranteed depending on which bait you use.
    Based on Eszenn's Archipelago Tweaks mod. See the full loot pool
    here:
    https://github.com/Eszenn/Webfishing-Archipelago-Tweaks/tree/main
    """
    display_name = "Game Mode"
    option_classic = 0
    option_harmonized = 1
    default = 1


class RareFishChecks(Toggle):
    """
    Affects Classic mode only. Whether "rare" (tier 3, very low odds) fish will have associated
    checks or not. Makes up about 21 checks - quite a few.

    Be mindful of reducing the total number of checks by too much.
    """
    display_name = "Rare Fish Checks"
    default = 1


# class GCQEasyCount(Range):
#     """
#     A number between 1 and 10 that dictates how many iterations of the 'easy' generic catch
#     quests will yield checks. 'Easy' quests are: catch lake fish, catch ocean fish.
#
#     Be mindful of reducing the total number of checks by too much.
#     """
#     display_name = "GCQ Easy Count"
#     range_start = 1
#     range_end = 10
#     default = 3
#
#
# class GCQHardCount(Range):
#     """
#     A number between 1 and 10 that dictates how many iterations of the 'hard' generic catch
#     quest will yield checks. 'Hard' quests are: catch large fish, catch small fish, catch chests,
#     catch fish in the rain, catch higher tier fish.
#
#     Be mindful of reducing the total number of checks by too much.
#     """
#     display_name = "GCQ Hard Count"
#     range_start = 1
#     range_end = 10
#     default = 2


class FishChanceEqualizer(Range):
    """
    For the Classic game mode. A number between 0 and 100 that dictates how much the fish catch
    chances are equalized. At 0, all fish have default odds. At 100, they all have equal odds.
    """
    display_name = "Fish Chance Equalizer"
    range_start = 0
    range_end = 100
    default = 0


class AutoHint(Toggle):
    """Whether all shop items are auto-hinted on initial connection"""
    display_name = "Auto Hint"
    default = 0


@dataclass
class WebfishingOptions(PerGameCommonOptions):
    goal: Goal
    total_completion: TotalCompletion
    rank: Rank
    game_mode: GameMode
    rare_fish_checks: RareFishChecks
    # gcq_easy_count: GCQEasyCount
    # gcq_hard_count: GCQHardCount
    fish_chance_equalizer: FishChanceEqualizer
    auto_hint: AutoHint
    # death_link: DeathLink
