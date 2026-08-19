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
        Debug.Log(
            "Platform trigger entered by: " +
            other.gameObject.name
        );

        PlayerScore playerScore =
            other.GetComponentInParent<PlayerScore>();

        if (playerScore == null)
        {
            Debug.Log(
                "Object does not have a PlayerScore component."
            );

            return;
        }

        Debug.Log(
            "PlayerScore found. Calling PlayerSteppedOn on: " +
            platform.gameObject.name
        );

        platform.PlayerSteppedOn(playerScore);
    }
}