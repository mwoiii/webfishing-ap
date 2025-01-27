using GDWeave.Godot;
using GDWeave.Modding;

namespace ArchipelagoTweaks;

public class EquippedRodPatch : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/playerdata.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // var guitar_shapes = []<\n>
        var guitarWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrVar,
            t => t is IdentifierToken { Name: "guitar_shapes"},
            t => t.Type is TokenType.OpAssign,
            t => t.Type is TokenType.BracketOpen,
            t => t.Type is TokenType.BracketClose,
            t => t.Type is TokenType.Newline
        ]);

        foreach (var token in tokens) {
            if (guitarWaiter.Check(token)) {
                // enum ROD{SIMPLE, TRAVELERS, COLLECTORS, SHINING, GLISTENING, OPULENT, RADIANT, ALPHA, SPECTRAL, PROSPEROUS}\n
                // var equipped_rod = ROD.SIMPLE\n
                yield return token;

                yield return new Token(TokenType.PrEnum);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.CurlyBracketOpen);
                yield return new IdentifierToken("SIMPLE");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("TRAVELERS");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("COLLECTORS");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("SHINING");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("GLISTENING");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("GLISTENING");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("OPULENT");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("RADIANT");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("ALPHA");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("SPECTRAL");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("PROSPEROUS");
                yield return new Token(TokenType.CurlyBracketClose);
                yield return new Token(TokenType.Newline);

                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("equipped_rod");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("ROD");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SIMPLE");
                yield return new Token(TokenType.Newline);

            } else {
                yield return token;
            }
        }
    }
}