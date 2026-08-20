using UnityEngine;

public class PlatformScoreTrigger : MonoBehaviour
{
    private CollapsingPlatform platform;

    private void Awake()
    {
        platform = GetComponentInParent<CollapsingPlatform>();

        if (platform == null)
        {
            Debug.LogError(
                "PlatformScoreTrigger: Could not find CollapsingPlatform parent!",
                gameObject
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerScore playerScore =
            other.GetComponentInParent<PlayerScore>();

        if (playerScore == null) return;

        platform.PlayerSteppedOn(playerScore);
    }
}