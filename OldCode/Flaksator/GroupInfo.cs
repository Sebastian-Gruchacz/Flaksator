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
                    break;
            }

            if (part == GrammaticalPart._Unknown)
            {
                this.AddInvalidCode(indexInString, code, "{INVALID_GROUP_FORMATTER}");
                return;
            }

            if ((part == GrammaticalPart.Noun && code.Length != 4) ||
                (part == GrammaticalPart.Adjective && code.Length != 5) ||
                (part == GrammaticalPart.Verb && code.Length != 4))
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

        public bool Analyze()
        {
            this.results.Clear();
            foreach (KeyValuePair<int, string> definitionError in this.definitionErrors)
                this.results[definitionError.Key] = definitionError.Value;

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
                bool error = this.definitionErrors.Count > 0;
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
                                VerbTense tense = VerbGrammar.GetTense(code[1]);
                                GrammaticalPerson person = VerbGrammar.GetPerson(code[2]);
                                DecliantionNumber amount = EnumHelper.GetWordAmount(code[3]);

                                if (person == GrammaticalPerson._Unknown)
                                    person = GrammaticalPerson.Third;
                                if (amount == DecliantionNumber._Unknown)
                                    amount = namount;

                                if (tense == VerbTense._Unknown || amount == DecliantionNumber._Unknown)
                                {
                                    this.results.Add(formatIndex, "{VERB_DEFINITION_INCOMPLETE}");
                                    error = true;
                                    continue;
                                }

                                Verb verb = VerbCollection.Collection.GetVerb(candidate =>
                                    VerbConjurer.Conjurer.MakeWord(candidate, tense, person, amount,
                                        agreementGender) != null);
                                if (verb == null)
                                {
                                    this.results.Add(formatIndex, "{NO_VALID_VERB}");
                                    error = true;
                                    continue;
                                }

                                string form = VerbConjurer.Conjurer.MakeWord(verb, tense, person, amount,
                                    agreementGender);
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
