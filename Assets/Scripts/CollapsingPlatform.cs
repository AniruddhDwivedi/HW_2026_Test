using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    [Header("Timing")]
    public float minLifetime = 4f;
    public float maxLifetime = 5f;
    public float spawnDelay = 2.5f;

    [Header("Platform")]
    public float platformSize = 9f;

    [Header("Prefab")]
    public GameObject platformPrefab;
    
    private bool playerHasScored = false;

    private void Start()
    {
        // Each platform gets its own independent lifetime.
        float lifetime = Random.Range(minLifetime, maxLifetime);

        Destroy(gameObject, lifetime);

        // This instance creates its child.
        Invoke(nameof(SpawnNextPlatform), spawnDelay);
    }

    private void SpawnNextPlatform()
    {
        Vector3[] directions =
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        Vector3 direction =
            directions[Random.Range(0, directions.Length)];

        Vector3 spawnPosition =
            transform.position + direction * platformSize;

        Instantiate(
            platformPrefab,
            spawnPosition,
            transform.rotation
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerHasScored)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerScore playerScore = other.GetComponent<PlayerScore>();

        if (playerScore == null)
            return;

        playerHasScored = true;

        playerScore.AddScore();
    }
}