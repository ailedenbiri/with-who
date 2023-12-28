using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "CardInfo")]
public class CardInfo : ScriptableObject
{
    public string cardName;
    public Sprite image;
    public GameObject prefab;
    public Sprite cardSprite;
    public Sprite headSprite;
}
