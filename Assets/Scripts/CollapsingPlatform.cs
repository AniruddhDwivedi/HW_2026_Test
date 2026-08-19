using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollapsingPlatform : MonoBehaviour
{
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
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            if (col.isTrigger)
            {
                scoreTrigger = col;
            }
            else
            {
                platformCollider = col;
            }
        }

        if (platformCollider == null)
        {
            Debug.LogError(
                "CollapsingPlatform: Could not find solid platform collider!",
                this
            );
            return;
        }

        if (scoreTrigger == null)
        {
            Debug.LogError(
                "CollapsingPlatform: Could not find trigger collider!",
                this
            );
            return;
        }

        finalScale = platformCollider.transform.localScale;
    }

    private void Start()
    {
        Debug.Log(
            gameObject.name +
            " started. Starting platform: " +
            isStartingPlatform
        );

        if (isStartingPlatform)
        {
            ActivatePlatform();
        }
        else
        {
            StartCoroutine(GrowPlatform());
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

        float lifetime =
            Random.Range(minLifetime, maxLifetime);

        Debug.Log(
            gameObject.name +
            " is now active. Lifetime: " +
            lifetime
        );

        Destroy(gameObject, lifetime);
    }

    public void PlayerSteppedOn(PlayerScore playerScore)
    {
        Debug.Log(
            "Player stepped on " +
            gameObject.name
        );

        if (!isActive)
        {
            Debug.Log(
                gameObject.name +
                " isn't active yet."
            );

            return;
        }

        if (playerHasScored)
        {
            Debug.Log(
                gameObject.name +
                " has already been scored."
            );

            return;
        }

        playerHasScored = true;

        playerScore.AddScore();

        Debug.Log(
            "Attempting to spawn child from " +
            gameObject.name
        );

        SpawnNextPlatform();
    }

    private void SpawnNextPlatform()
    {
        if (hasSpawnedChild)
        {
            Debug.Log(
                gameObject.name +
                " has already spawned a child."
            );

            return;
        }

        if (platformPrefab == null)
        {
            Debug.LogError(
                "PLATFORM PREFAB IS NOT ASSIGNED on " +
                gameObject.name,
                this
            );

            return;
        }

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

        if (validDirections.Count == 0)
        {
            Debug.LogError(
                "No valid spawn directions for " +
                gameObject.name
            );

            hasSpawnedChild = false;
            return;
        }

        Vector3 chosenDirection =
            validDirections[
                Random.Range(
                    0,
                    validDirections.Count
                )
            ];

        Vector3 spawnPosition =
            transform.position +
            chosenDirection * platformSize;

        Debug.Log(
            "Spawning child at " +
            spawnPosition +
            " in direction " +
            chosenDirection
        );

        GameObject child =
            Instantiate(
                platformPrefab,
                spawnPosition,
                transform.rotation
            );

        child.name = "Platform";


        if (child == null)
        {
            Debug.LogError(
                "Instantiate returned null!"
            );

            return;
        }

        CollapsingPlatform childPlatform =
            child.GetComponent<CollapsingPlatform>();

        if (childPlatform == null)
        {
            Debug.LogError(
                "Spawned object does not contain " +
                "CollapsingPlatform!",
                child
            );

            return;
        }

        childPlatform.isStartingPlatform = false;

        childPlatform.SetParentDirection(
            -chosenDirection
        );

        Debug.Log(
            "Successfully created child: " +
            child.name
        );
    }
}