namespace Motarjem.Core
{
    partial class Dictionary
    {
        private static Word[] Pronouns =
        {
            new Word
            {
                pos = PartOfSpeech.Pronoun,
                person = Person.First,
                count = PersonCount.Singular,
                english = "I",
                persian = "من",
            },
            new Word
            {
                pos = PartOfSpeech.Pronoun,
                person = Person.First,
                count = PersonCount.Plural,
                english = "We",
                persian = "ما",
            },
            new Word
            {
                pos = PartOfSpeech.Pronoun,
                person = Person.Second,
                count = PersonCount.Singular,
                english = "you",
                persian = "تو",
            },
            new Word
            {
                pos = PartOfSpeech.Pronoun,
                person = Person.Second,
                count = PersonCount.Plural,
                english = "you",
                persian = "شما",
            },
            new Word
            {
                pos = PartOfSpeech.Pronoun,
                person = Person.Third,
                count = PersonCount.Singular,
                sex = PersonSex.Male,
                english = "he",
                persian = "او",
            },
            new Word
            {
                pos = PartOfSpeech.Pronoun,
                person = Person.Third,
                count = PersonCount.Singular,
                sex = PersonSex.Female,
                english = "she",
                persian = "او",
            },
            new Word
            {
                pos = PartOfSpeech.Pronoun,
                person = Person.Third,
                count = PersonCount.Plural,
                english = "they",
                persian = "آنها",
            },
        };
        private static Word[] Verbs =
        {
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.First,
                count = PersonCount.Singular,
                tense = VerbTense.Present,
                english = "am",
                persian = "هستم",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.Third,
                count = PersonCount.Singular,
                tense = VerbTense.Present,
                english = "is",
                persian = "است",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.Third,
                count = PersonCount.Plural,
                tense = VerbTense.Present,
                english = "are",
                persian = "هستند",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.Second,
                count = PersonCount.Singular,
                tense = VerbTense.Present,
                english = "are",
                persian = "هستی",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.First,
                count = PersonCount.Plural,
                tense = VerbTense.Present,
                english = "are",
                persian = "هستیم",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.Second,
                count = PersonCount.Plural,
                tense = VerbTense.Present,
                english = "are",
                persian = "هستید",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.First,
                count = PersonCount.Singular,
                tense = VerbTense.Past,
                english = "was",
                persian = "بودم",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.Second,
                count = PersonCount.Singular,
                tense = VerbTense.Past,
                english = "was",
                persian = "بودی",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.Third,
                count = PersonCount.Singular,
                tense = VerbTense.Past,
                english = "was",
                persian = "بود",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.First,
                count = PersonCount.Plural,
                tense = VerbTense.Past,
                english = "were",
                persian = "بودیم",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.Second,
                count = PersonCount.Plural,
                tense = VerbTense.Past,
                english = "were",
                persian = "بودید",
            },
            new Word
            {
                pos = PartOfSpeech.ToBe,
                person = Person.Third,
                count = PersonCount.Plural,
                tense = VerbTense.Past,
                english = "were",
                persian = "بودند",
            },
            new Word
            {
                pos = PartOfSpeech.Verb,
                tense = VerbTense.Present,
                english = "have",
                persian = "دار",
            },
            new Word
            {
                pos = PartOfSpeech.Verb,
                person = Person.Third,
                count = PersonCount.Singular,
                tense = VerbTense.Present,
                english = "has",
                persian = "دارد",
            },
            new Word
            {
                pos = PartOfSpeech.Verb,
                tense = VerbTense.Past,
                english = "had",
                persian = "داشت",
            },
            new Word
            {
                pos = PartOfSpeech.Verb,
                tense = VerbTense.Present,
                english = "love",
                persian = "دوست",
                persian_2 = "دار",
            },
            new Word
            {
                pos = PartOfSpeech.Verb,
                tense = VerbTense.Present,
                english = "write",
                persian = "نویس",
            },
            new Word
            {
                pos = PartOfSpeech.Verb,
                tense = VerbTense.Past,
                english = "wrote",
                persian = "نوشت",
            },
            new Word
            {
                pos = PartOfSpeech.Verb,
                tense = VerbTense.Present,
                english = "hate",
                persian = "متنفر",
                persian_2 = "هست",
            },
        };
        private static Word[] Conjs =
        {
            new Word
            {
                pos = PartOfSpeech.Conjunction,
                english = "and",
                persian = "و",
            },
            new Word
            {
                pos = PartOfSpeech.Conjunction,
                english = "or",
                persian = "یا",
            },
            new Word
            {
                pos = PartOfSpeech.Conjunction,
                english = "but",
                persian = "اما",
            },
            new Word
            {
                pos = PartOfSpeech.Conjunction,
                english = "that",
                persian = "که",
            }
        };
        private static Word[] Determiners =
        {
            new Word
            {
                pos = PartOfSpeech.Determiner,
                english = "the"
            },
            new Word
            {
                pos = PartOfSpeech.Determiner,
                english = "a",
                persian = "یک",
            },
            new Word
            {
                pos = PartOfSpeech.Determiner,
                english = "an",
                persian = "یک",
            },
            new Word
            {
                pos = PartOfSpeech.Determiner,
                english = "some",
                persian = "مقداری",
            },
            new Word
            {
                pos = PartOfSpeech.Determiner,
                english = "many",
                persian = "خیلی",
            },
            new Word
            {
                pos = PartOfSpeech.Determiner,
                english = "few",
                persian = "کمی",
            },
            new Word
            {
                pos = PartOfSpeech.Determiner,
                english = "that",
                persian = "آن",
            }
        };
        private static Word[] Adjectives =
        {
            new Word
            {
                pos = PartOfSpeech.Adjective,
                english = "happy",
                persian = "خوشحال",
            },
        };
        private static Word[] Nouns =
        {
            new Word
            {
                pos = PartOfSpeech.Noun,
                count = PersonCount.Singular,
                english = "programmer",
                persian = "برنامه‌نویس"
            },
            new Word
            {
                pos = PartOfSpeech.Noun,
                count = PersonCount.Singular,
                english = "program",
                persian = "برنامه",
            },
            new Word
            {
                pos = PartOfSpeech.Noun,
                count = PersonCount.Singular,
                english = "cat",
                persian = "گربه"
            },
            new Word
            {
                pos = PartOfSpeech.Noun,
                count = PersonCount.Singular,
                english = "dog",
                persian = "سگ"
            },
            new Word
            {
                pos = PartOfSpeech.Noun,
                count = PersonCount.Singular,
                english = "butterfly",
                persian = "پروانه"
            },
            new Word
            {
                pos = PartOfSpeech.Noun,
                count = PersonCount.Singular,
                english = "fish",
                persian = "ماهی",
            },
            new Word
            {
                pos = PartOfSpeech.Noun,
                count = PersonCount.Singular,
                english = "book",
                persian = "کتاب",
            },
            new Word
            {
                pos = PartOfSpeech.Noun,
                count = PersonCount.Singular,
                english = "pen",
                persian = "مداد",
            },
        };
    }
}