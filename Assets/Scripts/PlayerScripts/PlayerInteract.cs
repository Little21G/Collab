using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // We need this line to talk to UI Images!

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Camera playerCamera; 
    public float interactDistance = 3f; 
    
    [Header("Crosshair UI")]
    public Image crosshair; 
    public Color normalColor = Color.white;
    public Color interactColor = Color.green; // Color it changes to when aiming at an item

    [Header("Input")]
    public InputAction interactAction; 

    // This stores whatever we are currently looking at
    private IInteractable currentInteractable;

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
        // 1. Constantly check what is in front of us
        CheckForInteractables();

        // 2. If we press 'E' AND we are looking at something, interact with it!
        if (interactAction.WasPressedThisFrame() && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckForInteractables()
    {
        // Shoot the laser from the center of the screen
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, interactDistance))
        {
            // Did we hit something with the IInteractable interface?
            IInteractable interactableObject = hitInfo.collider.GetComponent<IInteractable>();
            
            if (interactableObject != null)
            {
                // We are looking at a battery (or door, etc)!
                currentInteractable = interactableObject;
                
                if (crosshair != null) 
                    crosshair.color = interactColor; // Turn crosshair green
                    
                return; // Stop running the code below
            }
        }

        // If we get here, it means we AREN'T looking at anything interactable
        currentInteractable = null;
        
        if (crosshair != null) 
            crosshair.color = normalColor; // Keep crosshair white
    }
}