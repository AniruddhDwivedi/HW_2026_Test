using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string nextScene;

    void Start()
    {
        StartCoroutine(WaitAndSwitchScene());
    }

    IEnumerator WaitAndSwitchScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(nextScene);
    }
}