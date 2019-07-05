using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class AdjectiveNoun : NounPhrase
    {
        public Word Adjective;
        public Nominal Right;

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
                var noun = Right.Right as Noun;
                if (noun == null) return;
                if ("هوا".Contains(noun.Word.Persian.Last()))
                {
                    // TODO: should remove space after Noun
                    display.Print("ی");
                    display.PrintSpace();
                }
            }

            display.Print(Adjective.Persian, FontColor.LightRed);
            display.PrintSpace();
        }

        internal static AdjectiveNoun ParseEnglish(Queue<Word[]> words)
        {
            var adj = new AdjectiveNoun {Adjective = words.Dequeue().First(a => a.Pos == PartsOfSpeech.Adjective)};
            if (words.Any()
                && words.Peek().Any(a => a.IsNoun))
                adj.Right = Nominal.ParseEnglish(words);
            return adj;
        }
    }
}