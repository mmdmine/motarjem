using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class Verb : VerbPhrase
    {
        public Word word;
        public VerbPhraseTense tense;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(word.english, FontColor.Green);
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (tense == VerbPhraseTense.SimplePresent &&
                string.IsNullOrWhiteSpace(word.persian_2))
                display.Print("می", FontColor.Green);
            display.Print(word.persian, FontColor.Green);
            if (!string.IsNullOrWhiteSpace(word.persian_2))
            {
                if (tense == VerbPhraseTense.SimplePresent)
                    display.Print("می", FontColor.Green);
                display.Print(word.persian_2, FontColor.Green);
            }
            if (!string.IsNullOrWhiteSpace(word.persian_verb_identifier))
                display.Print(word.persian_verb_identifier, FontColor.Green);
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
