using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{

    void Start()
    {
    }

    public void Confirm()
    {
        if (SelectionHandler.instance != null && !SelectionHandler.instance.gameOver)
        {
            SelectionHandler.instance.TryGuess();
        }
    }

    public void ClearAll()
    {
        if (SelectionHandler.instance != null && !SelectionHandler.instance.gameOver)
        {
            // Clear all holes in current guess
            for (int i = 0; i < SelectionHandler.instance.holes.Length; i++)
            {
                SelectionHandler.instance.holes[i].OnClear();
            }
            SelectionHandler.instance.holeIndex = 0;
            SelectionHandler.instance.CheckMarkers();
        }
    }

    public void GiveUp()
    {
        if (SelectionHandler.instance != null && !SelectionHandler.instance.gameOver)
        {
            SelectionHandler.instance.gameOver = true;
            SelectionHandler.instance.playerWon = false;
            SelectionHandler.instance.DisplayGameResult(false); //player lost
        }
    }

    public void NewGame()
    {
        if (SelectionHandler.instance != null)
        {
            // Reset game state
            SelectionHandler.instance.guessIndex = 0;
            SelectionHandler.instance.holeIndex = 0;
            SelectionHandler.instance.gameOver = false;
            SelectionHandler.instance.playerWon = false;

            // Generate new secret code
            SelectionHandler.instance.GenerateSecretCode();

            // Clear all guesses
            // You need to implement this to reset all guess rows
            ClearAllGuesses();

            // Initialize first guess row
            if (SelectionHandler.instance.guesses.Length > 0 && SelectionHandler.instance.guesses[0] != null)
            {
                SelectionHandler.instance.holes = SelectionHandler.instance.guesses[0].GetComponentsInChildren<HoleScript>();
                for (int i = 0; i < SelectionHandler.instance.holes.Length; i++)
                {
                    if (SelectionHandler.instance.holes[i] != null)
                    {
                        SelectionHandler.instance.holes[i].id = i;
                        SelectionHandler.instance.holes[i].OnClear();
                    }
                }
            }
            // Update the secret code display
            SelectionHandler.instance.InitializeSecretCodeDisplay();
            SelectionHandler.instance.HideGameResult();
            SelectionHandler.instance.HideSecretCode();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ClearAllGuesses()
    {
        // Loop through all guess rows and clear them
        if (SelectionHandler.instance != null && SelectionHandler.instance.guesses != null)
        {
            for (int i = 0; i < SelectionHandler.instance.guesses.Length; i++)
            {
                if (SelectionHandler.instance.guesses[i] != null)
                {
                    // Clear all holes in this guess row
                    HoleScript[] holes = SelectionHandler.instance.guesses[i].GetComponentsInChildren<HoleScript>();
                    for (int j = 0; j < holes.Length; j++)
                    {
                        if (holes[j] != null)
                        {
                            holes[j].OnClear();
                        }
                    }

                    // Clear the feedback square
                    Transform squareTransform = SelectionHandler.instance.guesses[i].transform.Find("Square");
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
