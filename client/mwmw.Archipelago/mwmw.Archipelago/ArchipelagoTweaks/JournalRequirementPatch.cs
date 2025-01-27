using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace ArchipelagoTweaks;

public class JournalRequirementPatch : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/Shop/ShopButtons/shop_button.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // if tag_require != "" and not PlayerData.saved_tags.has(tag_require)
        var ownedWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken{Name:"tag_require"},
            t => t.Type is TokenType.OpNotEqual,
            t => t is ConstantToken {Value: StringVariant{Value: ""}},
            t => t.Type is TokenType.OpAnd,
            t => t.Type is TokenType.OpNot,
            t => t is IdentifierToken{Name:"PlayerData"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken{Name:"saved_tags"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken{Name:"has"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken{Name:"tag_require"},
            t => t.Type is TokenType.ParenthesisClose,
        ]);

        foreach (var token in tokens) {
            if (ownedWaiter.Check(token)) {
                // ... and get_node("/root/mwmwArchipelago").Config.mode != get_node("/root/mwmwArchipelago").Config.Gamemode.ALT
                yield return token;

                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("mode");
                yield return new Token(TokenType.OpNotEqual);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Config");
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