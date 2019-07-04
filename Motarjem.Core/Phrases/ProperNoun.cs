using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class ProperNoun : NounPhrase
    {
        public Word Noun;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Noun.English, FontColor.Black, FontStyle.Bold);
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            display.Print(Noun.Persian, FontColor.Black, FontStyle.Bold);
            display.PrintSpace();
        }
    }
}
