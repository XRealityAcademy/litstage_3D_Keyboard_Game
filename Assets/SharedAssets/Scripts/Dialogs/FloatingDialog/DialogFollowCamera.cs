using UnityEngine;

public class DialogFollowCamera : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform; // ✅ Get the player's main camera
    }

    void Update()
    {
        if (cameraTransform != null)
        {
            // ✅ Make the dialog face the camera
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            // ✅ (Optional) Flip it so it's readable
            Vector3 targetPosition = new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z);
            transform.LookAt(targetPosition);
            transform.Rotate(0, 180, 0);
        }
    }
}
