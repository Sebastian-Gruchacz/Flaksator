using System;
using System.Collections.Generic;
using System.Text;

using SharpDevs.Fleksator;
using SharpDevs.Fleksator.Grammar;

namespace Flaksator
{
    class DecodeBase
    {
        protected string DecodeLine(string verse)
        {
            List<string> descriptors = new List<string>();

            string result = null;

            int bracketIndex = verse.IndexOf('{');
            int elementNumber = 0;
            int closingBracket = 0;
            if (bracketIndex >= 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(verse.Substring(closingBracket, bracketIndex - closingBracket));

                while (bracketIndex >= 0)
                {
                    closingBracket = verse.IndexOf('}', bracketIndex + 1);
                    if (closingBracket < 0)
                        return null; //syntax error

                    // try finding valid word
                    string code = verse.Substring(bracketIndex + 1, closingBracket - bracketIndex - 1);
                    
                    descriptors.Add(code);
                    sb.Append("{" + elementNumber + "}");
                    elementNumber++;

                    bracketIndex = verse.IndexOf('{', closingBracket + 1);

                    if (bracketIndex >= 0)
                        sb.Append(verse.Substring(closingBracket + 1, bracketIndex - closingBracket - 1));
                    else
                        sb.Append(verse.Substring(closingBracket + 1));
                }

                //syntax error (note that there is no way to display brackets in text this way
                result = sb.ToString();
            }
            else
            {
                return verse; // no placeholders
            }

            // now decoding...
            string[] results = new string[descriptors.Count];
            Dictionary<int, GroupInfo> groups = new Dictionary<int, GroupInfo>();

            for (int i = 0; i < descriptors.Count; i++)
            {
                string code = descriptors[i];

                // check group
                int index = code.IndexOf(':');
                if (index >= 0)
                {
                    // this is group
                    string groupStr = code.Substring(0, index);
                    int groupNumber = -1;
                    if (!int.TryParse(groupStr, out groupNumber))
                    {
                        results[i] = "{INVALID_GROUP_FORMATTER}";
                    }
                    else
                    {
                        GroupInfo info = null;
                        if (groups.ContainsKey(groupNumber))
                            info = groups[groupNumber];
                        else
                        {
                            info = new GroupInfo(groupNumber);
                            groups.Add(groupNumber, info);
                        }

                        info.AddCode(i, code.Substring(index + 1));
                    }
                }
                else
                {
                    // this is normal word
                    results[i] = this.Decode(code);
                }
            }

            // decode groups
            foreach (GroupInfo group in groups.Values)
            {
                bool rzlt = group.Analyze();
                foreach (int ind in group.Results.Keys)
                {
                    results[ind] = group.Results[ind];
                }

            }
            
            return string.Format(result, results);
        }

        private string Decode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                switch (code[0])
                {
                    case 'A':   // Adjective (przymiotnik)
                        {
                            if (code.Length == 5)
                            {
                                InflectionCase aCase = EnumHelper.GetWordCase(code[1]);
                                DecliantionNumber amount = EnumHelper.GetWordAmount(code[2]);
                                GrammaticalGender genre = EnumHelper.GetWordGenre(code[3]);
                                AdjectiveLevel level = EnumHelper.GetAdjectiveLevel(code[4]);

                                Adjective adj = AdjectiveCollection.Collection.GetRandomAdjective();
                                if (adj == null)
                                    return "{NO_VALID_ADJECTIVE}";

                                string form = AdjectiveDecliner.Decliner.MakeWord(adj, genre, aCase, amount, level);
                                if (form != null)
                                    return form;

                                int tries = 0;
                                // try randomizing little more...
                                // TODO: implement some shuffle algorithm
                                while (form == null && tries++ < 10)
                                {
                                    adj = AdjectiveCollection.Collection.GetRandomAdjective();
                                    form = AdjectiveDecliner.Decliner.MakeWord(adj, genre, aCase, amount, level);
                                }
                                if (tries >= 10)
                                    return "{NO_VALID_ADJECTIVE}"; // 10 tries - probably no valid word in dic

                                return form;
                            }
                            break;
                        }
                    case 'N': // Noun (rzeczownik)
                        {
                            if (code.Length == 4)
                            {
                                InflectionCase aCase = EnumHelper.GetWordCase(code[1]);
                                DecliantionNumber amount = EnumHelper.GetWordAmount(code[2]);
                                GrammaticalGender genre = EnumHelper.GetWordGenre(code[3]);

                                Noun noun = NounCollection.Collection.GetNoun(genre,
                                    null,
                                    (amount == DecliantionNumber.Singular) ? true : false,
                                    (amount == DecliantionNumber.Plural) ? true : false);
                                if (noun == null)
                                    return "{NO_VALID_NOUN}";

                                /*if (amount == DecliantionNumber.Plural && !noun.CanBePlural)
                                    amount = DecliantionNumber.Singular;
                                else if (amount == DecliantionNumber.Singular && !noun.CanBeSingular)
                                    amount = DecliantionNumber.Plural;*/ // no longer needed

                                string form = NounDecliner.Decliner.MakeWord(noun, aCase, amount);
                                if (form != null)
                                    return form;

                                int tries = 0;
                                // try randomizing little more...
                                // TODO: implement some shuffle algorithm
                                while (form == null && tries++ < 10)
                                {
                                    noun = NounCollection.Collection.GetNoun(genre,
                                    null,
                                    (amount == DecliantionNumber.Singular) ? true : false,
                                    (amount == DecliantionNumber.Plural) ? true : false);

                                    form = NounDecliner.Decliner.MakeWord(noun, aCase, amount);
                                }
                                if (tries >= 10)
                                    return "{NO_VALID_NOUN}"; // 10 tries - probably no valid word in dic

                                return form;

                            }

                            break;
                        }
                    //case 'C': // Counter (liczebnik)
                    //    {
                    //        break;
                    //    }
                    //case 'V': // Verb (czsownik)
                    //    {
                    //        break;
                    //    }
                }
            }

            return "{INVALID_FORMATTER}";
        }
    }
}
