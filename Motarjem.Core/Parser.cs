using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO: Parse by location, not POS

namespace Motarjem.Core
{
    public static class Parser
    {
        public static IEnumerable<Sentence> ParseEnglish(IEnumerable<Token> tokens_enum)
        {
            var tokens = new Queue<Token>(tokens_enum);
            var words = new Queue<Word>();
            while (tokens.Any())
            {
                switch (tokens.Peek().type)
                {
                    case Token.Type.Dot:
                        tokens.Dequeue();
                        yield return GetNextSentence(words);
                        words.Clear();
                        break;
                    case Token.Type.QuestionMark:
                        tokens.Dequeue();
                        throw new NotImplementedException();
                    case Token.Type.Exclamation:
                        tokens.Dequeue();
                        throw new NotImplementedException();
                    case Token.Type.Space:
                        tokens.Dequeue();
                        break;
                    default:
                        {
                            var query = GetNextWord(tokens);
                            var matches = Dictionary.LookupEn(query);
                            if (!matches.Any())
                                throw new UndefinedWord(query);
                            words.Enqueue(matches.First());
                            break;
                        }
                }
            }
        }

        private static string GetNextWord(Queue<Token> tokens)
        {
            var str = new StringBuilder();
            while (tokens.Any())
            {
                if (tokens.Peek().type == Token.Type.Alphabet ||
                    tokens.Peek().type == Token.Type.Digit)
                    str.Append(tokens.Dequeue().charactor);
                else
                {
                    if (tokens.Peek().type == Token.Type.Space)
                        tokens.Dequeue();
                    break;
                }
            }
            return str.ToString();
        }

        private static Sentence GetNextSentence(Queue<Word> words)
        {
            // TODO: if + S1 + ',' + S2
            // Simple Sentence: Noun Phrase + Verb Phrase
            var sentence = new SimpleSentence { language = Language.English };
            sentence.np = GetNextNounPhrase(words);
            sentence.vp = GetNextVerbPhrase(words, FindSubject(sentence.np));
            // Conjunction Sentence: S1 + and + S2
            if (words.Any() && words.Peek().pos == PartOfSpeech.Conjunction)
            {
                var conj = new ConjSentence
                {
                    left = sentence,
                    conj = words.Dequeue(),
                };
                conj.right = GetNextSentence(words);
                return conj;
            }
            if (words.Any())
                throw new UnexpectedWord(words.Dequeue().english);
            return sentence;
        }

        private static NounPhrase GetNextNounPhrase(Queue<Word> words, bool child = false)
        {
            NounPhrase result;
            switch (words.Peek().pos)
            {
                // Det. + Noun e.g. the book
                case PartOfSpeech.Determiner:
                    {
                        var det = new DeterminerNoun
                        {
                            determiner = words.Dequeue(),
                        };
                        det.right = GetNextNounPhrase(words, true);
                        result = det;
                        break;
                    }
                // Adjective + Noun e.g. small book
                case PartOfSpeech.Adjective:
                    {
                        var adj = new AdjectiveNoun { adjective = words.Dequeue() };
                        if (words.Any()
                            && words.Peek().IsNoun)
                            adj.right = GetNextNounPhrase(words, true);
                        result = adj;
                        break;
                    }
                case PartOfSpeech.Pronoun:
                case PartOfSpeech.Noun:
                case PartOfSpeech.ProperNoun:
                    result = new Noun
                    {
                        word = words.Dequeue()
                    };
                    break;
                default:
                    throw new UnexpectedWord(words.Dequeue().english);
            }
            // Noun + Conj. + Noun e.g. Ali and Reza
            if (words.Any() && 
                words.Peek().pos == PartOfSpeech.Conjunction &&
                !child)
            {
                // TODO: Ambigous:
                // [Ali wrote [books and letters]] and [Reza read them].
                var conj = new ConjNoun { left = result };
                conj.conjunction = words.Dequeue();
                conj.right = GetNextNounPhrase(words, true);
                result = conj;
            }
            return result;
        }

        private static VerbPhrase GetNextVerbPhrase(Queue<Word> words, Word person)
        {
            if (words.Peek().IsVerb)
            {
                var verb = new Verb { word = words.Dequeue() };
                // Personality
                // lookup for special verbs
                var matches = from v in Dictionary.LookupVerbEn(verb.word.english)
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

                // generate Persian Verb
                if (string.IsNullOrWhiteSpace(verb.word.persian_verb_identifier))
                    verb.word.persian_verb_identifier = FindPersianIdentifier();

                string FindPersianIdentifier()
                {
                    if (person.count == PersonCount.Singular)
                    {
                        switch (person.person)
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
                    throw new GrammerError(verb.word.english);
                }

                // Object
                if (!words.Any())
                    return verb; // no more words left for object.

                if (verb.tense == VerbPhraseTense.Subjective
                    && (words.Peek().IsNoun 
                    || words.Peek().pos == PartOfSpeech.Determiner
                    || words.Peek().pos == PartOfSpeech.Adjective))
                {
                    return new SubjectiveVerb
                    {
                        toBe = verb,
                        status = GetNextNounPhrase(words)
                    };
                }
                else if (words.Peek().IsNoun 
                    || words.Peek().pos == PartOfSpeech.Determiner)
                {
                    return new ObjectiveVerb
                    {
                        action = verb,
                        objectNoun = GetNextNounPhrase(words)
                    };
                }

                return verb;
            }
            else
            {
                throw new UnexpectedWord(words.Dequeue().english);
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
                // TODO: 'a' or 'an' are singular, 'some' and ... are plural
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
    }
}
