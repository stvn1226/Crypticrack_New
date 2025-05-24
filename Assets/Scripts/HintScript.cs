using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum HintState
{
    Empty,
    CorrectPosition,
    CorrectColor
}

public class HintScript : MonoBehaviour
{
    [Header("Hint Colors")]
    public Color correctPositionColor = new Color(0.0f, 0.8f, 0.0f); // Green
    public Color correctColorColor = new Color(0.8f, 0.0f, 0.0f);     // Red
    public Color incorrectColor = new Color(0.58f, 0.58f, 0.58f);     // Gray

    [Header("Hint Image")]
    public Image hintImage;

    [Header("Animation")]
    public bool animateColorChanges = false;
    public float animationDuration = 0.3f;

    public int id;

    private Coroutine currentAnim;

    private void Awake()
    {
        if (hintImage == null)
            hintImage = GetComponent<Image>();

        ResetHint();
    }

    public void SetHintState(HintState state)
    {
        if (hintImage == null) return;

        Color targetColor = incorrectColor;

        switch (state)
        {
            case HintState.CorrectPosition:
                targetColor = correctPositionColor;
                break;
            case HintState.CorrectColor:
                targetColor = correctColorColor;
                break;
            case HintState.Empty:
                targetColor = incorrectColor;
                break;
        }

        if (animateColorChanges)
        {
            if (currentAnim != null) StopCoroutine(currentAnim);
            currentAnim = StartCoroutine(AnimateColorChange(targetColor));
        }
        else
        {
            hintImage.color = targetColor;
        }
    }

    public void ResetHint()
    {
        SetHintState(HintState.Empty);
    }

    private IEnumerator AnimateColorChange(Color targetColor)
    {
        Color startColor = hintImage.color;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            hintImage.color = Color.Lerp(startColor, targetColor, elapsed / animationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        hintImage.color = targetColor;
    }
}