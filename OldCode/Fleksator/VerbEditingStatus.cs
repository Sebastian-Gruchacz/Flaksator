using System.Collections.Generic;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public sealed class VerbEditingStatus
    {
        private readonly List<string> issues = new List<string>();

        public IList<string> Issues
        {
            get { return this.issues.AsReadOnly(); }
        }

        public bool NeedsWork
        {
            get { return this.issues.Count > 0; }
        }

        public string Summary
        {
            get { return this.issues.Count == 0 ? "gotowy" : string.Join(", ", this.issues.ToArray()); }
        }

        public static VerbEditingStatus Inspect(Verb verb)
        {
            VerbEditingStatus status = new VerbEditingStatus();
            if (verb == null)
            {
                status.issues.Add("brak rekordu");
                return status;
            }
            if (verb.Categories.Count == 0)
                status.issues.Add("brak kategorii");
            if (verb.Aspect == VerbAspect.Unknown)
                status.issues.Add("brak aspektu");
            if (verb.Forms.Count == 0)
            {
                status.issues.Add("brak form");
                return status;
            }

            string error;
            if (!VerbFormValidator.TryValidateCompleteFiniteParadigms(verb, out error))
                status.issues.Add(error ?? "niepoprawne formy");
            if ((verb.Aspect == VerbAspect.Imperfective ||
                verb.Aspect == VerbAspect.Biaspectual) &&
                verb.Forms.Exists(form => form.Tense == VerbTense.Present) &&
                !verb.Forms.Exists(form => form.Tense == VerbTense.Future))
                status.issues.Add("brak przyszłości");
            return status;
        }
    }
}
