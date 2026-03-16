using UnityEngine;
using UnityEngine.Rendering; 

public class ProximityGlitch : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform monster;
    public Volume glitchVolume; // Back to the Volume!

    [Header("Distance Settings")]
    public float startGlitchDistance = 10f; 
    public float maxGlitchDistance = 2f;    

    void Update()
    {
        if (player == null || monster == null || glitchVolume == null) return;

        float distance = Vector3.Distance(player.position, monster.position);

        if (distance <= startGlitchDistance)
        {
            float glitchIntensity = 1f - Mathf.Clamp01((distance - maxGlitchDistance) / (startGlitchDistance - maxGlitchDistance));
            glitchVolume.weight = glitchIntensity;
        }
        else
        {
            glitchVolume.weight = 0f;
        }
    }
}