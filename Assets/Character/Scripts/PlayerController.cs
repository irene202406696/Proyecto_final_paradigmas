using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float mouseSensitivity = 0.1f;

    private CharacterController controller;
    private float yaw; //horizontal rotation 
    private float pitch;  //vertical rotation 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        yaw = transform.eulerAngles.y; //start facing the direction the camera
        LockCursor(true); //hide the cursor and lock it to the center of the screen
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard == null || mouse == null) return; //if there is no keyboard or mouse, do nothing


        if (keyboard.escapeKey.wasPressedThisFrame) LockCursor(false); // Esc releases the mouse
        if (mouse.leftButton.wasPressedThisFrame) LockCursor(true); //clicking locks it again

        Vector2 look = mouse.delta.ReadValue() * mouseSensitivity;
        yaw += look.x; //moving the mouse sideways turns left/right
        pitch = Mathf.Clamp(pitch - look.y, -80f, 80f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f); // apply both rotations to the camera

        //MOVE
        Vector2 input = Vector2.zero;
        if (keyboard.wKey.isPressed) input.y += 1;
        if (keyboard.sKey.isPressed) input.y -= 1;
        if (keyboard.dKey.isPressed) input.x += 1;
        if (keyboard.aKey.isPressed) input.x -= 1;

        Vector3 forward = transform.forward; //camera forward direction flattened onto the ground
        forward.y = 0f;
        forward.Normalize();

        Vector3 dir = forward * input.y + transform.right * input.x;
        controller.Move(Vector3.ClampMagnitude(dir, 1f) * speed * Time.deltaTime); //prevents moving faster diagonally, makes the speed independent of the frame rate and moves the player and stops it if it hits something
    }

    private void LockCursor(bool locked)
    {
        // locks and hides the cursor
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}