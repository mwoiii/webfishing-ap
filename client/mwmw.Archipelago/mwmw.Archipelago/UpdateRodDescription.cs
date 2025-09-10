using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago;

public class UpdateRodDescription : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/inventory.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // str(data.catch_blurb)
        var idWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.BuiltInFunc,
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken{Name:"data"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken{Name:"catch_blurb"},
            t => t.Type is TokenType.ParenthesisClose,
        ]);

        foreach (var token in tokens) {
            if (idWaiter.Check(token)) {
                // var ap = get_node("/root/mwmwArchipelago")
                // if ap.Config.mode == ap.Config.Gamemode.ALT:
                //   desc = desc + "\n" + ap.get_harmonized_rod_desc(item["id"])
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
                yield return new IdentifierToken("mode");
                yield return new Token(TokenType.OpEqual);
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Gamemode");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("ALT");
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("desc");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("desc");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("\n"));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("ap");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_harmonized_rod_desc");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("item");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("id"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);

            } else {
                yield return token;
            }
        }
    }
}