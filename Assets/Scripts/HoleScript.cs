using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HoleScript : MonoBehaviour
{
    public int id;
    public TMP_Text label;
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        label = GetComponentInChildren<TMP_Text>(true);
    }

    public void OnSelect(int value, Color colorText, Color colorBg)
    {
        transform.localScale = Vector3.one * 2;
        label.text = value.ToString();
        label.color = colorText;
        img.color = colorBg;
    }

    public void OnClear()
    {
        transform.localScale = Vector3.one * 1;
        label.text = "";
        img.color = Color.white;
        SelectionHandler.instance.holeIndex = id;
    }
}
