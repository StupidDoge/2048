using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class LocalizedDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;
    private List<string> _keys;

    private void Start()
    {
        Localize();
        LocalizationManager.OnLanguageChange += OnLanguageChange;
    }

    private void OnDestroy()
    {
        LocalizationManager.OnLanguageChange -= OnLanguageChange;
    }

    private void OnLanguageChange()
    {
        Localize();
    }

    private void Init()
    {
        _dropdown = GetComponent<TMP_Dropdown>();

        _keys = new List<string>();

        foreach (var option in _dropdown.options)
            _keys.Add(option.text);
    }

    private void Localize(List<string> newKeys = null)
    {
        if (_dropdown == null)
            Init();

        if (newKeys != null)
            _keys = newKeys;

        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var key in _keys)
            options.Add(new TMP_Dropdown.OptionData(LocalizationManager.GetTranslate(key)));

        _dropdown.options = options;
        _dropdown.RefreshShownValue();
    }
}
