from . import WebfishingTestBase


class TestSomething(WebfishingTestBase):

    def test_t3_catches(self) -> None:
        """Test that tier 3 catches are impossible without 2 progressive baits"""
        locations = [
            "Alligator (1)",
            "Alligator (2)",
            "Axolotl (1)",
            "Axolotl (2)",
            "Bull Shark (1)",
            "Gar (1)",
            "Gar (2)",
            "Golden Bass (1)",
            "King Salmon (1)",
            "King Salmon (2)",
            "Mooneye (1)",
            "Mooneye (2)",
            "Pupfish (1)",
            "Pupfish (2)",
            "Coalacanth (1)",
            "Coalacanth (2)",
            "Golden Ray (1)",
            "Great White Shark (1)",
            "Great White Shark (2)",
            "Man O' War (1)",
            "Man O' War (2)",
            "Manta Ray (1)",
            "Manta Ray (2)",
            "Sea Turtle (1)",
            "Sea Turtle (2)",
            "Squid (1)",
            "Squid (2)",
            "Whale (1)",
            "Whale (2)",
            "Leedsichthys (1)",
            "Catch High Tier Fish (1)",
            "Catch High Tier Fish (2)",
            "Catch High Tier Fish (3)",
            "Catch High Tier Fish (4)",
            "Catch High Tier Fish (5)",
        ]
        items = [["Progressive Bait"]]
        self.assertAccessDependency(locations, items)
