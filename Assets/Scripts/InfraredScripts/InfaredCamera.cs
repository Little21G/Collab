using System.Collections; // We need this to use Coroutines (animations over time)
using UnityEngine;
using UnityEngine.InputSystem;

public class InfraredCamera : MonoBehaviour
{
    [Header("Controls")]
    public InputAction toggleAction; 

    [Header("Camera Objects")]
    public GameObject infraredScreenUI; 
    public GameObject thermalCameraObject; 

    [Header("Animation Settings")]
    public float transitionSpeed = 0.15f; // How fast the TV turns on/off (in seconds)

    private bool isCameraActive = false; 
    private RectTransform screenRect; // The UI component that controls the screen's size
    private Coroutine currentAnimation; // Keeps track of the animation so it doesn't glitch

    void Start()
    {
        // Grab the RectTransform from your UI screen so we can change its scale
        if (infraredScreenUI != null)
        {
            screenRect = infraredScreenUI.GetComponent<RectTransform>();
            infraredScreenUI.SetActive(false);
        }
        
        if (thermalCameraObject != null) thermalCameraObject.SetActive(false);
    }

    void OnEnable()
    {
        toggleAction.Enable();
    }

    void OnDisable()
    {
        toggleAction.Disable();
    }

    void Update()
    {
        if (toggleAction.WasPressedThisFrame())
        {
            ToggleInfrared();
        }
    }

    void ToggleInfrared()
    {
        isCameraActive = !isCameraActive;

        // If you spam the 'F' key, this stops the previous animation so they don't fight
        if (currentAnimation != null) 
        {
            StopCoroutine(currentAnimation);
        }

        // Start the correct animation based on whether the camera is now on or off
        if (isCameraActive)
        {
            currentAnimation = StartCoroutine(TurnOnTV());
        }
        else
        {
            currentAnimation = StartCoroutine(TurnOffTV());
        }
    }

    // --- OUR ANIMATIONS ---

    IEnumerator TurnOnTV()
    {
        // 1. Turn the camera and UI object ON immediately
        thermalCameraObject.SetActive(true);
        infraredScreenUI.SetActive(true);
        
        float timeElapsed = 0;
        
        // 2. Smash the screen completely flat on the Y axis (so it looks like a thin horizontal line)
        screenRect.localScale = new Vector3(1, 0, 1);

        // 3. Smoothly grow the Y scale from 0 to 1 over our transitionSpeed
        while (timeElapsed < transitionSpeed)
        {
            // Mathf.Lerp smoothly blends between two numbers
            float newY = Mathf.Lerp(0, 1, timeElapsed / transitionSpeed);
            screenRect.localScale = new Vector3(1, newY, 1);
            
            timeElapsed += Time.deltaTime;
            yield return null; // This tells Unity "Pause here and finish the rest next frame"
        }

        // 4. Force it to exactly 1 at the end just to be safe
        screenRect.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator TurnOffTV()
    {
        float timeElapsed = 0;

        // 1. Smoothly shrink the Y scale from 1 down to 0
        while (timeElapsed < transitionSpeed)
        {
            float newY = Mathf.Lerp(1, 0, timeElapsed / transitionSpeed);
            screenRect.localScale = new Vector3(1, newY, 1);
            
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        screenRect.localScale = new Vector3(1, 0, 1);
        
        // 2. Turn the actual objects OFF now that the screen is completely crushed
        infraredScreenUI.SetActive(false);
        thermalCameraObject.SetActive(false);
    }
}