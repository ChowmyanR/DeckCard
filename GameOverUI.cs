using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text player1ScoreText;     
    public TMP_Text player2ScoreText;
    public TMP_Text winnerText;

    void Start()
    {
        GameManager gm = GameManager.Instance;       // GETTING THE FINAL SCORES FROM THE GAME MANAGER AND DISPLAYING THEM

        if (gm == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        int p1 = gm.player1.score;
        int p2 = gm.player2.score;

        player1ScoreText.text =
            "Player 1 Score: " + p1;

        player2ScoreText.text =
            "Player 2 Score: " + p2;

        if (p1 > p2)                                                    // CONTIDION THAT DECIDES THE WINNER BY THE RESULT OF THE SCORES
            winnerText.text = "Player 1 Wins!";
        else if (p2 > p1)                                               // THE SCORE WILL BE DISPLAYED ON THE GAME OVER SCENE AND THE WINNER WILL BE ANNOUNCED
            winnerText.text = "Player 2 Wins!";
        else
            winnerText.text = "Match Draw!";
    }

    public void OnRematch()                                             // REMATCH BUTTON WILL RELOAD TO THE GAME SCENE FOR A NEW MATCH
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnMainMenu()                                            // MAIN MENU OPTION WILL GOES TO THE START SCENE
    {
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("StartScene");
    }
}
