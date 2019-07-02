namespace Motarjem.Core.Phrases
{
    internal class SubjectiveVerb : VerbPhrase
    {
        public NounPhrase Status;
        public VerbPhrase ToBe;

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