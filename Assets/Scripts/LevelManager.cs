using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Lose Condition")]
    public float loseHeight = -10f;

    [Header("Win UI")]
    public TMP_Text winText;

    private bool gameEnded = false;

    private void Start()
    {
        // Always hide the win message when the level starts.
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameEnded)
            return;

        PlayerScore player =
            FindFirstObjectByType<PlayerScore>();

        if (player == null)
            return;

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

        Debug.Log("LEVEL WON!");

        // Show the win message.
        if (winText != null)
        {
            winText.gameObject.SetActive(true);
        }

        // Freeze the entire game.
        Time.timeScale = 0f;
    }

    private void LoseLevel()
    {
        gameEnded = true;

        Debug.Log("LEVEL LOST!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}