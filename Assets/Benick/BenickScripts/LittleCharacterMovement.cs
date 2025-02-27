using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

// Taken from Brakeys video on NavMesh Navigation!
public class LittleCharacterMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Camera cam;
    public ParticleSystem selectParticle;

    void Start()
    {
        //var emission = selectParticle.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);

                selectParticle.transform.position = hit.point;
                selectParticle.Play();
            }
        }
        if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            selectParticle.Stop();
            /*var emission = selectParticle.emission;
            emission.enabled = true;*/
        }
    }
}
