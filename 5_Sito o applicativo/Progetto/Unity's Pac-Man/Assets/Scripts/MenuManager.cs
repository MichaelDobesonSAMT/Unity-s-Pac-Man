using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuCanvasPrefab;
    private int score;

    private void Start()
    {
        var canvas = Instantiate(MenuCanvasPrefab, 
            new Vector3(550,259.5f, 10), 
            Quaternion.identity);
        canvas.name = "Canvas";

        var buttons = canvas.GetComponentsInChildren<Transform>();
        foreach (var button in buttons)
        {
            if(button.name == "StartButton")
            {
                button.GetComponent<Button>().onClick.AddListener(IniziaGioco);
            }else if (button.name == "SettingsButton")
            {
                button.GetComponent<Button>().onClick.AddListener(GoToSettings);
            }else if (button.name == "ExitButton")
            {
                button.GetComponent<Button>().onClick.AddListener(EsciApplicazione);
            }
        }

        score = PlayerPrefs.GetInt("Score");
      //  GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = score.ToString();
    }
    public void IniziaGioco()
    {
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