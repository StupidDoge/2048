using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private RectTransform _rulesPanel; 

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack", true);
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void OpenScene3x3Field()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenScene4x4Field()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenScene5x5Field()
    {
        SceneManager.LoadScene(3);
    }

    public void OpenScene8x8Field()
    {
        SceneManager.LoadScene(4);
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(5);
    }

    public void OpenRules()
    {
        _rulesPanel.gameObject.SetActive(true);
    }

    public void CloseRules()
    {
        _rulesPanel.gameObject.SetActive(false);
    }
}
