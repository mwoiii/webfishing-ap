using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace ArchipelagoTweaks;

public class ItemPricePatch : IScriptMod {
    public bool ShouldRun(string path) => path is "res://Scenes/HUD/Shop/ShopButtons/button_item.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // func _setup():
        var setupWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken{Name: "_setup"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon
        ]);

        foreach (var token in tokens) {
            if (setupWaiter.Check(token)) {
                // var ap = get_node("/root/mwmwArchipelago")
                // if ap.Config.mode == ap.Config.Gamemode.ALT and ap.Config.ALT_FREE_ITEMS.has(item_id):
                //     cost = 0
                //     item_require = []
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
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ALT_FREE_ITEMS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("has");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("item_id");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("cost");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(0));

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("item_require");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);

            } else {
                yield return token;
            }
        }
    }
}