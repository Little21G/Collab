using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class InfraredCamera : MonoBehaviour
{
    [Header("Controls")]
    public InputAction toggleAction; 

    [Header("Camera Objects")]
    public GameObject infraredScreenUI; 
    public GameObject thermalCameraObject; 

    [Header("Animation Settings")]
    public float transitionSpeed = 0.15f; 

    [Header("Battery System")]
    public float maxBattery = 100f;
    public Slider batteryBar; 
    private float currentBattery;

    private bool isCameraActive = false; 
    private RectTransform screenRect; 
    private Coroutine currentAnimation; 

    void Start()
    {
        currentBattery = maxBattery;
        if (batteryBar != null)
        {
            batteryBar.maxValue = maxBattery;
            batteryBar.value = currentBattery;
            batteryBar.gameObject.SetActive(false); 
        }

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
            if (!isCameraActive && currentBattery > 0)
            {
                ToggleInfrared();
            }
            else if (isCameraActive)
            {
                ToggleInfrared();
            }
        }

        if (isCameraActive)
        {
            currentBattery -= (1f / 2f) * Time.deltaTime; 
            currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);

            if (batteryBar != null)
            {
                batteryBar.value = currentBattery;
            }

            if (currentBattery <= 0)
            {
                ToggleInfrared();
            }
        }
    }

    void ToggleInfrared()
    {
        isCameraActive = !isCameraActive;

        if (currentAnimation != null) 
        {
            StopCoroutine(currentAnimation);
        }

        if (isCameraActive)
        {
            currentAnimation = StartCoroutine(TurnOnTV());
        }
        else
        {
            currentAnimation = StartCoroutine(TurnOffTV());
        }
    }

    // --- ANIMATIONS ---

    IEnumerator TurnOnTV()
    {
        thermalCameraObject.SetActive(true);
        infraredScreenUI.SetActive(true);
        
        // We removed the instant battery turn-on from here!
        
        float timeElapsed = 0;
        screenRect.localScale = new Vector3(1, 0, 1);

        // 1. Wait for the screen to stretch open first
        while (timeElapsed < transitionSpeed)
        {
            float newY = Mathf.Lerp(0, 1, timeElapsed / transitionSpeed);
            screenRect.localScale = new Vector3(1, newY, 1);
            timeElapsed += Time.deltaTime;
            yield return null; 
        }

        screenRect.localScale = new Vector3(1, 1, 1);

        // 2. The Flicker Sequence!
        if (batteryBar != null)
        {
            batteryBar.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.05f); // Flash ON
            
            batteryBar.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.05f); // Flash OFF
            
            batteryBar.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.08f); // Flash ON slightly longer
            
            batteryBar.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.05f); // Flash OFF
            
            batteryBar.gameObject.SetActive(true);  // Finally stays ON
        }
    }

    IEnumerator TurnOffTV()
    {
        // Hide the battery immediately before the screen crushes down
        if (batteryBar != null) batteryBar.gameObject.SetActive(false);

        float timeElapsed = 0;

        while (timeElapsed < transitionSpeed)
        {
            float newY = Mathf.Lerp(1, 0, timeElapsed / transitionSpeed);
            screenRect.localScale = new Vector3(1, newY, 1);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        screenRect.localScale = new Vector3(1, 0, 1);
        infraredScreenUI.SetActive(false);
        thermalCameraObject.SetActive(false);
    }// --- BATTERY SYSTEM ---

    // The battery pickup will call this method when you interact with it!
    public void AddPower(float amount)
    {
        currentBattery += amount;
        
        // Make sure we don't overcharge past 100%
        currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);

        // Instantly update the UI slider so the player sees the jump
        if (batteryBar != null)
        {
            batteryBar.value = currentBattery;
        }

        Debug.Log($"Infrared Camera charged! Current battery is now: {currentBattery}/{maxBattery}");
    }
}