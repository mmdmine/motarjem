using System;

// TODO: split Persian Phrase and English Phrase
// TODO: override `Display()` methods

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

    public class NounPhrase : Phrase
    {
        protected override void DisplayEnglish(IDisplay display)
        {
            if (this is Noun)
            {
                var noun = this as Noun;
                if (noun.word.pos == PartOfSpeech.ProperNoun)
                {
                    display.Print(noun.word.english, FontColor.Black, FontStyle.Bold);
                }
                else if (noun.word.pos == PartOfSpeech.Pronoun)
                {
                    display.Print(noun.word.english, FontColor.Blue, FontStyle.Italic);
                }
                else
                {
                    display.Print(noun.word.english);
                }
                display.PrintSpace();
            }
            else if (this is DeterminerNoun)
            {
                var dn = this as DeterminerNoun;
                display.Print(dn.determiner.english, FontColor.LightBlue);
                display.PrintSpace();
                dn.right.Display(display, Language.English);
                display.PrintSpace();
            }
            else if (this is AdjectiveNoun)
            {
                var adj = this as AdjectiveNoun;
                display.Print(adj.adjective.english, FontColor.LightRed);
                display.PrintSpace();
                adj.right.Display(display, Language.English);
                display.PrintSpace();
            }
            else if (this is ConjNoun)
            {
                var conj = this as ConjNoun;
                conj.left.Display(display, Language.English);
                display.PrintSpace();
                display.Print(conj.conjunction.english, FontColor.Gray);
                display.PrintSpace();
                conj.right.Display(display, Language.English);
                display.PrintSpace();
            }
            else
            {
                display.Print("Undefined Noun Phrase: " + GetType().FullName, FontColor.Red);
            }
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (this is Noun)
            {
                var noun = this as Noun;
                if (noun.word.pos == PartOfSpeech.ProperNoun)
                {
                    display.Print(noun.word.persian, FontColor.Black, FontStyle.Bold);
                }
                else if (noun.word.pos == PartOfSpeech.Pronoun)
                {
                    display.Print(noun.word.persian, FontColor.Blue, FontStyle.Italic);
                }
                else
                {
                    display.Print(noun.word.persian);
                }
                display.PrintSpace();
            }
            else if (this is DeterminerNoun)
            {
                var dn = this as DeterminerNoun;
                display.Print(dn.determiner.persian, FontColor.LightBlue);
                display.PrintSpace();
                dn.right.Display(display, Language.Persian);
                display.PrintSpace();
            }
            else if (this is AdjectiveNoun)
            {
                var adj = this as AdjectiveNoun;
                adj.right.Display(display, Language.Persian);
                display.PrintSpace();
                display.Print(adj.adjective.persian, FontColor.LightRed);
                display.PrintSpace();
            }
            else if (this is ConjNoun)
            {
                var conj = this as ConjNoun;
                conj.left.Display(display, Language.Persian);
                display.PrintSpace();
                display.Print(conj.conjunction.persian, FontColor.Gray);
                display.PrintSpace();
                conj.right.Display(display, Language.Persian);
                display.PrintSpace();
            }
            else
            {
                display.Print("Undefined Noun Phrase: " + GetType().FullName, FontColor.Red);
            }
        }
    }

    public class Noun : NounPhrase
    {
        public Word word;
    }

    public class DeterminerNoun : NounPhrase
    {
        public Word determiner;
        public NounPhrase right;
    }

    public class ConjNoun : NounPhrase
    {
        public NounPhrase left;
        public Word conjunction;
        public NounPhrase right;
    }

    public class AdjectiveNoun : NounPhrase
    {
        public Word adjective;
        public NounPhrase right;
    }

    public class VerbPhrase : Phrase
    {
        protected override void DisplayEnglish(IDisplay display)
        {
            if (this is Verb)
            {
                var verb = this as Verb;
                display.Print(verb.word.english, FontColor.Green);
            }
            else if (this is PhrasalVerb)
            {
                var pv = this as PhrasalVerb;
                pv.action.Display(display, Language.English);
                display.Print(pv.preposition.english, FontColor.LightGreen);
            }
            else if (this is ObjectiveVerb)
            {
                var ov = this as ObjectiveVerb;
                ov.action.Display(display, Language.English);
                ov.objectNoun.Display(display, Language.English);
            }
            else if (this is SubjectiveVerb)
            {
                var sv = this as SubjectiveVerb;
                sv.toBe.Display(display, Language.English);
                display.Print(sv.status.english, FontColor.LightRed, FontStyle.Italic);
            }
            else
            {
                display.Print("Undefined Verb Phrase: " + GetType().FullName, FontColor.Red);
            }
        }

        protected override void DisplayPersian(IDisplay display)
        {
            if (this is Verb)
            {
                var verb = this as Verb;
                display.Print(verb.word.persian, FontColor.Green);
                if (!string.IsNullOrWhiteSpace(verb.word.persian_2))
                    display.Print(verb.word.persian_2, FontColor.Green);
                if (!string.IsNullOrWhiteSpace(verb.word.persian_verb_identifier))
                    display.Print(verb.word.persian_verb_identifier, FontColor.Green);
                display.Print(" ", FontColor.Green);
            }
            else if (this is ObjectiveVerb)
            {
                var ov = this as ObjectiveVerb;
                ov.objectNoun.Display(display, Language.Persian);
                display.Print("را ", FontColor.Gray);
                ov.action.Display(display, Language.Persian);
            }
            else if (this is SubjectiveVerb)
            {
                var sv = this as SubjectiveVerb;
                display.Print(sv.status.persian, FontColor.LightRed);
                sv.toBe.Display(display, Language.Persian);
            }
            else
            {
                display.Print("Undefined Verb Phrase: " + GetType().FullName, FontColor.Red);
            }
        }
    }

    public class Verb : VerbPhrase
    {
        public Word word;
    }

    public class PhrasalVerb : VerbPhrase
    {
        public VerbPhrase action;
        public Word preposition;
    }

    public class ObjectiveVerb : VerbPhrase
    {
        public VerbPhrase action;
        public NounPhrase objectNoun;
    }

    public class SubjectiveVerb : VerbPhrase
    {
        public VerbPhrase toBe;
        public Word status;
    }
}
