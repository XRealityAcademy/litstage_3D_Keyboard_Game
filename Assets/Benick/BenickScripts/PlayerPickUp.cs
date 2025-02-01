using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Got from youtube video!!!

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;

    // If you wanted to change where the object will be held, move the empty
    // game object named "ObjectGrabPoint" as a child of the place/object where you
    // want it to be held

    // For Example: It is currently a child of the camera, therefore the object will
    // move with the camera.
    [SerializeField] private Transform ObjectGrabPointTransform;

    [SerializeField] private LayerMask pickUpLayerMask;

    private ObjectGrabbable objectGrabbable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                // Not carrying an object, try to grab

                float pickUpDistance = 2f;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(ObjectGrabPointTransform);
                    }
                }
            }
            else
            {
                // Currently carrying something, drop
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }
}
