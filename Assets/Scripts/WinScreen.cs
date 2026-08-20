using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class WinScreen : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip menuVideo;
    public GameObject ui;
    public string gameScene;

    void Start()
    {
        videoPlayer.clip = menuVideo;
        videoPlayer.isLooping = true;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.prepareCompleted += OnMenuVideoPrepared;
        videoPlayer.Prepare();
    }

    private void OnMenuVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
        videoPlayer.prepareCompleted -= OnMenuVideoPrepared;
    }

    public void PlayGame()
    {
        if (ui != null) ui.SetActive(false);
        
        videoPlayer.Stop();
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}