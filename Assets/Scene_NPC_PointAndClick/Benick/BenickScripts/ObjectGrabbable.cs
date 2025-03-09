using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    public float speed = 2f;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
    }

    public void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            Vector3 newPosition = (objectGrabPointTransform.position - transform.position);
            objectRigidbody.velocity = newPosition * Mathf.Pow(speed, 2);
            objectRigidbody.AddForce(newPosition);
        }
    }
}
