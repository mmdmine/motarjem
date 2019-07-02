namespace Motarjem.Core.Phrases
{
    internal class ObjectiveVerb : VerbPhrase
    {
        public VerbPhrase Action;
        public NounPhrase ObjectNoun;

        protected override void DisplayEnglish(IDisplay display)
        {
            Action.Display(display, Language.English);
            ObjectNoun.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            ObjectNoun.Display(display, Language.Persian);

            display.Print("را", FontColor.Gray);
            display.PrintSpace();

            Action.Display(display, Language.Persian);
        }
    }
}
