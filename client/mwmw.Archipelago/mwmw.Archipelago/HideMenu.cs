using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class HideMenu : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/HUD/Esc Menu/esc_menu.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // OptionsMenu._close()
            var closeWaiter = new MultiTokenWaiter([
                t => t is IdentifierToken{Name:"OptionsMenu"},
                t => t.Type is TokenType.Period,
                t => t is IdentifierToken{Name:"_close"},
                t => t.Type is TokenType.ParenthesisOpen,
                t => t.Type is TokenType.ParenthesisClose
        ], allowPartialMatch: false);

            foreach (var token in tokens) {
                if (closeWaiter.Check(token)) {
                    // get_node("/root/mwmwArchipelago").Menu.visible = false
                    yield return token;

                    yield return new Token(TokenType.Newline, 1);
                    yield return new IdentifierToken("get_node");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("Menu");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("visible");
                    yield return new Token(TokenType.OpAssign);
                    yield return new ConstantToken(new BoolVariant(false));
                } else {
                    yield return token;
                }
            }
        }
    }
}
