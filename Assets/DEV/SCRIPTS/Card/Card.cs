using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using System.Linq;
using Unity.Burst.CompilerServices;


public class Card : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    [Header("Card Settings")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text info;
    [SerializeField] private Image img;
    [SerializeField] private RectTransform cardRect;


    [Header("Ground Settings")]
    [SerializeField] private List<Collider2D> colliders;
  

    private Collider2D[] allColliders;
    

   
    public CardInfo cardInfo;
    private Vector2 firstPos;

    private void Start()
    {
        nameText.text = cardInfo.name;
        info.text = cardInfo.info;
        img.sprite = cardInfo.image;


        allColliders = FindObjectsOfType<Collider2D>();

        // oyun basi tüm colliderlar kapalý
       
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        firstPos = cardRect.position;
       
      /*  foreach (Collider2D collider in allColliders)
        if (colliders.Contains(collider))
        {
         collider.enabled = true;
        }
        else
        {
         collider.enabled = false;
        } */

      
    }

    public void OnDrag(PointerEventData eventData)
    {
        cardRect.position = eventData.position;      
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

 

        if (hit.collider != null && colliders.Contains(hit.collider))
        {
            if(hit.collider.GetComponent<GridCO>().isEmpty)
            {
                UIManager uiManager = FindObjectOfType<UIManager>();               
                uiManager.IncreaseCityHappiness(5); //þehir mutluluðu artsýn 

                hit.collider.GetComponent<GridCO>().isEmpty = false;
                Vector2 spawnPosition = hit.collider.bounds.center+Vector3.up*0.3f;
                Instantiate(cardInfo.prefab, spawnPosition, Quaternion.identity);
                Destroy(gameObject);
                return;
            }

            
        }
        else if (hit.collider != null)
        {
            UIManager uiManager = FindObjectOfType<UIManager>();              
            uiManager.DecreaseCityHappiness(5); //þehrin mutluluðu azalsýn

            //can azalsýn
            UIManager.health--;
            FindObjectOfType<UIManager>().UpdateHearts();

        }
        else
        {
            //Eðer kart ekrana atýlmadýysa ve kart daha önce býrakýlmadýysa, tüm colliderlarý etkinleþtir
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = true;
            }
        }

        cardRect.position = firstPos;
      
    }

 
     

}


    

