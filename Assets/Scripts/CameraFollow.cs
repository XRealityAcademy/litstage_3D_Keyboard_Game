
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float offsetX = 0;  // Horizontal offset (left/right)
    public float offsetY = 3;  // Vertical offset (up/down)
    public float offsetZ = 10; // Depth offset (forward/backward)
    

    public Transform player; // Assign the Capsule in the Inspector
    public Vector3 offset;


    void Start()
    {
        transform.position = new Vector3(offsetX, offsetY, -offsetZ); // Moves camera backward once
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset; // Move the camera
            transform.LookAt(player); // Make the camera look at the player
        }
    }
}