using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelController : MonoBehaviour
{
    public static GameOverPanelController Instance;

    public static bool BlockBackButton = false;

    [SerializeField] private float _fadeDuration = 0.4f;
    [SerializeField] private Button _returnMoveButton;
    [Header("Game over and victory captions")]
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _victoryText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Fade()
    {
        CanvasGroup _canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn(_canvasGroup, _canvasGroup.alpha, 1));
    }

    public void ResetAlpha()
    {
        CanvasGroup _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < _fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / _fadeDuration);

            yield return null;
        }
        _returnMoveButton.enabled = true;
    }

    public void ShowResultCaption(bool victory)
    {
        if (victory)
            _victoryText.gameObject.SetActive(true);
        else
            _gameOverText.gameObject.SetActive(true);
    }

    public void HideResultCaption()
    {
        _victoryText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
    }
}
