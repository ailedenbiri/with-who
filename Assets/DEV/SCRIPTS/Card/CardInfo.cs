using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "CardInfo")]
public class CardInfo : ScriptableObject
{
    public string name;
    public string info;
    public Sprite image;
    public GameObject prefab;
}
