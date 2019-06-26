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

    public abstract class NounPhrase : Phrase
    {
    }

    public class Noun : NounPhrase
    {
        public Word word;

        protected override void DisplayEnglish(IDisplay display)
        {
            if (word.pos == PartOfSpeech.Pronoun)
            {
                display.Print(word.english, FontColor.Blue, FontStyle.Italic);
            }
            else if (word.pos == PartOfSpeech.ProperNoun)
            {
                display.Print(word.english, FontColor.Black, FontStyle.Bold);
            }
            else
            {
                display.Print(word.english);
            }
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (word.pos == PartOfSpeech.Pronoun)
            {
                display.Print(word.persian, FontColor.Blue, FontStyle.Italic);
            }
            else if(word.pos == PartOfSpeech.ProperNoun)
            {
                display.Print(word.persian, FontColor.Black, FontStyle.Bold);
            }
            else
            {
                display.Print(word.persian);
            }
            display.PrintSpace();
        }
    }

    public class DeterminerNoun : NounPhrase
    {
        public Word determiner;
        public NounPhrase right;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(determiner.english, FontColor.LightBlue);
            display.PrintSpace();

            right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            display.Print(determiner.persian, FontColor.LightBlue);
            display.PrintSpace();

            right.Display(display, Language.Persian);
        }
    }

    public class ConjNoun : NounPhrase
    {
        public NounPhrase left;
        public Word conjunction;
        public NounPhrase right;

        protected override void DisplayEnglish(IDisplay display)
        {
            left.Display(display, Language.English);

            display.Print(conjunction.english, FontColor.Gray);
            display.PrintSpace();

            right.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            left.Display(display, Language.Persian);

            display.Print(conjunction.persian, FontColor.Gray);
            display.PrintSpace();

            right.Display(display, Language.Persian);
        }
    }

    public class AdjectiveNoun : NounPhrase
    {
        public Word adjective;
        public NounPhrase right;

        protected override void DisplayEnglish(IDisplay display)
        {
            display.Print(adjective.english, FontColor.LightRed);
            display.PrintSpace();

            right?.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            right?.Display(display, Language.Persian);

            display.Print(adjective.persian, FontColor.LightRed);
            display.PrintSpace();
        }
    }

    public enum VerbPhraseTense
    {
        Subjective,
        SimplePresent,
        SimplePast,
    }

    public abstract class VerbPhrase : Phrase
    {
    }

    public class Verb : VerbPhrase
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

    public class PhrasalVerb : VerbPhrase
    {
        public VerbPhrase action;
        public Word preposition;

        protected override void DisplayEnglish(IDisplay display)
        {
            action.Display(display, Language.English);

            display.Print(preposition.english, FontColor.LightGreen);
            display.PrintSpace();
        }

        protected override void DisplayPersian(IDisplay display)
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectiveVerb : VerbPhrase
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

    public class SubjectiveVerb : VerbPhrase
    {
        public VerbPhrase toBe;
        public NounPhrase status;

        protected override void DisplayEnglish(IDisplay display)
        {
            toBe.Display(display, Language.English);

            //display.Print(status.english, FontColor.LightRed, FontStyle.Italic);
            //display.PrintSpace();
            status.Display(display, Language.English);
        }

        protected override void DisplayPersian(IDisplay display)
        {
            //display.Print(status.persian, FontColor.LightRed);
            //display.PrintSpace();
            status.Display(display, Language.Persian);

            toBe.Display(display, Language.Persian);
        }
    }
}
