using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.IO.Implementation
{
    internal class OldNounSerializer : IGrammaticalWordSerializer<Noun>
    {
        public Noun Load(string line)
        {
            if (string.IsNullOrEmpty(line)) return null;

            Noun noun = new Noun();

            string[] elements = line.Split('|');
            noun.Root = elements[0]; // always will be at last one element in nonempty string

            if (elements.Length > 1)
            {
                for (int i = 1; i < elements.Length; i++)
                {
                    string str = elements[i];
                    if (string.IsNullOrEmpty(str))
                        continue;

                    switch (str[0])
                    {
                            #region Nondeclinative item

                        case '#':
                        {
                            noun.IsConstant = true;
                            break;
                        }

                            #endregion

                            #region Main Data

                        case '!':
                        {
                            if (str.Length > 1)
                            {
                                noun.Genre = EnumHelper.GetWordGenre(str[1]);
                                // TODO: read more details
                                if (str.Length > 3)
                                {
                                    noun.DeclinationType = str.Substring(2, 2);
                                }
                            }
                            else
                            {
                                // err
                                return null;
                            }

                            break;
                        }

                        case '*':
                        {
                            if (str.Length > 1)
                            {
                                noun.IrregularGenre = EnumHelper.GetWordGenre(str[1]);
                                // TODO: read more details
                                noun.HasIrregularGenre = noun.IrregularGenre != GrammaticalGender._Unknown;
                            }
                            else
                            {
                                // err
                                return null;
                            }

                            break;
                        }


                        case '@':
                        {
                            if (str.Length > 1)
                            {
                                // take root for other cases then Nominative
                                noun.RootOther = str.Substring(1);
                            }
                            else
                            {
                                // err
                                return null;
                            }

                            break;
                        }
                        case '+':
                        {
                            if (str.Length > 2)
                            {
                                // Specifies other than first index is used
                                DecliantionNumber amount = EnumHelper.GetWordAmount(str[1]);
                                InflectionCase aCase = EnumHelper.GetWordCase(str[2]);
                                int postFixIndex = int.Parse(str.Substring(3));

                                switch (amount)
                                {
                                    case DecliantionNumber.Singular:
                                    {
                                        if (!noun.SingularPostfixSelector.ContainsKey(aCase))
                                            noun.SingularPostfixSelector.Add(aCase, postFixIndex);
                                        else
                                            noun.SingularPostfixSelector[aCase] = postFixIndex;
                                        break;
                                    }
                                    case DecliantionNumber.Plural:
                                    {
                                        if (!noun.PluralPostfixSelector.ContainsKey(aCase))
                                            noun.PluralPostfixSelector.Add(aCase, postFixIndex);
                                        else
                                            noun.PluralPostfixSelector[aCase] = postFixIndex;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                // err
                                return null;
                            }

                            break;
                        }

                            #endregion

                            #region Exception Cases

                        case '%':
                        {
                            if (str.Length > 3)
                            {
                                noun.IsException = true;
                                InflectionCase aCase = EnumHelper.GetWordCase(str[1]);
                                DecliantionNumber amount = EnumHelper.GetWordAmount(str[2]);
                                string txt = str.Substring(3);

                                WordToken token = new
                                    WordToken(txt, aCase, amount);
                                noun.Irregulars.Add(token);
                            }
                            else
                            {
                                // err
                                return null;
                            }

                            break;
                        }

                        case '^':
                        {
                            if (str.Length == 2)
                            {
                                // only singular/plural word - no sense meaning or grammar
                                DecliantionNumber amount = EnumHelper.GetWordAmount(str[1]);
                                if (amount == DecliantionNumber.Plural)
                                    noun.CanBePlural = false;
                                else if (amount == DecliantionNumber.Singular)
                                    noun.CanBeSingular = false;
                            }
                            else
                            {
                                // err
                                return null;
                            }

                            break;
                        }

                            #endregion

                            #region Categories

                        case '$':
                        {
                            string cats = str.Substring(1);
                            // noun.Categories.Clear();
                            if (!string.IsNullOrEmpty(cats))
                            {
                                string[] arr = cats.Split(',');
                                foreach (string catId in arr)
                                {
                                    int id = int.Parse(catId);

                                    if (!noun.Categories.Contains(id))
                                        noun.Categories.Add(id);
                                }

                            }

                            break;
                        }

                            #endregion
                    }
                }

            }

            // check all data is specified
            if (noun.Genre == GrammaticalGender._Unknown)
                return null;

            return noun;
        }

        public string Write(Noun word)
        {
            // write main root
            StringBuilder sb = new StringBuilder();
            sb.Append(word.Root);

            // write alternative root
            if (!string.IsNullOrEmpty(word.RootOther))
                sb.AppendFormat("|@{0}", word.RootOther);

            // Write main info
            char genre = EnumHelper.GetWordGenreCode(word.Genre);
            sb.AppendFormat("|!{0}{1}", genre, word.DeclinationType);

            // write limitations
            if (!word.CanBeSingular)
                sb.Append("|^1");
            if (!word.CanBePlural)
                sb.Append("|^2");

            // write categories
            if (word.Categories.Count > 0)
            {
                sb.Append("|$");
                for (int i = 0; i < word.Categories.Count; i++)
                {
                    if (i > 0)
                        sb.Append(',');

                    sb.Append(word.Categories[i]);
                }
            }
            // write "is const"
            if (word.IsConstant)
            {
                sb.Append("|#");
            }
            else
            {
                // write declination infos...
                foreach (InflectionCase aCase in word.SingularPostfixSelector.Keys)
                {
                    int index = word.SingularPostfixSelector[aCase];
                    if (index > 0)
                    {
                        char aCaseCode = EnumHelper.GetWordCaseCode(aCase);
                        char amountCode = EnumHelper.GetWordAmountCode(DecliantionNumber.Singular);
                        sb.AppendFormat("|+{0}{1}{2}", amountCode, aCaseCode, index.ToString());
                    }
                }

                foreach (InflectionCase aCase in word.PluralPostfixSelector.Keys)
                {
                    int index = word.PluralPostfixSelector[aCase];
                    if (index > 0)
                    {
                        char aCaseCode = EnumHelper.GetWordCaseCode(aCase);
                        char amountCode = EnumHelper.GetWordAmountCode(DecliantionNumber.Plural);
                        sb.AppendFormat("|+{0}{1}{2}", amountCode, aCaseCode, index.ToString());
                    }
                }

                // write irregular adjective connection
                if (word.HasIrregularGenre)
                    sb.AppendFormat("|*{0}", EnumHelper.GetWordGenreCode(word.IrregularGenre));

                // write irregular forms
                foreach (WordToken irToken in word.Irregulars)
                {
                    char aCase = EnumHelper.GetWordCaseCode(irToken.InflectionCase);
                    char amount = EnumHelper.GetWordAmountCode(irToken.DecliantionNumber);
                    sb.AppendFormat("|%{0}{1}{2}", aCase, amount, irToken.Text);
                }
            }

            return sb.ToString();
        }
    }
}
