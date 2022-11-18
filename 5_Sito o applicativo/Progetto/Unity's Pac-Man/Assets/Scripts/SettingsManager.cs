using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private int score;
    public Button BackButton;
    public Button ConfirmButton;

    private void Start()
    {
        Button btn1 = BackButton.GetComponent<Button>();
        Button btn2 = ConfirmButton.GetComponent<Button>();

        btn1.onClick.AddListener(ReturnToMenu);
        btn2.onClick.AddListener(ConfirmEdit);
        score = PlayerPrefs.GetInt("Score");
        //  GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = score.ToString();
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ConfirmEdit()
    {
        SceneManager.LoadScene(0);
    }
}