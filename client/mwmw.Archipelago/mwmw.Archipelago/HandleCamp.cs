using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class HandleCamp : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/HUD/Shop/loan_button.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // if PlayerData.money < pay_amt or PlayerData.loan_level > 2: return 
            var selectionWaiter = new MultiTokenWaiter([
                t => t.Type is TokenType.CfIf,
                t => t is IdentifierToken{Name:"PlayerData"},
                t => t.Type is TokenType.Period,
                t => t is IdentifierToken{Name:"money"},
                t => t.Type is TokenType.OpLess,
                t => t is IdentifierToken{Name:"pay_amt"},
                t => t.Type is TokenType.OpOr,
                t => t is IdentifierToken{Name:"PlayerData"},
                t => t.Type is TokenType.Period,
                t => t is IdentifierToken{Name:"loan_level"},
                t => t.Type is TokenType.OpGreater,
                t => t is ConstantToken{Value: IntVariant{Value: 2}},
                t => t.Type is TokenType.Colon,
                t => t.Type is TokenType.CfReturn
        ], allowPartialMatch: false);

            // PlayerData.loan_level += 1
            var levelWaiter = new MultiTokenWaiter([
                t => t is IdentifierToken{Name:"PlayerData"},
                t => t.Type is TokenType.Period,
                t => t is IdentifierToken{Name:"loan_level"},
                t => t.Type is TokenType.OpAssignAdd,
                t => t is ConstantToken{Value: IntVariant{Value: 1}},
        ], allowPartialMatch: false);

            foreach (var token in tokens) {
                if (selectionWaiter.Check(token)) {
                    // elif get_node("/root/mwmwArchipelago").Config.current_goal != null and get_node("/root/mwmwArchipelago).unlocked_level <= PlayerData.loan_level:
                    //   PlayerData.emit_signal("_float_number", "need more camp unlocks!", "#aa3939", -0.4, get_global_mouse_position())
                    //   return
                    yield return token;

                    yield return new Token(TokenType.Newline, 1);
                    yield return new Token(TokenType.CfElif);
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
                    yield return new IdentifierToken("get_node");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("unlocked_level");
                    yield return new Token(TokenType.OpLessEqual);
                    yield return new IdentifierToken("PlayerData");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("loan_level");
                    yield return new Token(TokenType.Colon);

                    yield return new Token(TokenType.Newline, 2);
                    yield return new IdentifierToken("PlayerData");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("emit_signal");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("_float_number"));
                    yield return new Token(TokenType.Comma);
                    yield return new ConstantToken(new StringVariant("need more camp unlocks!"));
                    yield return new Token(TokenType.Comma);
                    yield return new ConstantToken(new StringVariant("#aa3939"));
                    yield return new Token(TokenType.Comma);
                    yield return new ConstantToken(new RealVariant(-0.4));
                    yield return new Token(TokenType.Comma);
                    yield return new IdentifierToken("get_global_mouse_position");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.ParenthesisClose);

                    yield return new Token(TokenType.Newline, 2);
                    yield return new Token(TokenType.CfReturn);

                } else if (levelWaiter.Check(token)) {
                    // var ap = get_node("/root/mwmwArchipelago")
                    // if ap.Config.current_goal == ap.Config.Goal.COMPLETED_CAMP and PlayerData.loan_level >= 3:
                    //     ap.send_victory()
                    yield return token;

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
                    yield return new IdentifierToken("COMPLETED_CAMP");
                    yield return new Token(TokenType.OpAnd);
                    yield return new IdentifierToken("PlayerData");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("loan_level");
                    yield return new Token(TokenType.OpGreaterEqual);
                    yield return new ConstantToken(new IntVariant(3));
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
