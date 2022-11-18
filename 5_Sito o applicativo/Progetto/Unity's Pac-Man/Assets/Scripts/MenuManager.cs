using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private int score;
    public Button StartButton;
    public Button SettingsButton;
    public Button ExitButton;

    private void Start()
    {
        Button btn1 = StartButton.GetComponent<Button>();
        Button btn2 = SettingsButton.GetComponent<Button>();
        Button btn3 = ExitButton.GetComponent<Button>();

        btn1.onClick.AddListener(IniziaGioco);
        btn2.onClick.AddListener(GoToSettings);
        btn3.onClick.AddListener(EsciApplicazione);
        score = PlayerPrefs.GetInt("Score");
      //  GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = score.ToString();
    }
    public void IniziaGioco()
    {
        Debug.Log("funzia");
        SceneManager.LoadScene(1);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GoToSettings()
    {
        SceneManager.LoadScene(2);
    }
    public void EsciApplicazione()
    {
        Application.Quit();
    }
}