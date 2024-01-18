using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.EventSystems;

public class Cursor : MonoBehaviour
{
    public Sprite cursorUp;
    public Sprite cursorDown;

    public Image imageComponent;
    public GameObject im;

    void Start()
    {

        

    }




    void Update()
    {
        this.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            im.GetComponent<Image>().sprite = cursorDown;
        }

        if (Input.GetMouseButtonUp(0))
        {
            im.GetComponent<Image>().sprite = cursorUp;

        }



    }




}
