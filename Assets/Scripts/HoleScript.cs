using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HoleScript : MonoBehaviour
{
    public int id;
    public TMP_Text label;
    public Image img;
    public Color empty;
    public Image marker;
    public Button button;
    
    void Start()
    {
        empty = img.color;
        marker.gameObject.SetActive(false);
        SelectionHandler.instance.CheckMarkers();

        OnDisableButton();
    }

    public void OnSelect(int value, Color colorText, Color colorBg)
    {
        // transform.localScale = Vector3.one * 1.8f;
        transform.DOScale(Vector3.one * 1.8f, 0.2f);    
        label.text = value.ToString();
        label.color = colorText;
        img.color = colorBg;
        marker.transform.localScale = Vector3.one * 1f;
        SelectionHandler.instance.CheckMarkers();
    }

    public void OnClear()
    {
        transform.localScale = Vector3.one * 1;
        label.text = "";
        img.color = empty;
        marker.transform.localScale = Vector3.one * 1;
        SelectionHandler.instance.holeIndex = id;
        marker.gameObject.SetActive(false);
        SelectionHandler.instance.CheckMarkers();
    }

    public void OnWipe()
    {
        transform.localScale = Vector3.one * 1;
        label.text = "";
        img.color = empty;
        marker.transform.localScale = Vector3.one * 1;
        marker.gameObject.SetActive(false);
        SelectionHandler.instance.CheckMarkers();
    }

    public void OnDisableButton()
    {
        button.interactable = false;
    }

    public void OnEnableButton()
    {
        button.interactable = true;
    }

    public void ToggleMarker()
    {
        if (SelectionHandler.instance.holeIndex == id)
        {
            marker.gameObject.SetActive(true);
        }
        else
        {
            marker.gameObject.SetActive(false);
        }
    }

    public void DisableMarker()
    { 
        marker.gameObject.SetActive(false);
    }
}
