using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Motarjem.Core;

namespace Motarjem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DumpNP(NounPhrase np)
        {
            if (np is Noun)
            {
                var noun = np as Noun;
                if (noun.word.pos == PartOfSpeech.ProperNoun)
                {
                    output.Inlines.Add(new Run { FontStyle = FontStyles.Italic, Foreground = Brushes.Black, Text = noun.word.english + " " });
                }
                else if (noun.word.pos == PartOfSpeech.Pronoun)
                {
                    output.Inlines.Add(new Run { Foreground = Brushes.DarkOliveGreen, Text = noun.word.english + " " });
                }
                else
                {
                    output.Inlines.Add(new Run { Foreground = Brushes.Black, Text = noun.word.english + " " });
                }
            }
            else if (np is DeterminerNoun)
            {
                var dn = np as DeterminerNoun;
                output.Inlines.Add(new Run { Foreground = Brushes.DarkBlue, Text = dn.determiner.english + " " });
                DumpNP(dn.right);
            }
            else if (np is AdjectiveNoun)
            {
                var adj = np as AdjectiveNoun;
                output.Inlines.Add(new Run { Foreground = Brushes.DarkMagenta, Text = adj.adjective.english + " " });
                DumpNP(adj.right);
            }
            else if (np is ConjNoun)
            {
                var conj = np as ConjNoun;
                DumpNP(conj.left);
                output.Inlines.Add(new Run { Foreground = Brushes.LightGray, Text = conj.conjunction.english + " " });
                DumpNP(conj.right);
            }
            else
            {
                output.Inlines.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Noun Phrase: " + np.GetType().FullName });
            }
        }

        private void DumpVP(VerbPhrase vp)
        {
            if (vp is Verb)
            {
                var verb = vp as Verb;
                output.Inlines.Add(new Run { Foreground = Brushes.DarkSeaGreen, Text = verb.word.english + " " });
            }
            else if (vp is PhrasalVerb)
            {
                var pv = vp as PhrasalVerb;
                DumpVP(pv.action);
                output.Inlines.Add(new Run { Foreground = Brushes.DarkViolet, Text = pv.preposition.english + " " });
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
                output.Inlines.Add(new Run { Foreground = Brushes.DarkMagenta, Text = sv.status.english + " " });
            }
            else
            {
                output.Inlines.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Verb Phrase: " + vp.GetType().FullName + " " });
            }
        }
        
        private void DumpSentence(Sentence s)
        {
            if (s is ConjSentence)
            {
                var conj = s as ConjSentence;
                DumpSentence(conj.left);
                output.Inlines.Add(new Run { Foreground = Brushes.Gray, Text = conj.conj.english + " " });
                DumpSentence(conj.right);
            }
            else if (s is SimpleSentence)
            {
                var ss = s as SimpleSentence;
                DumpNP(ss.np);
                DumpVP(ss.vp);
                output.Inlines.Add(new Run { Text = ". " });
            }
            else
            {
                output.Inlines.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Sentence: " + s.GetType().FullName });
            }
        }

        private void DumpNPFa(NounPhrase np)
        {
            if (np is Noun)
            {
                var noun = np as Noun;
                if (noun.word.pos == PartOfSpeech.ProperNoun)
                {
                    output.Inlines.Add(new Run { FontStyle = FontStyles.Italic, Foreground = Brushes.Black, Text = noun.word.persian + " " });
                }
                else if (noun.word.pos == PartOfSpeech.Pronoun)
                {
                    output.Inlines.Add(new Run { Foreground = Brushes.DarkOliveGreen, Text = noun.word.persian + " " });
                }
                else
                {
                    output.Inlines.Add(new Run { Foreground = Brushes.Black, Text = noun.word.persian + " " });
                }
            }
            else if (np is DeterminerNoun)
            {
                var dn = np as DeterminerNoun;
                output.Inlines.Add(new Run { Foreground = Brushes.DarkBlue, Text = dn.determiner.persian + " " });
                DumpNPFa(dn.right);
            }
            else if (np is AdjectiveNoun)
            {
                var adj = np as AdjectiveNoun;
                DumpNPFa(adj.right);
                output.Inlines.Add(new Run { Foreground = Brushes.DarkMagenta, Text = adj.adjective.persian + " " });
            }
            else if (np is ConjNoun)
            {
                var conj = np as ConjNoun;
                DumpNPFa(conj.right);
                output.Inlines.Add(new Run { Foreground = Brushes.LightGray, Text = conj.conjunction.persian + " " });
                DumpNPFa(conj.left);
            }
            else
            {
                output.Inlines.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Noun Phrase: " + np.GetType().FullName });
            }
        }

        private void DumpVPFa(VerbPhrase vp)
        {
            if (vp is Verb)
            {
                var verb = vp as Verb;
                output.Inlines.Add(new Run { Foreground = Brushes.DarkSeaGreen, Text = verb.word.persian });
                if (!string.IsNullOrWhiteSpace(verb.word.persian_2))
                    output.Inlines.Add(new Run { Foreground = Brushes.DarkSeaGreen, Text = " " + verb.word.persian_2 });
                if (!string.IsNullOrWhiteSpace(verb.word.persian_verb_identifier))
                    output.Inlines.Add(new Run { Foreground = Brushes.DarkSeaGreen, Text = verb.word.persian_verb_identifier });
                output.Inlines.Add(new Run { Text = " " });
            }
            else if (vp is PhrasalVerb)
            {
                // TODO
                var pv = vp as PhrasalVerb;
                DumpVPFa(pv.action);
                output.Inlines.Add(new Run { Foreground = Brushes.DarkViolet, Text = pv.preposition.persian + " " });
            }
            else if (vp is ObjectiveVerb)
            {
                var ov = vp as ObjectiveVerb;
                DumpNPFa(ov.objectNoun);
                output.Inlines.Add(new Run { Text = "را " });
                DumpVPFa(ov.action);
            }
            else if (vp is SubjectiveVerb)
            {
                var sv = vp as SubjectiveVerb;
                output.Inlines.Add(new Run { Foreground = Brushes.DarkMagenta, Text = sv.status.persian + " " });
                DumpVPFa(sv.toBe);
            }
            else
            {
                output.Inlines.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Verb Phrase: " + vp.GetType().FullName + " " });
            }
        }

        private void DumpSentenceFa(Sentence s)
        {
            if (s is ConjSentence)
            {
                var conj = s as ConjSentence;
                DumpSentenceFa(conj.left);
                output.Inlines.Add(new Run { Foreground = Brushes.Gray, Text = conj.conj.persian + " " });
                DumpSentenceFa(conj.right);
            }
            else if (s is SimpleSentence)
            {
                var ss = s as SimpleSentence;
                DumpNPFa(ss.np);
                DumpVPFa(ss.vp);
                output.Inlines.Add(new Run { Text = ". " });
            }
            else
            {
                output.Inlines.Add(new Run { Foreground = Brushes.Red, Text = "Undefined Sentence: " + s.GetType().FullName });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            output.Inlines.Clear();
            try
            {
                var sentences = Parser.ParseEnglish(Token.Tokenize(input.Text));
                foreach (var s in sentences)
                {
                    DumpSentenceFa(Translator.Translate(s));
                    output.Inlines.Add(new LineBreak());
                    DumpSentence(s);
                    output.Inlines.Add(new LineBreak());
                }
            }
            catch (MotarjemException ex)
            {
                output.Inlines.Add(new Run { Foreground = Brushes.Red, Text = "Error: " });
                output.Inlines.Add(new Run { FontStyle = FontStyles.Italic, Foreground = Brushes.Red, Text = ex.GetType().FullName });
                output.Inlines.Add(new LineBreak());
                output.Inlines.Add(new Run { Foreground = Brushes.DarkRed, Text = ex.Message });
                output.Inlines.Add(new LineBreak());
                output.Inlines.Add(new Run { Foreground = Brushes.DarkRed, Text = ex.MessageFa });
            }
            catch (Exception ex)
            {
                output.Inlines.Add(new Run { Foreground = Brushes.Red, Text = "Internal Error: " });
                output.Inlines.Add(new Run { FontStyle = FontStyles.Italic, Foreground = Brushes.Red, Text = ex.GetType().FullName });
            }
        }
    }
}
