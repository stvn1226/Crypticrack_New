using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public PegScript peg { get; private set; }
    public GuessScript[] guesses = new GuessScript[8];
    public int guessIndex = 0;
    public HoleScript[] holes = new HoleScript[4];
    public int holeIndex = 0;
    public HintScript[] hints = new HintScript[4];
    public int hintIndex = 0;

    public static SelectionHandler instance;

    public int[] secretCode = new int[4];
    public bool gameOver = false;
    public bool playerWon = false;

    public Transform secretCodeHolesParent;
    private TMPro.TMP_Text[] secretCodeLabels;
    public GameObject secretCodeCover;
    public GameObject resultPanel;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateSecretCode();
        InitializeSecretCodeDisplay();
        if (guesses.Length > 0 && guesses[0] != null)
        {
            holes = guesses[0].GetComponentsInChildren<HoleScript>();
            for (int i = 0; i < holes.Length; i++)
            {
                if (holes[i] != null)
                {
                    holes[i].id = i;
                }
            }
        }

    }

    public void GenerateSecretCode()
    {
        for (int i = 0; i < secretCode.Length; i++)
        {
            secretCode[i] = UnityEngine.Random.Range(1, 7); // 1-6 for six peg colors
        }
        Debug.Log("Secret code generated");
        //Debug.Log(secretCode[0] + " " + secretCode[1] + " " + secretCode[2] + " " + secretCode[3]);

        // Uncomment for debugging
        Debug.Log($"Secret code: {string.Join(", ", secretCode)}");
    }

    public void InitializeSecretCodeDisplay()
    {
        // Get references to the secret code labels
        secretCodeLabels = new TMPro.TMP_Text[secretCode.Length];

        for (int i = 0; i < secretCode.Length; i++)
        {
            Transform holeTransform = secretCodeHolesParent.Find($"SecretHole ({i})");
            if (holeTransform != null)
            {
                secretCodeLabels[i] = holeTransform.GetComponentInChildren<TMPro.TMP_Text>();

                // Update the labels with the secret code values
                if (secretCodeLabels[i] != null)
                {
                    secretCodeLabels[i].text = secretCode[i].ToString();
                }
            }
        }

        // Hide the secret code for gameplay
        //HideSecretCode();
    }

    public void RevealSecretCode()
    {
        Debug.Log($"Secret code was: {string.Join(", ", secretCode)}");

        // Call this when the game ends
        if (secretCodeCover != null)
        {
            secretCodeCover.SetActive(false);
        }

        // Optional: You could add animation here to reveal the code dramatically
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

        // First check for correct positions
        for (int i = 0; i < secretCode.Length; i++)
        {
            if (currentGuess[i] == secretCode[i])
            {
                correctPosition++;
                secretCodeUsed[i] = true;
                guessUsed[i] = true;
            }
        }

        // Then check for correct colors in wrong positions
        for (int i = 0; i < secretCode.Length; i++)
        {
            if (guessUsed[i]) continue; // Skip already matched positions

            for (int j = 0; j < secretCode.Length; j++)
            {
                if (secretCodeUsed[j]) continue; // Skip already matched positions

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
            return;
        }

        // Move to next guess or end game
        NextGuess();
    }

    private void UpdateFeedback(int correctPosition, int correctColor)
    {
        // Implement this to update the feedback UI (the Square child of GuessHoriz)
        // You could use text or colored dots to show the feedback
        Debug.Log($"Feedback: {correctPosition} correct position, {correctColor} correct color");

        hintIndex = 0;

        //for (int i = 0; i < hints.Length; i++)
        //{
            for(int p = 0; p < correctPosition; p++)
            {
                hints[hintIndex].transform.localScale = Vector3.one * 1.5f;
                hints[hintIndex].hintImage.color = hints[hintIndex].correctPositionColor;
                hintIndex++;
            }
            for (int c = 0;  c < correctColor; c++)
            {
                hints[hintIndex].transform.localScale = Vector3.one * 1.5f;
                hints[hintIndex].hintImage.color = hints[hintIndex].correctColorColor;
                hintIndex++;
            }

    }

    public void NextGuess()
    {
        // Clear current holes
        //for (int i = 0; i < holes.Length; i++)
        //{
        //    holes[i].OnClear();
        //}

        //disable all markers on previous guess;
        for (int i = 0; i < holes.Length; i++)
        {
            holes[i].ToggleMarker(holes.Length);
        }


        // Reset hole index
        holeIndex = 0;

        // Move to next guess
        guessIndex++;

        // Check if we're out of guesses
        if (guessIndex >= guesses.Length)
        {
            gameOver = true;
            DisplayGameResult(false);
            return;
        }
        
        if (guesses[guessIndex] != null)
        {
            holes = guesses[guessIndex].GetComponentsInChildren<HoleScript>();
            hints = guesses[guessIndex].GetComponentsInChildren<HintScript>();
            for (int i = 0; i < holes.Length; i++)
            {
                if (holes[i] != null)
                {
                    holes[i].id = i;
                }
            }
            for (int j  = 0; j < hints.Length; j++)
            {
                if (hints[j] != null)
                {
                    hints[j].id = j;
                }
            }
        }
    }

    public void DisplayGameResult(bool won)
    {
        // Reveal the secret code
        RevealSecretCode();

        // Show result panel
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);

            // Update result text
            TMPro.TMP_Text resultText = resultPanel.GetComponentInChildren<TMPro.TMP_Text>();
            if (resultText != null)
            {
                resultText.text = won ?
                    "Congratulations! You cracked the code!" :
                    "Game Over! The code remains unbroken.";
            }
        }
    }


    public void FillHole(int value, Color colorTxt, Color colorBg)
    {
        Debug.Log("Peg chosen.");
        if (holeIndex >= holes.Length)
        {
            return;
        }
        //if (holes[holeIndex].label.text != "")
        //{
        //    holeIndex += 1;
        //}

        holes[holeIndex].OnSelect(value, colorTxt, colorBg);
        holeIndex += 1;
        //if (holeIndex > 3)
        //{
        //    holeIndex = 3;
        //}
        //holes[holeIndex - 1].ToggleMarker(holeIndex);
        //holes[holeIndex].ToggleMarker(holeIndex);
        CheckMarkers();
    }

    public void TryGuess(int currGuessIndex, int[] answer)
    {
        Debug.Log("Try guess.");
    }
    
    public void NextGuess(int currGuessIndex)
    {
        Debug.Log("Next guess.");
    }

    public void CheckMarkers()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            holes[i].ToggleMarker(holeIndex);
        }
    }
}
