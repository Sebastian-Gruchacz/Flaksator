using System.Collections.Generic;

namespace SharpDevs.Fleksator.Grammar
{
    public class Adjective : DeflectionableGrammaticalWord
    {
        public bool IsLevelledComplex { get; set; } = true;

        public string LevelHigherForm { get; set; } = null;

        public string LevelHighestForm { get; set; } = null;

        /// <summary>
        /// Specifies whether this Adj. shall be used in Higher on Highest levels (no mather of the reason)
        /// <remarks>It's added because some adj. sound stupid or mean nothing sense, so it will disable such things.</remarks>
        /// </summary>
        public bool CanBeLevelled { get; set; } = true;


        public List<AdjectiveWordToken> Irregulars { get; } = new List<AdjectiveWordToken>();

        public List<int> Categories { get; } = new List<int>();


        public string GetForm(GrammaticalGender genre, InflectionCase aCase, DecliantionNumber amount, AdjectiveLevel level)
        {
            AdjectiveWordToken token = new AdjectiveWordToken(null, aCase, genre, amount, level);
            foreach (AdjectiveWordToken tok in Irregulars)
                if (tok.Is(token))
                    return tok.Text;

            return null;
        }

        public bool AnalyzeLine(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                string[] elements = line.Split('|');

                this.Root = elements[0]; // always will be at last one element in nonempty string

                if (elements.Length > 1)
                {
                    for (int i = 1; i < elements.Length; i++)
                    {
                        string str = elements[i];
                        if (string.IsNullOrEmpty(str))
                            continue;

                        switch (str[0])
                        {
                                #region Irregular levelling

                            case '*':
                            {
                                if (str.Length > 1)
                                {
                                    this.IsLevelledComplex = false;

                                    if (str[1] == '+')
                                        this.LevelHighestForm = str.Substring(2);
                                    else
                                        this.LevelHigherForm = str.Substring(1);
                                }
                                else
                                {
                                    // err
                                    return false;
                                }

                                break;
                            }

                                #endregion

                                #region Nondeclinative item

                            case '#':
                            {
                                this.IsConstant = true;

                                break;
                            }

                            case '!':
                            {
                                this.CanBeLevelled = false;

                                break;
                            }

                                #endregion

                                #region Exception Cases

                            case '%':
                            {
                                if (str.Length > 5)
                                {
                                    this.IsException = true;
                                    InflectionCase aCase = EnumHelper.GetWordCase(str[1]);
                                    DecliantionNumber amount = EnumHelper.GetWordAmount(str[2]);
                                    GrammaticalGender genre = EnumHelper.GetWordGenre(str[3]);
                                    AdjectiveLevel level = EnumHelper.GetAdjectiveLevel(str[4]);
                                    string txt = str.Substring(5);

                                    AdjectiveWordToken token = new
                                        AdjectiveWordToken(txt, aCase, genre, amount, level);
                                    this.Irregulars.Add(token);
                                }
                                else
                                {
                                    // err
                                    return false;
                                }

                                break;
                            }

                                #endregion

                                #region Categories

                            case '$':
                            {
                                string cats = str.Substring(1);
                                Categories.Clear();
                                if (!string.IsNullOrEmpty(cats))
                                {
                                    string[] arr = cats.Split(',');
                                    foreach (string catId in arr)
                                    {
                                        int id = int.Parse(catId);

                                        if (!Categories.Contains(id))
                                            Categories.Add(id);
                                    }

                                }

                                break;
                            }

                                #endregion
                        }
                    }
                }
                else
                {
                    // set defaults
                    this.IsException = false;
                    this.IsConstant = false;
                }

                //if (adj.IsException && adj.IsLevelledComplex)
                //{
                //    // not supported
                //}
                //else

                return true;
            }

            return false;
        }
    }
}
