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
                if (!enumerator.MoveNext())
                    throw new UnexpectedEnd();
                adj.right = GetNextNounPhrase(enumerator);
                output = adj;
            }
            else
            {
                throw new UnexpectedWord(enumerator.Current.english);
            }
            if (enumerator.Current.pos == PartOfSpeech.Conjunction)
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
            VerbPhrase output;
            var toBe = false;
            var sentenceObject = false;
            if (enumerator.Current.IsVerb)
            {
                var result = new Verb { word = enumerator.Current };
                var matches = from w in Dictionary.LookupEn(enumerator.Current.english)
                              where w.person == person.person && w.count == person.count
                              select w;
                if (matches.Count() == 1)
                {
                    result.word = matches.SingleOrDefault();
                }
                else if (matches.Count() > 1)
                {
                    // TODO: ?
                }
                else
                {
                    switch (person.person)
                    {
                        case Person.First:
                            result.word.persian_verb_identifier = person.count == PersonCount.Singular ? "م" : "یم";
                            break;
                        case Person.Second:
                            result.word.persian_verb_identifier = person.count == PersonCount.Singular ? "ی" : "ید";
                            break;
                        case Person.Third:
                            result.word.persian_verb_identifier = person.count == PersonCount.Singular ?
                                result.word.tense == VerbTense.Present ? "د" : ""
                                : "ند";
                            break;
                    }
                }
                
                if (enumerator.Current.pos == PartOfSpeech.ToBe)
                    toBe = true;

                // TODO: After some verbs, sentence comes,
                //  like 'said', 'think', ...
                //  Ali said "This is great"
                //  I think he is clever

                output = result;
                if (!enumerator.MoveNext())
                    return output;
            }
            else
            {
                throw new UnexpectedWord(enumerator.Current.english);
            }
            if (toBe)
            {
                if (enumerator.Current.pos == PartOfSpeech.Adjective ||
                    enumerator.Current.IsNoun)
                {
                    output = new SubjectiveVerb { toBe = output, status = enumerator.Current };
                    if (!enumerator.MoveNext())
                        return output;
                }
                // TODO: tenses uses 'to be + verb'
                else
                {
                    throw new UnexpectedWord(enumerator.Current.english);
                }
            }
            else if (enumerator.Current.IsNoun)
            {
                if (sentenceObject)
                {
                    // TODO: parse next sentence as object
                    throw new NotImplementedException();
                }
                else
                {
                    output = new ObjectiveVerb { action = output, objectNoun = GetNextNounPhrase(enumerator) };
                }
            }
            else if (enumerator.Current.pos == PartOfSpeech.Preposition)
            {
                output = new PhrasalVerb { action = output, preposition = enumerator.Current };
                if (!enumerator.MoveNext())
                    return output;
            }
            return output;
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
            var sentence = new SimpleSentence();
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
