namespace SharpDevs.Fleksator.Grammar
{
    public sealed class PronounForm
    {
        public InflectionCase Case { get; set; }

        public GrammaticalPerson Person { get; set; }

        public DecliantionNumber Number { get; set; }

        public GrammaticalGender Gender { get; set; }

        public PronounVariant Variant { get; set; }

        public string Text { get; set; }

        public bool Matches(InflectionCase aCase, GrammaticalPerson person,
            DecliantionNumber number, GrammaticalGender gender, PronounVariant variant)
        {
            return this.Case == aCase && this.Person == person && this.Number == number &&
                this.Gender == gender && this.Variant == variant;
        }
    }
}
