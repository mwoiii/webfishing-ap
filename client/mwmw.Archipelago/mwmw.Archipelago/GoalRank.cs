using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class GoalRank : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/HUD/BadgePopup/badge_popup.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // level = lvl
            var levelWaiter = new MultiTokenWaiter([
                t => t is IdentifierToken{Name:"level"},
                t => t.Type is TokenType.OpAssign,
                t => t is IdentifierToken{Name:"lvl"},
        ], allowPartialMatch: false);

            foreach (var token in tokens) {
                if (levelWaiter.Check(token)) {
                    // var ap = get_node("/root/mwmwArchipelago")
                    // if ap.Config.current_goal == ap.Config.Goal.RANK and lvl >= ap.Config.rank_goal:
                    //     ap.send_victory()
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
                    yield return new IdentifierToken("current_goal");
                    yield return new Token(TokenType.OpEqual);
                    yield return new IdentifierToken("ap");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("Config");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("Goal");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("RANK");
                    yield return new Token(TokenType.OpAnd);
                    yield return new IdentifierToken("lvl");
                    yield return new Token(TokenType.OpGreaterEqual);
                    yield return new IdentifierToken("ap");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("Config");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("rank_goal");
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 2);
                    yield return new IdentifierToken("ap");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("send_victory");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new Token(TokenType.ParenthesisClose);

                } else {
                    yield return token;
                }
            }
        }
    }
}
