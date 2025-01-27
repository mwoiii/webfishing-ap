using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class LockButtons : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/HUD/Shop/ShopButtons/shop_button.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // unlocked.visible = owned
            var unlockedWaiter = new MultiTokenWaiter([
                t => t is IdentifierToken{Name:"unlocked"},
                t => t.Type is TokenType.Period,
                t => t is IdentifierToken{Name:"visible"},
                t => t.Type is TokenType.OpAssign,
                t => t is IdentifierToken{Name:"owned"}
        ], allowPartialMatch: false);

            // var hud
            var hudWaiter = new MultiTokenWaiter([
                t => t.Type is TokenType.PrVar,
                t => t is IdentifierToken{Name:"hud"},
        ], allowPartialMatch: false);

            foreach (var token in tokens) {

                if (unlockedWaiter.Check(token)) {
                    // if get_node("/root/mwmwArchipelago").Config.LOCKED_ITEMS.has(slot_name) and get_node("/root/mwmwArchipelago").Config.current_goal != null and not ap_button:
                    //     locked = true
                    //     warnings += "\nMust be received through Archipelago!"
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
                    yield return new IdentifierToken("Config");
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
                    yield return new IdentifierToken("Config");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("current_goal");
                    yield return new Token(TokenType.OpNotEqual);
                    yield return new ConstantToken(new NilVariant());
                    yield return new Token(TokenType.OpAnd);
                    yield return new Token(TokenType.OpNot);
                    yield return new IdentifierToken("ap_button");
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 2);
                    yield return new IdentifierToken("locked");
                    yield return new Token(TokenType.OpAssign);
                    yield return new ConstantToken(new BoolVariant(true));

                    yield return new Token(TokenType.Newline, 2);
                    yield return new IdentifierToken("warnings");
                    yield return new Token(TokenType.OpAssignAdd);
                    yield return new ConstantToken(new StringVariant("\nMust be received through Archipelago!"));

                } else if (hudWaiter.Check(token)) {
                    // var ap_button = false
                    yield return token;

                    yield return new Token(TokenType.Newline, 0);
                    yield return new Token(TokenType.PrVar);
                    yield return new IdentifierToken("ap_button");
                    yield return new Token(TokenType.OpAssign);
                    yield return new ConstantToken(new BoolVariant(false));

                } else {
                    yield return token;
                }
            }
        }
    }
}
