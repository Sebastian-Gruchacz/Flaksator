using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDevs.Fleksator
{
    public class Adjective : DeclinativeWord
    {
        private bool isLevelledComplex = true;

        public bool IsLevelledComplex
        {
            get { return isLevelledComplex; }
            set { isLevelledComplex = value; }
        }

        private string levelHigerForm = null;

        public string LevelHigherForm
        {
            get { return levelHigerForm; }
            set { levelHigerForm = value; }
        }

        private string levelHigestForm = null;

        public string LevelHighestForm
        {
            get { return levelHigestForm; }
            set { levelHigestForm = value; }
        }

        private bool canBeLevelled = true;
        /// <summary>
        /// Specifies whether this Adj. shall be used in Higher on Highest levels (no mather of the reason)
        /// <remarks>It's added because some adj. sound stupid or mean nothing sense, so it will disable such things.</remarks>
        /// </summary>
        public bool CanBeLevelled
        {
            get { return canBeLevelled; }
            set { canBeLevelled = value; }
        }


        private List<AdjectiveWordToken> irregulars = new List<AdjectiveWordToken>();

        public List<AdjectiveWordToken> Irregulars
        {
            get { return irregulars; }
        }

        private List<int> categories = new List<int>();
        public List<int> Categories
        {
            get { return categories; }
        }


        public string GetForm(WordGenre genre, WordCase aCase, WordAmount amount,
            AdjectiveLevel level)
        {
            AdjectiveWordToken token = new AdjectiveWordToken(null, aCase, genre, amount, level);
            foreach (AdjectiveWordToken tok in irregulars)
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
                                    this.canBeLevelled = false;

                                    break;
                                }

                            #endregion

                            #region Exception Cases

                            case '%':
                                {
                                    if (str.Length > 5)
                                    {
                                        this.IsException = true;
                                        WordCase aCase = EnumHelper.GetWordCase(str[1]);
                                        WordAmount amount = EnumHelper.GetWordAmount(str[2]);
                                        WordGenre genre = EnumHelper.GetWordGenre(str[3]);
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
                                    categories.Clear();
                                    if (!string.IsNullOrEmpty(cats))
                                    {
                                        string[] arr = cats.Split(',');
                                        foreach (string catId in arr)
                                        {
                                            int id = int.Parse(catId);

                                            if (!categories.Contains(id))
                                                categories.Add(id);
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
