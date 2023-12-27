using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using System.Linq;
using DG.Tweening;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler

{


    [Header("Card Settings")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text info;
    [SerializeField] private Image img;
    [SerializeField] private RectTransform cardRect;

    private RectTransform cardImage;


    [Header("Ground Settings")]
    [SerializeField] private List<Collider2D> colliders;


    private Collider2D[] allColliders;

    private bool cardSelected = false;



    public CardInfo cardInfo;
    private Vector2 firstPos;

    private void Start()
    {
        nameText.text = cardInfo.cardName;
        info.text = cardInfo.info;
        img.sprite = cardInfo.image;
        transform.GetChild(0).GetComponent<Image>().sprite = cardInfo.cardSprite;

        cardImage = transform.GetChild(1).GetComponent<RectTransform>();


        allColliders = FindObjectsOfType<Collider2D>();

        // oyun basi tüm colliderlar kapalý

    }

    private void Update()
    {
        if (cardSelected)
        {
            if (Input.GetMouseButtonUp(0))
            {
                cardSelected = false;
                cardImage.gameObject.SetActive(false);
                GetComponentInChildren<CanvasGroup>().DOFade(1f, 0.3f);
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && colliders.Contains(hit.collider))
                {
                    if (hit.collider.GetComponent<GridCO>().isEmpty)
                    {

                        GameManager.i.CorrectCardPlaced();

                        UIManager uiManager = FindObjectOfType<UIManager>();
                        uiManager.IncreaseCityHappiness(5); //þehir mutluluðu artsýn 

                        hit.collider.GetComponent<GridCO>().isEmpty = false;
                        Vector2 spawnPosition = hit.collider.bounds.center + Vector3.up * 0.3f;
                        Instantiate(cardInfo.prefab, spawnPosition, Quaternion.identity);
                        GameObject p = Instantiate(GameAssets.i.characterPlacedParticle, spawnPosition, Quaternion.identity);
                        p.GetComponentInChildren<ParticleSystem>().Play();
                        Destroy(p.gameObject, 5f);
                        Destroy(gameObject);
                        return;
                    }
                }
                else if (hit.collider != null)
                {
                    UIManager uiManager = FindObjectOfType<UIManager>();
                    uiManager.DecreaseCityHappiness(5); //þehrin mutluluðu azalsýn

                    //can azalsýn
                    UIManager.i.health--;
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
                cardImage.position = firstPos;
            }
        }

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        firstPos = cardImage.position;
        cardSelected = true;
        cardImage.gameObject.SetActive(true);
        GetComponentInChildren<CanvasGroup>().DOFade(0f, 0.3f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        cardImage.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }


}




