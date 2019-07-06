using System;
using System.Collections.Generic;
using System.Linq;
using Motarjem.Core.Dictionary;

namespace Motarjem.Core.Phrases
{
    public abstract class VerbPhrase : Phrase
    {
        internal static VerbPhrase ParseEnglish(Queue<Word[]> words, WordPronoun subject)
        {
            if (!words.Peek().Any(word => word is WordVerb)) throw new UnexpectedWord(words.Dequeue()[0].English);
            
            var matches = words.Dequeue().Where(word => word is WordVerb).Cast<WordVerb>().ToArray();
            
            // fallback value is first verb in matched words
            var verb = new PhraseVerb {Verb = matches[0]};
            
            // Personality
            
            // lookup for special verbs
            matches = matches.Where(word => word is WordVerb wordVerb &&
                                             wordVerb.Person == subject.Person &&
                                             wordVerb.Count == subject.Count)
                .ToArray();
            if (matches.Length == 1)
            {
                // exactly one match
                verb.Verb = matches[0];
            }
            else if (matches.Length > 1)
            {
                // TODO?
            }
            
            // Generating Persian Verb
            if (string.IsNullOrWhiteSpace(verb.Verb.PersianVerbIdentifier))
                verb.Verb.PersianVerbIdentifier = GetPersianVerbIdentifier(subject);

            // Tenses
            
            // Noun + To Be + Adjective/Noun
            if (verb.Verb.VerbType == VerbType.ToBe)
                verb.Tense = VerbPhraseTense.Subjective;
            
            // Noun + Verb
            else if (verb.Verb.Tense == VerbTense.Present)
                verb.Tense = VerbPhraseTense.SimplePresent;
            
            // Noun + Past Verb
            else if (verb.Verb.Tense == VerbTense.Past)
                verb.Tense = VerbPhraseTense.SimplePast;
            
            // Undefined Grammar    
            else
                throw new GrammarError(verb.Verb.English);

            // Object
            if (!words.Any())
                return verb; // no more words left for object.

            if (verb.Tense == VerbPhraseTense.Subjective && (
                    words.Peek().Any(word => word is WordNoun) ||
                    words.Peek().Any(word => word is WordAdj)  ||
                    words.Peek().Any(word => word is WordDet)))
                return new PhraseVerbSubjective
                {
                    ToBe = verb,
                    Status = NounPhrase.ParseEnglish(words)
                };
            
            if (words.Peek().Any(word => word is WordNoun) ||
                words.Peek().Any(word => word is WordDet))
                return new PhraseVerbObjective
                {
                    Action = verb,
                    Object = NounPhrase.ParseEnglish(words)
                };

            return verb;
        }

        private static string GetPersianVerbIdentifier(WordPronoun subject)
        {
            switch (subject.Count)
            {
                case PersonCount.Singular:
                    switch (subject.Person)
                    {
                        case Person.First:
                            return "م";
                        case Person.Second:
                            return "ی";
                        case Person.Third:
                            return "";
                        case Person.All:
                        default:
                            throw new GrammarError("Finding Subject");
                    }

                case PersonCount.Plural:
                    switch (subject.Person)
                    {
                        case Person.First:
                            return "یم";
                        case Person.Second:
                            return "ید";
                        case Person.Third:
                            return "ند";
                        case Person.All:
                        default:
                            throw new GrammarError("Finding Subject");
                    }

                case PersonCount.All:
                default:
                    throw new GrammarError("Finding Subject");
            }
        }
    }
}