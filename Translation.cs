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
        private static string currentTranslation;

        public static void Initialize()
        {
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
                        Debug.Log(key + " " + field.Value.ToString());
                        dict.Add(key, field.Value.ToString());
                    }
                }
                string translationLanguage = translation.Attribute("language").ToString();
                translations[ translationLanguage ] = dict;

                IEnumerator<string> enumerator = translations.Keys.GetEnumerator();
                enumerator.MoveNext();
                currentTranslation = enumerator.Current;
            }
        }

        public static string[] GetTranslations()
        {
            return translations.Keys.ToArray();
        }

        public static void SetCurrentTranslation( string translation )
        {
            if( translations.ContainsKey(translation)) {
                currentTranslation = translation;
            }
            else
            {
                Debug.LogError("Invalid translation language " + translation);
            }
        }

        public static string GetText(string category, string field)
        {
            string key = category + "_" + field;
            if( !translations[ currentTranslation ].ContainsKey(key) )
            {
                Debug.LogError("Translation " + key + " not found for language " + currentTranslation + "!");
                return "ERROR " + key;
            }
            return translations[ currentTranslation ][ key ];
        }
    }
}
