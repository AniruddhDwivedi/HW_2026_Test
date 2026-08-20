using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip menuVideo;
    public VideoClip transitionVideo;
    public GameObject ui;
    public string gameScene;

    private bool transitioning = false;

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
        if (transitioning) return;
        transitioning = true;
        if (ui != null) ui.SetActive(false);

        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnMenuVideoFinished;
    }
    private void OnMenuVideoFinished(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnMenuVideoFinished;
        videoPlayer.clip = transitionVideo;
        videoPlayer.isLooping = false;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.prepareCompleted += OnTransitionPrepared;
        videoPlayer.Prepare();
    }

    private void OnTransitionPrepared(VideoPlayer vp)
    {
        videoPlayer.prepareCompleted -= OnTransitionPrepared;
        vp.Play();
        videoPlayer.loopPointReached += OnTransitionFinished;
    }

    private void OnTransitionFinished(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnTransitionFinished;
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnMenuVideoPrepared;
            videoPlayer.prepareCompleted -= OnTransitionPrepared;
            videoPlayer.loopPointReached -= OnTransitionFinished;
        }
    }
}