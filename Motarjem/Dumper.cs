using Motarjem.Core;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

// TODO: colors

namespace Motarjem
{
    internal static class EnglishDump
    {
        private static InlineCollection output;

        private static void DumpNP(NounPhrase np)
        {
            if (np is Noun)
            {
                var noun = np as Noun;
                if (noun.word.pos == PartOfSpeech.ProperNoun)
                {
                    output.Add(new Run { FontStyle = FontStyles.Italic, Foreground = Brushes.Black, Text = noun.word.english + " " });
                }
                else if (noun.word.pos == PartOfSpeech.Pronoun)
                {
                    output.Add(new Run { Foreground = Brushes.DarkOliveGreen, Text = noun.word.english + " " });
                }
                else
                {
                    output.Add(new Run { Foreground = Brushes.Black, Text = noun.word.english + " " });
                }
            }
            else if (np is DeterminerNoun)
            {
                var dn = np as DeterminerNoun;
                output.Add(new Run { Foreground = Brushes.DarkBlue, Text = dn.determiner.english + " " });
                DumpNP(dn.right);
            }
            else if (np is AdjectiveNoun)
            {
                var adj = np as AdjectiveNoun;
                output.Add(new Run { Foreground = Brushes.DarkMagenta, Text = adj.adjective.english + " " });
                DumpNP(adj.right);
            }
            else if (np is ConjNoun)
            {
                var conj = np as ConjNoun;
                DumpNP(conj.left);
                output.Add(new Run { Foreground = Brushes.LightGray, Text = conj.conjunction.english + " " });
                DumpNP(conj.right);
            }
            else
            {
                output.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Noun Phrase: " + np.GetType().FullName });
            }
        }

        private static void DumpVP(VerbPhrase vp)
        {
            if (vp is Verb)
            {
                var verb = vp as Verb;
                output.Add(new Run { Foreground = Brushes.DarkSeaGreen, Text = verb.word.english + " " });
            }
            else if (vp is PhrasalVerb)
            {
                var pv = vp as PhrasalVerb;
                DumpVP(pv.action);
                output.Add(new Run { Foreground = Brushes.DarkViolet, Text = pv.preposition.english + " " });
            }
            else if (vp is ObjectiveVerb)
            {
                var ov = vp as ObjectiveVerb;
                DumpVP(ov.action);
                DumpNP(ov.objectNoun);
            }
            else if (vp is SubjectiveVerb)
            {
                var sv = vp as SubjectiveVerb;
                DumpVP(sv.toBe);
                output.Add(new Run { Foreground = Brushes.DarkMagenta, Text = sv.status.english + " " });
            }
            else
            {
                output.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Verb Phrase: " + vp.GetType().FullName + " " });
            }
        }

        public static void DumpSentence(Sentence s)
        {
            if (s is ConjSentence)
            {
                var conj = s as ConjSentence;
                DumpSentence(conj.left);
                output.Add(new Run { Foreground = Brushes.Gray, Text = conj.conj.english + " " });
                DumpSentence(conj.right);
            }
            else if (s is SimpleSentence)
            {
                var ss = s as SimpleSentence;
                DumpNP(ss.np);
                DumpVP(ss.vp);
                output.Add(new Run { Text = ". " });
            }
            else
            {
                output.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Sentence: " + s.GetType().FullName });
            }
        }

        public static void SetOutput(InlineCollection outputInlineCollection)
        {
            output = outputInlineCollection;
        }
    }

    internal static class PersianDump
    {
        private static InlineCollection output;

        private static void DumpNP(NounPhrase np)
        {
            if (np is Noun)
            {
                var noun = np as Noun;
                if (noun.word.pos == PartOfSpeech.ProperNoun)
                {
                    output.Add(new Run { FontStyle = FontStyles.Italic, Foreground = Brushes.Black, Text = noun.word.persian + " " });
                }
                else if (noun.word.pos == PartOfSpeech.Pronoun)
                {
                    output.Add(new Run { Foreground = Brushes.DarkOliveGreen, Text = noun.word.persian + " " });
                }
                else
                {
                    output.Add(new Run { Foreground = Brushes.Black, Text = noun.word.persian + " " });
                }
            }
            else if (np is DeterminerNoun)
            {
                var dn = np as DeterminerNoun;
                output.Add(new Run { Foreground = Brushes.DarkBlue, Text = dn.determiner.persian + " " });
                DumpNP(dn.right);
            }
            else if (np is AdjectiveNoun)
            {
                var adj = np as AdjectiveNoun;
                DumpNP(adj.right);
                output.Add(new Run { Foreground = Brushes.DarkMagenta, Text = adj.adjective.persian + " " });
            }
            else if (np is ConjNoun)
            {
                var conj = np as ConjNoun;
                DumpNP(conj.right);
                output.Add(new Run { Foreground = Brushes.LightGray, Text = conj.conjunction.persian + " " });
                DumpNP(conj.left);
            }
            else
            {
                output.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Noun Phrase: " + np.GetType().FullName });
            }
        }

        private static void DumpVP(VerbPhrase vp)
        {
            if (vp is Verb)
            {
                var verb = vp as Verb;
                output.Add(new Run { Foreground = Brushes.DarkSeaGreen, Text = verb.word.persian });
                if (!string.IsNullOrWhiteSpace(verb.word.persian_2))
                    output.Add(new Run { Foreground = Brushes.DarkSeaGreen, Text = " " + verb.word.persian_2 });
                if (!string.IsNullOrWhiteSpace(verb.word.persian_verb_identifier))
                    output.Add(new Run { Foreground = Brushes.DarkSeaGreen, Text = verb.word.persian_verb_identifier });
                output.Add(new Run { Text = " " });
            }
            else if (vp is PhrasalVerb)
            {
                // TODO
                var pv = vp as PhrasalVerb;
                DumpVP(pv.action);
                output.Add(new Run { Foreground = Brushes.DarkViolet, Text = pv.preposition.persian + " " });
            }
            else if (vp is ObjectiveVerb)
            {
                var ov = vp as ObjectiveVerb;
                DumpNP(ov.objectNoun);
                output.Add(new Run { Text = "را " });
                DumpVP(ov.action);
            }
            else if (vp is SubjectiveVerb)
            {
                var sv = vp as SubjectiveVerb;
                output.Add(new Run { Foreground = Brushes.DarkMagenta, Text = sv.status.persian + " " });
                DumpVP(sv.toBe);
            }
            else
            {
                output.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Verb Phrase: " + vp.GetType().FullName + " " });
            }
        }

        public static void DumpSentence(Sentence s)
        {
            if (s is ConjSentence)
            {
                var conj = s as ConjSentence;
                DumpSentence(conj.left);
                output.Add(new Run { Foreground = Brushes.Gray, Text = conj.conj.persian + " " });
                DumpSentence(conj.right);
            }
            else if (s is SimpleSentence)
            {
                var ss = s as SimpleSentence;
                DumpNP(ss.np);
                DumpVP(ss.vp);
                output.Add(new Run { Text = ". " });
            }
            else
            {
                output.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Sentence: " + s.GetType().FullName });
            }
        }

        public static void SetOutput(InlineCollection outputInlineCollection)
        {
            output = outputInlineCollection;
        }
    }
}
