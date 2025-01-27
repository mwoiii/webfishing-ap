using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace ArchipelagoTweaks;

public class RodLootTablePatch : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {

        // item_data = PlayerData.FALLBACK_ITEM
        var heldItemWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken { Name: "item_data" },
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken { Name: "PlayerData" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "FALLBACK_ITEM" },
            t => t.Type is TokenType.Newline
        ], allowPartialMatch: true);

        // treasure_mult = 2.0
        var fishTypeWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken { Name:"fish_type"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken{Value: StringVariant{Value: "rain"}}
        ]);

        // alt_chance > 0.0
        var zoneWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken{Name: "alt_chance"},
            t => t.Type is TokenType.OpGreater,
            t => t is ConstantToken{Value: RealVariant{Value: 0.0}},
            t => t.Type is TokenType.OpAnd,
            t => t.Type is TokenType.BuiltInFunc,
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.OpLess,
            t => t is IdentifierToken{Name: "zone"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken{Name: "alt_chance"}
        ]);

        foreach (var token in tokens) {
            if (heldItemWaiter.Check(token)) {
                yield return token;

                // print("Equipped item with id: " + item_data["id"])
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextPrint);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Equipped item with id: "));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("item_data");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("id"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                // match item_data["id"]:
                yield return new Token(TokenType.CfMatch);
                yield return new IdentifierToken("item_data");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("id"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_simple": equipped_rod = PlayerData.ROD.SIMPLE
                yield return new ConstantToken(new StringVariant("fishing_rod_simple"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SIMPLE");
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_travelers": equipped_rod = PlayerData.ROD.TRAVELERS
                yield return new ConstantToken(new StringVariant("fishing_rod_travelers"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("TRAVELERS");
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_collectors": equipped_rod = PlayerData.ROD.COLLECTORS
                yield return new ConstantToken(new StringVariant("fishing_rod_collectors"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("COLLECTORS");
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_collectors_shining": equipped_rod = PlayerData.ROD.SHINING
                yield return new ConstantToken(new StringVariant("fishing_rod_collectors_shining"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SHINING");
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_collectors_opulent": equipped_rod = PlayerData.ROD.OPULENT
                yield return new ConstantToken(new StringVariant("fishing_rod_collectors_opulent"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("OPULENT");
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_collectors_glistening": equipped_rod = PlayerData.ROD.GLISTENING
                yield return new ConstantToken(new StringVariant("fishing_rod_collectors_glistening"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("GLISTENING");
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_collectors_radiant": equipped_rod = PlayerData.ROD.RADIANT
                yield return new ConstantToken(new StringVariant("fishing_rod_collectors_radiant"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("RADIANT");
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_collectors_alpha": equipped_rod = PlayerData.ROD.ALPHA
                yield return new ConstantToken(new StringVariant("fishing_rod_collectors_alpha"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ALPHA");
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_prosperous": equipped_rod = PlayerData.ROD.PROSPEROUS
                yield return new ConstantToken(new StringVariant("fishing_rod_prosperous"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("PROSPEROUS");
                yield return new Token(TokenType.Newline, 2);

                // "fishing_rod_skeleton": equipped_rod = PlayerData.ROD.SPECTRAL
                yield return new ConstantToken(new StringVariant("fishing_rod_skeleton"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SPECTRAL");
                yield return new Token(TokenType.Newline, 1);

                // print("Equipped Rod: ", PlayerData.equipped_rod)
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextPrint);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Equipped Rod: "));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

            } else if (fishTypeWaiter.Check(token)) {
                // var ap = get_node("/root/mwmwArchipelago")
                // if ap.Config.mode == ap.Config.Gamemode.ALT:
                //   var zone = fish_type
                //   var table = "trash"
                //   match PlayerData.equipped_rod:
                //     PlayerData.ROD.SIMPLE: table = "trash"
                //     PlayerData.ROD.TRAVELERS: table = "travelers"
                //     PlayerData.ROD.COLLECTORS: table = "collectors"
                //     PlayerData.ROD.SHINING: table = "shining"
                //     PlayerData.ROD.OPULENT: table = "opulent"
                //     PlayerData.ROD.GLISTENING: table = "glistening"
                //     PlayerData.ROD.RADIANT: table = "radiant"
                //     PlayerData.ROD.ALPHA: table = "alpha"
                //     PlayerData.ROD.SPECTRAL: table = "spectral"
                //     PlayerData.ROD.PROSPEROUS: table = "prosperous"
                //   if rod_cast_data == "salty" and not type_lock: zone = "ocean"
                //   if rod_cast_data == "fresh" and not type_lock: zone = "lake"
                //   if table == "spectral" and zone != "alien" and zone != "void":
                //     if in_rain:
                //       zone = "rain"
                //     else:
                //       table = "trash"
                //   if table != "spectral" and zone == "alien":
                //     zone = "lake"
                //     table = "trash"
                //   force_av_size = false
                //   if table == "trash": force_av_size = true
                //   if in_rain and table == "prosperous":
                //     able = str(table, "_", "rain")
                //   fish_type = str(zone, "_", table)
                //   print(fish_type)
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
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("fish_type");

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("trash"));

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfMatch);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SIMPLE");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("trash"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("TRAVELERS");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("travelers"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("COLLECTORS");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("collectors"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SHINING");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("shining"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("OPULENT");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("opulent"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("GLISTENING");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("glistening"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("RADIANT");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("radiant"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ALPHA");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("alpha"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SPECTRAL");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("spectral"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("PROSPEROUS");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("prosperous"));

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("rod_cast_data");
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("salty"));
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("type_lock");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("ocean"));

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("rod_cast_data");
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("fresh"));
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("type_lock");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("lake"));

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("spectral"));
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.OpNotEqual);
                yield return new ConstantToken(new StringVariant("alien"));
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.OpNotEqual);
                yield return new ConstantToken(new StringVariant("void"));
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 3);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("in_rain");
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 4);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("rain"));

                yield return new Token(TokenType.Newline, 3);
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 4);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("trash"));

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpNotEqual);
                yield return new ConstantToken(new StringVariant("spectral"));
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("alien"));
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("lake"));

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("trash"));

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("force_av_size");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("trash"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("force_av_size");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("in_rain");
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("prosperous"));
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 3);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextStr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_rain"));
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("fish_type");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextStr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_"));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("table");
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextPrint);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("fish_type");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

            } else if (zoneWaiter.Check(token)) {
                // ... or zone.alt_chance > 0.0 and get_node("/root/mwmwArchipelago").mode == get_node("/root/mwmwArchipelago").Gamemode.ALT
                yield return token;

                yield return new Token(TokenType.OpOr);
                yield return new IdentifierToken("zone");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("alt_chance");
                yield return new Token(TokenType.OpGreater);
                yield return new ConstantToken(new RealVariant(0.0));
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("mode");
                yield return new Token(TokenType.OpEqual);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Gamemode");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ALT");

            } else {
                yield return token;
            }
        }
    }
}