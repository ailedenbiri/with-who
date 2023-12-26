using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridCO[] level2Grids;
    [SerializeField] private GridCO[] grids;
    private int totalCards;
    bool levelCompleted;
    private void Start()
    {
        grids = GameObject.FindObjectsOfType<GridCO>(false);
        Camera.main.transform.position = GetAveragePosition(grids);

        
        totalCards = GameObject.FindObjectsOfType<Card>(false).Length;
      
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        foreach (GridCO grid in grids)
        {
            grid.transform.localPosition = grid.startPos;
        }
        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<GridCO>().isEmpty)
            {
                hit.transform.localPosition = hit.collider.GetComponent<GridCO>().startPos + Vector3.up * 0.1f;
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


    public void CheckCorrectPlacement()
    {

        totalCards--;

        // Eðer tüm kartlar doðru yerleþtirildiyse
        if (totalCards == 0)
        {
            // next level
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            levelCompleted = false;
        }
    }

}