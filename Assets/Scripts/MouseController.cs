using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //for left click
        {
            CheckMouseClick(0);
        }
        if (Input.GetMouseButtonDown(1)) //for right mouse click
        {
            CheckMouseClick(1);
        }
        if (Input.GetMouseButtonDown(2)) //for mouse wheel click
        {
            CheckMouseClick(2);
        }

    }

    void CheckMouseClick(int mouseButton)
    {

    }
}
