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
            if (words.Peek().Any(a => a.IsVerb))
            {
                var word = words.Dequeue();
                var verb = new Verb { word = word[0] };
                // Personality
                // lookup for special verbs
                var matches = from v in word
                              where v.IsVerb && v.person == subject.person && v.count == subject.count
                              select v;
                if (matches.Count() == 1)
                {
                    // exactly one match
                    verb.word = matches.SingleOrDefault();
                }
                else if (matches.Count() > 1)
                {
                    // TODO?
                }

                // generate Persian Verb
                if (string.IsNullOrWhiteSpace(verb.word.persian_verb_identifier))
                    verb.word.persian_verb_identifier = FindPersianIdentifier();

                string FindPersianIdentifier()
                {
                    if (subject.count == PersonCount.Singular)
                    {
                        switch (subject.person)
                        {
                            default:
                            case Person.All:
                            case Person.Third:
                                throw new Exception(); // TODO?
                            case Person.First:
                                return "م";
                            case Person.Second:
                                return "ی";
                        }
                    }
                    else if (subject.count == PersonCount.Plural)
                    {
                        switch (subject.person)
                        {
                            default:
                            case Person.All:
                                throw new Exception(); // TODO?
                            case Person.First:
                                return "یم";
                            case Person.Second:
                                return "ید";
                            case Person.Third:
                                return "ند";
                        }
                    }
                    else
                    {
                        throw new Exception(); // TODO?
                    }
                }

                // Tense
                if (verb.word.pos == PartsOfSpeech.ToBe)
                {
                    // Noun + To Be + Adjective/Noun
                    verb.tense = VerbPhraseTense.Subjective;
                }
                else if (verb.word.tense == VerbTense.Present)
                {
                    // Noun + Verb
                    verb.tense = VerbPhraseTense.SimplePresent;
                }
                else if (verb.word.tense == VerbTense.Past)
                {
                    // Noun + Past Verb
                    verb.tense = VerbPhraseTense.SimplePast;
                }
                else
                {
                    // Undefined Grammer
                    throw new GrammerError(verb.word.english);
                }

                // Object
                if (!words.Any())
                    return verb; // no more words left for object.

                if (verb.tense == VerbPhraseTense.Subjective && (
                    words.Peek().Any(a => a.IsNoun)
                    || words.Peek().Any(a => a.pos == PartsOfSpeech.Determiner)
                    || words.Peek().Any(a => a.pos == PartsOfSpeech.Adjective)))
                {
                    return new SubjectiveVerb
                    {
                        toBe = verb,
                        status = NounPhrase.ParseEnglish(words)
                    };
                }
                else if (words.Peek().Any(a => a.IsNoun)
                    || words.Peek().Any(a => a.pos == PartsOfSpeech.Determiner))
                {
                    return new ObjectiveVerb
                    {
                        action = verb,
                        objectNoun = NounPhrase.ParseEnglish(words)
                    };
                }

                return verb;
            }
            else
            {
                throw new UnexpectedWord(words.Dequeue()[0].english);
            }
        }
    }
}
