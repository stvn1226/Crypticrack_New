using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CheckButton : MonoBehaviour
{
    Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => MastermindGame.I.CheckCurrentRow());
    }

    void Update() => btn.interactable = SelectionHandler.Instance.RowIsFull();
}

