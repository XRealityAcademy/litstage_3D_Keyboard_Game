using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Taken from Brakeys video on NavMesh Navigation!
public class LittleCharacterMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Camera cam;
    public ParticleSystem selectParticle;
    
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
        if (Physics.Raycast(ray, out hit))
        {
            redCircle.transform.position = hit.point;
            greenCircle.transform.position = hit.point;

            if (hit.collider.CompareTag("Object"))      // If hit object, move red circle to hit point
            {
                redCircle.SetActive(true);
                greenCircle.SetActive(false);
            }
            else
            {
                redCircle.SetActive(false);
            }

            if (hit.collider.CompareTag("Ground"))      // If hit ground, move green circle to hit point
            {
                greenCircle.SetActive(true);
                redCircle.SetActive(false);

                if (Input.GetMouseButtonDown(0))
                {   
                    agent.SetDestination(hit.point);
                    yellowCircle.transform.position = hit.point;
                    yellowCircle.transform.rotation = Quaternion.LookRotation(Vector3.forward, hit.point);
                    yellowCircle.SetActive(true);

                    selectParticle.transform.position = hit.point;
                    selectParticle.Emit(1);

                }
            }
            else
            {
                greenCircle.SetActive(false);
            }


        }

        if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            selectParticle.Stop();
            if (isTraveling)
            {
                yellowCircle.SetActive(false);
                isTraveling = false;
            }
        }
        else
        {
            isTraveling = true;
        }
    }
}
