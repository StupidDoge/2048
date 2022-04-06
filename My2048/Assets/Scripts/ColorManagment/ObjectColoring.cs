using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectColoring : MonoBehaviour
{
    public enum ObjectType
    {
        Background,
        Button,
        LightText,
        DarkText,
        LightImage
    }

    [SerializeField] private ObjectType _objectType;

    private Image _image;
    private TextMeshProUGUI _text;

    void Start()
    {
        ColorObject();
    }

    public void ColorObject()
    {
        switch (_objectType)
        {
            case (ObjectType.Background):
            case (ObjectType.Button):
            case (ObjectType.LightImage):
                _image = GetComponent<Image>();
                _image.color = ColorManager.Instance.ColorObject(_objectType);
                break;
            case (ObjectType.LightText):
            case (ObjectType.DarkText):
                _text = GetComponent<TextMeshProUGUI>();
                _text.color = ColorManager.Instance.ColorObject(_objectType);
                break;

        }
    }
}
