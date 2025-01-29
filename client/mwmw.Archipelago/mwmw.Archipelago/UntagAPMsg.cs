using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class UntagAPMsg : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/Singletons/SteamNetwork.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // GAMECHAT = GAMECHAT + msg
            var messageWaiter = new MultiTokenWaiter([
                t => t is IdentifierToken{Name:"GAMECHAT"},
                t => t.Type is TokenType.OpAssign,
                t => t is IdentifierToken{Name:"GAMECHAT"},
                t => t.Type is TokenType.OpAdd,
                t => t is IdentifierToken{Name:"msg"},
        ], allowPartialMatch: false);

            foreach (var token in tokens) {
                if (messageWaiter.Check(token)) {
                    //elif local == "ap":
                    //	text = "\n" + text
                    //	LOCAL_GAMECHAT_COLLECTIONS.append(text)
                    //	if LOCAL_GAMECHAT_COLLECTIONS.size() > max_chat_length:
                    //    LOCAL_GAMECHAT_COLLECTIONS.remove(0)
                    //	LOCAL_GAMECHAT = ""
                    //	for msg in LOCAL_GAMECHAT_COLLECTIONS:
                    //	  LOCAL_GAMECHAT = LOCAL_GAMECHAT + msg
                    yield return token;

                    yield return new Token(TokenType.Newline, 1);
                    yield return new Token(TokenType.CfElif);
                    yield return new IdentifierToken("local");
                    yield return new Token(TokenType.OpEqual);
                    yield return new ConstantToken(new StringVariant("ap"));
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 2);
                    yield return new IdentifierToken("text");
                    yield return new Token(TokenType.OpAssign);
                    yield return new ConstantToken(new StringVariant("\n"));
                    yield return new Token(TokenType.OpAdd);
                    yield return new IdentifierToken("text");

                    yield return new Token(TokenType.Newline, 2);
                    yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("append");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new IdentifierToken("text");
                    yield return new Token(TokenType.ParenthesisClose);

                    yield return new Token(TokenType.Newline, 2);
                    yield return new Token(TokenType.CfIf);
                    yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("size");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.OpGreater);
                    yield return new IdentifierToken("max_chat_length");
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 3);
                    yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("remove");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new IntVariant(0));
                    yield return new Token(TokenType.ParenthesisClose);

                    yield return new Token(TokenType.Newline, 2);
                    yield return new IdentifierToken("LOCAL_GAMECHAT");
                    yield return new Token(TokenType.OpAssign);
                    yield return new ConstantToken(new StringVariant(""));

                    yield return new Token(TokenType.Newline, 2);
                    yield return new Token(TokenType.CfFor);
                    yield return new IdentifierToken("msg");
                    yield return new Token(TokenType.OpIn);
                    yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 3);
                    yield return new IdentifierToken("LOCAL_GAMECHAT");
                    yield return new Token(TokenType.OpAssign);
                    yield return new IdentifierToken("LOCAL_GAMECHAT");
                    yield return new Token(TokenType.OpAdd);
                    yield return new IdentifierToken("msg");

                } else {
                    yield return token;
                }
            }
        }
    }
}
