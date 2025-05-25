using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
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
            // Accede al grupo de hoyos directamente desde el Guess actual
            var currentGuess = SelectionHandler.instance.guesses[SelectionHandler.instance.guessIndex];
            foreach (var hole in currentGuess.holes)
            {
                hole.OnWipe();
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
        if (SelectionHandler.instance == null) return;

        SelectionHandler handler = SelectionHandler.instance;

        // Reset game state
        handler.guessIndex = 0;
        handler.holeIndex = 0;
        handler.gameOver = false;
        handler.playerWon = false;

        ClearAllGuesses();

        // Limpia el primer grupo
        if (handler.guesses.Length > 0 && handler.guesses[0] != null)
        {
            foreach (var hole in handler.guesses[0].holes)
            {
                hole.OnWipe();
            }
        }

        handler.HideGameResult();
        handler.HideSecretCode();
        
        // We add this to HideSecretCode() function, to assure
        // it will happen when we finish the animation (avoid visual problems)
        // handler.GenerateSecretCode();
        // handler.InitializeSecretCodeDisplay();
        
        handler.CheckMarkers();
    }

    public void QuitGame()
    {
        // Application.Quit();
        // We change the scene
    }

    private void ClearAllGuesses()
    {
        if (SelectionHandler.instance == null || SelectionHandler.instance.guesses == null)
            return;

        foreach (var guess in SelectionHandler.instance.guesses)
        {
            if (guess == null) continue;

            // Limpia hoyos y pistas directamente desde GuessScript
            guess.ResetGuess();

            // Opcional: limpia el texto de feedback si existe
            Transform squareTransform = guess.transform.Find("Square");
            if (squareTransform != null)
            {
                var feedbackText = squareTransform.GetComponentInChildren<TMPro.TMP_Text>();
                if (feedbackText != null)
                    feedbackText.text = "";
            }
        }
    }
}
