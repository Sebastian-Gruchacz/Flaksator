using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDevs.Helpers.Presentation
{
    /// <summary>
    /// Encapsulation class for objects that do not ovveride ToString() method to be used in ListBoxes, Comboxes etc. You can have both readable title and object / enum value properly linked to item on the list.
    /// </summary>
    /// <typeparam name="T">Type that will be encapsulated, stored inside</typeparam>
    [System.Reflection.Obfuscation(Exclude = true)]
    public class Option<T>
    {
        /// <summary>
        /// Initializes object with stored object and value that will be dispalyed
        /// </summary>
        /// <param name="key">Object that is stored.</param>
        /// <param name="value">Text that will be displayed.</param>
        public Option(T key, string text)
        {
            _value = text;
            _key = key;
        }
    
        private T _key;
        private string _value;

        /// <summary>
        /// Display text.
        /// </summary>
        public string Text
        {
            get { return _value; }
            set { _value = value; }
        }
	
        /// <summary>
        /// The very stored object.
        /// </summary>
        public T Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Will return text that was set for display.
        /// </summary>
        /// <returns>Text.</returns>
        public override string ToString()
        {
            return this._value;
        }
    }
}
