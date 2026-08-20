using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float distance = 5f;
    public float height = 2f;
    public float sensitivity = 0.1f;
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
        yaw += mouseInput.x * sensitivity;
        pitch -= mouseInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 target = player.position + Vector3.up * height;
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);

        transform.position = target + offset;
        transform.LookAt(target);
    }
}