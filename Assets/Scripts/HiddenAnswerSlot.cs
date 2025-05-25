using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HiddenAnswerSlot : MonoBehaviour
{
    public TMP_Text label;
    public Image img;
    public Color emptyImg;
    
    void Start()
    {
        emptyImg = img.color;
    }

    public void OnShowAnswer(int value, Color colorText, Color colorBg)
    {
        transform.localScale = Vector3.one * 2;
        label.text = value.ToString();
        label.color = colorText;
        img.color = colorBg;
    }

    public void OnResetAnswer()
    {
        transform.localScale = Vector3.one * 1;
        label.text = "";
        img.color = emptyImg;
    }
}
