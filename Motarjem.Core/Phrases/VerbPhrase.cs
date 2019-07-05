using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    public abstract class VerbPhrase : Phrase
    {
        internal static VerbPhrase ParseEnglish(Queue<Word[]> words, Word subject)
        {
            if (!words.Peek().Any(a => a.IsVerb)) throw new UnexpectedWord(words.Dequeue()[0].English);
            var word = words.Dequeue();
            var verb = new Verb {Word = word.First(a => a.IsVerb)};
            // Personality
            // lookup for special verbs
            var matches =
                (from v in word
                    where v.IsVerb && v.Person == subject.Person && v.Count == subject.Count
                    select v).ToArray();
            if (matches.Length == 1)
            {
                // exactly one match
                verb.Word = matches[0];
            }
            else if (matches.Length > 1)
            {
                // TODO?
            }

            // generate Persian Verb
            if (string.IsNullOrWhiteSpace(verb.Word.PersianVerbIdentifier))
                verb.Word.PersianVerbIdentifier = GetPersianVerbIdentifier(subject);

            // Tense
            if (verb.Word.Pos == PartsOfSpeech.ToBe)
                // Noun + To Be + Adjective/Noun
                verb.Tense = VerbPhraseTense.Subjective;
            else if (verb.Word.Tense == VerbTense.Present)
                // Noun + Verb
                verb.Tense = VerbPhraseTense.SimplePresent;
            else if (verb.Word.Tense == VerbTense.Past)
                // Noun + Past Verb
                verb.Tense = VerbPhraseTense.SimplePast;
            else
                // Undefined Grammar
                throw new GrammarError(verb.Word.English);

            // Object
            if (!words.Any())
                return verb; // no more words left for object.

            if (verb.Tense == VerbPhraseTense.Subjective && (
                    words.Peek().Any(a => a.IsNoun)
                    || words.Peek().Any(a => a.Pos == PartsOfSpeech.Determiner)
                    || words.Peek().Any(a => a.Pos == PartsOfSpeech.Adjective)))
                return new SubjectiveVerb
                {
                    ToBe = verb,
                    Status = NounPhrase.ParseEnglish(words)
                };
            if (words.Peek().Any(a => a.IsNoun)
                || words.Peek().Any(a => a.Pos == PartsOfSpeech.Determiner))
                return new ObjectiveVerb
                {
                    Action = verb,
                    ObjectNoun = NounPhrase.ParseEnglish(words)
                };

            return verb;
        }

        private static string GetPersianVerbIdentifier(Word subject)
        {
            switch (subject.Count)
            {
                case PersonCount.Singular:
                    switch (subject.Person)
                    {
                        default:
                            throw new Exception(); // TODO?
                        case Person.First:
                            return "م";
                        case Person.Second:
                            return "ی";
                        case Person.Third:
                            return "";
                    }

                case PersonCount.Plural:
                    switch (subject.Person)
                    {
                        default:
                            throw new Exception(); // TODO?
                        case Person.First:
                            return "یم";
                        case Person.Second:
                            return "ید";
                        case Person.Third:
                            return "ند";
                    }

                default:
                    throw new Exception(); // TODO?
            }
        }
    }
}