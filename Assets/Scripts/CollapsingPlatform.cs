using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollapsingPlatform : MonoBehaviour
{
    AudioManager audioManager;

    [Header("Timing")]
    public float minLifetime = 4f;
    public float maxLifetime = 5f;
    public float spawnDuration = 2.5f;

    [Header("Platform")]
    public float platformSize = 9f;

    [Header("Prefab")]
    public GameObject platformPrefab;

    [Header("State")]
    public bool isStartingPlatform = false;

    private Vector3 parentDirection;

    private Collider platformCollider;
    private Collider scoreTrigger;

    private Vector3 finalScale;

    private bool isActive = false;
    private bool hasSpawnedChild = false;
    private bool playerHasScored = false;

    public void SetParentDirection(Vector3 direction)
    {
        parentDirection = direction;
    }

    private void Awake()
    {
        Collider[] colliders =
            GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            if (col.isTrigger)
                scoreTrigger = col;
            else
                platformCollider = col;
        }

        if (platformCollider != null)
        {
            finalScale =
                platformCollider.transform.localScale;
        }
        else
        {
            Debug.LogError(
                "CollapsingPlatform: No solid collider found!",
                this
            );
        }
    }

    private void Start()
    {

        audioManager = FindFirstObjectByType<AudioManager>();
        if (isStartingPlatform)
        {
            // Starting platform is already fully grown.
            ActivatePlatform();
        }
        else
        {
            // Every generated platform grows into existence.
            StartCoroutine(GrowPlatform());
            audioManager.PlayHover();
        }
    }

    private IEnumerator GrowPlatform()
    {
        Transform platformTransform =
            platformCollider.transform;

        platformTransform.localScale = Vector3.zero;

        platformCollider.enabled = false;
        scoreTrigger.enabled = false;

        float elapsed = 0f;

        while (elapsed < spawnDuration)
        {
            elapsed += Time.deltaTime;

            float progress =
                Mathf.Clamp01(elapsed / spawnDuration);

            platformTransform.localScale =
                Vector3.Lerp(
                    Vector3.zero,
                    finalScale,
                    progress
                );

            yield return null;
        }

        platformTransform.localScale = finalScale;

        ActivatePlatform();
    }

    private void ActivatePlatform()
    {
        isActive = true;

        platformCollider.enabled = true;
        scoreTrigger.enabled = true;

        // The platform's death timer starts when it becomes usable.
        float lifetime =
            Random.Range(minLifetime, maxLifetime);


        audioManager.PlayClick();
        Destroy(gameObject, lifetime);
    }

    public void PlayerSteppedOn(PlayerScore playerScore)
    {
        Debug.Log("Player stepped on " + gameObject.name);

        if (!isActive)
        {
            Debug.Log("Platform isn't active yet.");
            return;
        }

        if (playerHasScored)
        {
            Debug.Log("Platform has already been scored.");
            return;
        }

        playerHasScored = true;

        playerScore.AddScore();

        SpawnNextPlatform();
    }

    private void SpawnNextPlatform()
    {
        if (hasSpawnedChild)
            return;

        hasSpawnedChild = true;

        Vector3[] directions =
        {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right
        };

        List<Vector3> validDirections =
            new List<Vector3>();

        foreach (Vector3 direction in directions)
        {
            if (Vector3.Dot(direction, parentDirection) < 0.5f)
            {
                validDirections.Add(direction);
            }
        }

        Vector3 chosenDirection =
            validDirections[
                Random.Range(0, validDirections.Count)
            ];

        Vector3 spawnPosition =
            transform.position +
            chosenDirection * platformSize;

        Debug.Log(
            "Spawning child platform at " + spawnPosition
        );

        GameObject child =
            Instantiate(
                platformPrefab,
                spawnPosition,
                transform.rotation
            );

        CollapsingPlatform childPlatform =
            child.GetComponent<CollapsingPlatform>();

        if (childPlatform != null)
        {
            childPlatform.isStartingPlatform = false;
            childPlatform.SetParentDirection(-chosenDirection);
        }
    }
}