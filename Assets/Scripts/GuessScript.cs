using UnityEngine;

public class GuessScript : MonoBehaviour
{
    [Header("Hole Group")]
    public HoleScript[] holes;

    [Header("Hint Group")]
    public HintScript[] hints;

    private void Awake()
    {
        // Set IDs for holes
        for (int i = 0; i < holes.Length; i++)
        {
            if (holes[i] != null)
                holes[i].id = i;
        }

        // Set IDs for hints
        for (int i = 0; i < hints.Length; i++)
        {
            if (hints[i] != null)
                hints[i].id = i;
        }
    }

    /// <summary>
    /// Clears all holes and resets hint visuals
    /// </summary>
    public void ResetGuess()
    {
        foreach (var hole in holes)
        {
            if (hole != null)
                hole.OnWipe();
        }

        foreach (var hint in hints)
        {
            if (hint != null)
                hint.ResetHint();
        }
    }
}