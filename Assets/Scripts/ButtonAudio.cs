using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerEnterHandler
{
    public AudioManager audioManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioManager != null)
        {
            audioManager.PlayHover();
        }
    }

    public void PlayClick()
    {
        if (audioManager != null)
        {
            audioManager.PlayClick();
        }
    }
}