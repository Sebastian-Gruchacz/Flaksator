using System;
using System.Collections.Generic;
using System.Text;

using SharpDevs.Fleksator;
using SharpDevs.Fleksator.Grammar;

namespace Flaksator
{
    class GroupInfo
    {
        public GroupInfo(int groupNumber)
            : this(groupNumber.ToString())
        {
            this.groupNumber = groupNumber;
        }

        public GroupInfo(string groupName)
        {
            this.groupName = groupName;
            int parsedNumber;
            this.groupNumber = int.TryParse(groupName, out parsedNumber) ? parsedNumber : -1;
        }

        private int groupNumber;

        public int GroupNumber
        {
            get { return this.groupNumber; }
        }

        private string groupName;

        public string GroupName
        {
            get { return this.groupName; }
        }

        private List<string> codes = new List<string>();
        private List<int> indexes = new List<int>();
        private Dictionary<int, string> definitionErrors = new Dictionary<int, string>();
        private Dictionary<int, List<int>> categoryIds = new Dictionary<int, List<int>>();

        public void AddCode(int indexInString, string code)
        {
            string grammarCode;
            List<int> categories;
            if (!WordFormatterParser.TryParse(code, out grammarCode, out categories))
            {
                this.AddInvalidCode(indexInString, code, "{INVALID_GROUP_FORMATTER}");
                return;
            }

            code = grammarCode;

            if (code == "R")
            {
                if (this.reflexiveIndex >= 0)
                {
                    this.AddInvalidCode(indexInString, code, "{GROUP_HAS_MULTIPLE_REFLEXIVE_PARTICLES}");
                    return;
                }

                this.reflexiveIndex = this.codes.Count;
                this.codes.Add(code);
                this.indexes.Add(indexInString);
                this.categoryIds[indexInString] = categories;
                return;
            }

            // now can have only Noun and several adjectives and nouns describing it
            GrammaticalPart part = GrammaticalPart._Unknown;

            switch (code[0])
            {
                case 'A': 
                    part = GrammaticalPart.Adjective;
                    break;
                case 'N':
                    part = GrammaticalPart.Noun;
                    break;
                case 'V':
                    part = GrammaticalPart.Verb;
                    this.verbCount++;
                    break;
                case 'P':
                    part = GrammaticalPart.Pronoun;
                    break;
            }

            if (part == GrammaticalPart._Unknown)
            {
                this.AddInvalidCode(indexInString, code, "{INVALID_GROUP_FORMATTER}");
                return;
            }

            if ((part == GrammaticalPart.Noun && code.Length != 4) ||
                (part == GrammaticalPart.Adjective && code.Length != 5) ||
                (part == GrammaticalPart.Verb && code.Length != 4 && code.Length != 5) ||
                (part == GrammaticalPart.Pronoun && code.Length != 5 && code.Length != 6))
            {
                this.AddInvalidCode(indexInString, code, "{INVALID_GROUP_FORMATTER}");
                return;
            }

            if (part == GrammaticalPart.Noun)
            {
                if (this.nounIndex >= 0)
                {
                    this.AddInvalidCode(indexInString, code, "{GROUP_HAS_MULTIPLE_NOUNS}");
                    return;
                }

                this.nounIndex = this.codes.Count;
            }

            this.codes.Add(code);
            this.indexes.Add(indexInString);
            this.categoryIds[indexInString] = categories;
        }

        private void AddInvalidCode(int indexInString, string code, string error)
        {
            this.codes.Add(code);
            this.indexes.Add(indexInString);
            this.definitionErrors[indexInString] = error;
        }

        private int nounIndex = -1;
        private int reflexiveIndex = -1;
        private int verbCount;

        public bool Analyze()
        {
            this.results.Clear();
            foreach (KeyValuePair<int, string> definitionError in this.definitionErrors)
                this.results[definitionError.Key] = definitionError.Value;

            string reflexiveDefinitionError = null;
            if (this.reflexiveIndex >= 0 && this.verbCount != 1)
            {
                reflexiveDefinitionError = this.verbCount == 0
                    ? "{GROUP_REFLEXIVE_REQUIRES_VERB}"
                    : "{GROUP_REFLEXIVE_REQUIRES_SINGLE_VERB}";
                this.results[this.indexes[this.reflexiveIndex]] = reflexiveDefinitionError;
            }

            // get noun...
            if (this.nounIndex < 0)
            {
                // cannot analyze groups without noun...
                this.FillMissingResults("{GROUP_REQUIRES_NOUN}");
                return false;
            }

            InflectionCase naCase = EnumHelper.GetWordCase(this.codes[this.nounIndex][1]);
            DecliantionNumber namount = EnumHelper.GetWordAmount(this.codes[this.nounIndex][2]);
            GrammaticalGender ngenre = EnumHelper.GetWordGenre(this.codes[this.nounIndex][3]);
            // todo: categories

            if (naCase == InflectionCase._Unknown || namount == DecliantionNumber._Unknown ||
                    ngenre == GrammaticalGender._Unknown)
            {
                this.results.Add(this.indexes[this.nounIndex], "{NOUN_DEFINITION_INCOMPLETE}");
                this.FillMissingResults("{GROUP_NOUN_INVALID}");
                return false;
            }

            Noun noun = NounCollection.Collection.GetNounEx(
                ngenre,
                this.categoryIds[this.indexes[this.nounIndex]],
                namount == DecliantionNumber.Singular,
                namount == DecliantionNumber.Plural);
            if (noun == null)
            {
                this.results.Add(this.indexes[this.nounIndex], "{NO_VALID_NOUN}");
                this.FillMissingResults("{GROUP_NOUN_UNAVAILABLE}");
                return false;
            }
            else
            {
                string nounStr =  NounDecliner.Decliner.MakeWord(noun, naCase, namount);
                bool error = this.definitionErrors.Count > 0 || reflexiveDefinitionError != null;
                if (!string.IsNullOrEmpty(nounStr))
                    this.results.Add(this.indexes[this.nounIndex], nounStr);
                else
                {
                    this.results.Add(this.indexes[this.nounIndex], "{CANT_DECLINE_NOUN}");
                    error = true;
                }

                GrammaticalGender agreementGender = noun.HasIrregularGenre
                    ? noun.IrregularGenre
                    : noun.Genre;

                for (int codeIndex = 0; codeIndex < this.codes.Count; codeIndex++)
                {
                    if (codeIndex == this.nounIndex)
                        continue;

                    int formatIndex = this.indexes[codeIndex];
                    if (this.definitionErrors.ContainsKey(formatIndex))
                    {
                        error = true;
                        continue;
                    }

                    string code = this.codes[codeIndex];

                    switch (code[0])
                    {
                        #region Adjective
                        case 'A':   // Adjective (przymiotnik)
                            {
                                if (code.Length == 5)
                                {
                                    InflectionCase aCase = EnumHelper.GetWordCase(code[1]);
                                    DecliantionNumber amount = EnumHelper.GetWordAmount(code[2]);
                                    GrammaticalGender genre = EnumHelper.GetWordGenre(code[3]);
                                    AdjectiveLevel level = EnumHelper.GetAdjectiveLevel(code[4]);

                                    if (aCase == InflectionCase._Unknown)
                                        aCase = naCase;

                                    if (amount == DecliantionNumber._Unknown)
                                        amount = namount;

                                    if (genre == GrammaticalGender._Unknown)
                                        genre = agreementGender;

                                    if (aCase == InflectionCase._Unknown ||
                                        amount == DecliantionNumber._Unknown ||
                                        genre == GrammaticalGender._Unknown ||
                                        level == AdjectiveLevel._Unknown)
                                    {
                                        this.results.Add(formatIndex, "{ADJECTIVE_DEFINITION_INCOMPLETE}");
                                        error = true;
                                        continue;
                                    }

                                    Adjective adj = AdjectiveCollection.Collection.GetAdjective(candidate =>
                                        WordFormatterParser.MatchesCategories(candidate.Categories,
                                            this.categoryIds[formatIndex]) &&
                                        AdjectiveDecliner.Decliner.MakeWord(candidate, genre, aCase,
                                            amount, level) != null);
                                    if (adj == null)
                                    {
                                        this.results.Add(formatIndex, "{NO_VALID_ADJECTIVE}");
                                        error = true;
                                        continue;
                                    }

                                    string form = AdjectiveDecliner.Decliner.MakeWord(adj, genre, aCase,
                                        amount, level);
                                    this.results.Add(formatIndex, form);
                                }
                                else
                                {
                                    this.results[formatIndex] = "{INVALID_GROUP_FORMATTER}";
                                    error = true;
                                }
                                break;
                            }
                        #endregion

                        case 'V':   // Verb (czasownik)
                            {
                                if (reflexiveDefinitionError != null)
                                {
                                    this.results.Add(formatIndex, "{GROUP_REFLEXIVE_AMBIGUOUS}");
                                    error = true;
                                    continue;
                                }

                                VerbTense tense = VerbGrammar.GetTense(code[1]);
                                GrammaticalPerson person = VerbGrammar.GetPerson(code[2]);
                                DecliantionNumber amount = EnumHelper.GetWordAmount(code[3]);
                                GrammaticalGender gender = code.Length == 5
                                    ? EnumHelper.GetWordGenre(code[4])
                                    : GrammaticalGender._Unknown;

                                if (person == GrammaticalPerson._Unknown)
                                    person = GrammaticalPerson.Third;
                                if (amount == DecliantionNumber._Unknown)
                                    amount = namount;
                                if (gender == GrammaticalGender._Unknown)
                                    gender = agreementGender;

                                if (tense == VerbTense._Unknown || amount == DecliantionNumber._Unknown ||
                                    (tense == VerbTense.Past && gender == GrammaticalGender._Unknown))
                                {
                                    this.results.Add(formatIndex, "{VERB_DEFINITION_INCOMPLETE}");
                                    error = true;
                                    continue;
                                }

                                Verb verb = VerbCollection.Collection.GetVerb(candidate =>
                                    candidate.CanBeActive &&
                                    candidate.IsReflexive == (this.reflexiveIndex >= 0) &&
                                    WordFormatterParser.MatchesCategories(candidate.Categories,
                                        this.categoryIds[formatIndex]) &&
                                    VerbConjurer.Conjurer.MakeWord(candidate, tense, person, amount,
                                        gender) != null);
                                if (verb == null)
                                {
                                    this.results.Add(formatIndex, "{NO_VALID_VERB}");
                                    error = true;
                                    continue;
                                }

                                string form = VerbConjurer.Conjurer.MakeWord(verb, tense, person, amount,
                                    gender);
                                this.results.Add(formatIndex, form);
                                break;
                            }
                        case 'R':   // Reflexive particle (partykuła zwrotna)
                            {
                                if (!this.results.ContainsKey(formatIndex))
                                    this.results.Add(formatIndex, "się");
                                break;
                            }
                        case 'P':   // Pronoun (zaimek)
                            {
                                if (code.Length == 6 && code[5] != 'P')
                                {
                                    this.results.Add(formatIndex, "{INVALID_GROUP_FORMATTER}");
                                    error = true;
                                    continue;
                                }

                                InflectionCase aCase = EnumHelper.GetWordCase(code[1]);
                                GrammaticalPerson person = VerbGrammar.GetPerson(code[2]);
                                DecliantionNumber amount = EnumHelper.GetWordAmount(code[3]);
                                GrammaticalGender gender = EnumHelper.GetWordGenre(code[4]);
                                PronounVariant variant = code.Length == 6
                                    ? PronounVariant.Prepositional
                                    : PronounVariant.Neutral;

                                if (person == GrammaticalPerson._Unknown)
                                    person = GrammaticalPerson.Third;
                                if (amount == DecliantionNumber._Unknown)
                                    amount = namount;
                                if (gender == GrammaticalGender._Unknown)
                                    gender = agreementGender;

                                if (aCase == InflectionCase._Unknown ||
                                    amount == DecliantionNumber._Unknown ||
                                    gender == GrammaticalGender._Unknown)
                                {
                                    this.results.Add(formatIndex, "{PRONOUN_DEFINITION_INCOMPLETE}");
                                    error = true;
                                    continue;
                                }

                                Pronoun pronoun = PronounCollection.Collection.GetPronoun(candidate =>
                                    PronounDecliner.Decliner.MakeWord(candidate, aCase, person, amount,
                                        gender, variant) != null);
                                if (pronoun == null)
                                {
                                    this.results.Add(formatIndex, "{NO_VALID_PRONOUN}");
                                    error = true;
                                    continue;
                                }

                                string form = PronounDecliner.Decliner.MakeWord(pronoun, aCase, person,
                                    amount, gender, variant);
                                this.results.Add(formatIndex, form);
                                break;
                            }
                    }

                }

                this.FillMissingResults("{INVALID_GROUP_FORMATTER}");
                return !error && this.results.Count == this.indexes.Count;
            }
        }

        private void FillMissingResults(string error)
        {
            foreach (int index in this.indexes)
                if (!this.results.ContainsKey(index))
                    this.results[index] = error;
        }

        private Dictionary<int, string> results = new Dictionary<int,string>();
        public Dictionary<int, string> Results
        {
            get { return this.results; }
        }

    }
}
