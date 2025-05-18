using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessScript : MonoBehaviour
{
    // This script is used to identify each guess row
    // You can extend it with additional functionality like highlighting active rows

    public int guessNumber; // The index of this guess row

    // Highlight the current active row
    public void SetActive(bool active)
    {
        // Optional: Change color or add indicator to show this is the active row
        // For example, add a small arrow or highlight
        Transform highlightIndicator = transform.Find("ActiveIndicator");
        if (highlightIndicator != null)
        {
            highlightIndicator.gameObject.SetActive(active);
        }
    }
}
