using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuCanvasPrefab;
    public static bool isJoystick = false;

    void Start()
    {
        isJoystick = false;
        var canvas = Instantiate(MenuCanvasPrefab,
            new Vector3(550, 259.5f, 10),
            Quaternion.identity);
        canvas.name = "Canvas";

        GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(IniziaGioco);
        GameObject.Find("SettingsButton").GetComponent<Button>().onClick.AddListener(GoToGameMode);
        GameObject.Find("ExitButton").GetComponent<Button>().onClick.AddListener(EsciApplicazione);
    }

    private void Update()
    {
        // If X on joystick is pressed then start the game
        if (Input.GetKeyDown("joystick button 1"))
        {
            IniziaGioco();
        }

        // If O on joystick is pressed then exit application
        if (Input.GetKeyDown("joystick button 2"))
        {
            EsciApplicazione();
        }

        // If [] on joystick is pressed then go to game mode page
        if (Input.GetKeyDown("joystick button 0"))
        {
            isJoystick = true;
            GoToGameMode();
        }
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