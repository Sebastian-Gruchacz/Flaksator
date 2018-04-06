using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDevs.Helpers.Presentation
{
    /// <summary>
    /// Specialized version of <paramref name="Option<T>">Option<T></paramref> type taht stores additional field used for display in some scenarios.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Reflection.Obfuscation(Exclude = true)]
    public class OptionDescribed<T> : Option<T>
    {
        public OptionDescribed(T value, string displayedText, string description)
            : base(value, displayedText)
        {
            this.desc = description;
        }

        private string desc;
        /// <summary>
        /// Extra text that can be used to disply after item selection
        /// </summary>
        public string Description
        {
            get { return this.desc; }
            set { this.desc = value; }
        }

    }
}
