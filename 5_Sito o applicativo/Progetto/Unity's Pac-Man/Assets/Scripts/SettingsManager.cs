using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    // Public variables
    public GameObject SettingsCanvasPrefab;

    // Private variables
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
    private TextMeshProUGUI PStext;
    private TextMeshProUGUI PLtext;
    private TextMeshProUGUI LGtext;
    private TextMeshProUGUI SPPtext;
    private TextMeshProUGUI SPDtext;


    void Start()
    {
        GetComponents();
    }

    void Update()
    {
        GStext.text = GS.value.ToString();
        WPtext.text = WP.value.ToString() + "%";
        BStext.text = BS.value.ToString();
        PStext.text = PS.value.ToString();
        PLtext.text = PL.value.ToString();
        LGtext.text = LG.value.ToString();
        SPPtext.text = SPP.value.ToString() + "%";
        SPDtext.text = SPD.value.ToString();
    }

    public void GetComponents() {
        var canvas = Instantiate(SettingsCanvasPrefab,
                new Vector3(550, 259.5f, 10),
                Quaternion.identity);
        canvas.name = "Canvas";

        GameObject.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(ReturnToMenu);
        GameObject.Find("ButtonConfirm").GetComponent<Button>().onClick.AddListener(ConfirmEdit);
        GameObject.Find("ButtonDefault").GetComponent<Button>().onClick.AddListener(ResetValues);

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

        GS.wholeNumbers = true;
        GS.minValue = 15;
        GS.maxValue = 21;

        WP.wholeNumbers = true;
        WP.minValue = 0;
        WP.maxValue = 50;

        BS.wholeNumbers = true;
        BS.minValue = 1;
        BS.maxValue = 6;

        PS.wholeNumbers = true;
        PS.minValue = 1;
        PS.maxValue = 5;

        PL.wholeNumbers = true;
        PL.minValue = 0;
        PL.maxValue = 10;

        LG.wholeNumbers = true;
        LG.minValue = 0;
        LG.maxValue = 5;

        SPP.wholeNumbers = true;
        SPP.minValue = 0;
        SPP.maxValue = 100;

        SPD.wholeNumbers = true;
        SPD.minValue = 5;
        SPD.maxValue = 15;

        GS.value = PlayerPrefs.GetInt("GridSize");
        WP.value = PlayerPrefs.GetFloat("WallPercent") * 100;
        BS.value = (1 - PlayerPrefs.GetFloat("BlinkySpeed")) / 0.125f;
        PS.value = (1 - PlayerPrefs.GetFloat("PacManSpeed")) / 0.0625f - 9;
        PL.value = PlayerPrefs.GetInt("PlayerLives");
        LG.value = PlayerPrefs.GetInt("LivesGained");
        SPP.value = PlayerPrefs.GetFloat("SuperPillPercent") * 100;
        SPD.value = PlayerPrefs.GetInt("SuperPillDuration");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ConfirmEdit()
    {
        PlayerPrefs.SetInt("GridSize", Mathf.RoundToInt(GS.value));
        PlayerPrefs.SetFloat("WallPercent", WP.value / 100);
        PlayerPrefs.SetFloat("BlinkySpeed", 1 - BS.value * 0.125f);
        PlayerPrefs.SetFloat("PacManSpeed", 1 - (PS.value + 9) * 0.0625f);
        PlayerPrefs.SetInt("PlayerLives", Mathf.RoundToInt(PL.value));
        PlayerPrefs.SetInt("LivesGained", Mathf.RoundToInt(LG.value));
        PlayerPrefs.SetFloat("SuperPillPercent", SPP.value / 100);
        PlayerPrefs.SetInt("SuperPillDuration", Mathf.RoundToInt(SPD.value));
        ReturnToMenu();
    }

    public void ResetValues()
    {
        PlayerPrefs.SetInt("GridSize", 20);
        PlayerPrefs.SetFloat("WallPercent", 0.3f);
        PlayerPrefs.SetFloat("BlinkySpeed", 0.5f);
        PlayerPrefs.SetFloat("PacManSpeed", 0.25f);
        PlayerPrefs.SetInt("PlayerLives", 3);
        PlayerPrefs.SetInt("LivesGained", 1);
        PlayerPrefs.SetFloat("SuperPillPercent", 0.25f);
        PlayerPrefs.SetInt("SuperPillDuration", 10);

        GS.value = PlayerPrefs.GetInt("GridSize");
        WP.value = PlayerPrefs.GetFloat("WallPercent") * 100;
        BS.value = (1 - PlayerPrefs.GetFloat("BlinkySpeed")) / 0.125f;
        PS.value = (1 - PlayerPrefs.GetFloat("PacManSpeed")) / 0.0625f - 9;
        PL.value = PlayerPrefs.GetInt("PlayerLives");
        LG.value = PlayerPrefs.GetInt("LivesGained");
        SPP.value = PlayerPrefs.GetFloat("SuperPillPercent") * 100;
        SPD.value = PlayerPrefs.GetInt("SuperPillDuration");
    }
}