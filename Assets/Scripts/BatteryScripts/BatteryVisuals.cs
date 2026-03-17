using UnityEngine;

public class BatteryVisuals : MonoBehaviour
{
    [Header("Materials")]
    public Material normalMaterial;   // How it looks normally
    public Material thermalMaterial;  // The glowing green one

    [Header("Layers (Type exactly as they appear in Unity)")]
    public string normalLayerName = "Default";
    public string thermalLayerName = "ThermalBypass"; 

    private Renderer rend;
    private InfraredCamera cameraScript;
    private bool wasThermalOn = false; 

    void Start()
    {
        rend = GetComponent<Renderer>();
        cameraScript = Object.FindFirstObjectByType<InfraredCamera>();

        // --- THE FIX: FORCE THE STARTING STATE ---
        // Instantly snap to the normal material and layer when the game starts
        if (rend != null && normalMaterial != null)
        {
            rend.material = normalMaterial;
        }
        gameObject.layer = LayerMask.NameToLayer(normalLayerName);

        // Sync our tracker with the camera's actual starting state
        if (cameraScript != null)
        {
            wasThermalOn = cameraScript.isCameraActive;
        }
    }

    void Update()
    {
        if (cameraScript == null) return;

        // Check if the thermal camera is currently turned on using your fast boolean
        bool isThermalCurrentlyOn = cameraScript.isCameraActive;

        // If the TV state JUST changed, swap the materials!
        if (isThermalCurrentlyOn != wasThermalOn)
        {
            if (isThermalCurrentlyOn)
            {
                // TV turned ON -> Go Green!
                rend.material = thermalMaterial;
                gameObject.layer = LayerMask.NameToLayer(thermalLayerName);
            }
            else
            {
                // TV turned OFF -> Go Normal!
                rend.material = normalMaterial;
                gameObject.layer = LayerMask.NameToLayer(normalLayerName);
            }
            
            // Update our tracker
            wasThermalOn = isThermalCurrentlyOn;
        }
    }
}