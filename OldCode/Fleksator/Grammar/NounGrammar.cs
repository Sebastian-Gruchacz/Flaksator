namespace SharpDevs.Fleksator.Grammar
{
    public static class NounGrammar
    {
        public static int GetPostFixIndex(Noun noun, DecliantionNumber amount, InflectionCase aCase)
        {
            switch (amount)
            {
                case DecliantionNumber.Plural:
                {
                    if (noun.PluralPostfixSelector.ContainsKey(aCase))
                        return noun.PluralPostfixSelector[aCase];
                    break;
                }

                case DecliantionNumber.Singular:
                {
                    if (noun.SingularPostfixSelector.ContainsKey(aCase))
                        return noun.SingularPostfixSelector[aCase];
                    break;
                }
            }

            return 0;
        }

        public static void SetPostIndex(Noun noun, InflectionCase aCase, DecliantionNumber amount, int postFixIndex)
        {
            switch (amount)
            {
                case DecliantionNumber.Plural:
                {
                    if (postFixIndex > 0)
                    {
                        if (noun.PluralPostfixSelector.ContainsKey(aCase))
                            noun.PluralPostfixSelector[aCase] = postFixIndex;
                        else
                            noun.PluralPostfixSelector.Add(aCase, postFixIndex);
                    }
                    else if (noun.PluralPostfixSelector.ContainsKey(aCase))
                        noun.PluralPostfixSelector.Remove(aCase);

                    break;
                }

                case DecliantionNumber.Singular:
                {
                    if (postFixIndex > 0)
                    {
                        if (noun.SingularPostfixSelector.ContainsKey(aCase))
                            noun.SingularPostfixSelector[aCase] = postFixIndex;
                        else
                            noun.SingularPostfixSelector.Add(aCase, postFixIndex);
                    }
                    else if (noun.SingularPostfixSelector.ContainsKey(aCase))
                        noun.SingularPostfixSelector.Remove(aCase);

                    break;
                }
            }

        }

        public static string GetForm(Noun noun, InflectionCase aCase, DecliantionNumber amount)
        {
            WordToken token = new WordToken(null, aCase, amount);
            foreach (WordToken tok in noun.Irregulars)
                if (tok.Is(token))
                    return tok.Text;

            return null;
        }

        public static void UpdateIrregular(Noun noun, WordToken aToken)
        {
            // find and update existing one
            foreach (WordToken token in noun.Irregulars)
                if (token.DecliantionNumber == aToken.DecliantionNumber &&
                    token.InflectionCase == aToken.InflectionCase)
                {
                    token.Text = aToken.Text;
                    return;
                }

            // not found - add new
            noun.Irregulars.Add(aToken);
        }
    }
}