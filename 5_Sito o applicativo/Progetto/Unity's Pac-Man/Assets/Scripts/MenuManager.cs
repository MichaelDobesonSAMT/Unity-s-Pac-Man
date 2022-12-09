using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuCanvasPrefab;

    void Start()
    {
        var canvas = Instantiate(MenuCanvasPrefab,
            new Vector3(550, 259.5f, 10),
            Quaternion.identity);
        canvas.name = "Canvas";

        var buttons = canvas.GetComponentsInChildren<Transform>();
        foreach (var button in buttons)
        {
            if (button.name == "StartButton")
            {
                button.GetComponent<Button>().onClick.AddListener(IniziaGioco);
            }
            else if (button.name == "SettingsButton")
            {
                button.GetComponent<Button>().onClick.AddListener(GoToGameMode);
            }
            else if (button.name == "ExitButton")
            {
                button.GetComponent<Button>().onClick.AddListener(EsciApplicazione);
            }
        }
    }
    public void IniziaGioco()
    {
        SceneManager.LoadScene(1);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GoToGameMode()
    {
        SceneManager.LoadScene(3);
    }
    public void EsciApplicazione()
    {
        Application.Quit();
    }
}