using Motarjem.Core.Sentences;

namespace Motarjem.Core.Phrases
{
    /// <summary>
    /// Subjective Sentence: Noun Phrase + [Subjective Phrase: To Be + Noun Phrase]
    /// </summary>
    internal class PhraseVerbSubjective : VerbPhrase
    {
        public PhraseVerb ToBe { get; internal set; }
        public NounPhrase Status { get; internal set; }

        protected override void DisplayEnglish(IDisplay display)
        {
            ToBe.Display(display, Language.English);
            Status.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            Status.Display(display, Language.Persian);
            ToBe.Display(display, Language.Persian);
        }
    }
}