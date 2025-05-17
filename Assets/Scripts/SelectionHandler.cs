using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public PegScript peg { get; private set; }
    public HoleScript[] holes = new HoleScript[4];
    public int holeIndex = 0;

    public static SelectionHandler instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillHole(int value, Color colorTxt, Color colorBg)
    {
        Debug.Log("Peg chosen.");
        if (holeIndex >= holes.Length)
        {
            return;
        }
        //if (holes[holeIndex].label.text != "")
        //{
        //    holeIndex += 1;
        //}

        holes[holeIndex].OnSelect(value, colorTxt, colorBg);
        holeIndex += 1;
    }
}
