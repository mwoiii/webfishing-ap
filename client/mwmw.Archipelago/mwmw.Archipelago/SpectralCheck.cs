using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago;

public class SpectralCheck : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Interactables/Spectral/spectral_spawn.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // func _activate(actor):
        var ownedWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken{Name:"_activate"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken{Name:"actor"},
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
        ]);

        foreach (var token in tokens) {
            if (ownedWaiter.Check(token)) {
                // var ap = get_node("/root/mwmwArchipelago")
                // ap.send_item_check(item_id)
                // if ap.Config.current_goal != null:
                //   PlayerData._send_notification("Sent a check!", 0)
                //   return
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
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("send_item_check");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("item_id");
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("current_goal");
                yield return new Token(TokenType.OpNotEqual);
                yield return new ConstantToken(new NilVariant());
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_send_notification");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Sent a check!"));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfReturn);

            } else {
                yield return token;
            }
        }
    }
}