using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreController : MonoBehaviour
{
    public static HighScoreController Instance;

    public enum HighScoreType
    {
        Field3x3,
        Field4x4,
        Field5x5,
        Field8x8
    }

    [SerializeField] private TextMeshProUGUI _highScore;
    [SerializeField] private HighScoreType _highScoreType;

    public HighScoreType CurrentHighScoreType => _highScoreType;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        SetHighScore(PlayerPrefs.GetInt("HighScore" + _highScoreType.ToString()));
    }

    public void SetHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt("HighScore" + _highScoreType.ToString()))
            PlayerPrefs.SetInt("HighScore" + _highScoreType.ToString(), score);

        _highScore.text = PlayerPrefs.GetInt("HighScore" + _highScoreType.ToString()).ToString();
    }
}
