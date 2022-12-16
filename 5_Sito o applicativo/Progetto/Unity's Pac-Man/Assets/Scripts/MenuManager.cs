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