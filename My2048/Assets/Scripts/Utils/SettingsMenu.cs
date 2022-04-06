using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _themeList;
    [SerializeField] private TMP_Dropdown _languageList;
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _headerUkraine;

    private void Start()
    {
        if (PlayerPrefs.HasKey(ThemeController.ThemeKey))
            _themeList.value = (int)ThemeController.CurrentTheme;

        _languageList.value = LocalizationManager.SelectedLanguage;
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
            if (Input.GetKeyDown(KeyCode.Escape))
                GoToMenu();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetCurrentTheme(int themeIndex)
    {
        ThemeController.CurrentTheme = (ThemeController.Themes)themeIndex;
        SaveManager.Save(ThemeController.ThemeKey, (int)ThemeController.CurrentTheme);
        ColorManager.Instance.ChooseColorSet(themeIndex);
        ColorScene();
        SetHeader();
    }

    private void ColorScene()
    {
        ObjectColoring[] objects = FindObjectsOfType<ObjectColoring>();

        foreach (ObjectColoring obj in objects)
        {
            obj.GetComponent<ObjectColoring>();
            obj.ColorObject();
        }
    }

    private void SetHeader()
    {
        if (ThemeController.CurrentTheme == ThemeController.Themes.Ukraine)
        {
            _header.gameObject.SetActive(false);
            _headerUkraine.gameObject.SetActive(true);
            _headerUkraine.GetComponent<ObjectColoring>().ColorObject();
        }
        else
        {
            _header.gameObject.SetActive(true);
            _headerUkraine.gameObject.SetActive(false);
            _header.GetComponent<ObjectColoring>().ColorObject();
        }
    }
}
