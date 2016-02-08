using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDevs.Fleksator
{
    public class Noun : DeclinativeWord
    {
        private WordGenre genre = WordGenre._Unknown;
        /// <summary>
        /// Genre of word
        /// </summary>
        public WordGenre Genre
        {
            get { return genre; }
            set { genre = value; }
        }
        private bool isIrregularGenre = false;
        public bool HasIrregularGenre
        {
            get { return isIrregularGenre; }
            set { isIrregularGenre = value; }
        }

        private WordGenre irrGenre = WordGenre._Unknown;
        public WordGenre IrregularGenre
        {
            get { return irrGenre; }
            set { irrGenre = value; }
        }

        private string rootOther;
        /// <summary>
        /// Root for other cases then Nominative
        /// </summary>
        public string RootOther
        {
            get { return rootOther; }
            set { rootOther = value; }
        }

        private string declinationType = "";
        /// <summary>
        /// Each declination type shall be choosed for this noun as default
        /// </summary>
        public string DeclinationType
        {
            get { return declinationType; }
            set { declinationType = value; }
        }

        private bool canBeSingular = true;

        public bool CanBeSingular
        {
            get { return canBeSingular; }
            set { canBeSingular = value; }
        }

        private bool canBePlural = true;

        public bool CanBePlural
        {
            get { return canBePlural; }
            set { canBePlural = value; }
        }

        private List<int> categories = new List<int>();
        public List<int> Categories
        {
            get { return categories; }
        }

        #region Post Fix Indexing

        private Dictionary<WordCase, int> singularPostfixSelector = new Dictionary<WordCase, int>();

        public Dictionary<WordCase, int> SingularPostfixSelector
        {
            get { return singularPostfixSelector; }
            set { singularPostfixSelector = value; }
        }


        private Dictionary<WordCase, int> pluralPostfixSelector = new Dictionary<WordCase, int>();

        public Dictionary<WordCase, int> PluralPostfixSelector
        {
            get { return pluralPostfixSelector; }
            set { pluralPostfixSelector = value; }
        }

        public int GetPostFixIndex(WordAmount amount, WordCase aCase)
        {
            switch (amount)
            {
                case WordAmount.Plural:
                    {
                        if (pluralPostfixSelector.ContainsKey(aCase))
                            return pluralPostfixSelector[aCase];
                        break;
                    }

                case WordAmount.Singular:
                    {
                        if (singularPostfixSelector.ContainsKey(aCase))
                            return singularPostfixSelector[aCase];
                        break;
                    }
            }

            return 0;
        }

        public void SetPostIndex(WordCase aCase, WordAmount amount, int postFixIndex)
        {
            switch (amount)
            {
                case WordAmount.Plural:
                    {
                        if (postFixIndex > 0)
                        {
                            if (pluralPostfixSelector.ContainsKey(aCase))
                                pluralPostfixSelector[aCase] = postFixIndex;
                            else
                                pluralPostfixSelector.Add(aCase, postFixIndex);
                        }
                        else
                            if (pluralPostfixSelector.ContainsKey(aCase))
                                pluralPostfixSelector.Remove(aCase);

                        break;
                    }

                case WordAmount.Singular:
                    {
                        if (postFixIndex > 0)
                        {
                            if (singularPostfixSelector.ContainsKey(aCase))
                                singularPostfixSelector[aCase] = postFixIndex;
                            else
                                singularPostfixSelector.Add(aCase, postFixIndex);
                        }
                        else
                            if (singularPostfixSelector.ContainsKey(aCase))
                                singularPostfixSelector.Remove(aCase);

                        break;
                    }
            }

        }

        #endregion

        public string GetForm(WordCase aCase, WordAmount amount)
        {
            WordToken token = new WordToken(null, aCase, amount);
            foreach (WordToken tok in irregulars)
                if (tok.Is(token))
                    return tok.Text;

            return null;
        }

        private List<WordToken> irregulars = new List<WordToken>();
        /// <summary>
        /// Irregular forms of this Noun
        /// </summary>
        public List<WordToken> Irregulars
        {
            get { return irregulars; }
        }

        public void UpdateIrregular(WordToken aToken)
        {
            // find and update existing one
            foreach (WordToken token in this.irregulars)
                if (token.WordAmount == aToken.WordAmount &&
                    token.WordCase == aToken.WordCase)
                {
                    token.Text = aToken.Text;
                    return;
                }

            // not found - add new
            this.irregulars.Add(aToken);
        }

        #region IO Operations

        public bool AnalyzeLine(string line)
        {
            this.irregulars.Clear();
            this.categories.Clear();
            this.singularPostfixSelector.Clear();
            this.pluralPostfixSelector.Clear();

            if (!string.IsNullOrEmpty(line))
            {
                string[] elements = line.Split('|');
                this.root = elements[0];    // always will be at last one element in nonempty string

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
                                    this.IsConstant = true;
                                    break;
                                }

                            #endregion

                            #region Main Data

                            case '!':
                                {
                                    if (str.Length > 1)
                                    {
                                        this.genre = EnumHelper.GetWordGenre(str[1]);
                                        // TODO: read more details
                                        if (str.Length > 3)
                                        {
                                            this.declinationType = str.Substring(2, 2);
                                        }
                                    }
                                    else
                                    {
                                        // err
                                        return false;
                                    }

                                    break;
                                }

                            case '*':
                                {
                                    if (str.Length > 1)
                                    {
                                        this.IrregularGenre = EnumHelper.GetWordGenre(str[1]);
                                        // TODO: read more details
                                        this.HasIrregularGenre = this.irrGenre != WordGenre._Unknown;
                                    }
                                    else
                                    {
                                        // err
                                        return false;
                                    }

                                    break;
                                }


                            case '@':
                                {
                                    if (str.Length > 1)
                                    {
                                        // take root for other cases then Nominative
                                        this.rootOther = str.Substring(1);
                                    }
                                    else
                                    {
                                        // err
                                        return false;
                                    }

                                    break;
                                }
                            case '+':
                                {
                                    if (str.Length > 2)
                                    {
                                        // Specifies other than first index is used
                                        WordAmount amount = EnumHelper.GetWordAmount(str[1]);
                                        WordCase aCase = EnumHelper.GetWordCase(str[2]);
                                        int postFixIndex = int.Parse(str.Substring(3));

                                        switch (amount)
                                        {
                                            case WordAmount.Singular:
                                                {
                                                    if (!singularPostfixSelector.ContainsKey(aCase))
                                                        singularPostfixSelector.Add(aCase, postFixIndex);
                                                    else
                                                        singularPostfixSelector[aCase] = postFixIndex;
                                                    break;
                                                }
                                            case WordAmount.Plural:
                                                {
                                                    if (!pluralPostfixSelector.ContainsKey(aCase))
                                                        pluralPostfixSelector.Add(aCase, postFixIndex);
                                                    else
                                                        pluralPostfixSelector[aCase] = postFixIndex;
                                                    break;
                                                }
                                        }
                                    }
                                    else
                                    {
                                        // err
                                        return false;
                                    }

                                    break;
                                }
                            #endregion

                            #region Exception Cases

                            case '%':
                                {
                                    if (str.Length > 3)
                                    {
                                        this.IsException = true;
                                        WordCase aCase = EnumHelper.GetWordCase(str[1]);
                                        WordAmount amount = EnumHelper.GetWordAmount(str[2]);
                                        string txt = str.Substring(3);

                                        WordToken token = new
                                            WordToken(txt, aCase, amount);
                                        this.irregulars.Add(token);
                                    }
                                    else
                                    {
                                        // err
                                        return false;
                                    }

                                    break;
                                }

                            case '^':
                                {
                                    if (str.Length == 2)
                                    {
                                        // only singular/plural word - no sense meaning or grammar
                                        WordAmount amount = EnumHelper.GetWordAmount(str[1]);
                                        if (amount == WordAmount.Plural)
                                            this.canBePlural = false;
                                        else if (amount == WordAmount.Singular)
                                            this.canBeSingular = false;
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

                // check all data is specified
                if (this.genre == WordGenre._Unknown)
                    return false;

                return true;
            }

            return false;
        }


        public string WriteNoun()
        {
            // write main root
            StringBuilder sb = new StringBuilder();
            sb.Append(this.root);

            // write alternative root
            if (!string.IsNullOrEmpty(this.rootOther))
                sb.AppendFormat("|@{0}", this.rootOther);

            // Write main info
            char genre = EnumHelper.GetWordGenreCode(this.genre);
            sb.AppendFormat("|!{0}{1}", genre, this.declinationType);

            // write limitations
            if (!this.canBeSingular)
                sb.Append("|^1");
            if (!this.canBePlural)
                sb.Append("|^2");

            // write categories
            if (this.categories.Count > 0)
            {
                sb.Append("|$");
                for (int i = 0; i < this.categories.Count; i++)
                {
                    if (i > 0)
                        sb.Append(',');

                    sb.Append(this.categories[i]);
                }
            }
            // write "is const"
            if (this.IsConstant)
            {
                sb.Append("|#");
            }
            else
            {
                // write declination infos...
                foreach (WordCase aCase in singularPostfixSelector.Keys)
                {
                    int index = singularPostfixSelector[aCase];
                    if (index > 0)
                    {
                        char aCaseCode = EnumHelper.GetWordCaseCode(aCase);
                        char amountCode = EnumHelper.GetWordAmountCode(WordAmount.Singular);
                        sb.AppendFormat("|+{0}{1}{2}", amountCode, aCaseCode, index.ToString());
                    }
                }

                foreach (WordCase aCase in pluralPostfixSelector.Keys)
                {
                    int index = pluralPostfixSelector[aCase];
                    if (index > 0)
                    {
                        char aCaseCode = EnumHelper.GetWordCaseCode(aCase);
                        char amountCode = EnumHelper.GetWordAmountCode(WordAmount.Plural);
                        sb.AppendFormat("|+{0}{1}{2}", amountCode, aCaseCode, index.ToString());
                    }
                }

                // write irregular adjective connection
                if (this.HasIrregularGenre)
                    sb.AppendFormat("|*{0}", EnumHelper.GetWordGenreCode(this.IrregularGenre));

                // write irregular forms
                foreach (WordToken irToken in this.irregulars)
                {
                    char aCase = EnumHelper.GetWordCaseCode(irToken.WordCase);
                    char amount = EnumHelper.GetWordAmountCode(irToken.WordAmount);
                    sb.AppendFormat("|%{0}{1}{2}", aCase, amount, irToken.Text);
                }
            }

            return sb.ToString();
        }

        #endregion

    }

}
