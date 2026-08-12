using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public class VerbConjurer
    {
        #region Singleton Implementation

        private static volatile VerbConjurer _decliner = null;
        private static object _lockObject = new object();

        public static VerbConjurer Conjurer
        {
            get
            {
                if (_decliner == null)
                {
                    lock (_lockObject)
                    {
                        if (_decliner == null)
                            _decliner = new VerbConjurer();
                    }
                }

                return _decliner;
            }
        }

        private VerbConjurer()
        { }

        #endregion

        public string MakeWord(Verb verb, VerbTense tense, GrammaticalPerson person,
            DecliantionNumber number, GrammaticalGender gender)
        {
            if (verb == null || tense == VerbTense._Unknown || person == GrammaticalPerson._Unknown ||
                number == DecliantionNumber._Unknown)
                return null;

            GrammaticalGender agreementGender = this.NormalizeGender(tense, number, gender);
            foreach (VerbForm form in verb.Forms)
                if (form.Matches(tense, person, number, agreementGender))
                    return form.Text;

            return null;
        }

        private GrammaticalGender NormalizeGender(VerbTense tense, DecliantionNumber number,
            GrammaticalGender gender)
        {
            if (tense != VerbTense.Past || gender == GrammaticalGender._Unknown)
                return gender;

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
