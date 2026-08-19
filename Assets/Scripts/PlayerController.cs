using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;

    private Rigidbody rig;
    private Vector2 moveInput;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
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
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        rig.MovePosition(rig.position + movement * speed * Time.fixedDeltaTime);
    }
}