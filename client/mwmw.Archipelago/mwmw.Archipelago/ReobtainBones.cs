using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago;

public class ReobtainBones : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Interactables/npc_test.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // actor.hud.shop_id = shop_id
        var idWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken{Name:"actor"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken{Name:"hud"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken{Name:"shop_id"},
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken{Name:"shop_id"},
        ]);

        foreach (var token in tokens) {
            if (idWaiter.Check(token)) {
                // var ap = get_node("/root/mwmwArchipelago")
                // if required_tag != "" and ap.Config.current_goal != null:
                //   ap.reobtain_spectral_bones()
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
                yield return new IdentifierToken("required_tag");
                yield return new Token(TokenType.OpNotEqual);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("current_goal");
                yield return new Token(TokenType.OpNotEqual);
                yield return new ConstantToken(new NilVariant());
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("reobtain_spectral_bones");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);

            } else {
                yield return token;
            }
        }
    }
}