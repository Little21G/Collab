using UnityEngine;
using UnityEngine.InputSystem; // Using the new Input System again!

public class MouseLook : MonoBehaviour
{
    // Variables you can tweak in the Inspector
    public float mouseSensitivity = 100f;
    public Transform playerBody; // This links the camera to the player's body
    public InputAction lookAction;

    private float xRotation = 0f;

    void Start()
    {
        // This locks your mouse cursor to the center of the screen and hides it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        lookAction.Enable();
    }

    void OnDisable()
    {
        lookAction.Disable();
    }

    void Update()
    {
        // 1. Read the mouse movement (Delta)
        Vector2 mouseInput = lookAction.ReadValue<Vector2>();

        // 2. Multiply by sensitivity and Time.deltaTime for smooth framing
        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        // 3. Calculate looking up and down (Pitch)
        xRotation -= mouseY; 
        
        // 4. Clamp the rotation so the player can't backflip their neck!
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 5. Apply the up/down rotation to the Camera itself
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 6. Rotate the ENTIRE Player body left and right (Yaw)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}