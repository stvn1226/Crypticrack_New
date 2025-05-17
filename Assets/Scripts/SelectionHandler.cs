using System;
using TMPro;
using UnityEngine;

[Serializable]
public class HoleRow
{
    public HoleScript[] slots;
    public TMP_Text feedback;
}

public class SelectionHandler : MonoBehaviour
{
    public static SelectionHandler Instance { get; private set; }

    [SerializeField] HoleRow[] rows;          // ≤ 8 rows, each with 4 HoleScripts

    int rowIndex;                             // which row we’re filling (0-based)
    int slotIndex;                            // next slot inside that row

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    /* ---------- called by PegScript ---------- */
    public void PushColour(int value, Color txtTint, Color bgTint)
    {
        if (rowIndex >= rows.Length) return;              // board full

        HoleScript[] cur = rows[rowIndex].slots;
        if (slotIndex >= cur.Length) return;              // row already full

        cur[slotIndex].Fill(value, txtTint, bgTint);
        slotIndex++;
    }

    /* ---------- helpers used by UI / game ---------- */

    public bool RowIsFull()
    {
        if (rowIndex >= rows.Length) return false;
        return slotIndex >= rows[rowIndex].slots.Length;
    }

    public int[] GetValues()          // returns the current row’s int[4]
    {
        HoleScript[] cur = rows[rowIndex].slots;
        int[] g = new int[cur.Length];
        for (int i = 0; i < cur.Length; i++) g[i] = cur[i].GetValue();
        return g;
    }

    public bool AdvanceRow()          // moves to next row; returns false if none left
    {
        rowIndex++;
        slotIndex = 0;
        return rowIndex < rows.Length;
    }

    public void ClearCurrentRowVisuals()
    {
        foreach (var h in rows[rowIndex].slots) h.Clear();
        slotIndex = 0;
    }
    
    public TMP_Text GetCurrentRowFeedback() => 
        rowIndex < rows.Length ? rows[rowIndex].feedback : null;
}
