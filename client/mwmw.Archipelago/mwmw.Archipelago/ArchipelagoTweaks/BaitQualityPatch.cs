using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago;

public class BaitQualityPatch : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // const GRAVITY = 32.0 
        var gravityWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrConst,
            t => t is IdentifierToken{Name: "GRAVITY"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken{Value: RealVariant{Value: 32.0}},
            t => t.Type is TokenType.Newline,
            t => t.Type is TokenType.PrConst,
        ]);

        foreach (var token in tokens) {
            if (gravityWaiter.Check(token)) {
                // Replace const with var for BAIT_DATA
                yield return new Token(TokenType.PrVar);

            } else {
                yield return token;
            }
        }
    }
}