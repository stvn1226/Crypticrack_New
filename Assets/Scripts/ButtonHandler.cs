using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    private SelectionHandler selectionHandler;

    void Start()
    {
        selectionHandler = SelectionHandler.instance;
    }

    public void Confirm()
    {
        if (selectionHandler != null && !selectionHandler.gameOver)
        {
            selectionHandler.TryGuess();
        }
    }

    public void ClearAll()
    {
        if (selectionHandler != null && !selectionHandler.gameOver)
        {
            // Clear all holes in current guess
            for (int i = 0; i < selectionHandler.holes.Length; i++)
            {
                selectionHandler.holes[i].OnClear();
            }
            selectionHandler.holeIndex = 0;
            selectionHandler.CheckMarkers();
        }
    }

    public void GiveUp()
    {
        if (selectionHandler != null && !selectionHandler.gameOver)
        {
            selectionHandler.gameOver = true;
            selectionHandler.playerWon = false;
            selectionHandler.DisplayGameResult(false); //player lost
        }
    }

    public void NewGame()
    {
        if (selectionHandler != null)
        {
            // Reset game state
            selectionHandler.guessIndex = 0;
            selectionHandler.holeIndex = 0;
            selectionHandler.gameOver = false;
            selectionHandler.playerWon = false;

            // Generate new secret code
            selectionHandler.GenerateSecretCode();

            // Clear all guesses
            // You need to implement this to reset all guess rows
            ClearAllGuesses();

            // Initialize first guess row
            if (selectionHandler.guesses.Length > 0 && selectionHandler.guesses[0] != null)
            {
                selectionHandler.holes = selectionHandler.guesses[0].GetComponentsInChildren<HoleScript>();
                for (int i = 0; i < selectionHandler.holes.Length; i++)
                {
                    if (selectionHandler.holes[i] != null)
                    {
                        selectionHandler.holes[i].id = i;
                        selectionHandler.holes[i].OnClear();
                    }
                }
            }
            // Update the secret code display
            selectionHandler.InitializeSecretCodeDisplay();

        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ClearAllGuesses()
    {
        // Loop through all guess rows and clear them
        if (selectionHandler != null && selectionHandler.guesses != null)
        {
            for (int i = 0; i < selectionHandler.guesses.Length; i++)
            {
                if (selectionHandler.guesses[i] != null)
                {
                    // Clear all holes in this guess row
                    HoleScript[] holes = selectionHandler.guesses[i].GetComponentsInChildren<HoleScript>();
                    for (int j = 0; j < holes.Length; j++)
                    {
                        if (holes[j] != null)
                        {
                            holes[j].OnClear();
                        }
                    }

                    // Clear the feedback square
                    Transform squareTransform = selectionHandler.guesses[i].transform.Find("Square");
                    if (squareTransform != null)
                    {
                        TMPro.TMP_Text feedbackText = squareTransform.GetComponentInChildren<TMPro.TMP_Text>();
                        if (feedbackText != null)
                        {
                            feedbackText.text = "";
                        }
                    }
                }
            }
        }
    }
}
