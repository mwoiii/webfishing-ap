﻿using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace mwmw.Archipelago {
    public class InteractIntercept : IScriptMod {
        public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
            // Input.is_action_just_pressed("interact")
            var assignWaiter = new MultiTokenWaiter([
                t => t.Type is TokenType.CfIf,
                t => t is IdentifierToken{Name:"Input"},
                t => t.Type is TokenType.Period,
                t => t is IdentifierToken{Name:"is_action_just_pressed"},
                t => t.Type is TokenType.ParenthesisOpen,
                t => t is ConstantToken{Value: StringVariant{Value: "interact"}},
                t => t.Type is TokenType.ParenthesisClose
        ], allowPartialMatch: false);


            foreach (var token in tokens) {
                // appending to after line:
                // and not get_node("/root/mwmwArchipelago").Menu.visible (:)
                if (assignWaiter.Check(token)) {
                    yield return token;

                    yield return new Token(TokenType.OpAnd);
                    yield return new Token(TokenType.OpNot);
                    yield return new IdentifierToken("get_node");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("/root/mwmwArchipelago"));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("Menu");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("visible");
                } else {
                    yield return token;
                }
            }
        }
    }
}
