using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class GridCO : MonoBehaviour
{

    public bool isEmpty = true;
    public Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }


  
}






