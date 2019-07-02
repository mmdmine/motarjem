using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class Verb : VerbPhrase
    {
        public Word Word;
        public VerbPhraseTense Tense;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Word.English, FontColor.Green);
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (Tense == VerbPhraseTense.SimplePresent &&
                string.IsNullOrWhiteSpace(Word.Persian2))
                display.Print("می", FontColor.Green);
            display.Print(Word.Persian, FontColor.Green);
            if (!string.IsNullOrWhiteSpace(Word.Persian2))
            {
                if (Tense == VerbPhraseTense.SimplePresent)
                    display.Print("می", FontColor.Green);
                display.Print(Word.Persian2, FontColor.Green);
            }
            if (!string.IsNullOrWhiteSpace(Word.PersianVerbIdentifier))
                display.Print(Word.PersianVerbIdentifier, FontColor.Green);
            display.PrintSpace();
        }
    }

    public enum VerbPhraseTense
    {
        Subjective,
        SimplePresent,
        SimplePast,
    }
}
