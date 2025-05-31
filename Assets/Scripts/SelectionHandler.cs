using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class SelectionHandler : MonoBehaviour
{
    public static SelectionHandler instance;
    
    public PegScript peg { get; private set; }
    
    public PegScript[] pegs = new PegScript[6];
    
    [Header("Game Flow")]
    public GuessScript[] guesses = new GuessScript[8];
    public int guessIndex = 0;
    public int holeIndex = 0;
    public int hintIndex = 0;
    
    public HiddenAnswerSlot[] hiddenAnswers = new HiddenAnswerSlot[4];
    
    [Header("Game State")]
    public int[] secretCode = new int[4];
    public bool gameOver = false;
    public bool playerWon = false;

    //[Header("UI")]
    public Transform secretCodeHolesParent;
    public GameObject secretCodeLabelParent;
    private TMP_Text secretCodeLabel;
    public GameObject secretCodeCover;
    public GameObject resultPanel;

    private HoleScript[] holes => guesses[guessIndex].holes;
    private HintScript[] hints => guesses[guessIndex].hints;
    
    private Dictionary<int, PegScript> pegDict = new Dictionary<int, PegScript>();
    
    public RectTransform coverRect;

    public Vector3 initialPoint;
    public RectTransform endPoint;
    
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
        secretCodeLabel = secretCodeLabelParent.GetComponentInChildren<TMP_Text>();
        initialPoint = coverRect.position;
        
        // We register Pegs in a dictionary to copy the data in the answer stuff
        for (int i = 0; i < pegs.Length; i++)
        {
            // print($"Peg Index: {i}");
            pegDict.Add(i, pegs[i]);;
        }
        
        GenerateSecretCode();

        //Secret Code Display is for testing purposes, to see the code during gameplay
        InitializeSecretCodeDisplay();

        if (guesses.Length > 0 && guesses[0] != null)
        {
            EnableColumn();
            CheckMarkers();
        }
    }

    public PegScript GetPeg(int id)
    {
        if (pegDict.TryGetValue(id, out PegScript peg))
        {
            print($"Peg Index: {id}");
            return peg;
        }
        else
        {
            Debug.LogError($"Peg with id {id} not found!");
            return null;
        }
    }

    public void GenerateSecretCode()
    {
        for (int i = 0; i < secretCode.Length; i++)
        {
            secretCode[i] = Random.Range(1, 7); // 1-6 peg colors
        }

        for (int i = 0; i < secretCode.Length; i++)
        {
            int secretDigit = secretCode[i];
            // Is needed to pass the secretDigit - 1 because in dictionary we register values from 0 to 5
            // In secretDigits we register from 1 to 6, to make the values be in the range, we should take 1 (x - 1)
            hiddenAnswers[i].OnShowAnswer(secretDigit, GetPeg(secretDigit-1).GetTextColor(), GetPeg(secretDigit-1).GetBgColor());
        }

        Debug.Log($"Secret code: {string.Join(", ", secretCode)}");
    }

    public void InitializeSecretCodeDisplay()
    {
        string secretCodeString = "";

        foreach (int p in secretCode)
        {
            secretCodeString += p + " ";
        }

        secretCodeLabel.text = secretCodeString;
        secretCodeLabelParent.GetComponent<Image>().color = Color.blue;

        foreach (int q in secretCode)
        {
            int ct = 0;
            foreach (int i in secretCode)
            {
                if (q == i)
                {
                    ct++;
                }

                if (ct > 1)
                {
                    secretCodeLabelParent.GetComponent<Image>().color = Color.red;
                    break;
                }
            }
        }
    }

    public void RevealSecretCode()
    {
        Debug.Log($"Secret code was: {string.Join(", ", secretCode)}");

        StartCoroutine(AnimateSecretReveal());
        
        // if (secretCodeCover != null)
        //     secretCodeCover.SetActive(false);
    }

    IEnumerator AnimateSecretReveal()
    {
        float maxAnimTime = .3f;
        for (float i = 0; i < maxAnimTime; i += Time.deltaTime)
        {
            coverRect.position = Vector3.Lerp(initialPoint, endPoint.position, i / maxAnimTime);
            yield return null;
        }
        coverRect.position = endPoint.position;
        yield return null;
    }
    
    IEnumerator AnimateSecretHide()
    {
        float maxAnimTime = .05f;
        for (float i = 0; i < maxAnimTime; i += Time.deltaTime)
        {
            coverRect.position = Vector3.Lerp(endPoint.position, initialPoint, i / maxAnimTime);
            yield return null;
        }
        coverRect.position = initialPoint;
        yield return null;

        OnResetAnswer();
    }

    public void HideSecretCode()
    {
        // Call this when starting a new game
        // if (secretCodeCover != null)
        // {
        //     secretCodeCover.SetActive(true);
        // }
        StartCoroutine(AnimateSecretHide());
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

        foreach (var hole in holes)
        {
           hole.OnDisableButton();
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
            hole.DisableMarker();

        holeIndex = 0;
        guessIndex++;

        if (guessIndex >= guesses.Length)
        {
            gameOver = true;
            DisplayGameResult(false);
            return;
        }

        EnableColumn();
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
                    "Game Over! The code remains infallible.";
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

    public void EnableColumn()
    {
        foreach (var hole in holes)
        {
            hole.OnEnableButton();   
        }
    }
    

    public void CheckMarkers()
    {
        foreach (var hole in holes)
        {
            if (hole != null)
                hole.ToggleMarker();
        }
    }


    public void OnResetAnswer()
    {
        GenerateSecretCode();
        InitializeSecretCodeDisplay();
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
