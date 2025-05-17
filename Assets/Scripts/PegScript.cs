using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PegScript : MonoBehaviour
{
    Button      btn;
    Image       img;
    private TMP_Text label;
    
    void Awake()
    {
        btn   = GetComponent<Button>();
        img   = GetComponent<Image>();
        label = GetComponentInChildren<TMP_Text>(true); // “true” searches inactive children too

        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (!int.TryParse(label.text, out int value))
        {
            Debug.LogWarning($"{name}: label.text is not an int!");
            return;
        }

        SelectionHandler.Instance.PushColour(value, label.color, img.color);
    }
}
