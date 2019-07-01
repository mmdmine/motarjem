namespace Motarjem.Core.Phrases
{
    internal class ObjectiveVerb : VerbPhrase
    {
        public VerbPhrase action;
        public NounPhrase objectNoun;

        protected override void DisplayEnglish(IDisplay display)
        {
            action.Display(display, Language.English);
            objectNoun.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            objectNoun.Display(display, Language.Persian);

            display.Print("را", FontColor.Gray);
            display.PrintSpace();

            action.Display(display, Language.Persian);
        }
    }
}
