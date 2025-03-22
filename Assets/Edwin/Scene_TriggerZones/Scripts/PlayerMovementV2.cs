using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementV2 : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera cam;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;

        if (agent == null)
        {
            Debug.LogError("❌ No NavMeshAgent found on Player!");
        }
        if (cam == null)
        {
            Debug.LogError("❌ No Main Camera found!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click to move
        {
            Debug.Log("🖱️ Click detected!");

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the layer exists
            int layerMask = LayerMask.GetMask("WalkableGround");
            if (layerMask == 0)
            {
                Debug.LogError("❌ 'Walkable' Layer does NOT exist! Make sure you created it.");
                return;
            }

            // Perform Raycast with LayerMask
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log($"🟢 Raycast Hit: {hit.collider.name} at {hit.point}");

                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(hit.point);
                    Debug.Log("🚀 Moving to: " + hit.point);
                }
                else
                {
                    Debug.LogError("❌ Player is NOT on a NavMesh!");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Raycast didn't hit anything! Make sure WalkableGround has a Collider.");
            }
        }
    }
}