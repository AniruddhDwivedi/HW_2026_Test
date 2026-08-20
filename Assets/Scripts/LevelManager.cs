using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public string loseScene;
    public string winScene;
    public string mainMenuScene;

    [Header("Lose Condition")]
    public float loseHeight = -10f;

    [Header("Score Tracking")]
    public TMP_Text ScoreText;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    public TMP_InputField winScoreInput;

    [Header("Input")]
    [SerializeField] private InputActionReference pauseAction;

    private PlayerScore player;

    private bool gameEnded = false;
    private bool isPaused = false;
    private int currScore = 0;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerScore>();
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPausePerformed;
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePerformed;
            pauseAction.action.Disable();
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = FindFirstObjectByType<PlayerScore>();

        if (ScoreText != null)
        {
            ScoreText.gameObject.SetActive(true);
            ScoreText.text = "Score: 0";
        }

        if (pausePanel != null) pausePanel.SetActive(false);

        if (winScoreInput != null && player != null) winScoreInput.text = player.winScore.ToString();
        
    }

    private void Update()
    {
        if (gameEnded || player == null) return;

        if (player.score != currScore)
        {
            currScore = player.score;

            if (ScoreText != null) ScoreText.text = "Score: " + currScore;
        }

        if (player.transform.position.y <= loseHeight) LoseLevel();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        if (gameEnded) return;
        if (isPaused) ResumeGame();
        else PauseGame();
        
    }

    public void PauseGame()
    {
        if (gameEnded) return;

        isPaused = true;

        if (pausePanel != null) pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null) pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void SetWinScore()
    {
        if (player == null || winScoreInput == null) return;

        if (int.TryParse(winScoreInput.text, out int newScore))
        {
            newScore = Mathf.Max(1, newScore);
            player.winScore = newScore;
            winScoreInput.text = newScore.ToString();

            Debug.Log("Win score changed to: " + newScore);
        }
        else
        {
            winScoreInput.text = player.winScore.ToString();
            Debug.LogWarning("Invalid win score entered.");
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void WinLevel()
    {
        if (gameEnded) return;

        gameEnded = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(winScene);
    }

    private void LoseLevel()
    {
        if (gameEnded) return;

        gameEnded = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(loseScene);
    }
}