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

    void Start()
    {
        empty = img.color;
        marker.gameObject.SetActive(false);
        SelectionHandler.instance.CheckMarkers();
    }

    public void OnSelect(int value, Color colorText, Color colorBg)
    {
        transform.localScale = Vector3.one * 2;
        label.text = value.ToString();
        label.color = colorText;
        img.color = colorBg;
        marker.transform.localScale = Vector3.one * 0.5f;
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
}
