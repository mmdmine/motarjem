using System;
using Motarjem.Core.Sentences;

namespace Motarjem.Core.Phrases
{
    /// <summary>
    /// Collection of Words
    /// </summary>
    public abstract class Phrase
    {
        protected abstract void DisplayEnglish(IDisplay display);
        protected abstract void DisplayPersian(IDisplay display);

        public void Display(IDisplay display, Language lang)
        {
            switch (lang)
            {
                case Language.English:
                    DisplayEnglish(display);
                    break;
                case Language.Persian:
                    DisplayPersian(display);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(); // only when language is not set.
            }
        }
    }
}