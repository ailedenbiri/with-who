using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GridCO[] grids;
    private int totalCards;

    public GridCO[] newGrids;

    public bool isGamePlaying = true;

    public LevelInfo levelInfo;

    string levelInfoText;

    [HideInInspector] public List<string> CompletedCards;

    public void UpdateLevelInfo()
    {
        levelInfoText = "";
        foreach (var item in levelInfo.LevelInfos)
        {
            string[] temp = item.Split(" ");
            string temp2 = "";
            for (int i = 0; i < temp.Length; i++)
            {
                if (i == 0)
                {
                    temp2 += $"<u>{temp[i]} </u>";
                }
                else
                {
                    temp2 += $"{temp[i]} ";
                }
            }
            if (CompletedCards.Contains(temp2.Split(" ")[0].Substring(3)))
            {
                //levelInfoText += " <sprite=1> " + temp2 + "\n";
            }
            else
            {
                levelInfoText += " <sprite=0> " + temp2 + "\n";
            }
        }
        GameObject.Find("LevelInfoText").GetComponent<TextMeshProUGUI>().text = levelInfoText;
    }

    private void Start()
    {
        levelInfoText = "";

        foreach (var item in levelInfo.LevelInfos)
        {
            string[] temp = item.Split(" ");
            string temp2 = "";
            for (int i = 0; i < temp.Length; i++)
            {
                if (i == 0)
                {
                    temp2 += $"<u>{temp[i]} </u>";
                }
                else
                {
                    temp2 += $"{temp[i]} ";
                }
            }
            levelInfoText += " <sprite=0> " + temp2 + "\n";

        }
        GameObject.Find("LevelInfoText").GetComponent<TextMeshProUGUI>().text = levelInfoText;
        GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>().text = "Level " + (PlayerPrefs.GetInt("SelectedLevel", 0) + 1).ToString();
        Time.timeScale = 1f;

        grids = GameObject.FindObjectsOfType<GridCO>(false);
        Camera.main.transform.position = GetAveragePosition(grids);
        totalCards = GameObject.FindObjectsOfType<Card>(false).Length;

        for (int i = 0; i < newGrids.Length; i++)
        {
            newGrids[i].transform.DOScale(Vector3.zero, 0.4f).SetDelay(i * 0.1f).From();
        }

        FindInActiveObjectByName("Button_MainMenu").GetComponent<Button>().onClick.AddListener(() => GoMainMenu());
    }

    void Update()
    {
        foreach (GridCO grid in grids)
        {
            grid.transform.localPosition = grid.startPos;
        }
        if (isGamePlaying)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<GridCO>().isEmpty)
                    {
                        hit.transform.localPosition = hit.collider.GetComponent<GridCO>().startPos + Vector3.up * 0.1f;
                    }
                }
            }
        }


    }

    public void CameraShake(float duration, Vector3 strength)
    {
        Camera.main.DOKill(true);
        Camera.main.DOShakePosition(duration, strength);
    }

    Vector3 GetAveragePosition(GridCO[] grids)
    {
        Vector3 totalPositions = Vector3.zero;
        foreach (var item in grids)
        {
            totalPositions += item.transform.position;
        }
        Vector3 averagePos = totalPositions / grids.Length;
        averagePos.z = -10;
        return averagePos;
    }


    public void CorrectCardPlaced()
    {
        totalCards--;

        // Eğer tüm kartlar doğru yerleştirildiyse
        if (totalCards == 0)
        {
            isGamePlaying = false;
            GameObject.Find("ConfettiDirectionalRainbow").GetComponent<ParticleSystem>().Play();
            DOVirtual.DelayedCall(2f, () =>
            {
                UIManager.i.winPanel.gameObject.SetActive(true);
                Time.timeScale = 0f;
                UIManager.i.winPanel.DOFade(1f, 0.8f).SetUpdate(true);
            });
        }
    }

    public void GoNextLevel()
    {
        PlayerPrefs.SetInt("SelectedLevel", PlayerPrefs.GetInt("SelectedLevel", 0) + 1);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene($"New Level - {PlayerPrefs.GetInt("SelectedLevel", 0) + 1}");
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

}