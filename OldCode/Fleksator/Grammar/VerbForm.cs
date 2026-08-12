namespace SharpDevs.Fleksator.Grammar
{
    public sealed class VerbForm
    {
        public VerbTense Tense { get; set; }

        public GrammaticalPerson Person { get; set; }

        public DecliantionNumber Number { get; set; }

        public GrammaticalGender Gender { get; set; }

        public string Text { get; set; }

        public bool Matches(VerbTense tense, GrammaticalPerson person,
            DecliantionNumber number, GrammaticalGender gender)
        {
            return this.Tense == tense && this.Person == person && this.Number == number &&
                (this.Gender == GrammaticalGender._Unknown || this.Gender == gender);
        }
    }
}
