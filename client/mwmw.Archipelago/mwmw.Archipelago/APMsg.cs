using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class APMsg : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // Network._send_message(final, final_color, chat_local)
            var messageWaiter = new MultiTokenWaiter([
                t => t is IdentifierToken{Name:"Network"},
                t => t.Type is TokenType.Period,
                t => t is IdentifierToken{Name:"_send_message"},
                t => t.Type is TokenType.ParenthesisOpen,
                t => t is IdentifierToken{Name:"final"},
                t => t.Type is TokenType.Comma,
                t => t is IdentifierToken{Name:"final_color"},
                t => t.Type is TokenType.Comma,
                t => t is IdentifierToken{Name:"chat_local"},
                t => t.Type is TokenType.ParenthesisClose
        ], allowPartialMatch: false);

            foreach (var token in tokens) {
                if (messageWaiter.Check(token)) {
                    // if chat_local: get_node("/root/mwmwArchipelago").say(final)
                    yield return token;

                    yield return new Token(TokenType.Newline, 1);
                    yield return new Token(TokenType.CfIf);
                    yield return new IdentifierToken("chat_local");
                    yield return new Token(TokenType.Colon);
                    yield return new IdentifierToken("get_node");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("say");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new IdentifierToken("final_text");
                    yield return new Token(TokenType.ParenthesisClose);

                } else {
                    yield return token;
                }
            }
        }
    }
}
