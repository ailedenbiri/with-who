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

public class Card : MonoBehaviour, IPointerDownHandler, IDragHandler

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
        img.sprite = cardInfo.headSprite;

        cardImage = transform.GetChild(1).GetComponent<RectTransform>();
        cardImage.GetComponent<Image>().sprite = cardInfo.image;

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
                        Taptic.Medium();
                        GameManager.i.CorrectCardPlaced();

                        UIManager uiManager = FindObjectOfType<UIManager>();
                        uiManager.IncreaseCityHappiness(5); //þehir mutluluðu artsýn 

                        hit.collider.GetComponent<GridCO>().isEmpty = false;
                        Vector2 spawnPosition = hit.collider.bounds.center;
                        Instantiate(cardInfo.prefab, spawnPosition + Vector2.up * 0.3f, Quaternion.identity);
                        GameObject p = Instantiate(GameAssets.i.characterPlacedParticle, spawnPosition, Quaternion.identity);
                        p.GetComponentInChildren<ParticleSystem>().Play();
                        Destroy(p.gameObject, 5f);
                        Destroy(gameObject);
                        return;
                    }
                }
                else if (hit.collider != null)
                {
                    UIManager.i.DecreaseCityHappiness(5); //þehrin mutluluðu azalsýn

                    //can azalsýn
                    GameManager.i.CameraShake(0.2f, Vector3.one * 0.1f);
                    UIManager.i.health--;
                    UIManager.i.UpdateHearts();
                    GameAssets.i.CreateFailedMark(hit.collider.gameObject.transform.position);
                    Taptic.Medium();
                    if (UIManager.i.health == 0)
                    {
                        GameManager.i.isGamePlaying = false;
                        DOVirtual.DelayedCall(1.3f, () =>
                        {
                            Taptic.Heavy();
                            UIManager.i.losePanel.gameObject.SetActive(true);
                            Time.timeScale = 0f;
                            UIManager.i.losePanel.DOFade(1f, 0.8f).SetUpdate(true);

                        });

                    }
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
        if (GameManager.i.isGamePlaying)
        {
            firstPos = cardImage.position;
            cardSelected = true;
            cardImage.gameObject.SetActive(true);
            GetComponentInChildren<CanvasGroup>().DOFade(0f, 0.3f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        cardImage.position = eventData.position;
    }
}




