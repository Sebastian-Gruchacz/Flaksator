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
        {
            this.groupNumber = groupNumber;
        }

        private int groupNumber;

        public int GroupNumber
        {
            get { return groupNumber; }
        }

        private List<string> codes = new List<string>();
        private List<int> indexes = new List<int>();

        public void AddCode(int indexInString, string code)
        {
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

            if (part == GrammaticalPart.Noun)
            {
                if (nounIndex >= 0)
                    throw new ArgumentException("Only one Noun is possible in group", "code");

                nounIndex = codes.Count;
            }

            codes.Add(code);
            indexes.Add(indexInString);
        }

        private int nounIndex = -1;

        public bool Analyze()
        {
            results.Clear();

            // get noun...
            if (nounIndex < 0)
            {
                // cannot analyze groups without noun...
                return false;
            }

            InflectionCase naCase = EnumHelper.GetWordCase(codes[nounIndex][1]);
            DecliantionNumber namount = EnumHelper.GetWordAmount(codes[nounIndex][2]);
            GrammaticalGender ngenre = EnumHelper.GetWordGenre(codes[nounIndex][3]);
            // todo: categories

            if (naCase == InflectionCase._Unknown || namount == DecliantionNumber._Unknown ||
                    ngenre == GrammaticalGender._Unknown)
            {
                results.Add(indexes[nounIndex], "{NOUN_DEFINITION_INCOMPLETE}");
                return false;
            }

            Noun noun = NounCollection.Collection.GetRandomNoun(ngenre);
            if (noun == null)
            {
                results.Add(indexes[nounIndex], "{NO_VALID_NOUN}");
                // TODO: fill rest of the placeholders
                return false;
            }
            else
            {
                if (namount == DecliantionNumber.Plural && !noun.CanBePlural)
                    namount = DecliantionNumber.Singular;
                else if (namount == DecliantionNumber.Singular && !noun.CanBeSingular)
                    namount = DecliantionNumber.Plural;

                string nounStr =  NounDecliner.Decliner.MakeWord(noun, naCase, namount);
                if (!string.IsNullOrEmpty(nounStr))
                    results.Add(indexes[nounIndex], nounStr);
                else
                    results.Add(indexes[nounIndex], "{CANT_DECLINE_NOUN}");

                bool error = false;

                for (int codeIndex = 0; codeIndex < codes.Count; codeIndex++)
                {
                    if (codeIndex == nounIndex)
                        continue;

                    string code = codes[codeIndex];
                    int formatIndex = indexes[codeIndex];

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
                                        genre = ngenre;

                                    Adjective adj = AdjectiveCollection.Collection.GetRandomAdjective();
                                    if (adj == null)
                                    {
                                        results.Add(formatIndex, "{NO_VALID_ADJECTIVE}");
                                        error = true;
                                        continue;
                                    }

                                    string form = AdjectiveDecliner.Decliner.MakeWord(adj, noun, aCase,
                                        amount, level);

                                    if (form != null)
                                    {
                                        results.Add(formatIndex, form);
                                        continue;
                                    }

                                    int tries = 0;
                                    // try randomizing little more...
                                    // TODO: implement some shuffle algorithm
                                    while (form == null && tries++ < 10)
                                    {
                                        adj = AdjectiveCollection.Collection.GetRandomAdjective();
                                        form = AdjectiveDecliner.Decliner.MakeWord(adj, noun, aCase,
                                            amount, level);
                                    }
                                    if (tries >= 10)
                                    {
                                        results.Add(formatIndex, "{NO_VALID_ADJECTIVE}");
                                        error = true;
                                        continue;
                                    }

                                    results.Add(formatIndex, form);
                                }
                                break;
                            }
                        #endregion

                            // TODO: Verbs
                    }

                }

                return !error;
            }

            return true;
        }

        private Dictionary<int, string> results = new Dictionary<int,string>();
        public Dictionary<int, string> Results
        {
            get { return results; }
        }

    }
}
