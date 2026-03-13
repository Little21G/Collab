using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.UI; // We MUST add this to talk to UI elements like our Slider!

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f; 
    
    public InputAction moveAction; 
    public InputAction sprintAction; 

    [Header("Stamina System")]
    public float maxStamina = 7.5f;
    public Slider staminaBar; // This creates a slot in the Inspector for your UI
    private float currentStamina; // Tracks exactly how much stamina you have left

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Start the game with full stamina
        currentStamina = maxStamina;
        
        // Set up the UI slider's max value
        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }
    }

    void OnEnable()
    {
        moveAction.Enable();
        sprintAction.Enable(); 
    }

    void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable(); 
    }

    void Update()
    {
        // 1. Read input
        Vector2 input = moveAction.ReadValue<Vector2>();
        
        // Check if the player is actually pressing WASD, not just standing still
        bool isMoving = input.magnitude > 0.1f; 
        bool isAttemptingToSprint = sprintAction.IsPressed();

        float currentSpeed = walkSpeed;

        // 2. Stamina Logic
        // If holding Shift, AND actually moving, AND we have stamina left...
        if (isAttemptingToSprint && isMoving && currentStamina > 0)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= Time.deltaTime; // Drain stamina by 1 unit per second
        }
        else 
        {
            // If we aren't sprinting, recover stamina!
            if (currentStamina < maxStamina)
            {
                currentStamina += Time.deltaTime; // Recover stamina by 1 unit per second
            }
        }

        // 3. Keep stamina safely between 0 and 7.5 so the math doesn't break
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // 4. Update the visual bar on the screen
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;
        }

        // 5. Move the player
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }
}