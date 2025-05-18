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
    private Image[] hintImages;

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
        hintImages = GetComponentsInChildren<Image>();

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
    /// Updates the hint colors based on the guess results
    /// </summary>
    /// <param name="correctPosition">Number of pegs with correct color and position</param>
    /// <param name="correctColor">Number of pegs with correct color but wrong position</param>
    public void UpdateHints(int correctPosition, int correctColor)
    {
        // Log the results for debugging
        Debug.Log($"HintScript: Updating hints for guess {guessIndex} - Correct position: {correctPosition}, Correct color: {correctColor}");

        // Make sure we have enough hint images
        if (hintImages == null || hintImages.Length < 4)
        {
            Debug.LogError("HintScript: Not enough hint images found!");
            return;
        }

        // Stop all running animations if we're animating
        if (animateColorChanges)
        {
            StopAllCoroutines();
        }

        // Reset all hints to the 'incorrect' color first
        for (int i = 0; i < hintImages.Length; i++)
        {
            if (animateColorChanges)
            {
                StartCoroutine(AnimateColorChange(hintImages[i], incorrectColor));
            }
            else
            {
                hintImages[i].color = incorrectColor;
            }
        }

        int hintIndex = 0;

        // Set green hints first (correct position and color)
        for (int i = 0; i < correctPosition && hintIndex < hintImages.Length; i++)
        {
            if (animateColorChanges)
            {
                StartCoroutine(AnimateColorChange(hintImages[hintIndex], correctPositionColor));
            }
            else
            {
                hintImages[hintIndex].color = correctPositionColor;
            }
            hintIndex++;
        }

        // Set red hints next (correct color but wrong position)
        for (int i = 0; i < correctColor && hintIndex < hintImages.Length; i++)
        {
            if (animateColorChanges)
            {
                StartCoroutine(AnimateColorChange(hintImages[hintIndex], correctColorColor));
            }
            else
            {
                hintImages[hintIndex].color = correctColorColor;
            }
            hintIndex++;
        }

        // The remaining hints stay gray (already set above)
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
        if (hintImages == null) return;

        foreach (Image hint in hintImages)
        {
            hint.color = incorrectColor;
        }
    }

    // Optional: Accessibility feature for colorblind mode
    public void EnableColorblindMode(bool enable)
    {
        // If implementing colorblind mode, you could add special patterns or shapes
        // to the hint images instead of just relying on colors
    }
}
