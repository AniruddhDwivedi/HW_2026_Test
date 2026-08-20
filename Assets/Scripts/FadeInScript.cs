using UnityEngine;
using TMPro;
using System.Collections;

public class SequentialTextFade : MonoBehaviour
{
    public TMP_Text[] texts;
    public int fadeFrames = 30;

    void Start()
    {
        foreach (TMP_Text text in texts)
        {
            Color color = text.color;
            color.a = 0f;
            text.color = color;
        }

        StartCoroutine(FadeTexts());
    }

    IEnumerator FadeTexts()
    {
        foreach (TMP_Text text in texts)
        {
            for (int frame = 0; frame < fadeFrames; frame++)
            {
                float alpha = (float)frame / (fadeFrames - 1);
                Color color = text.color;
                color.a = alpha;
                text.color = color;

                yield return null;
            }

            Color finalColor = text.color;
            finalColor.a = 1f;
            text.color = finalColor;
        }
    }
}