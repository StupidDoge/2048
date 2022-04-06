using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static int Points { get; private set; }
    public static bool GameStarted { get; private set; }
    public static bool CanReturnMove { get; set; }

    [SerializeField] private RectTransform _restartGamePanel;
    [SerializeField] private RectTransform _gameResultPanel;
    [SerializeField] private TextMeshProUGUI _points;
    [SerializeField] private Button _returnMoveButton;

    private string saveKey;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        saveKey = "saved" + HighScoreController.Instance.CurrentHighScoreType.ToString();
        StartGame();
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
            if (Input.GetKeyDown(KeyCode.Escape))
                GoToMenu();
    }

    public void StartGame()
    {
        GameStarted = true;
        CanReturnMove = false;
        GameOverPanelController.Instance.HideResultCaption();

        if (PlayerPrefs.HasKey(saveKey + "Score"))
            Load();
        else
        {
            SetPoints(0);
            Field.Instance.GenerateField();
        }
    }

    public void ClickRestartButton()
    {
        if (GameStarted)
            OpenRestartGamePanel();
        else
            RestartGame();
    }

    public void OpenRestartGamePanel()
    {
        GameStarted = false;
        _restartGamePanel.gameObject.SetActive(true);
    }

    public void CloseRestartGamePanel()
    {
        GameStarted = true;
        _restartGamePanel.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        CanReturnMove = false;
        GameStarted = true;

        GameOverPanelController.Instance.ResetAlpha();
        GameOverPanelController.Instance.HideResultCaption();
        _restartGamePanel.gameObject.SetActive(false);

        SetPoints(0);
        Field.Instance.GenerateField();
        Save();
    }

    public void ReturnMove()
    {
        if (CanReturnMove)
        {
            Field.Instance.ResetField();
            AddPoints(-Field.Instance.PointsPerMove);
            CanReturnMove = false;
            GameOverPanelController.Instance.HideResultCaption();

            if (!GameStarted)
            {
                GameStarted = true;
                GameOverPanelController.Instance.ResetAlpha();
            }

            Save();
        }
    }

    public void AddPoints(int points)
    {
        SetPoints(Points + points);
    }

    private void SetPoints(int points)
    {
        Points = points;
        _points.text = Points.ToString();

        HighScoreController.Instance.SetHighScore(Points);
    }

    public void Win()
    {
        GameStarted = false;
        StartCoroutine(ShowGameResultPanel(true));
    }

    public void Lose()
    {
        GameStarted = false;
        StartCoroutine(ShowGameResultPanel(false));
    }

    IEnumerator ShowGameResultPanel(bool victory)
    {
        _returnMoveButton.enabled = false;
        yield return new WaitForSeconds(0.3f);
        _gameResultPanel.SetAsLastSibling();
        GameOverPanelController.Instance.ShowResultCaption(victory);
        GameOverPanelController.Instance.Fade();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Save()
    {
        SaveManager.Save(saveKey + "Score", Points);

        for (int x = 0; x < Field.Instance.FieldSize; x++)
            for (int y = 0; y < Field.Instance.FieldSize; y++)
                SaveManager.Save((saveKey + "_" + x + y).ToString(), Field.Instance.CellMatrix[x, y].Value);
    }

    private void Load()
    {
        CanReturnMove = false;
        Points = SaveManager.Load(saveKey + "Score");
        SetPoints(Points);

        Field.Instance.GenerateField(false);

        for (int x = 0; x < Field.Instance.FieldSize; x++)
            for (int y = 0; y < Field.Instance.FieldSize; y++)
                Field.Instance.CellMatrix[x, y].SetValues(x, y, SaveManager.Load((saveKey + "_" + x + y).ToString()));
    }
}
