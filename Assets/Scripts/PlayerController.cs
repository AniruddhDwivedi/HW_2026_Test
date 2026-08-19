using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 10f;

    private Rigidbody rig;
    private Vector2 moveInput;
    private Transform cameraTransform;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
    }

    void FixedUpdate()
    {
        // Get camera directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Ignore camera's vertical angle
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Convert WASD into camera-relative movement
        Vector3 movement =
            cameraForward * moveInput.y +
            cameraRight * moveInput.x;

        if (movement.sqrMagnitude > 0.01f)
        {
            movement.Normalize();

            // Move the player
            rig.MovePosition(
                rig.position + movement * speed * Time.fixedDeltaTime
            );

            // Turn the player toward movement direction
            Quaternion targetRotation =
                Quaternion.LookRotation(movement);

            Quaternion newRotation = Quaternion.Slerp(
                rig.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

            rig.MoveRotation(newRotation);
        }
    }
}