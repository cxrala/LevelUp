using System.Text;
using System;
using System.Collections.Generic;

public class NameGenerator {
    private NameGenerator() { }

    public interface ITerm { }

    public struct NonTerminal : ITerm {
        public static implicit operator NonTerminal(string value) => new(value);

        public string value;

        public NonTerminal(string value) { this.value = value; }
    }

    public struct Terminal : ITerm {
        public static implicit operator Terminal(char value) => new(value);

        public char value;

        public Terminal(char value) { this.value = value; }
    }

    public struct Grammar {
        public string start;
        public Dictionary<string, List<(double weighting, List<ITerm> right)>> rules;

        public Grammar(string start, Dictionary<string, List<(double weighting, List<ITerm> right)>> rules) {
            this.start = start;
            this.rules = new Dictionary<string, List<(double weighting, List<ITerm> right)>>();
            foreach (var kvp in rules) {
                double totalWeighting = 0;
                List<(double weighting, List<ITerm> right)> productions = new();
                foreach (var production in kvp.Value) {
                    double exped = Math.Exp(production.weighting);
                    totalWeighting += exped;
                    productions.Add((exped, production.right));
                }
                for (int i = 0; i < productions.Count; ++i) {
                    productions[i] = (productions[i].weighting / totalWeighting, productions[i].right);
                }
                this.rules[kvp.Key] = productions;
            }
        }
    }

    public static Random random = new Random();

    public static List<Grammar> grammars = new() {
        new Grammar(
            "S",
            new() { {
                    "S", new() { (0.5, new() { (NonTerminal)"W", (Terminal)' ', (NonTerminal)"W" }), (1, new() { (NonTerminal)"W" }) }
                }, {
                    "W", new() { (0, new() { (NonTerminal)"Syll" }), (1, new() { (NonTerminal)"Syll", (NonTerminal)"Syll" }),
                        (1, new() { (NonTerminal)"Syll", (NonTerminal)"Syll" }), (0.7, new() { (NonTerminal)"Syll", (NonTerminal)"Syll" }) }
                }, {
                    "Syll", new() { (1, new() { (NonTerminal)"Onset", (NonTerminal)"Nucleus", (NonTerminal)"Coda" }), (1, new() { (NonTerminal)"Onset", (NonTerminal)"Nucleus" }) }
                }, {
                    "Onset", new() { (1, new() { (NonTerminal)"C" }), (0.8, new() { (NonTerminal)"K", (NonTerminal)"R" }) }
                }, {
                    "Nucleus", new() { (1, new() { (NonTerminal)"V" }) }
                }, {
                    "Coda", new() { (1, new() { (Terminal)'n' }), (1, new() { (Terminal)'r' }), (1, new() { (Terminal)'s' }),}
                }, {
                    "C", new() {
                        (1, new() { (NonTerminal)"K" }),
                        (1, new() { (Terminal)'m' }), (1, new() { (Terminal)'n' }), (1, new() { (Terminal)'n', (Terminal)'g' }),
                        (1, new() { (Terminal)'s' }), (0.2, new() { (Terminal)'z' }), (1, new() { (Terminal)'h' }),
                        (1, new() { (NonTerminal)"R" })
                    }
                }, {
                    "V", new() { (1.2, new() { (Terminal)'a' }), (1, new() { (Terminal)'i' }), (0.7, new() { (Terminal)'u' }), (1, new() { (Terminal)'e' }), (1, new() { (Terminal)'o' }),}
                }, {
                    "K", new() { (0.8, new() { (Terminal)'p', (Terminal)'h' }), (1, new() { (Terminal)'t', (Terminal)'h' }), (1, new() { (Terminal)'k', (Terminal)'h' }),
                        (1, new() { (Terminal)'p' }), (1, new() { (Terminal)'t' }), (1, new() { (Terminal)'k' }),
                        (1, new() { (Terminal)'b' }), (1, new() { (Terminal)'d' }), (1, new() { (Terminal)'g' }),}
                }, {
                    "R", new() {
                        (1, new() { (Terminal)'r' }), (1, new() { (Terminal)'r', (Terminal)'h' }), (1, new() { (Terminal)'l'}),
                        (1, new() { (Terminal)'y' }), (1, new() { (Terminal)'w', (Terminal)'h' }), (1, new() { (Terminal)'w'}),}
                },
            }
            ),
    };

    public static string GenerateName() {
        return GenerateName(grammars[random.Next() % grammars.Count]);
    }

    public static string GenerateName(int i) {
        return GenerateName(grammars[i]);
    }

    public static string GenerateName(Grammar grammar) {
        List<ITerm> sequence = new List<ITerm>() { (NonTerminal)grammar.start };
        int i = 0;
        while (i < sequence.Count) {
            if (sequence[i] is NonTerminal n) {
                sequence.RemoveAt(i);
                if (grammar.rules.TryGetValue(n.value, out var rule) && rule.Count > 0) {
                    List<ITerm> chosen = rule[0].right;
                    double choice = random.NextDouble();
                    foreach (var item in rule) {
                        choice -= item.weighting;
                        if (choice < 0) {
                            chosen = item.right;
                            break;
                        }
                    }
                    sequence.InsertRange(i, chosen);
                }
            } else {
                ++i;
            }
        }
        StringBuilder result = new StringBuilder();
        foreach (var term in sequence) {
            if (term is Terminal t) {
                result.Append(t.value);
            }
        }
        return result.ToString();
    }
}
