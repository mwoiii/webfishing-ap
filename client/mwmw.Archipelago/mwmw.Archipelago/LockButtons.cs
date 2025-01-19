using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class LockButtons : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/HUD/Shop/ShopButtons/shop_button.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // unlocked.visible = owned
            var assignWaiter = new MultiTokenWaiter([
                t => t is IdentifierToken{Name:"unlocked"},
                t => t.Type is TokenType.Period,
                t => t is IdentifierToken{Name:"visible"},
                t => t.Type is TokenType.OpAssign,
                t => t is IdentifierToken{Name:"owned"}
        ], allowPartialMatch: false);


            foreach (var token in tokens) {
                // if get_node("/root/mwmwArchipelago").LOCKED_ITEMS.has(slot_name) and get_node("/root/mwmwArchipelago").active:
                //     locked = true
                //     warnings += "\nMust be received through Archipelago!"
                if (assignWaiter.Check(token)) {
                    yield return token;

                    /*
                    // checking for names
                    yield return new Token(TokenType.Newline, 1);
                    yield return new Token(TokenType.BuiltInFunc, 64);
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new IdentifierToken("slot_name");
                    yield return new Token(TokenType.ParenthesisClose);
                    */

                    yield return new Token(TokenType.Newline, 1);
                    yield return new Token(TokenType.CfIf);
                    yield return new IdentifierToken("get_node");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("LOCKED_ITEMS");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("has");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new IdentifierToken("slot_name");
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.OpAnd);
                    yield return new IdentifierToken("get_node");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("active");
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 2);
                    yield return new IdentifierToken("locked");
                    yield return new Token(TokenType.OpAssign);
                    yield return new ConstantToken(new BoolVariant(true));

                    yield return new Token(TokenType.Newline, 2);
                    yield return new IdentifierToken("warnings");
                    yield return new Token(TokenType.OpAssignAdd);
                    yield return new ConstantToken(new StringVariant("\nMust be received through Archipelago!"));
                } else {
                    yield return token;
                }
            }
        }
    }
}
