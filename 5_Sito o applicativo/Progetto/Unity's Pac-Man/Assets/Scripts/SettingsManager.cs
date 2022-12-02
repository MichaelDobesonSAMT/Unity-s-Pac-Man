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
    private Slider LG;
    private Slider SPP;
    private Slider SPD;
    private TextMeshProUGUI GStext;
    private TextMeshProUGUI WPtext;
    private TextMeshProUGUI BStext;
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
        LG = GameObject.Find("SliderLG").GetComponent<Slider>();
        SPP = GameObject.Find("SliderSPP").GetComponent<Slider>();
        SPD = GameObject.Find("SliderSPD").GetComponent<Slider>();

        GStext = GameObject.Find("ValGS").GetComponent<TextMeshProUGUI>();
        WPtext = GameObject.Find("ValWP").GetComponent<TextMeshProUGUI>();
        BStext = GameObject.Find("ValBS").GetComponent<TextMeshProUGUI>();
        LGtext = GameObject.Find("ValLG").GetComponent<TextMeshProUGUI>();
        SPPtext = GameObject.Find("ValSPP").GetComponent<TextMeshProUGUI>();
        SPDtext = GameObject.Find("ValSPD").GetComponent<TextMeshProUGUI>();

        GS.maxValue = 5;
        GS.minValue = 50;
        WP.maxValue = 0;
        WP.minValue = 100;
        BS.maxValue = 0.5f;
        BS.minValue = 5;
        LG.maxValue = 0;
        LG.minValue = 3;
        SPP.maxValue = 0;
        SPP.minValue = 100;
        SPD.maxValue = 1;
        SPD.minValue = 10;

        GS.value = PlayerPrefs.GetFloat("GridSize");
        WP.value = PlayerPrefs.GetFloat("WallPercent");
        BS.value = PlayerPrefs.GetFloat("BlinkySpeed");
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
        PlayerPrefs.SetFloat("LivesGained", LG.value);
        PlayerPrefs.SetFloat("SuperPillPercent", SPP.value);
        PlayerPrefs.SetFloat("SuperPillDuration", SPD.value);
        ReturnToMenu();
    }
}