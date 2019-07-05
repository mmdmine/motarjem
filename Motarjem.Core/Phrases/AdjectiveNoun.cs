using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class AdjectiveNoun : NounPhrase
    {
        public Word Adjective;
        public NounPhrase Right;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Adjective.English, FontColor.LightRed);
            display.PrintSpace();

            Right?.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (Right != null)
            {
                Right.Display(display, Language.Persian);
                var nominal = Right as Nominal;
                if (nominal != null)
                {
                    var noun = nominal.Right as Noun;
                    if (noun != null &&
                        "هوا".Contains(noun.Word.Persian.Last())) 
                    {
                        // TODO: should remove space after Noun
                        display.Print("ی");
                        display.PrintSpace();
                    }
                }
                else
                {
                    var adj = Right as AdjectiveNoun;
                    if (adj != null)
                    {
                        adj.Display(display, Language.Persian);
                    }
                }
            }

            display.Print(Adjective.Persian, FontColor.LightRed);
            display.PrintSpace();
        }

        internal static AdjectiveNoun ParseEnglish(Queue<Word[]> words)
        {
            var adj = new AdjectiveNoun {Adjective = words.Dequeue().First(a => a.Pos == PartsOfSpeech.Adjective)};
            if (words.Any())
            {
                if (words.Peek().Any(a => a.Pos == PartsOfSpeech.Adjective))
                    adj.Right = ParseEnglish(words);
                else if (words.Peek().Any(a => a.IsNoun))
                    adj.Right = Nominal.ParseEnglish(words);
            }
            return adj;
        }
    }
}