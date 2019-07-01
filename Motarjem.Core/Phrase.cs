using System;

// TODO: split Persian Phrase and English Phrase

namespace Motarjem.Core
{
    public abstract class Phrase
    {
        protected abstract void DisplayEnglish(IDisplay display);
        protected abstract void DisplayPersian(IDisplay display);

        public void Display(IDisplay display, Language lang)
        {
            if (lang == Language.English)
                DisplayEnglish(display);
            else if (lang == Language.Persian)
                DisplayPersian(display);
            else
                throw new Exception(); // only when language is not set.
        }
    }
}
