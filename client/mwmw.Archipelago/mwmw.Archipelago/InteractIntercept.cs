using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class InteractIntercept : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // _exit_chat()
            //
            // if not using_chat
            var inputWaiter = new MultiTokenWaiter([
                t => t is IdentifierToken{Name:"_exit_chat"},
                t => t.Type is TokenType.ParenthesisOpen,
                t => t.Type is TokenType.ParenthesisClose,
                t => t.Type is TokenType.Newline,
                t => t.Type is TokenType.Newline,
                t => t.Type is TokenType.CfIf,
        ], allowPartialMatch: false);

            foreach (var token in tokens) {
                if (inputWaiter.Check(token)) {
                    // ... not get_node("/root/mwmwArchipelago").Menu.visible and... 
                    yield return token;

                    yield return new Token(TokenType.OpNot);
                    yield return new IdentifierToken("get_node");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("Menu");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("visible");
                    yield return new Token(TokenType.OpAnd);

                } else {
                    yield return token;
                }
            }
        }
    }
}
