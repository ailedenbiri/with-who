using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public static CardController instance;
    [SerializeField] private Camera cam;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }



  
    public void InstantiateCard(Card sender)
    {
        Vector3 mousePosition = Input.mousePosition;
   

        Vector2 worldPosition = cam.ScreenToWorldPoint(mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            Vector2 spawnPosition = hit.point;

            Instantiate(sender.cardInfo.prefab, spawnPosition, Quaternion.identity);
            sender.gameObject.SetActive(false);
        }
    } 
}



        