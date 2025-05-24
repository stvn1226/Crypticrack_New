using UnityEngine;
using TMPro;

public class SelectionHandler : MonoBehaviour
{
    public static SelectionHandler instance;
    
    public PegScript peg { get; private set; }
    
    [Header("Game Flow")]
    public GuessScript[] guesses = new GuessScript[8];
    public int guessIndex = 0;
    public int holeIndex = 0;
    public int hintIndex = 0;
    
    [Header("Game State")]
    public int[] secretCode = new int[4];
    public bool gameOver = false;
    public bool playerWon = false;

    [Header("UI")]
    public Transform secretCodeHolesParent;
    private TMP_Text[] secretCodeLabels;
    public GameObject secretCodeCover;
    public GameObject resultPanel;

    private HoleScript[] holes => guesses[guessIndex].holes;
    private HintScript[] hints => guesses[guessIndex].hints;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // We clean the start method with a simpler way / Way more optimal
    void Start()
    {
        GenerateSecretCode();
        InitializeSecretCodeDisplay();

        if (guesses.Length > 0 && guesses[0] != null)
        {
            CheckMarkers();
        }
    }

    public void GenerateSecretCode()
    {
        for (int i = 0; i < secretCode.Length; i++)
        {
            secretCode[i] = Random.Range(1, 7); // 1-6 peg colors
        }

        Debug.Log($"Secret code: {string.Join(", ", secretCode)}");
    }

    public void InitializeSecretCodeDisplay()
    {
        secretCodeLabels = new TMP_Text[secretCode.Length];

        for (int i = 0; i < secretCode.Length; i++)
        {
            Transform holeTransform = secretCodeHolesParent.Find($"SecretHole ({i})");
            if (holeTransform != null)
            {
                secretCodeLabels[i] = holeTransform.GetComponentInChildren<TMP_Text>();
                if (secretCodeLabels[i] != null)
                    secretCodeLabels[i].text = secretCode[i].ToString();
            }
        }

        //HideSecretCode(); // Optional
    }

    public void RevealSecretCode()
    {
        Debug.Log($"Secret code was: {string.Join(", ", secretCode)}");

        if (secretCodeCover != null)
            secretCodeCover.SetActive(false);
    }

    public void HideSecretCode()
    {
        // Call this when starting a new game
        if (secretCodeCover != null)
        {
            secretCodeCover.SetActive(true);
        }
    }

    public void TryGuess()
    {
        // Check if all holes in current guess are filled
        for (int i = 0; i < holes.Length; i++)
        {
            if (string.IsNullOrEmpty(holes[i].label.text))
            {
                Debug.Log("Fill all holes before confirming");
                return; // Not all holes are filled
            }
        }

        // Get current guess values
        int[] currentGuess = new int[holes.Length];
        for (int i = 0; i < holes.Length; i++)
        {
            int.TryParse(holes[i].label.text, out currentGuess[i]);
        }

        // Count correct position and color
        int correctPosition = 0;
        int correctColor = 0;

        // Make copies to avoid modifying originals
        bool[] secretCodeUsed = new bool[secretCode.Length];
        bool[] guessUsed = new bool[currentGuess.Length];

        for (int i = 0; i < secretCode.Length; i++)
        {
            if (currentGuess[i] == secretCode[i])
            {
                correctPosition++;
                secretCodeUsed[i] = true;
                guessUsed[i] = true;
            }
        }

        for (int i = 0; i < currentGuess.Length; i++)
        {
            if (guessUsed[i]) continue;
            for (int j = 0; j < secretCode.Length; j++)
            {
                if (secretCodeUsed[j]) continue;
                if (currentGuess[i] == secretCode[j])
                {
                    correctColor++;
                    secretCodeUsed[j] = true;
                    break;
                }
            }
        }

        // Update feedback display for current guess
        UpdateFeedback(correctPosition, correctColor);

        // Check for win condition
        if (correctPosition == secretCode.Length)
        {
            playerWon = true;
            gameOver = true;
            DisplayGameResult(true);
        }
        else
        {
            // Move to next guess or end game
            NextGuess();
        }
    }

    private void UpdateFeedback(int correctPosition, int correctColor)
    {
        // Implement this to update the feedback UI (the Square child of GuessHoriz)
        // You could use text or colored dots to show the feedback
        Debug.Log($"Feedback: {correctPosition} correct position, {correctColor} correct color");

        hintIndex = 0;

        for (int i = 0; i < correctPosition && hintIndex < hints.Length; i++, hintIndex++)
        {
            hints[hintIndex].SetHintState(HintState.CorrectPosition);
        }

        for (int i = 0; i < correctColor && hintIndex < hints.Length; i++, hintIndex++)
        {
            hints[hintIndex].SetHintState(HintState.CorrectColor);
        }
    }

    // Optimize code
    public void NextGuess()
    {
        // Disable all markers on previous holes
        foreach (var hole in holes)
            hole.ToggleMarker();

        holeIndex = 0;
        guessIndex++;

        if (guessIndex >= guesses.Length)
        {
            gameOver = true;
            DisplayGameResult(false);
            return;
        }

        CheckMarkers(); // Show marker on first hole of next guess
    }

    public void DisplayGameResult(bool won)
    {
        RevealSecretCode();

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
            TMP_Text resultText = resultPanel.GetComponentInChildren<TMP_Text>();
            if (resultText != null)
            {
                resultText.text = won ?
                    "Congratulations! You cracked the code!" :
                    "Game Over! The code remains unbroken.";
            }
        }
    }

    public void HideGameResult()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }


    public void FillHole(int value, Color colorTxt, Color colorBg)
    {
        if (holeIndex >= holes.Length || gameOver) return;

        holes[holeIndex].OnSelect(value, colorTxt, colorBg);
        holeIndex++;
        CheckMarkers();
    }

    public void CheckMarkers()
    {
        foreach (var hole in holes)
        {
            if (hole != null)
                hole.ToggleMarker();
        }
    }
    
    public void TryGuess(int currGuessIndex, int[] answer)
    {
        Debug.Log("Try guess.");
    }
    
    public void NextGuess(int currGuessIndex)
    {
        Debug.Log("Next guess.");
    }
}
