using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using SharpDevs.Helpers.Ini;

namespace SharpDevs.Helpers
{
    public class IniFile : IDisposable
    {
        public IniFile()
        {
            this._sections = new ChangeList<IniSection>();
            this._sections.OnChange += new EventHandler(this._sections_OnChange);
        }

        void _sections_OnChange(object sender, EventArgs e)
        {
            this.SetChanged();
        }

        private ChangeList<IniSection> _sections;
        private bool changed = false;

        public bool Changed
        {
            get { return this.changed; }
        }

        internal void SetChanged()
        {
            this.changed = true;
        }

        public ChangeList<IniSection> Sections
        {
            get { return this._sections; }
        }

        public IniSection this[string sectionName]
        {
            get
            {
                foreach (IniSection section in this._sections)
                    if (!section.IsComment && section.Name == sectionName)
                        return section;

                return null;

            }
        //    set { /* set the specified index to value here */ }
        }

        public IniKey GetKey(string sectionName, string keyName)
        {
            IniSection section = this[sectionName];
            if (section != null)
            {
                IniKey key = section[keyName];
                return key;
            }
            else
                return null;
        }

        public string GetKeyValue(string sectionName, string keyName)
        {
            IniKey key = this.GetKey(sectionName, keyName);
            if (key != null && !key.Multiline)
                return key.Value;
            else
                return null;
        }

        public string GetKeyValue(string sectionName, string keyName, int lineIndex)
        {
            IniKey key = this.GetKey(sectionName, keyName);
            if (key != null && key.Multiline)
            {
                if (key.Values.Length > lineIndex)
                    return key.Values[lineIndex];
                else
                    return null;
            }
            else
                return null;
        }

        public void SetKeyValue(string sectionName, string keyName, string value)
        {
            // Get / create section
            IniSection section = this[sectionName];
            if (section == null)
            {
                section = new IniSection(sectionName);
                this.Sections.Add(section);
            }

            // Get / create Key
            IniKey key = section[keyName];
            if (key == null)
            {
                key = new IniKey();
                key.Name = keyName;
                section.Keys.Add(key);
                key.OnChange += new EventHandler(this._sections_OnChange);
            }

            // Set new Value
            key.Value = value;

            this.SetChanged();
        }

        public void SetKeyValuesDimension(string sectionName, string keyName, int newDimension)
        {
            // Get / create section
            IniSection section = this[sectionName];
            if (section == null)
            {
                section = new IniSection(sectionName);
                this.Sections.Add(section);
            }

            // Get / create Key
            IniKey key = section[keyName];
            if (key == null)
            {
                key = new IniKey();
                key.Name = keyName;
                key.OnChange += new EventHandler(this._sections_OnChange);
                section.Keys.Add(key);
            }

            // Setting dimension 0 - causes to not use multiple values
            if (newDimension < 1)
            {
                key.Values = null;
                key.Value = null;
                return;
            }

            // Create new array with values
            string[] array = new string[newDimension];


            // Copy old values
            if (key.Multiline)
            {
                for (int i = 0; i < Math.Min(key.Values.Length, newDimension); i++)
                    array[i] = key.Values[i];
            }
            else
                array[0] = key.Value;

            // Keep new values array in Key
            key.Values = array;

            this.SetChanged();
        }

        public void SetKeyValue(string sectionName, string keyName, int lineIndex, string value)
        {
            // Get / create section
            IniSection section = this[sectionName];
            if (section == null)
            {
                section = new IniSection(sectionName);
                this.Sections.Add(section);
            }

            // Get / create Key
            IniKey key = section[keyName];
            if (key == null)
            {
                key = new IniKey();
                key.Name = keyName;
                key.OnChange += new EventHandler(this._sections_OnChange);
                section.Keys.Add(key);
            }

            // Calculate new values array
            int arrayDim = lineIndex + 1;
            if (key.Multiline)
            {
                arrayDim = Math.Max(arrayDim, key.Values.Length);
            }
            else
            {
                if (arrayDim < 1)
                    arrayDim = 1;
            }

            string[] array = new string[arrayDim];

            // Copy old values
            if (key.Multiline)
            {
                for (int i = 0; i < key.Values.Length; i++)
                    array[i] = key.Values[i];
            }
            else
                array[0] = key.Value;

            // insert new value
            array[lineIndex] = value;

            // Keep new values array in Key
            key.Values = array;

            this.SetChanged();
        }

        public void DeleteKey(string sectionName, string keyName)
        {
            // Get / create section
            IniSection section = this[sectionName];
            if (section == null)
            {
                return; // no section, no key...
            }

            // Get / create Key
            IniKey key = section[keyName];
            if (key != null)
            {
                section.Keys.Remove(key);
                key.OnChange += null;
            }

            this.SetChanged();
        }

        public void DeleteSection(string sectionName)
        {
            IniSection section = this[sectionName];
            if (section == null)
                return; // no section, no deletion

            this._sections.Remove(section);
            section.OnChange -= new EventHandler(this._sections_OnChange);;

            this.SetChanged();
        }

        /// <summary>
        /// Remember to close Stream after this method completes
        /// </summary>
        /// <param name="stream"></param>
        public void Load(StreamReader stream)
        {
            this._sections.Clear();
            string line = null;

            line = stream.ReadLine();

            while (line != null)
            {
                if (line.Trim().StartsWith(";") ||
                    (line.Trim().StartsWith("[") && line.Trim().EndsWith("]")))
                {
                    IniSection section = IniSection.FromStream(ref line, stream);
                    if (section != null)
                    {
                        this._sections.Add(section);
                        section.OnChange += new EventHandler(this._sections_OnChange);
                    }
                }
                else
                {
                    // next linereads in section / key methods - used for comments
                    line = stream.ReadLine();
                    if (line != null)
                        line = line.Trim();
                }
            }

            this.changed = false;
        }

        public void LoadFromFile(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            if (info.Exists)
            {
                StreamReader sr = new StreamReader(info.FullName, true);
                this.Load(sr);
                sr.Close();
            }
            else
                throw new FileNotFoundException();
        }

        public void Save(StreamWriter stream)
        {
            foreach (IniSection section in this.Sections)
            {
                section.WriteToStream(stream, true);
            }

            this.changed = false;
        }

        public void SaveToFile(string filePath)
        {
            StreamWriter sw = new StreamWriter(filePath, false);
            this.Save(sw);
            sw.Close();
        }


        #region IDisposable Members

        public void Dispose()
        {
            // TODO: implement correctly Disposable Method pattern
           
        }

        #endregion
    }
}
