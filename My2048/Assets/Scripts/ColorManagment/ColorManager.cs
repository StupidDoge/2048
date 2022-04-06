using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color[] CurrentThemeColorsArray { get; private set; }

    public Color CurrentBackgroundColor { get; private set; }
    public Color CurrentButtonColor { get; private set; }
    public Color CurrentDarkTextColor { get; private set; }
    public Color CurrentLightTextColor { get; private set; }
    public Color CurrentLightImageColor { get; private set; }

    [Header("Classic theme colors")]
    [SerializeField] private Color[] _classicColorsArray;
    [SerializeField] private Color _classicBackgroundColor;
    [SerializeField] private Color _classicButtonColor;
    [SerializeField] private Color _classicDarkTextColor;
    [SerializeField] private Color _classicLightTextColor;
    [SerializeField] private Color _classicLightImageColor;
    [Space(10)]
    [Header("Dark theme colors")]
    [SerializeField] private Color[] _darkColorsArray;
    [SerializeField] private Color _darkBackgroundColor;
    [SerializeField] private Color _darkButtonColor;
    [SerializeField] private Color _darkDarkTextColor;
    [SerializeField] private Color _darkLightTextColor;
    [SerializeField] private Color _darkLightImageColor;
    [Space(10)]
    [Header("Ukraine theme colors")]
    [SerializeField] private Color[] _ukraineColorsArray;
    [SerializeField] private Color _ukraineBackgroundColor;
    [SerializeField] private Color _ukraineButtonColor;
    [SerializeField] private Color _ukraineDarkTextColor;
    [SerializeField] private Color _ukraineLightTextColor;
    [SerializeField] private Color _ukraineLightImageColor;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (PlayerPrefs.HasKey(ThemeController.ThemeKey))
            ThemeController.CurrentTheme = (ThemeController.Themes)SaveManager.Load(ThemeController.ThemeKey);
        else
            ThemeController.CurrentTheme = ThemeController.Themes.Classic;
        ChooseColorSet((int)ThemeController.CurrentTheme);
    }

    public void ChooseColorSet(int id)
    {
        switch (id)
        {
            case 0:
                SetColors(_classicColorsArray, _classicBackgroundColor, _classicButtonColor, _classicDarkTextColor, _classicLightTextColor, _classicLightImageColor);
                break;
            case 1:
                SetColors(_darkColorsArray, _darkBackgroundColor, _darkButtonColor, _darkDarkTextColor, _darkLightTextColor, _darkLightImageColor);
                break;
            case 2:
                SetColors(_ukraineColorsArray, _ukraineBackgroundColor, _ukraineButtonColor, _ukraineDarkTextColor, _ukraineLightTextColor, _ukraineLightImageColor);
                break;
        }
    }

    private void SetColors(Color[] array, params Color[] colors)
    {
        CurrentThemeColorsArray = array;
        CurrentBackgroundColor = colors[0];
        CurrentButtonColor = colors[1];
        CurrentDarkTextColor = colors[2];
        CurrentLightTextColor = colors[3];
        CurrentLightImageColor = colors[4];
    }

    public Color ColorObject(ObjectColoring.ObjectType type)
    {
        Color colorToReturn = Color.white;

        switch (type)
        {
            case ObjectColoring.ObjectType.Background:
                colorToReturn = CurrentBackgroundColor;
                break;
            case ObjectColoring.ObjectType.Button:
                colorToReturn = CurrentButtonColor;
                break;
            case ObjectColoring.ObjectType.LightText:
                colorToReturn = CurrentLightTextColor;
                break;
            case ObjectColoring.ObjectType.DarkText:
                colorToReturn = CurrentDarkTextColor;
                break;
            case ObjectColoring.ObjectType.LightImage:
                colorToReturn = CurrentLightImageColor;
                break;
        }

        return colorToReturn;
    }
}
