using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    private int score;
    public GameObject SettingsCanvasPrefab;

    private Slider GS;
    private Slider WP;
    private Slider BS;
    private Slider PS;
    private Slider PL;
    private Slider LG;
    private Slider SPP;
    private Slider SPD;
    private TextMeshProUGUI GStext;
    private TextMeshProUGUI WPtext;
    private TextMeshProUGUI BStext;
    private TextMeshProUGUI PLtext;
    private TextMeshProUGUI PStext;
    private TextMeshProUGUI LGtext;
    private TextMeshProUGUI SPPtext;
    private TextMeshProUGUI SPDtext;


    private void Start()
    {
        getComponents();
    }

    public void Update()
    {
        GStext.text = GS.value.ToString();
        WPtext.text = WP.value.ToString();
        BStext.text = BS.value.ToString();
        PStext.text = PS.value.ToString();
        PLtext.text = PL.value.ToString();
        LGtext.text = LG.value.ToString();
        SPPtext.text = SPP.value.ToString();
        SPDtext.text = SPD.value.ToString();
    }

    public void getComponents() {
        var canvas = Instantiate(SettingsCanvasPrefab,
                new Vector3(550, 259.5f, 10),
                Quaternion.identity);
        canvas.name = "Canvas";

        GameObject.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(ReturnToMenu);
        GameObject.Find("ButtonConfirm").GetComponent<Button>().onClick.AddListener(ReturnToMenu);

        GS = GameObject.Find("SliderGS").GetComponent<Slider>();
        WP = GameObject.Find("SliderWP").GetComponent<Slider>();
        BS = GameObject.Find("SliderBS").GetComponent<Slider>();
        PS = GameObject.Find("SliderPS").GetComponent<Slider>();
        PL = GameObject.Find("SliderPL").GetComponent<Slider>();
        LG = GameObject.Find("SliderLG").GetComponent<Slider>();
        SPP = GameObject.Find("SliderSPP").GetComponent<Slider>();
        SPD = GameObject.Find("SliderSPD").GetComponent<Slider>();

        GStext = GameObject.Find("ValGS").GetComponent<TextMeshProUGUI>();
        WPtext = GameObject.Find("ValWP").GetComponent<TextMeshProUGUI>();
        BStext = GameObject.Find("ValBS").GetComponent<TextMeshProUGUI>();
        PStext = GameObject.Find("ValPS").GetComponent<TextMeshProUGUI>();
        PLtext = GameObject.Find("ValPL").GetComponent<TextMeshProUGUI>();
        LGtext = GameObject.Find("ValLG").GetComponent<TextMeshProUGUI>();
        SPPtext = GameObject.Find("ValSPP").GetComponent<TextMeshProUGUI>();
        SPDtext = GameObject.Find("ValSPD").GetComponent<TextMeshProUGUI>();

        GS.maxValue = 50;
        GS.minValue = 5;
        WP.maxValue = 100;
        WP.minValue = 1;
        BS.maxValue = 10;
        BS.minValue = 1;
        PS.maxValue = 10;
        PS.minValue = 1;
        PL.maxValue = 10;
        PL.minValue = 1;
        LG.maxValue = 3;
        LG.minValue = 0;
        SPP.maxValue = 100;
        SPP.minValue = 1;
        SPD.maxValue = 10;
        SPD.minValue = 1;

        GS.value = PlayerPrefs.GetFloat("GridSize");
        WP.value = PlayerPrefs.GetFloat("WallPercent");
        BS.value = PlayerPrefs.GetFloat("BlinkySpeed");
        PS.value = PlayerPrefs.GetFloat("PacManSpeed");
        PL.value = PlayerPrefs.GetFloat("PlayerLives");
        LG.value = PlayerPrefs.GetFloat("LivesGained");
        SPP.value = PlayerPrefs.GetFloat("SuperPillPercent");
        SPD.value = PlayerPrefs.GetFloat("SuperPillDuration");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ConfirmEdit()
    {
        PlayerPrefs.SetFloat("GridSize", GS.value);
        PlayerPrefs.SetFloat("BlinkySpeed", BS.value);
        PlayerPrefs.SetFloat("WallPercent", WP.value);
        PlayerPrefs.SetFloat("PacManSpeed", PS.value);
        PlayerPrefs.SetFloat("PlayerLives", PL.value);
        PlayerPrefs.SetFloat("LivesGained", LG.value);
        PlayerPrefs.SetFloat("SuperPillPercent", SPP.value);
        PlayerPrefs.SetFloat("SuperPillDuration", SPD.value);
        ReturnToMenu();
    }
}