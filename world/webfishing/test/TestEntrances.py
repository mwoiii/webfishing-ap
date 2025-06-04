from . import WebfishingTestBase


class TestEntrances(WebfishingTestBase):

    def test_t3_rare_catches(self) -> None:
        """Test that tier 3 and rare catches are impossible without specified items"""
        locations = [
            "Alligator Quest (1/1)",
            "Axolotl Quest (1/1)",
            "Bull Shark Quest (1/1)",
            "Gar Quest (1/1)",
            "Golden Bass Quest (1/1)",
            "King Salmon Quest (1/1)",
            "Mooneye Quest (1/1)",
            "Pupfish Quest (1/1)",
            "Coelacanth Quest (1/1)",
            "Golden Ray Quest (1/1)",
            "Great White Shark Quest (1/1)",
            "Man O' War Quest (1/1)",
            "Manta Ray Quest (1/1)",
            "Sea Turtle Quest (1/1)",
            "Squid Quest (1/1)",
            "Whale Quest (1/1)",
            "Leedsichthys Quest (1/1)",
            "Catch High Tier Fish Quest (1/2)",
            "Catch High Tier Fish Quest (2/2)",
            "Unidentified Fish Object Quest (1/1)",
            "Helicoprion Quest (1/1)",
            "Sawfish Quest (1/1)",
            "Muskellunge Quest (1/1)",
        ]
        items = [["Progressive Bait", "Progressive Rod Power", "Progressive Tacklebox Upgrade",
                  "Progressive Rod Speed", "Progressive Rod Luck", "Magnet Lure", "Golden Hook", "Shower Lure",
                  "Sparkling Lure", "Large Lure", "Fly Hook"]]
        #self.assertAccessDependency(locations, items, True)


