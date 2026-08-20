using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public string loseScene;
    public string winScene;

    [Header("Lose Condition")]
    public float loseHeight = -10f;

    [Header("Score Tracking")]
    public TMP_Text ScoreText;

    private PlayerScore player;
    private bool gameEnded = false;
    private int currScore = 0;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerScore>();

        if (ScoreText != null)
        {
            ScoreText.gameObject.SetActive(true);
            ScoreText.text = "Score: 0";
        }
    }

    private void Update()
    {
        if (gameEnded || player == null)
            return;

        if (player.score != currScore)
        {
            currScore = player.score;

            if (ScoreText != null)
            {
                ScoreText.text = "Score: " + currScore;
            }
        }

        if (player.transform.position.y <= loseHeight)
        {
            LoseLevel();
        }
    }

    public void WinLevel()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        SceneManager.LoadScene(winScene);
    }

    private void LoseLevel()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        SceneManager.LoadScene(loseScene);
    }
}