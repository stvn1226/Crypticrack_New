using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PegScript : MonoBehaviour
{
    
    public TMP_Text value;
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        value = GetComponentInChildren<TMP_Text>(true);
    }

    public void OnClick()
    {
        if (!int.TryParse(value.text, out int valueInt))
        {
            Debug.LogWarning($"{name}: value.text is not an int!");
            return;
        }

        Debug.Log("Clicked! " + value.text);

        SelectionHandler.instance.FillHole(valueInt, value.color, img.color);
    }
}
