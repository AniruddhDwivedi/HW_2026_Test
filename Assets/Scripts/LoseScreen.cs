using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    public GameObject ui;
    public string gameScene;

    public void PlayGame()
    {
        if (ui != null) ui.SetActive(false);
        
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
