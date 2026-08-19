using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float distance = 5f;
    public float height = 2f;
    public float sensitivity = 0.1f;

    // Vertical orbit limits
    public float minPitch = 5f;
    public float maxPitch = 75f;

    private float yaw;
    private float pitch = 20f;

    private Transform player;

    void Start()
    {
        player = transform.parent;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        Vector2 mouseInput = Mouse.current.delta.ReadValue();

        // Horizontal orbit
        yaw += mouseInput.x * sensitivity;

        // Vertical orbit
        pitch -= mouseInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Build the orbit rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Player's position is the center of our orbit
        Vector3 target = player.position + Vector3.up * height;

        // Position camera behind the orbit direction
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);

        transform.position = target + offset;

        // Always look at the player
        transform.LookAt(target);
    }
}