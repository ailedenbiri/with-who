using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{

    [Header("LosePanel")]
    [HideInInspector] public CanvasGroup losePanel;

    [Header("WinPanel")]
    [HideInInspector] public CanvasGroup winPanel;





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
        losePanel = GameObject.Find("LosePanel").GetComponent<CanvasGroup>();
        losePanel.gameObject.SetActive(false);
        losePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => GameManager.i.RestartLevel());
        winPanel = GameObject.Find("WinPanel").GetComponent<CanvasGroup>();
        winPanel.gameObject.SetActive(false);
        winPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => GameManager.i.GoNextLevel());
    }

    // HEARTH
    public void UpdateHearts()
    {
        hearts[0].transform.parent.DOKill(true);
        hearts[0].transform.parent.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < health;
        }
    }

    // CITY HAPPINESS

    public void UpdateFaces()
    {
        if (cityHappiness >= 75) //HAPPY
        {
            happyFace.gameObject.SetActive(true);
            normalFace.gameObject.SetActive(false);
            sadFace.gameObject.SetActive(false);
        }
        else if (50 <= cityHappiness && cityHappiness < 75) //NORMAL
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
