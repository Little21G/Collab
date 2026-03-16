using UnityEngine;

// Remember, this requires the IInteractable interface we made earlier!
public class BatteryPickup : MonoBehaviour, IInteractable
{
    [Header("Battery Settings")]
    public float powerAmount = 25f; // How much juice this battery gives

    public void Interact()
    {
        // 1. Look for YOUR InfraredCamera script in the scene
        InfraredCamera infraCam = FindObjectOfType<InfraredCamera>();

        // 2. If we found it, give it the power!
        if (infraCam != null)
        {
            infraCam.AddPower(powerAmount);
            
            // 3. Destroy the battery so it disappears from the world
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("You picked up a battery, but couldn't find the InfraredCamera script!");
        }
    }
}