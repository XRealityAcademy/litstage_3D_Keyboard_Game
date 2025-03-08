using UnityEngine;

/// <summary>
/// Makes the camera follow the player with an adjustable offset.
/// Allows modifying the offset via the Unity Inspector and in real-time using input keys.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Camera Target")]
    [Tooltip("The target (player) that the camera follows.")]
    public Transform player;

    [Header("Camera Offset Settings")]
    [Tooltip("The offset position from the player.")]
    public Vector3 offset = new Vector3(0, 3, 10); // Default values (editable in Inspector)

    [Header("Offset Adjustment Speed")]
    [Tooltip("Speed at which offset changes when using keys.")]
    public float adjustmentSpeed = 0.1f;

    /// <summary>
    /// Sets the initial camera position.
    /// </summary>
    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned to CameraFollow!");
            return;
        }

        transform.position = player.position + offset; // Set initial position
    }

    /// <summary>
    /// Updates the camera position after the player moves.
    /// </summary>
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset; // Follow the player
            transform.LookAt(player); // Ensure the camera faces the player
        }
    }

    /// <summary>
    /// Allows real-time modification of the camera offset using keyboard input.
    /// </summary>
    void Update()
    {
        AdjustOffset();
    }

    /// <summary>
    /// Changes the camera offset based on user input.
    /// </summary>
    void AdjustOffset()
    {
        float moveSpeed = adjustmentSpeed * Time.deltaTime; // 

        // Zoom in/out using Mouse Scroll
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        offset.z += scroll * 5f; // 

        // Move camera up/down
        if (Input.GetKey(KeyCode.R)) offset.y += moveSpeed; // Move up
        if (Input.GetKey(KeyCode.F)) offset.y -= moveSpeed; // Move down

        //  Move left/right
        if (Input.GetKey(KeyCode.A)) offset.x -= moveSpeed;
        if (Input.GetKey(KeyCode.D)) offset.x += moveSpeed;

        // Apply the new offset smoothly
        transform.position = Vector3.Lerp(transform.position, player.position + offset, 5f * Time.deltaTime);
    }
}