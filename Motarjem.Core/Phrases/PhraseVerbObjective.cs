using Motarjem.Core.Sentences;

namespace Motarjem.Core.Phrases
{
    /// <summary>
    /// a Phrase containing a Verb and Its Object 
    /// </summary>
    internal class PhraseVerbObjective : VerbPhrase
    {
        public VerbPhrase Action { get; internal set; }
        public NounPhrase Object { get; internal set; }

        protected override void DisplayEnglish(IDisplay display)
        {
            Action.Display(display, Language.English);
            Object.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            Object.Display(display, Language.Persian);

            display.Print("را", FontColor.Gray);
            display.PrintSpace();

            Action.Display(display, Language.Persian);
        }
    }
}