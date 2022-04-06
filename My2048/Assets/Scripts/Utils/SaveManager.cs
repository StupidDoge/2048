using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    public static void Save(string key, int savedValue)
    {
        PlayerPrefs.SetInt(key, savedValue);
    }

    public static int Load(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        else
            return 0;
    }
}
