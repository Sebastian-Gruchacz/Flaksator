using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using SharpDevs.Fleksator.Grammar;
using SharpDevs.Helpers.Ini;

namespace SharpDevs.Fleksator
{
    /// <summary>
    /// Provides transaltions for enumerations - for interface displays
    /// </summary>
    public sealed class EnumTranslator
    {
        #region Static Memebers

        static EnumTranslator()
        {

        }

        private static volatile Dictionary<string, EnumTranslator> translators = new Dictionary<string, EnumTranslator>();
        private static object lockObject = new object();

        public static EnumTranslator GetTranslator()
        {
            string languageCode = System.Globalization.CultureInfo.CurrentUICulture.Name;

            if (!translators.ContainsKey(languageCode))
            {
                lock(lockObject)
                {
                    if (!translators.ContainsKey(languageCode))
                    {
                        EnumTranslator ret = new EnumTranslator();

                        Stream str = null;
                        StreamReader tr = null;
                        SharpDevs.Helpers.IniFile file = null;

                        try
                        {
                            //System.Resources.ResourceManager mngr = new System.Resources.ResourceManager("SharpDevs.Fleksator.Translations", typeof(EnumTranslator).Assembly);
                            string asm = string.Format(".\\{0}\\SharpDevs.Fleksator.resources.dll", languageCode);
                            FileInfo ifo = new FileInfo(asm);
                            if (!ifo.Exists)
                            {
                                asm = string.Format(".\\en-US\\SharpDevs.Fleksator.resources.dll");
                                ifo = new FileInfo(asm);// if thius not exists - throw exception too
                            }
                            if (ifo.Exists)
                            {
                                Assembly resAsm = Assembly.LoadFile(ifo.FullName);
                                str = resAsm.GetManifestResourceStream("SharpDevs.Fleksator.Translations.txt");    // DO NOT TRANSLATE
                                tr = new StreamReader(str);

                                file = new SharpDevs.Helpers.IniFile();
                                file.Load(tr);
                            }
                            else
                                return null; // probably design time...
                        }
                        catch (Exception ex)
                        {
                            throw new IOException(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                "Error when reading from embeded resources."), ex);
                        }
                        finally
                        {
                            if (tr != null)
                                tr.Close();

                            if (str != null)
                                str.Close();
                        }

                        if (file != null)
                        {
                            try
                            {
                                Type[] sections = new Type[] {typeof(AdjectiveLevel), typeof(InflectionCase), typeof(GrammaticalGender),
                                    typeof(DecliantionNumber), typeof(GrammaticalPart)};

                                foreach (Type sectionEnum in sections)
                                {
                                    IniSection sect = file[sectionEnum.Name];

                                    string[] enumNames = Enum.GetNames(sectionEnum);

                                    switch (sectionEnum.Name)
                                    {
                                        case "AdjectiveLevel":
                                            {
                                                FillDictionary(ret.translationsAdjectiveLevel, sect, enumNames);
                                                break;
                                            }
                                        case "InflectionCase":
                                            {
                                                FillDictionary(ret.translationsWordCase, sect, enumNames);
                                                break;
                                            }
                                        case "GrammaticalGender":
                                            {
                                                FillDictionary(ret.translationsWordGenre, sect, enumNames);
                                                break;
                                            }
                                        case "DecliantionNumber":
                                            {
                                                FillDictionary(ret.translationsWordAmount, sect, enumNames);
                                                break;
                                            }
                                        case "GrammaticalPart":
                                            {
                                                FillDictionary(ret.translationsGrammarPart, sect, enumNames);
                                                break;
                                            }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                    "Error when reading transaltions."), ex);

                            }

                            file.Dispose();
                        }

                        // TODO: initialize translator
                        translators.Add(languageCode, ret);
                    }
                }
            }

            return translators[languageCode];
        }

        private static void FillDictionary(Dictionary<string, string> dic, IniSection sec, string[] names)
        {
            dic.Clear();

            foreach (string name in names)
            {
                IniKey key = sec[name];
                if (key == null)
                    throw new ApplicationException("Missing eneumeration key.");

                dic.Add(name, key.Value);
            }
        }

        #endregion

        private EnumTranslator()
        {

        }

        Dictionary<string, string> translationsAdjectiveLevel = new Dictionary<string, string>();
        public string TranslateAdjectiveLevel(AdjectiveLevel level)
        {
            return translationsAdjectiveLevel[level.ToString()];
        }

        Dictionary<string, string> translationsWordCase = new Dictionary<string, string>();
        public string TranslateWordCase(InflectionCase inflectionCase)
        {
            return translationsWordCase[inflectionCase.ToString()];
        }

        Dictionary<string, string> translationsWordGenre = new Dictionary<string, string>();
        public string TranslateWordGenre(GrammaticalGender genre)
        {
            return translationsWordGenre[genre.ToString()];
        }

        Dictionary<string, string> translationsWordAmount = new Dictionary<string, string>();
        public string TranslateWordAmount(DecliantionNumber amount)
        {
            return translationsWordAmount[amount.ToString()];
        }

        Dictionary<string, string> translationsGrammarPart = new Dictionary<string, string>();
        public string TranslateGrammarPart(GrammaticalPart part)
        {
            return translationsGrammarPart[part.ToString()];
        }


    }
}
