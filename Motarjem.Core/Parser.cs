using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO: Parse by location, not POS

namespace Motarjem.Core
{
    public static class Parser
    {
        public static IEnumerable<Sentence> ParseEnglish(IEnumerable<Token> tokens)
        {
            var result = new List<Sentence>();
            var words = new List<Word>();
            var tokenEnum = tokens.GetEnumerator();
            if (!tokenEnum.MoveNext())
                return result;
            while (tokenEnum.Current != null)
            {
                var moveNext = true;
                if (tokenEnum.Current.type == Token.Type.Dot)
                {
                    var wordEnum = words.GetEnumerator();
                    wordEnum.MoveNext();
                    result.Add(GetNextSentence(wordEnum));
                    words.Clear();
                }
                else if (tokenEnum.Current.type == Token.Type.QuestionMark)
                {
                    throw new NotImplementedException();
                    // words.Clear();
                }
                else if (tokenEnum.Current.type == Token.Type.Exclamation)
                {
                    throw new NotImplementedException();
                    // words.Clear();
                }
                else
                {
                    var query = GetNextWord(tokenEnum);
                    var matches = Dictionary.LookupEn(query);
                    if (!matches.Any())
                        throw new UndefinedWord(query);
                    words.Add(matches.First());
                    if (tokenEnum.Current?.type != Token.Type.Space)
                        moveNext = false;
                }
                if (moveNext)
                    tokenEnum.MoveNext();
            }
            return result;
        }

        private static string GetNextWord(IEnumerator<Token> enumerator)
        {
            var str = new StringBuilder();
            while (enumerator.Current != null)
            {
                if (enumerator.Current.type == Token.Type.Alphabet || 
                    enumerator.Current.type == Token.Type.Digit)
                    str.Append(enumerator.Current.charactor);
                else
                    break;
                enumerator.MoveNext();
            }
            return str.ToString();
        }

        private static NounPhrase GetNextNounPhrase(IEnumerator<Word> enumerator)
        {
            // TODO: reimplement this
            NounPhrase output;
            if (enumerator.Current.IsNoun)
            {
                output = new Noun { word = enumerator.Current };
                if (!enumerator.MoveNext())
                    return output;
            }
            else if (enumerator.Current.pos == PartOfSpeech.Determiner)
            {
                var dn = new DeterminerNoun { determiner = enumerator.Current };
                if (!enumerator.MoveNext())
                    throw new UnexpectedEnd();
                dn.right = GetNextNounPhrase(enumerator);
                output = dn;
            }
            else if (enumerator.Current.pos == PartOfSpeech.Adjective)
            {
                var adj = new AdjectiveNoun { adjective = enumerator.Current };
                if (enumerator.MoveNext() &&
                    (enumerator.Current.IsNoun || enumerator.Current.pos == PartOfSpeech.Adjective))
                    adj.right = GetNextNounPhrase(enumerator);
                else
                    return adj; // This don't let you to use 'and' between adjectives
                                // like 'Ali is smart and clever'
                                // but need to parse 'Ali is smart and Reza is clever' correctly
                output = adj;
            }
            else
            {
                throw new UnexpectedWord(enumerator.Current.english);
            }
            if (enumerator.Current?.pos == PartOfSpeech.Conjunction)
            {
                var result = new ConjNoun { left = output, conjunction = enumerator.Current };
                if (!enumerator.MoveNext())
                    throw new UnexpectedEnd();
                result.right = GetNextNounPhrase(enumerator);
                output = result;
            }
            return output;
        }

        private static VerbPhrase GetNextVerbPhrase(IEnumerator<Word> enumerator, Word person)
        {
            if (enumerator.Current.IsVerb)
            {
                var verb = new Verb();
                // Personality
                // lookup for special verbs
                var matches = from v in Dictionary.LookupVerbEn(enumerator.Current.english)
                              where v.person == person.person && v.count == person.count
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
                else
                {
                    // generate Persian Verb
                    verb.word = enumerator.Current;
                    verb.word.persian_verb_identifier = FindPersianIdentifier();

                    string FindPersianIdentifier()
                    {
                        if (person.count == PersonCount.Singular)
                        {
                            switch (person.person)
                            {
                                default:
                                case Person.All:
                                    throw new Exception(); // TODO?
                                case Person.First:
                                    return "م";
                                case Person.Second:
                                    return "ی";
                                case Person.Third:
                                    return verb.word.tense == VerbTense.Present ? "د" : "";
                            }
                        }
                        else if (person.count == PersonCount.Plural)
                        {
                            switch (person.person)
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
                }

                // Tense
                if (verb.word.pos == PartOfSpeech.ToBe)
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
                    throw new GrammerError(enumerator.Current.english);
                }

                // Object
                if (!enumerator.MoveNext())
                    return verb; // no more words left for object.

                if (verb.tense == VerbPhraseTense.Subjective
                    && (enumerator.Current.IsNoun 
                    || enumerator.Current.pos == PartOfSpeech.Determiner
                    || enumerator.Current.pos == PartOfSpeech.Adjective))
                {
                    return new SubjectiveVerb
                    {
                        toBe = verb,
                        status = GetNextNounPhrase(enumerator)
                    };
                }
                else if (enumerator.Current.IsNoun 
                    || enumerator.Current.pos == PartOfSpeech.Determiner)
                {
                    return new ObjectiveVerb
                    {
                        action = verb,
                        objectNoun = GetNextNounPhrase(enumerator)
                    };
                }

                return verb;
            }
            else
            {
                throw new UnexpectedWord(enumerator.Current.english);
            }
        }

        private static Word FindSubject(NounPhrase np)
        {
            if (np is Noun)
            {
                var noun = np as Noun;
                return new Word {
                    count = noun.word.count,
                    person = noun.word.pos == PartOfSpeech.Pronoun ? noun.word.person : Person.Third,
                };
            }
            if (np is DeterminerNoun)
            {
                // TODO: 'a' or 'an' is singular, 'some' and ... are plural
                var dn = np as DeterminerNoun;
                return FindSubject(dn.right);
            }
            if (np is AdjectiveNoun)
            {
                var an = np as AdjectiveNoun;
                return FindSubject(an.right);
            }
            if (np is ConjNoun)
            {
                var cn = np as ConjNoun;
                var subject = new Word
                {
                    count = PersonCount.Plural,
                };
                var left = FindSubject(cn.left);
                var right = FindSubject(cn.right);
                if (left.pos == PartOfSpeech.Pronoun)
                {
                    subject.person = left.person;
                }
                if (right.pos == PartOfSpeech.Pronoun)
                {
                    subject.person = right.person;
                }
                if (subject.person == Person.All)
                {
                    subject.person = Person.Third;
                }
                return subject;
            }
            return null;
        }

        private static Sentence GetNextSentence(IEnumerator<Word> enumerator)
        {
            // TODO: if + S1 + ',' + S2
            // Simple Sentence: Noun Phrase + Verb Phrase
            var sentence = new SimpleSentence { language = Language.English };
            sentence.np = GetNextNounPhrase(enumerator);
            sentence.vp = GetNextVerbPhrase(enumerator, FindSubject(sentence.np));
            // Conjunction Sentence: S1 + and + S2
            if (enumerator.Current?.pos == PartOfSpeech.Conjunction)
            {
                var conj = new ConjSentence { conj = enumerator.Current, left = sentence };
                if (!enumerator.MoveNext())
                    return sentence;
                conj.right = GetNextSentence(enumerator);
                return conj;
            }

            if (enumerator.MoveNext())
                throw new UnexpectedWord(enumerator.Current.english);

            return sentence;
        }
    }
}
