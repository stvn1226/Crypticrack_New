using UnityEngine;
using TMPro;

public class MastermindGame : MonoBehaviour
{
    public static MastermindGame I { get; private set; }

    [Header("Secret combination (size 4)")]
    [SerializeField] int[] secret = { 5, 3, 2, 1 };

    [SerializeField] SelectionHandler combMgr;

    void Awake() => I = this;

    public void CheckCurrentRow()
    {
        int[] guess = combMgr.GetValues();
        (int exact, int misplaced) = Evaluate(secret, guess);

        string line = $"Exact: {exact} | Misplaced: {misplaced}";
        Debug.Log(line);

        // --- row-local feedback ---
        TMP_Text rowFb = combMgr.GetCurrentRowFeedback();
        if (rowFb) rowFb.text = line;

        // --- global win/lose handling ---
        if (exact == secret.Length)
        {
            Debug.Log("🎉 YOU WIN!");
            if (rowFb) rowFb.text += "  —  YOU WIN!";
        }
        else
        {
            if (!combMgr.AdvanceRow())
            {
                Debug.Log("❌ No rows left—You lose.");
                if (rowFb) rowFb.text += "  —  YOU LOSE";
            }
        }
    }


    /* ---------- evaluation algorithm ---------- */
    static (int exact, int misplaced) Evaluate(int[] s, int[] g)
    {
        int len = s.Length, exact = 0, misplaced = 0;
        bool[] usedS = new bool[len], usedG = new bool[len];

        // pass 1: exact matches
        for (int i = 0; i < len; i++)
        {
            if (g[i] == s[i]) { exact++; usedS[i] = usedG[i] = true; }
        }
        // pass 2: colour exists elsewhere
        for (int i = 0; i < len; i++)
        {
            if (usedG[i]) continue;
            for (int j = 0; j < len; j++)
            {
                if (usedS[j]) continue;
                if (g[i] == s[j])
                {
                    misplaced++; usedS[j] = true; break;
                }
            }
        }
        return (exact, misplaced);
    }
}