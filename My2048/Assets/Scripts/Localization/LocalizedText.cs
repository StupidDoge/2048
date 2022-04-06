using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private string _key;

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
        _text = GetComponent<TextMeshProUGUI>();
        _key = _text.text;
    }

    private void Localize(string newKey = null)
    {
        if (_text == null)
            Init();

        if (newKey != null)
            _key = newKey;

        _text.text = LocalizationManager.GetTranslate(_key);

    }
}
