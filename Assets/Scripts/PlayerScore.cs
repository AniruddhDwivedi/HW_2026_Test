using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;

    public void AddScore()
    {
        score++;

        Debug.Log("Platform score: " + score);
    }
}