using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;
using Motarjem.Core.Sentences;

namespace Motarjem.Core.Phrases
{
    /// <summary>
    /// a Phrase contains Adjectives and Nouns 
    /// </summary>
    internal class PhraseAdjective : NounPhrase
    {
        public WordAdj Adjective { get; internal set; }
        public NounPhrase Right { get; internal set; }

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Adjective.English, FontColor.LightRed);
            display.PrintSpace();

            Right?.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            DisplayPersianAdjective(this, display, nested: false);
        }

        private static void DisplayPersianAdjective(PhraseAdjective adjective, IDisplay display, bool nested)
        {
            switch (adjective.Right)
            {
                case PhraseAdjective adj:
                    DisplayPersianAdjective(adj, display, nested: true);
                    break;
                case PhraseNominal noun:
                    DisplayPersianNestedNominal(noun, display);
                    break;
                case null:
                    break;
            }

            display.Print(adjective.Adjective.Persian, FontColor.LightRed);
            
            // Persian nested adjectives that ends with 'A', 'O' or 'Eh'
            // should append 'Yaye-Mianji'
            if (nested && "اوه".Any(ch => adjective.Adjective.Persian.EndsWith(ch.ToString())))
                display.Print("ی");
            
            display.PrintSpace();
        }

        private static void DisplayPersianNestedNominal(PhraseNominal nominal, IDisplay display)
        {
            switch (nominal.Right)
            {
                // Nominal: Noun
                // Nominal: Noun + Noun
                case PhraseNoun noun:
                    display.Print(noun.Noun.Persian);
                    
                    // see DisplayPersianAdjective()
                    if ("اوه".Any(ch => noun.Noun.Persian.EndsWith(ch.ToString())))
                        display.Print("ی");
                    
                    display.PrintSpace();
                    
                    if (nominal.Left != null)
                        DisplayPersianNestedNominal(nominal.Left, display);
                    
                    break;
                
                // Nominal: Noun + Preposition
                case PhrasePrepNoun prep:
                    throw new NotImplementedException();
            }
        }

        internal static PhraseAdjective ParseEnglish(Queue<Word[]> words)
        {
            var adj = new PhraseAdjective {Adjective = (WordAdj)words.Dequeue().First(word => word is WordAdj)};
            if (!words.Any()) return adj;
            if (words.Peek().Any(word => word is WordAdj))
                adj.Right = ParseEnglish(words);
            else if (words.Peek().Any(word => word is WordNoun))
                adj.Right = PhraseNominal.ParseEnglish(words);
            return adj;
        }
    }
}