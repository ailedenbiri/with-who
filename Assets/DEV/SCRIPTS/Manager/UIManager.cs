using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : Singleton<UIManager>
{

    [Header("MainMenu")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject quitGameButton;


    [Header("PausePanel")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject playButton;



    [Header("Settings")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject soundOnButton;
    [SerializeField] private GameObject soundOffButton;

    [Header("LosePanel")]
    [SerializeField] private GameObject losePanel;

    [Header("WinPanel")]
    [SerializeField] private GameObject winPanel;





    [Header("Health")]

    public int health = 3;
    [SerializeField] private Image[] hearts;

    [Header("Happiness")]

    [SerializeField] private Image happyFace;
    [SerializeField] private Image normalFace;
    [SerializeField] private Image sadFace;

    private int cityHappiness = 100;

    private void Start()
    {
        cityHappiness = 50;
        UpdateFaces();
       
    }

    // HEARTH
    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < health;
        }
    }

    // CITY HAPPINESS

    public void UpdateFaces()
    {
        if(cityHappiness >= 75) //HAPPY
        {
           happyFace.gameObject.SetActive(true);
           normalFace.gameObject.SetActive(false);
           sadFace.gameObject.SetActive(false);
        }
        else if(50 <= cityHappiness && cityHappiness < 75) //NORMAL
        {
            happyFace.gameObject.SetActive(false);
            normalFace.gameObject.SetActive(true);
            sadFace.gameObject.SetActive(false);
        }
        else //SAD                               
        {
            happyFace.gameObject.SetActive(false);
            normalFace.gameObject.SetActive(false);
            sadFace.gameObject.SetActive(true);
        }
    }


    public void IncreaseCityHappiness(int amount)
    {
        cityHappiness += amount;

        if (cityHappiness > 100)
        {
            cityHappiness = 100;
        }
        UpdateFaces();
    }






    public void DecreaseCityHappiness(int amount)
    {

        cityHappiness -= amount;


        if (cityHappiness < 0)
        {
            cityHappiness = 0;
        }

        UpdateFaces();
    }

}
