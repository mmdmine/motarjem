namespace Motarjem.Core.Phrases
{
    internal class SubjectiveVerb : VerbPhrase
    {
        public VerbPhrase toBe;
        public NounPhrase status;

        protected override void DisplayEnglish(IDisplay display)
        {
            toBe.Display(display, Language.English);
            status.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            status.Display(display, Language.Persian);
            toBe.Display(display, Language.Persian);
        }
    }
}
