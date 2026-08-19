using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;

    [Header("Level Goal")]
    public int winScore = 50;

    private bool levelWon = false;

    public void AddScore()
    {
        if (levelWon)
            return;

        score++;

        Debug.Log("Platform score: " + score);

        if (score >= winScore)
        {
            WinLevel();
        }
    }

    private void WinLevel()
    {
        levelWon = true;

        Debug.Log("LEVEL WON!");

        LevelManager levelManager =
            FindFirstObjectByType<LevelManager>();

        if (levelManager != null)
        {
            levelManager.WinLevel();
        }
    }
}