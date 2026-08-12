using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public sealed class PronounDecliner
    {
        private static readonly PronounDecliner decliner = new PronounDecliner();

        public static PronounDecliner Decliner
        {
            get { return decliner; }
        }

        private PronounDecliner()
        { }

        public string MakeWord(Pronoun pronoun, InflectionCase aCase, GrammaticalPerson person,
            DecliantionNumber number, GrammaticalGender gender)
        {
            return this.MakeWord(pronoun, aCase, person, number, gender, PronounVariant.Neutral);
        }

        public string MakeWord(Pronoun pronoun, InflectionCase aCase, GrammaticalPerson person,
            DecliantionNumber number, GrammaticalGender gender, PronounVariant variant)
        {
            if (pronoun == null || aCase == InflectionCase._Unknown ||
                person == GrammaticalPerson._Unknown || number == DecliantionNumber._Unknown ||
                gender == GrammaticalGender._Unknown)
                return null;

            GrammaticalGender agreementGender = this.NormalizeGender(number, gender);
            foreach (PronounForm form in pronoun.Forms)
                if (form.Matches(aCase, person, number, agreementGender, variant))
                    return form.Text;

            return null;
        }

        private GrammaticalGender NormalizeGender(DecliantionNumber number, GrammaticalGender gender)
        {
            if (number == DecliantionNumber.Singular)
            {
                if (gender == GrammaticalGender.MasculineLife ||
                    gender == GrammaticalGender.MasculinePerson ||
                    gender == GrammaticalGender.MasculineThing)
                    return GrammaticalGender.MasculineThing;

                return gender;
            }

            return gender == GrammaticalGender.MasculinePerson
                ? GrammaticalGender.MasculinePerson
                : GrammaticalGender.Feminine;
        }
    }
}
