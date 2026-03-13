using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Camera playerCamera; // We need to shoot the ray from the camera's eyes
    public float interactDistance = 3f; // How close you need to be to interact
    
    [Header("Input")]
    public InputAction interactAction; // The slot for our 'E' key

    void OnEnable()
    {
        interactAction.Enable();
    }

    void OnDisable()
    {
        interactAction.Disable();
    }

    void Update()
    {
        // Notice we use WasPressedThisFrame() instead of IsPressed()
        // We only want the interaction to trigger ONCE when you tap 'E', not spam 60 times a second if you hold it!
        if (interactAction.WasPressedThisFrame())
        {
            AttemptInteraction();
        }
    }

    void AttemptInteraction()
    {
        // 1. Create the invisible laser from the camera's position, pointing straight forward
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        
        // 2. This variable will store information about whatever our laser hits
        RaycastHit hitInfo;

        // 3. Shoot the laser! (If it hits something within our interactDistance, this returns true)
        if (Physics.Raycast(ray, out hitInfo, interactDistance))
        {
            // For now, let's just print a message to the console to prove it works
            Debug.Log("You interacted with: " + hitInfo.collider.gameObject.name);
            
            // This is where we will eventually tell a door to open or an item to be picked up!
        }
    }
}