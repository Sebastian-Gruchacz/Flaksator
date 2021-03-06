using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpDevs.Helpers.Ini
{
    public class IniKey
    {
        public IniKey()
        {
            this._multiline = false;
            this._isComment = false;
        }

        internal event EventHandler OnChange;

        protected void SetChanged()
        {
            if (this.OnChange != null)
                this.OnChange(this, new EventArgs());
        }

        private string _name;
        private string _value;
        private string[] _values;
        private bool _multiline;
        private bool _isComment;

        public bool IsComment
        {
            get { return this._isComment; }
        }

        public bool Multiline
        {
            get { return this._multiline; }
        }

        public string[] Values
        {
            get { return this._values; }
            set 
            {
                this._multiline = true;
                this._values = value;
                this.SetChanged();
            }
        }

        public string Value
        {
            get { return this._value; }
            set 
            {
                this._multiline = false;
                this._value = value;
                this.SetChanged();
            }
        }

        public string Name
        {
            get { return this._name; }
            set 
            {
                this._name = value;
                this._isComment = (this._name.StartsWith(";"));
                this.SetChanged();
            }
        }

        internal void WriteToStream(StreamWriter stream)
        {
            if (this._multiline)
            {
                stream.WriteLine("{0}=<<--", this._name);

                foreach (string str in this._values)
                    stream.WriteLine(str);               

                stream.WriteLine("--");
            }
            else if (this._isComment)
                stream.WriteLine(string.Format(";{0}", this._value));
            else
                stream.WriteLine(string.Format("{0}={1}", this._name, this._value));
        }

        internal static IniKey FromStream(ref string line, StreamReader stream)
        {
            if (line != null)
            {
                IniKey key = null;

                if (line.Trim().Length == 0)
                {
                    // proceed to the next line
                    line = stream.ReadLine(); 
                    
                    return null;
                }

                if (line.StartsWith(";"))
                {
                    key = new IniKey();
                    key.Value = line.Substring(1);
                    key._isComment = true;

                    // proceed to the next line
                    line = stream.ReadLine();

                    return key;
                }

                int pos = line.IndexOf('=');
                if (pos > 0)
                {
                    key = new IniKey();
                    key.Name = line.Substring(0, pos);
                    string rest = line.Substring(pos + 1);
                    if (rest.Trim() == "<<--")
                    {
                        key._multiline = true;
                        List<string> values = new List<string>();

                        // proceed to the next line
                        line = stream.ReadLine();

                        while (line != null && line.Trim() != "--")
                        {
                            values.Add(line);

                            // proceed to the next line
                            line = stream.ReadLine();
                        }

                        key._values = values.ToArray();

                        if (line != null)
                            line = stream.ReadLine();

                        return key;
                    }
                    else
                    {
                        key._value = rest;

                        // proceed to the next line
                        line = stream.ReadLine();

                        return key;
                    }
                }
                else
                {
                    // This is key without value...
                    key = new IniKey();
                    key.Name = line.Trim();

                    // proceed to the next line
                    line = stream.ReadLine();

                    return key;
                }

            }

            return null;
        }
    }
}
