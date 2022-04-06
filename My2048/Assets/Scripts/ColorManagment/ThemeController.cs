using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeController : MonoBehaviour
{
    public enum Themes
    {
        Classic,
        Dark,
        Ukraine
    }

    public static Themes CurrentTheme { get; set; }
    public readonly static string ThemeKey = "current_theme";
}
