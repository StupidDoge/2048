using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LocalizationManager : MonoBehaviour
{
    public static int SelectedLanguage { get; private set; }

    public static event LanguageChangeHandler OnLanguageChange;
    public delegate void LanguageChangeHandler();

    private static Dictionary<string, List<string>> _localization;

    [SerializeField] private TextAsset _textFile;

    private static string _saveKey = "current_language";

    private void Awake()
    {
        if (_localization == null)
            LoadLocalization();

        LoadLanguage();
    }

    private void LoadLocalization()
    {
        _localization = new Dictionary<string, List<string>>();

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(_textFile.text);

        foreach (XmlNode key in xmlDocument["Keys"].ChildNodes)
        {
            string keyString = key.Attributes["name"].Value;

            var values = new List<string>();
            foreach (XmlNode translate in key["Translates"].ChildNodes)
                values.Add(translate.InnerText);

            _localization[keyString] = values;
        }
    }

    public void SetLanguage(int id)
    {
        SelectedLanguage = id;
        OnLanguageChange?.Invoke();

        SaveManager.Save(_saveKey, SelectedLanguage);
    }

    public static void LoadLanguage()
    {
        SelectedLanguage = SaveManager.Load(_saveKey);
        OnLanguageChange?.Invoke();
    }

    /*public static string SetSettingsCaption()
    {
        string caption = "settings";

        switch (SelectedLanguage)
        {
            case 0:
                caption = "Settings";
                break;
            case 1:
                caption = "Настройки";
                break;
            case 2:
                caption = "Налаштування";
                break;
        }

        return caption;
    }*/

    public static string GetTranslate(string key, int languageId = -1)
    {
        if (languageId == -1)
            languageId = SelectedLanguage;

        if (_localization.ContainsKey(key))
            return _localization[key][languageId];

        return key;
    }
}
