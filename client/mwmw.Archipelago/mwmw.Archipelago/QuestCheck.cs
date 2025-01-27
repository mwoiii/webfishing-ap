using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class QuestCheck : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/Singletons/playerdata.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // money += data["gold_reward"]
            var rewardWaiter = new MultiTokenWaiter([
                t => t is IdentifierToken{Name:"money"},
                t => t.Type is TokenType.OpAssignAdd,
                t => t is IdentifierToken{Name:"data"},
                t => t.Type is TokenType.BracketOpen,
                t => t is ConstantToken{Value: StringVariant{Value: "gold_reward"}},
                t => t.Type is TokenType.BracketClose
        ], allowPartialMatch: false);

            foreach (var token in tokens) {
                if (rewardWaiter.Check(token)) {
                    // get_node("/root/mwmwArchipelago").send_quest_check(data["goal_id"], data["tier"])
                    yield return token;

                    yield return new Token(TokenType.Newline, 1);
                    yield return new IdentifierToken("get_node");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("send_quest_check");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new IdentifierToken("data");
                    yield return new Token(TokenType.BracketOpen);
                    yield return new ConstantToken(new StringVariant("goal_id"));
                    yield return new Token(TokenType.BracketClose);
                    yield return new Token(TokenType.Comma);
                    yield return new IdentifierToken("data");
                    yield return new Token(TokenType.BracketOpen);
                    yield return new ConstantToken(new StringVariant("tier"));
                    yield return new Token(TokenType.BracketClose);
                    yield return new Token(TokenType.ParenthesisClose);

                } else {
                    yield return token;
                }
            }
        }
    }
}
