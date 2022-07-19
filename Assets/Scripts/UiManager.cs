using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private static float waveDisplayTime = 4f;

    private static Text healthText;
    private static Text scoreText;
    private static Text energyText;
    private static Text waveNumText;
    private static Image eqpElementIcon;
    [SerializeField]
    private GameObject endGameInfo, pauseInfo;
    [SerializeField]
    private Sprite[] elementIcons = new Sprite[3];

    // Start is called before the first frame update
    void Start()
    {
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        energyText = GameObject.Find("EnergyText").GetComponent<Text>();
        eqpElementIcon = GameObject.Find("EquippedElementIcon").GetComponent<Image>();
        waveNumText = GameObject.Find("WaveNumText").GetComponent<Text>();
        StartCoroutine(DisplayWaveInfo(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Changes the properties of the 'elementText'
    public void UpdateElementIcon(Element.ElementType elmt)
    {
        switch (elmt)
        {
            case Element.ElementType.Fire:
                eqpElementIcon.sprite = elementIcons[0];
                break;
            case Element.ElementType.Water:
                eqpElementIcon.sprite = elementIcons[1];
                break;
            case Element.ElementType.Nature:
                eqpElementIcon.sprite = elementIcons[2];
                break;
        }
    }

    public void ActivateEndText(int finalScore)
    {
        endGameInfo.SetActive(true);
        endGameInfo.transform.Find("FinalScoreText").GetComponent<Text>().text = "Final Score: " + finalScore;
    }

    public void ChangePauseInfoState(bool state)
    {
        pauseInfo.SetActive(state);
    }

    //Changes the text of the 'healthText'
    public static void UpdateHealthText(int health)
    {
        healthText.text = health.ToString();
    }

    //Changes the text of the 'scoreText'
    public static void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    //Changes the text of the 'energyText'
    public static void UpdateEnergyText(int energy)
    {
        energyText.text = energy.ToString();
    }

    public static IEnumerator DisplayWaveInfo(int waveNum)
    {
        waveNumText.gameObject.SetActive(true);
        waveNumText.text = "Wave " + waveNum;
        yield return new WaitForSeconds(waveDisplayTime);
        waveNumText.gameObject.SetActive(false);
    }
}
