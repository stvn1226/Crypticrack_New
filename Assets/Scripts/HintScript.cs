using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintScript : MonoBehaviour
{
    // Color definitions for the hints
    [Header("Hint Colors")]
    [Tooltip("Color for correct value in correct position")]
    public Color correctPositionColor = new Color(0.0f, 0.8f, 0.0f); // Green

    [Tooltip("Color for correct value in wrong position")]
    public Color correctColorColor = new Color(0.8f, 0.0f, 0.0f); // Red

    [Tooltip("Color for incorrect value")]
    public Color incorrectColor = new Color(0.58f, 0.58f, 0.58f); // Gray

    // References to the hint image components
    public Image hintImage;

    // Hint's identifier
    public int id;

    // Animation settings
    [Header("Animation Settings")]
    [Tooltip("Should the hint color changes be animated?")]
    public bool animateColorChanges = true;

    [Tooltip("Duration of the color change animation in seconds")]
    public float animationDuration = 0.5f;

    // Reference to the guess this hint belongs to
    private int guessIndex;

    private void Awake()
    {
        // Get all hint image components
        hintImage = GetComponent<Image>();

        // Find the guess index by traversing up the hierarchy
        Transform guessTransform = transform.parent;
        if (guessTransform != null && guessTransform.name.StartsWith("GuessHoriz"))
        {
            string name = guessTransform.name;
            if (name == "GuessHoriz")
                guessIndex = 0;
            else
            {
                string indexStr = name.Substring(name.IndexOf('(') + 1);
                indexStr = indexStr.Substring(0, indexStr.IndexOf(')'));
                int.TryParse(indexStr, out guessIndex);
            }
        }
    }

    /// <summary>
    /// Animates the transition from the current color to the target color
    /// </summary>
    private IEnumerator AnimateColorChange(Image image, Color targetColor)
    {
        Color startColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            image.color = Color.Lerp(startColor, targetColor, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is exactly what we want
        image.color = targetColor;
    }

    /// <summary>
    /// Reset the hints to their initial state (all gray)
    /// </summary>
    public void ResetHints()
    {
        if (hintImage == null) return;

        //foreach (Image hint in hintImage)
        //{
        //    hint.color = incorrectColor;
        //}
    }

    // Optional: Accessibility feature for colorblind mode
    public void EnableColorblindMode(bool enable)
    {
        // If implementing colorblind mode, you could add special patterns or shapes
        // to the hint images instead of just relying on colors
    }
}
