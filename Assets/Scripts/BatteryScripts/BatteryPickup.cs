using UnityEngine;

public class BatteryPickup : MonoBehaviour, IInteractable
{
    [Header("Battery Settings")]
    public float powerAmount = 25f; // How much power this battery gives

    public void Interact()
    {
        // 1. Find the player's camera script in the scene
        InfraredCamera cameraScript = Object.FindFirstObjectByType<InfraredCamera>();

        // 2. If we found it, send power to it!
        if (cameraScript != null)
        {
            cameraScript.AddPower(powerAmount);
        }

        // 3. Destroy the battery object so it disappears from the world
        Destroy(gameObject);
    }
}