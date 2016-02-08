using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpDevs.Helpers.Ini
{
    public class IniSection
    {
        public IniSection(string name)
        {
            _name = name;
            _keys = new ChangeList<IniKey>();
            _keys.OnChange += new EventHandler(_keys_OnChange);
            _isComment = false;
        }

        void _keys_OnChange(object sender, EventArgs e)
        {
            SetChanged();
        }

        internal event EventHandler OnChange;

        protected void SetChanged()
        {
            if (OnChange != null)
                OnChange(this, new EventArgs());
        }

        private string _name;
        private ChangeList<IniKey> _keys;
        private bool _isComment;

        public bool IsComment
        {
            get { return _isComment; }
        }

        public List<IniKey> Keys
        {
            get { return _keys; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _isComment = (_name.StartsWith(";"));
                SetChanged();
            }
        }

        public IniKey this[string keyName]
        {
            get
            {
                foreach (IniKey key in this.Keys)
                    if (!key.IsComment && key.Name == keyName)
                        return key;

                return null;

            }
            set 
            {
                foreach (IniKey key in _keys)
                    if (!key.IsComment && key.Name == keyName)
                    {
                        _keys.Remove(key);
                        key.OnChange -= new EventHandler(_keys_OnChange);
                        _keys.Add(value);
                        value.OnChange += new EventHandler(_keys_OnChange);
                    }
                // SetChanged();    - no need here, will be executed by remove, add        
            }
        }

        internal void WriteToStream(StreamWriter stream, bool writeKeys)
        {
            if (!_isComment)
            {
                stream.WriteLine(string.Format("[{0}]", this._name));
                if (writeKeys)
                {
                    foreach (IniKey key in this.Keys)
                    {
                        key.WriteToStream(stream);
                    }
                }

                stream.WriteLine("");
            }
            else
                stream.WriteLine(string.Format(";{0}", this._name));
        }

        internal static IniSection FromStream(ref string line, StreamReader stream)
        {
            if (line != null)
            {
                line = line.Trim();
                
                IniSection section = null;

                // return section being comment
                if (line.StartsWith(";"))
                {
                    section = new IniSection(line.Substring(1));
                    section._isComment = true;

                    // proceed to the next line
                    line = stream.ReadLine();

                    return section;
                }                

                if (line.StartsWith("[") && line.EndsWith("]"))
                    section = new IniSection(line.Substring(1, line.Length - 2));

                // proceed to the next line
                line = stream.ReadLine();
                if (line != null)
                    line = line.Trim();

                while (line != null && !(line.StartsWith("[") && line.EndsWith("]")))
                {
                    IniKey key = IniKey.FromStream(ref line, stream);
                    if (key != null)
                    {
                        section._keys.Add(key);
                        key.OnChange += new EventHandler(section._keys_OnChange);
                    }

                    // Trim returned line
                    if (line != null)
                        line = line.Trim();
                }
                
                return section;
            }

            return null;
        }
    }
}
