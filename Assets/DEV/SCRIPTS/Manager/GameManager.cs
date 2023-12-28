using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GridCO[] grids;
    private int totalCards;

    public GridCO[] newGrids;
    private void Start()
    {
        Time.timeScale = 1f;

        grids = GameObject.FindObjectsOfType<GridCO>(false);
        Camera.main.transform.position = GetAveragePosition(grids);
        totalCards = GameObject.FindObjectsOfType<Card>(false).Length;

        for (int i = 0; i < newGrids.Length; i++)
        {
            newGrids[i].transform.DOScale(Vector3.zero, 0.4f).SetDelay(i * 0.1f).From();
        }
    }

    void Update()
    {

        foreach (GridCO grid in grids)
        {
            grid.transform.localPosition = grid.startPos;
        }
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

        // Eðer tüm kartlar doðru yerleþtirildiyse
        if (totalCards == 0)
        {
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

}