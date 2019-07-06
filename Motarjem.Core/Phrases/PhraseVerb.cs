using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    internal class PhraseVerb : VerbPhrase
    {
        public VerbPhraseTense Tense { get; internal set; }
        public WordVerb Verb { get; internal set; }

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(Verb.English, FontColor.Green);
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (Tense == VerbPhraseTense.SimplePresent &&
                string.IsNullOrWhiteSpace(Verb.Persian2))
                display.Print("می", FontColor.Green);
            display.Print(Verb.Persian, FontColor.Green);
            if (!string.IsNullOrWhiteSpace(Verb.Persian2))
            {
                if (Tense == VerbPhraseTense.SimplePresent)
                    display.Print("می", FontColor.Green);
                display.Print(Verb.Persian2, FontColor.Green);
            }

            if (!string.IsNullOrWhiteSpace(Verb.PersianVerbIdentifier))
                display.Print(Verb.PersianVerbIdentifier, FontColor.Green);
            display.PrintSpace();
        }
    }

    public enum VerbPhraseTense
    {
        Subjective,
        SimplePresent,
        SimplePast
    }
}