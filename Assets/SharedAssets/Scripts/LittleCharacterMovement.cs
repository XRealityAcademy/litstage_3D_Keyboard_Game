using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Taken from Brakeys video on NavMesh Navigation!
public class LittleCharacterMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Camera cam;
    
    public Image crosshair;

    public GameObject redCircle;
    public GameObject greenCircle;
    public GameObject yellowCircle;

    bool isTraveling;
    Ray ray;
    RaycastHit hit;

    void Start()
    {
        //var emission = selectParticle.emission;

        redCircle.SetActive(false);
        greenCircle.SetActive(false);
        yellowCircle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(crosshair.transform.position);
        if (Physics.Raycast(ray, out hit))                  // If the raycast hits something, output to hit
        {
            redCircle.transform.position = hit.point;       // Constantly move both circles to hit point
            greenCircle.transform.position = hit.point;

            if (hit.collider.CompareTag("Object"))          // If hit tag "object", show red circle instead of green circle
            {
                redCircle.SetActive(true);
                greenCircle.SetActive(false);
            }
            else
            {
                redCircle.SetActive(false);
            }

            if (hit.collider.CompareTag("Ground"))          // If hit tag "ground", show green circle instead of red circle
            {
                greenCircle.SetActive(true);
                redCircle.SetActive(false);

                if (Input.GetMouseButtonDown(0))
                {   
                    agent.SetDestination(hit.point);
                    yellowCircle.transform.position = hit.point;                                                // If left click on ground, move yellow circle to where you clicked
                    yellowCircle.transform.rotation = Quaternion.LookRotation(Vector3.forward, hit.point);      // Still figuring out the orientation stuff
                    yellowCircle.SetActive(true);                                                               // Show yellow circle
                }
            }
            else
            {
                greenCircle.SetActive(false);
            }
        }

        if (agent.remainingDistance <= agent.stoppingDistance) // If little character makes it to its destination...
        {
            if (isTraveling)                                   // Make sure yellow circle doesn't disappear until the little character is moving
            {
                yellowCircle.SetActive(false);                 // Make yellow circle disappear
                isTraveling = false;
            }
        }
        else
        {
            isTraveling = true;
        }
    }
}
