using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Required for the Crosshair Image

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Camera playerCamera; 
    public float interactDistance = 3f; 
    
    [Header("UI Settings")]
    public Image crosshair; 
    public Color normalColor = Color.white;
    public Color interactColor = Color.green; // Color it changes to when aiming at an item
    public GameObject interactText; // The "Press [E] to Pick Up" text object

    [Header("Input")]
    public InputAction interactAction; 

    // This stores whatever we are currently looking at
    private IInteractable currentInteractable;

    void Start()
    {
        // Make sure the text prompt is hidden when the game starts
        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }

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
        // Shoot the invisible laser from the center of the screen
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hitInfo;

        // Did the laser hit something within range?
        if (Physics.Raycast(ray, out hitInfo, interactDistance))
        {
            // Try to get the IInteractable component from what we hit
            IInteractable interactableObject = hitInfo.collider.GetComponent<IInteractable>();
            
            if (interactableObject != null)
            {
                // We ARE looking at an interactable object (like the battery)!
                currentInteractable = interactableObject;
                
                // Change crosshair color
                if (crosshair != null) 
                    crosshair.color = interactColor; 
                
                // Show the "Press E" text
                if (interactText != null) 
                    interactText.SetActive(true); 
                    
                return; // Stop running the rest of the code in this method
            }
        }

        // If we get down here, it means we are NOT looking at anything interactable
        currentInteractable = null;
        
        // Reset crosshair to normal color
        if (crosshair != null) 
            crosshair.color = normalColor; 
            
        // Hide the "Press E" text
        if (interactText != null) 
            interactText.SetActive(false); 
    }
}