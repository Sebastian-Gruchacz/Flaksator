using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public class NounDecliner
    {
        #region Singleton Implementation

        private static volatile NounDecliner _decliner = null;
        private static object _lockObject = new object();

        public static NounDecliner Decliner
        {
            get
            {
                if (_decliner == null)
                {
                    lock (_lockObject)
                    {
                        if (_decliner == null)
                            _decliner = new NounDecliner();
                    }
                }

                return _decliner;
            }
        }

        private NounDecliner()
        { }

        #endregion

        #region IO Operations

        public void LoadPostfixes(string filePath)
        {
            this.endings.Clear();

            FileInfo fi = new FileInfo(filePath);
            TextReader tr = new StreamReader(fi.FullName, Encoding.Default, true);
            string line = null;
            try
            {
                while ((line = tr.ReadLine()) != null)
                {
#if DEBUG
                    if (line.Trim().StartsWith("EOF"))
                        break; // for testing purposes
#else
                    if (line.Trim().StartsWith("EOF"))
                        continue; // ignore
#endif
                    if (line.Trim().StartsWith(";"))
                        continue; // entry is commented out

                    string[] fields = line.Split('|');
                    if (fields.Length != 2)
                        return; // Error

                    if (fields[0].Length != 5)
                        return; // error

                    string str = fields[0];
                    GrammaticalGender genre = EnumHelper.GetWordGenre(str[0]);
                    InflectionCase aCase = EnumHelper.GetWordCase(str[1]);
                    DecliantionNumber amount = EnumHelper.GetWordAmount(str[2]);
                    string declinType = str.Substring(3, 2);

                    string[] postfixes = fields[1].Split(' ');

                    NounPostfixToken token = new NounPostfixToken(genre, aCase, amount,
                        declinType, postfixes);

                    this.endings.Add(token);
                }
            }
            finally
            {
                tr.Close();
            }
        }

        public void SavePostfixes(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            TextWriter tw = new StreamWriter(fi.FullName, false, Encoding.Unicode);
            try
            {
                foreach (NounPostfixToken token in this.endings)
                {
                    char genre = EnumHelper.GetWordGenreCode(token.Genre);
                    char aCase = EnumHelper.GetWordCaseCode(token.InflectionCase);
                    char aAmount = EnumHelper.GetWordAmountCode(token.DecliantionNumber);

                    string postFixesString = "";

                    if (token.Postfixes.Length > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (string postfix in token.Postfixes)
                        {
                            if (sb.Length > 0)
                                sb.Append(" ");
                            sb.Append(postfix);
                        }

                        postFixesString = sb.ToString();
                    }
                    else
                        if (token.Postfixes.Length == 1)
                            postFixesString = token.Postfixes[0];

                    string line = string.Format("{0}{1}{2}{3}|{4}",
                        genre, aCase, aAmount, token.Declination, postFixesString);

                    tw.WriteLine(line);
                }
            }
            finally
            {
                tw.Close();
            }
        }

        #endregion

        #region Dictionary Edition

        public string GetPostFix(GrammaticalGender genre, string decType, InflectionCase aCase, DecliantionNumber amount)
        {
            foreach (NounPostfixToken token in this.endings)
            {
                if (token.Declination == decType && token.Genre == genre &&
                    token.DecliantionNumber == amount && token.InflectionCase == aCase)
                {
                    if (token.Postfixes.Length > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (string postfix in token.Postfixes)
                        {
                            if (sb.Length > 0)
                                sb.Append(" ");
                            sb.Append(postfix);
                        }

                        return sb.ToString();
                    }
                    else
                        if (token.Postfixes.Length == 1)
                            return token.Postfixes[0];
                }
            }

            return "";
        }

        public void AddUpdatePostfix(NounPostfixToken token)
        {
            foreach (NounPostfixToken aToken in this.endings)
            {
                if (token.Declination == aToken.Declination && token.Genre == aToken.Genre &&
                    token.DecliantionNumber == aToken.DecliantionNumber && token.InflectionCase == aToken.InflectionCase)
                {
                    this.endings.Remove(aToken);
                    break;
                }
            }

            this.endings.Add(token);
        }

        #endregion

        private string SelectPostfix(Noun word, InflectionCase aCase, DecliantionNumber amount,
    string root, params string[] postfixes)
        {
            foreach (NounPostfixToken token in this.endings)
            {
                if (token.Declination == word.DeclinationType &&
                    token.Genre == word.Genre && token.DecliantionNumber == amount
                    && token.InflectionCase == aCase)
                {
                    if (token.Postfixes.Length > 1)
                    {
                        int indx = NounGrammar.GetPostFixIndex(word, amount, aCase);

                        if (indx < token.Postfixes.Length)
                            return token.Postfixes[indx];
                        else
                            return "";
                    }
                    else
                    {
                        return token.Postfixes[0];
                    }
                }
            }

            return "";           //postfixes[0];
        }

        List<NounPostfixToken> endings = new List<NounPostfixToken>();

        private string Soften(Noun word, string root)
        {
            return root;
        }

        public string MakeWord(Noun word, InflectionCase aCase, DecliantionNumber amount)
        {
            if (aCase == InflectionCase._Unknown || amount == DecliantionNumber._Unknown)
                return null;

            if ((amount == DecliantionNumber.Singular && !word.CanBeSingular) ||
                (amount == DecliantionNumber.Plural && !word.CanBePlural))
                return null;

            if (word.IsConstant)
                return word.Root;

            if (word.IsException)
            {
                string except = NounGrammar.GetForm(word, aCase, amount);
                if (except != null)
                    return except; // else - calculate own...
            }

            string prefix = "";
            string postfix = "";
            string root = ((aCase != InflectionCase.Nominative || amount != DecliantionNumber.Singular) &&
                !string.IsNullOrEmpty(word.RootOther))
                ? word.RootOther : word.Root;

            #region Switching

            postfix = this.SelectPostfix(word, aCase, amount, root);
            if (postfix == "*")
                postfix = "";

#if SW
            switch (word.Genre)
            {
            #region MasculineLife
                
                case GrammaticalGender.MasculineLife:
                    {                       
                        switch (aCase)
                        {
                            case InflectionCase.Nominative: // Mianownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Genitive: //Dopełniacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Dative: // Celownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Accusative: // Biernik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Ablative: // Narzędnik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Locative: // Miejscownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Vocative: //Wołacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }

                        break;
                    }

                #endregion

            #region MasculinePerson

                case GrammaticalGender.MasculinePerson:
                    {
                        switch (aCase)
                        {
                            case InflectionCase.Nominative: // Mianownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Genitive: //Dopełniacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Dative: // Celownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Accusative: // Biernik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Ablative: // Narzędnik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Locative: // Miejscownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Vocative: //Wołacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                        break;
                    }

                #endregion

            #region MasculineThing

                case GrammaticalGender.MasculineThing:
                    {
                        switch (aCase)
                        {
                            case InflectionCase.Nominative: // Mianownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break; // Nothing
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                root = Soften(word, root);
                                                postfix = "y";
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Genitive: //Dopełniacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                root = Soften(word, root);
                                                //postfix = SelectPostfix(word, root, "a", "u");
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                root = Soften(word, root);
                                                //postfix = SelectPostfix(word, root, "y", "ów");
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Dative: // Celownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Accusative: // Biernik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Ablative: // Narzędnik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Locative: // Miejscownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Vocative: //Wołacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                        break;
                    }

                #endregion

            #region Feminine

                case GrammaticalGender.Feminine:
                    {
                        switch (aCase)
                        {
                            case InflectionCase.Nominative: // Mianownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Genitive: //Dopełniacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Dative: // Celownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Accusative: // Biernik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Ablative: // Narzędnik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Locative: // Miejscownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Vocative: //Wołacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                        break;
                    }

                #endregion

            #region Neuter

                case GrammaticalGender.Neuter:
                    {
                        switch (aCase)
                        {
                            case InflectionCase.Nominative: // Mianownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Genitive: //Dopełniacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Dative: // Celownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Accusative: // Biernik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Ablative: // Narzędnik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Locative: // Miejscownik
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case InflectionCase.Vocative: //Wołacz
                                {
                                    switch (amount)
                                    {
                                        case DecliantionNumber.Singular:
                                            {
                                                break;
                                            }
                                        case DecliantionNumber.Plural:
                                            {
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                        break;
                    }

                #endregion

            }
#endif

            #endregion

            return string.Format("{0}{1}{2}", prefix, root, postfix);
        }
    }
}
