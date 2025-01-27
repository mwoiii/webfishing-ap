using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace ArchipelagoTweaks;

public class BuddyLootTablePatch : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Props/fish_trap.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // $catch_ring.play()
        var ringWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.Dollar,
            t => t is IdentifierToken{Name: "catch_ring"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken{Name: "play"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
        ]);

        // var roll = Globals._roll_loot_table(fish_type, 2)
        var rollWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrVar,
            t => t is IdentifierToken{Name: "roll"},
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken{Name: "Globals"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken{Name: "_roll_loot_table"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken{Name: "fish_type"},
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken{Value: IntVariant{Value: 2}},
            t => t.Type is TokenType.ParenthesisClose
        ]);

        foreach (var token in tokens) {
            if (ringWaiter.Check(token)) {
                // var table = ""
                // match PlayerData.equipped_rod:
                //   PlayerData.ROD.SIMPLE: table = "trash"
                //   PlayerData.ROD.TRAVELERS: table = "travelers"
                //   PlayerData.ROD.COLLECTORS: table = "collectors"
                //   PlayerData.ROD.SHINING: table = "shining"
                //   PlayerData.ROD.OPULENT: table = "opulent"
                //   PlayerData.ROD.GLISTENING: table = "glistening"
                //   PlayerData.ROD.RADIANT: table = "radiant"
                //   PlayerData.ROD.ALPHA: table = "alpha"
                //   PlayerData.ROD.SPECTRAL: table = "spectral"
                //   PlayerData.ROD.PROSPEROUS: table = "prosperous"
                // var loot_table = str(fish_type, "_", table)\n
                // print(fish_type)
                yield return token;

                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));

                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.CfMatch);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SIMPLE");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("trash"));

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("TRAVELERS");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("travelers"));

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("COLLECTORS");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("collectors"));

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SHINING");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("shining"));
                yield return new Token(TokenType.Newline, 2);

                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("OPULENT");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("opulent"));
                yield return new Token(TokenType.Newline, 2);

                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("GLISTENING");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("glistening"));

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("RADIANT");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("radiant"));

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ALPHA");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("alpha"));

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SPECTRAL");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("spectral"));

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("PROSPEROUS");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("prosperous"));

                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("loot_table");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextStr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("fish_type");
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_"));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextPrint);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("fish_type");
                yield return new Token(TokenType.ParenthesisClose);

            } else if (rollWaiter.Check(token)) {
                // var ap = get_node("/root/mwmwArchipelago")
                // if ap.Config.mode == ap.Config.Gamemode.ALT:
                //   roll = Globals._roll_loot_table(loot_table, 2)
                yield return token;

                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("mode");
                yield return new Token(TokenType.OpEqual);
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Gamemode");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ALT");
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("roll");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("Globals");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_roll_loot_table");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("loot_table");
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(2));
                yield return new Token(TokenType.ParenthesisClose);

            } else {
                yield return token;
            }
        }
    }
}