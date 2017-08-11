using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.SceneCapture.Plugin
{
    public static class Translation
    {
        private readonly static string TRANSLATION_FILE = ConstantValues.ConfigDir + @"\translations.xml";
        private readonly static string TRANSLATION_FILE_SYBARIS = ConstantValues.ConfigDirSybaris + @"\translations.xml";

        private static Dictionary<string, Dictionary<string, string>> translations;

        private static string _currentTranslation;
        public static string CurrentTranslation
        {
            get {
                return _currentTranslation;
            }
            set {
                _currentTranslation = value;
            }
        }

        public static void Initialize( string language )
        {
            CurrentTranslation = language;

            translations = new Dictionary<string, Dictionary<string, string>>();
            XDocument xml = XDocument.Load(TRANSLATION_FILE);
            if( xml == null )
            {
                xml = XDocument.Load(TRANSLATION_FILE_SYBARIS);
                if( xml == null )
                {
                    Debug.LogError("Failed to load translation file from SceneCapture Config directory. 翻訳ファイルの読み込みに失敗しました。 " + TRANSLATION_FILE);
                    return;
                }
            }

            foreach( XElement translation in xml.Element("Translations").Elements())
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach( XElement category in translation.Elements())
                {
                    foreach( XElement field in category.Elements())
                    {
                        string key = category.Name + "_" + field.Name;
                        dict.Add(key, field.Value.ToString());
                    }
                }
                string translationLanguage = translation.Attribute("language").Value.ToString();
                translations[ translationLanguage ] = dict;

                IEnumerator<string> enumerator = translations.Keys.GetEnumerator();
                enumerator.MoveNext();
            }
        }

        public static string[] GetTranslations()
        {
            return translations.Keys.ToArray();
        }

        public static bool HasTranslation( string translation )
        {
            return translations.ContainsKey(translation);
        }

        public static string GetText(string category, string field)
        {
            if(!translations.ContainsKey(CurrentTranslation))
            {
                Debug.LogError("Language " + CurrentTranslation + " not defined!");
                return "ERROR";
            }
            string key = category + "_" + field;
            if( !translations[ CurrentTranslation ].ContainsKey(key) )
            {
                Debug.LogError("Translation " + key + " not found for language " + CurrentTranslation + "!");
                return "ERROR " + key;
            }
            return translations[ CurrentTranslation ][ key ];
        }
    }
}
