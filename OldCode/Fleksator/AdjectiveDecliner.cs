using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public class AdjectiveDecliner
    {
        #region Singleton Implementation

        private static volatile AdjectiveDecliner _decliner = null;
        private static object _lockObject = new object();

        public static AdjectiveDecliner Decliner
        {
            get
            {
                if (_decliner == null)
                {
                    lock (_lockObject)
                    {
                        if (_decliner == null)
                            _decliner = new AdjectiveDecliner();
                    }
                }

                return _decliner;
            }
        }

        private AdjectiveDecliner()
        { }

        #endregion

        private bool useNonLevellingRule = true;

        public bool UseNonLevellingRule
        {
            get { return this.useNonLevellingRule; }
            set { this.useNonLevellingRule = value; }
        }



        /// <summary>
        /// Green postfix
        /// </summary>
        /// <param name="word"></param>
        /// <param name="root"></param>
        /// <param name="endings"></param>
        /// <returns></returns>
        private string MakePostfix(Adjective word, ref string root, string[] endings)
        {
            if (root.Length > 0)
            {
                string tmp = root.Substring(0, root.Length - 1);
                char lastLetter = root[root.Length - 1];

                string ending = tmp.Substring(tmp.Length - 1);
                if (ending == "k" || ending == "g" || ending == "i")
                {
                    root = root.Substring(0, root.Length - 2);
                    if (ending != "i")
                        root += ending;
                    return endings[1];
                }
                else
                {
                    root = tmp;
                    return endings[0];
                }
            }
            else
                return "";
        }

        /// <summary>
        /// Blue Postfix
        /// </summary>
        /// <param name="word"></param>
        /// <param name="root"></param>
        /// <param name="endings"></param>
        /// <returns></returns>
        private string MakePostfixBlue(Adjective word, ref string root, string[] endings)
        {
            if (root.Length > 0)
            {
                string tmp = root.Substring(0, root.Length - 1); ;
                char lastLetter = root[root.Length - 1];

                string ending = tmp.Substring(tmp.Length - 1);
                if (ending == "i")
                {
                    root = root.Substring(0, root.Length - 2);
                    return endings[1];
                }
                else
                {
                    root = tmp;
                    return endings[0];
                }
            }
            else
                return "";
        }

        /// <summary>
        /// Used with Red postfix
        /// </summary>
        /// <param name="word"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private string Soften(Adjective word, string root)
        {
            if (root.Length > 1)
            {
                string let2 = root.Substring(root.Length - 2);
                string roo1 = root.Substring(0, root.Length - 2);
                switch (let2)
                {
                    #region Zamiana "y" na "i"

                    case "by":
                        {
                            return roo1 + "bi";
                        }
                    case "my":
                        {
                            return roo1 + "mi";
                        }
                    case "ny":
                        {
                            return roo1 + "ni";
                        }
                    case "wy":
                        {
                            return roo1 + "wi";
                        }

                    #endregion

                    #region Wymiana spó³g³oski, "y" pozostaje "y"

                    case "ry":
                        {
                            return roo1 + "rzy";
                        }

                    #endregion

                    #region Wymiana spó³g³oski, "i" przechodzi w "y"

                    case "ki":
                        {
                            return roo1 + "cy";
                        }
                    case "gi":
                        {
                            return roo1 + "dzy";
                        }

                    #endregion

                    #region Wymiana spó³g³oski, "y" przechodzi w "i"

                    case "³y":
                        {
                            return roo1 + "li";
                        }

                    case "ty":
                        {
                            return roo1 + "ci";
                        }

                    #endregion

                    default:
                        {
                            if (root.Length > 2)
                            {
                                string let3 = root.Substring(root.Length - 3);
                                string roo2 = root.Substring(0, root.Length - 3);

                                switch (let3)
                                {
                                    #region 3 literowa Wymiana spó³g³oski, "y" przechodzi w "i"

                                    case "szy":
                                        {
                                            return roo2 + "si";
                                        }
                                    case "chy":
                                        {
                                            return roo2 + "si";
                                        }
                                    case "sty":
                                        {
                                            return roo2 + "ści";
                                        }
                                    #endregion

                                    default:
                                        {
                                            return root; // reszta bez zmian
                                            /*
                                             * Uwaga na koñcówkê "¿y", która czasem zamienia siê na "zi"
                                             * a czasem zostaje bez zmian. Takie wyrazy wymagaj¹ obs³ugi wyj¹tków
                                             */
                                        }


                                }
                            }
                            else
                                return root;
                        }

                }
            }
            else
                return "";
        }

        string[] ending_I_Y = new string[] { "y", "i" };
        string[] ending_E_IE = new string[] { "e", "ie" };
        string[] ending_A_IA = new string[] { "a", "ia" };
        string[] ending_Aa_IAa = new string[] { "ą", "ią" };

        public string MakeWord(Adjective word, Noun noun, InflectionCase aCase, DecliantionNumber amount)
        {
            return this.MakeWord(word, (noun.HasIrregularGenre) ? noun.IrregularGenre : noun.Genre, aCase, amount, AdjectiveLevel.Equal);
        }

        public string MakeWord(Adjective word, Noun noun, InflectionCase aCase, DecliantionNumber amount,
            AdjectiveLevel level)
        {
            return this.MakeWord(word, (noun.HasIrregularGenre) ? noun.IrregularGenre : noun.Genre,
                aCase, amount, level);
        }

        public string MakeWord(Adjective word, GrammaticalGender genre, InflectionCase aCase, DecliantionNumber amount)
        {
            return this.MakeWord(word, genre, aCase, amount, AdjectiveLevel.Equal);
        }

        public string MakeWord(Adjective word, GrammaticalGender genre, InflectionCase aCase,
            DecliantionNumber amount, AdjectiveLevel level)
        {
            // check all arguments are provided...
            if (genre == GrammaticalGender._Unknown || aCase == InflectionCase._Unknown
                || amount == DecliantionNumber._Unknown || level == AdjectiveLevel._Unknown)
                return null;

            // constants
            if (word.IsConstant)
                return word.Root;

            // check word has exceptions
            if (word.IsException)
            {
                string except = word.GetForm(genre, aCase, amount, level);
                if (except != null)
                    return except; // alse - calculate own...
            }

            // now check if word has locked levels
            if (this.useNonLevellingRule && !word.CanBeLevelled &&
                (level == AdjectiveLevel.Higher || level == AdjectiveLevel.Highest))
                return null;

            string prefix = "";
            string postfix = "";
            string root = word.Root;

            // pick valid prefix for declination

            #region Levelling

            switch (level)
            {
                case AdjectiveLevel.Equal:
                    {
                        // nothing
                        break;
                    }
                case AdjectiveLevel.Higher:
                    {
                        if (!word.IsLevelledComplex && !string.IsNullOrEmpty(word.LevelHigherForm))
                            root = word.LevelHigherForm;
                        else
                            prefix = "bardziej ";
                        break;
                    }
                case AdjectiveLevel.Highest:
                    {
                        if (!word.IsLevelledComplex && !string.IsNullOrEmpty(word.LevelHighestForm))
                            root = word.LevelHighestForm;
                        else
                            prefix = "najbardziej ";
                        break;
                    }
            }

            #endregion

            // Chnage root / postfix depending of selected gender, number etc

            switch (amount)
            {
                #region DecliantionNumber.Singular

                case DecliantionNumber.Singular:
                    {
                        switch (genre)
                        {
                            case GrammaticalGender.MasculinePerson:
                            case GrammaticalGender.MasculineLife:
                                {
                                    switch (aCase)
                                    {
                                        case InflectionCase.Nominative:
                                        case InflectionCase.Vocative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                break;
                                            }
                                        case InflectionCase.Genitive:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "go";
                                                break;
                                            }
                                        case InflectionCase.Dative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "mu";
                                                break;
                                            }
                                        case InflectionCase.Accusative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "go";
                                                break;
                                            }
                                        case InflectionCase.Ablative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "m";
                                                break;
                                            }
                                        case InflectionCase.Locative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "m";
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case GrammaticalGender.MasculineThing:
                                {
                                    switch (aCase)
                                    {
                                        case InflectionCase.Nominative:
                                        case InflectionCase.Vocative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                break;
                                            }
                                        case InflectionCase.Genitive:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "go";
                                                break;
                                            }
                                        case InflectionCase.Dative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "mu";
                                                break;
                                            }
                                        case InflectionCase.Accusative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                break;
                                            }
                                        case InflectionCase.Ablative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "m";
                                                break;
                                            }
                                        case InflectionCase.Locative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "m";
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case GrammaticalGender.Feminine:
                                {
                                    switch (aCase)
                                    {
                                        case InflectionCase.Nominative:
                                        case InflectionCase.Vocative:
                                            {
                                                //postfix = MakePostfix(word, ref root, ending_A_IA);
                                                break;
                                            }
                                        case InflectionCase.Genitive:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "j";
                                                break;
                                            }
                                        case InflectionCase.Dative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "j";
                                                break;
                                            }
                                        case InflectionCase.Accusative:
                                            {
                                                postfix = this.MakePostfixBlue(word, ref root, this.ending_Aa_IAa);
                                                break;
                                            }
                                        case InflectionCase.Ablative:
                                            {
                                                postfix = this.MakePostfixBlue(word, ref root, this.ending_Aa_IAa);
                                                break;
                                            }
                                        case InflectionCase.Locative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "j";
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case GrammaticalGender.Neuter:
                                {
                                    switch (aCase)
                                    {
                                        case InflectionCase.Nominative:
                                        case InflectionCase.Vocative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                break;
                                            }
                                        case InflectionCase.Genitive:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "go";
                                                break;
                                            }
                                        case InflectionCase.Dative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                postfix += "mu";
                                                break;
                                            }
                                        case InflectionCase.Accusative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                break;
                                            }
                                        case InflectionCase.Ablative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "m";
                                                break;
                                            }
                                        case InflectionCase.Locative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "m";
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                        break;
                    }

                #endregion

                #region DecliantionNumber.Plural

                case DecliantionNumber.Plural:
                    {
                        switch (genre)
                        {
                            case GrammaticalGender.MasculinePerson:
                                {
                                    switch (aCase)
                                    {
                                        case InflectionCase.Nominative:
                                        case InflectionCase.Vocative:
                                            {
                                                if (root.EndsWith("cona") || root.EndsWith("iona"))
                                                {
                                                    root = root.Substring(0, root.Length - 3);
                                                    postfix = "eni";
                                                }
                                                else
                                                {
                                                    root = this.MakeWord(word, genre, InflectionCase.Nominative, DecliantionNumber.Singular, level);
                                                    root = this.Soften(word, root);
                                                    //postfix = MakePostfix(word, ref root, ending_I_Y);
                                                    prefix = ""; // above launch of MakeWord will add prefix either
                                                }
                                                break;
                                            }
                                        case InflectionCase.Genitive:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "ch";
                                                break;
                                            }
                                        case InflectionCase.Dative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "m";
                                                break;
                                            }
                                        case InflectionCase.Accusative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "ch";
                                                break;
                                            }
                                        case InflectionCase.Ablative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "mi";
                                                break;
                                            }
                                        case InflectionCase.Locative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "ch";
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case GrammaticalGender.Feminine:
                            case GrammaticalGender.MasculineLife:
                            case GrammaticalGender.MasculineThing:
                            case GrammaticalGender.Neuter:
                                {
                                    switch (aCase)
                                    {
                                        case InflectionCase.Nominative:
                                        case InflectionCase.Vocative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                break;
                                            }
                                        case InflectionCase.Genitive:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "ch";
                                                break;
                                            }
                                        case InflectionCase.Dative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "m";
                                                break;
                                            }
                                        case InflectionCase.Accusative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_E_IE);
                                                break;
                                            }
                                        case InflectionCase.Ablative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "mi";
                                                break;
                                            }
                                        case InflectionCase.Locative:
                                            {
                                                postfix = this.MakePostfix(word, ref root, this.ending_I_Y);
                                                postfix += "ch";
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

            return string.Format("{0}{1}{2}", prefix, root, postfix);
        }
    }

}
