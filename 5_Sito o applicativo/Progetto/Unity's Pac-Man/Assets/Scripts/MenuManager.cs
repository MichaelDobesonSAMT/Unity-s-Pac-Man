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

        GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(IniziaGioco);
        GameObject.Find("SettingsButton").GetComponent<Button>().onClick.AddListener(GoToGameMode);
        GameObject.Find("ExitButton").GetComponent<Button>().onClick.AddListener(EsciApplicazione);
    }
    public void IniziaGioco()
    {
        SceneManager.LoadScene(1);
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