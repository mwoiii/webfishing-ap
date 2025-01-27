using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class GoalCompletion : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/Singletons/playerdata.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // print("nope i dont have ", q)
            var newFishWaiter = new MultiTokenWaiter([
                t => t.Type is TokenType.BuiltInFunc,
                t => t.Type is TokenType.ParenthesisOpen,
                t => t is ConstantToken{Value: StringVariant{Value: "nope i dont have "}},
                t => t.Type is TokenType.Comma,
                t => t is IdentifierToken{Name:"q"},
                t => t.Type is TokenType.ParenthesisClose
        ], allowPartialMatch: false);

            foreach (var token in tokens) {
                if (newFishWaiter.Check(token)) {
                    // var total_count = 0
                    // var caught_count = 0
                    // for cat in PlayerData.journal_logs.keys():
                    //      for key in PlayerData.journal_logs[cat].keys():
                    // 	        var catch_data = PlayerData.journal_logs[cat][key]
                    // 	        total_count += PlayerData.ITEM_QUALITIES.size()
                    // 	        caught_count += catch_data["quality"].size()
                    // var ap = get_node("/root/mwmwArchipelago")
                    // if ap.Config.current_goal == ap.Config.Goal.TOTAL_COMPLETION and (caught_count / total_count) * 100 >= ap.Config.total_completion_goal:
                    //     ap.send_victory()
                    yield return token;

                    yield return new Token(TokenType.Newline, 2);
                    yield return new Token(TokenType.PrVar);
                    yield return new IdentifierToken("total_count");
                    yield return new Token(TokenType.OpAssign);
                    yield return new ConstantToken(new RealVariant(0.0));

                    yield return new Token(TokenType.Newline, 2);
                    yield return new Token(TokenType.PrVar);
                    yield return new IdentifierToken("caught_count");
                    yield return new Token(TokenType.OpAssign);
                    yield return new ConstantToken(new RealVariant(0.0));

                    yield return new Token(TokenType.Newline, 2);
                    yield return new Token(TokenType.CfFor);
                    yield return new IdentifierToken("cat");
                    yield return new Token(TokenType.OpIn);
                    yield return new IdentifierToken("PlayerData");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("journal_logs");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("keys");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 3);
                    yield return new Token(TokenType.CfFor);
                    yield return new IdentifierToken("key");
                    yield return new Token(TokenType.OpIn);
                    yield return new IdentifierToken("PlayerData");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("journal_logs");
                    yield return new Token(TokenType.BracketOpen);
                    yield return new IdentifierToken("cat");
                    yield return new Token(TokenType.BracketClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("keys");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 4);
                    yield return new Token(TokenType.PrVar);
                    yield return new IdentifierToken("catch_data");
                    yield return new Token(TokenType.OpAssign);
                    yield return new IdentifierToken("PlayerData");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("journal_logs");
                    yield return new Token(TokenType.BracketOpen);
                    yield return new IdentifierToken("cat");
                    yield return new Token(TokenType.BracketClose);
                    yield return new Token(TokenType.BracketOpen);
                    yield return new IdentifierToken("key");
                    yield return new Token(TokenType.BracketClose);

                    yield return new Token(TokenType.Newline, 4);
                    yield return new IdentifierToken("total_count");
                    yield return new Token(TokenType.OpAssignAdd);
                    yield return new IdentifierToken("PlayerData");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("ITEM_QUALITIES");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("size");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new Token(TokenType.ParenthesisClose);

                    yield return new Token(TokenType.Newline, 4);
                    yield return new IdentifierToken("caught_count");
                    yield return new Token(TokenType.OpAssignAdd);
                    yield return new IdentifierToken("catch_data");
                    yield return new Token(TokenType.BracketOpen);
                    yield return new ConstantToken(new StringVariant("quality"));
                    yield return new Token(TokenType.BracketClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("size");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new Token(TokenType.ParenthesisClose);

                    yield return new Token(TokenType.Newline, 2);
                    yield return new Token(TokenType.PrVar);
                    yield return new IdentifierToken("ap");
                    yield return new Token(TokenType.OpAssign);
                    yield return new IdentifierToken("get_node");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                    yield return new Token(TokenType.ParenthesisClose);

                    yield return new Token(TokenType.Newline, 2);
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
                    yield return new IdentifierToken("TOTAL_COMPLETION");
                    yield return new Token(TokenType.OpAnd);
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new IdentifierToken("caught_count");
                    yield return new Token(TokenType.OpDiv);
                    yield return new IdentifierToken("total_count");
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.OpMul);
                    yield return new ConstantToken(new RealVariant(100.0));
                    yield return new Token(TokenType.OpGreaterEqual);
                    yield return new IdentifierToken("ap");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("Config");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("total_completion_goal");
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 3);
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
