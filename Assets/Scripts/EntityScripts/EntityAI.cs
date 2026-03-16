using UnityEngine;

public class EntityAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Wander Borders (X and Z axis)")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minZ = -10f;
    [SerializeField] private float maxZ = 10f;

    [Header("Player Tracking")]
    [SerializeField] private Transform player; // We need to know where the player is!
    [SerializeField] private float minTimeBetweenChases = 5f;
    [SerializeField] private float maxTimeBetweenChases = 15f;
    [SerializeField] private float chaseDuration = 3f; // How long it walks toward you

    private Vector3 targetPosition;
    private bool isTrackingPlayer = false;
    private float stateTimer;

    void Start()
    {
        GetNewRandomTarget();
        // Pick a random amount of time before it first decides to track the player
        stateTimer = Random.Range(minTimeBetweenChases, maxTimeBetweenChases);
    }

    void Update()
    {
        // 1. Count down the timer
        stateTimer -= Time.deltaTime;

        // 2. Switch states when the timer hits 0
        if (stateTimer <= 0)
        {
            isTrackingPlayer = !isTrackingPlayer; // Flip the switch

            if (isTrackingPlayer)
            {
                // Set the timer for how long to chase
                stateTimer = chaseDuration; 
            }
            else
            {
                // Set the timer for how long to wander before chasing again
                stateTimer = Random.Range(minTimeBetweenChases, maxTimeBetweenChases); 
                GetNewRandomTarget(); // Immediately pick a random spot to walk away to
            }
        }

        // 3. If we are tracking the player, constantly update the target to the player's position
        if (isTrackingPlayer && player != null)
        {
            // Keep the entity's Y position so it doesn't float up or sink down to the player's camera level
            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        }

        // 4. Move the entity
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 5. Look at the target (Only rotate if we are actually moving, to prevent glitchy snapping)
        if (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            Vector3 lookDirection = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            transform.LookAt(lookDirection);
        }

        // 6. If we are just wandering and reach the random spot, pick a new one
        if (!isTrackingPlayer && Vector3.Distance(transform.position, targetPosition) < 0.2f)
        {
            GetNewRandomTarget();
        }
    }

    void GetNewRandomTarget()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
    }
}