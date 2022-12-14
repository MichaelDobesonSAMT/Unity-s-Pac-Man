using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviour
{

    void Start()
    {
        GameObject.Find("Easy").GetComponent<Button>().onClick.AddListener(EasyMode);
        GameObject.Find("Medium").GetComponent<Button>().onClick.AddListener(MediumMode);
        GameObject.Find("Hard").GetComponent<Button>().onClick.AddListener(HardMode);
        GameObject.Find("Custom").GetComponent<Button>().onClick.AddListener(Settings);

        GameObject.Find("Custom").SetActive(true);
    }

    private void Update()
    {
        // If [] on joystick is pressed then change game mode to easy
        if (Input.GetKeyDown("joystick button 0"))
        {
            EasyMode();
        }

        // If X on joystick is pressed then change game mode to medium
        if (Input.GetKeyDown("joystick button 1"))
        {
            MediumMode();
        }

        // If O on joystick is pressed then change game mode to hard
        if (Input.GetKeyDown("joystick button 2"))
        {
            HardMode();
        }

        // If a joystick is connected the Custom button gets deactivated
        if (MenuManager.isJoystick)
        {
            GameObject.Find("Custom").SetActive(false);
            MenuManager.isJoystick = false;
        }
    }

    public void EasyMode()
    {
        PlayerPrefs.SetInt("GridSize", 15);
        PlayerPrefs.SetFloat("WallPercent", 0.1f);
        PlayerPrefs.SetFloat("BlinkySpeed", 0.9f);
        PlayerPrefs.SetFloat("PacManSpeed", 0.25f);
        PlayerPrefs.SetInt("PlayerLives", 5);
        PlayerPrefs.SetInt("LivesGained", 5);
        PlayerPrefs.SetFloat("SuperPillPercent", 1f);
        PlayerPrefs.SetInt("SuperPillDuration", 15);
        SceneManager.LoadScene(0);
    }
    public void MediumMode()
    {
        PlayerPrefs.SetInt("GridSize", 20);
        PlayerPrefs.SetFloat("WallPercent", 0.3f);
        PlayerPrefs.SetFloat("BlinkySpeed", 0.5f);
        PlayerPrefs.SetFloat("PacManSpeed", 0.25f);
        PlayerPrefs.SetInt("PlayerLives", 3);
        PlayerPrefs.SetInt("LivesGained", 1);
        PlayerPrefs.SetFloat("SuperPillPercent", 0.25f);
        PlayerPrefs.SetInt("SuperPillDuration", 10);
        SceneManager.LoadScene(0);
    }
    public void HardMode()
    {
        PlayerPrefs.SetInt("GridSize", 20);
        PlayerPrefs.SetFloat("WallPercent", 0.4f);
        PlayerPrefs.SetFloat("BlinkySpeed", 0.2f);
        PlayerPrefs.SetFloat("PacManSpeed", 0.1f);
        PlayerPrefs.SetInt("PlayerLives", 1);
        PlayerPrefs.SetInt("LivesGained", 1);
        PlayerPrefs.SetFloat("SuperPillPercent", 0.5f);
        PlayerPrefs.SetInt("SuperPillDuration", 5);
        SceneManager.LoadScene(0);
    }
    public void Settings()
    {
        SceneManager.LoadScene(2);
    }
}
