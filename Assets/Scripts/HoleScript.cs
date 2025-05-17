using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HoleScript : MonoBehaviour
{
    [SerializeField] TMP_Text label; // drag child Label here

    Image img;
    int storedValue = -1;

    void Awake() => img = GetComponent<Image>();

    public void Fill(int value, Color textTint, Color backgroundTint)
    {
        storedValue = value;
        img.color  = backgroundTint;
        label.color = textTint;
        label.text = value.ToString();

        transform.localScale = Vector3.one * 1.5f;
    }

    public int  GetValue() => storedValue;

    public void Clear()
    {
        storedValue = -1;
        img.color   = Color.white; // “empty” tint; tweak if desired
        label.text  = "";
    }
}
