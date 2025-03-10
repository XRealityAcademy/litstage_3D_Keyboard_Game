using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
        redCircle.SetActive(false);
        greenCircle.SetActive(false);
        yellowCircle.SetActive(false);
    }

    void Update()
    {
        // **🛠️ Step 1: Check if clicking on UI elements (buttons, icons, etc.)**
        if (IsPointerOverUIElement())
        {
            return; // ✅ Prevent movement if clicking UI
        }

        // **🛠️ Step 2: Cast a ray from the crosshair to detect world objects**
        ray = cam.ScreenPointToRay(crosshair.transform.position);

        if (Physics.Raycast(ray, out hit))
        {
            // ✅ Log the Raycast hit ONLY when clicking
            if (Input.GetMouseButtonDown(0))
            {
             //   Debug.Log($"🖱️ Clicked on: {hit.collider.gameObject.name} (Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)})");
            }

            // ✅ **Detect SpaceElements only when clicking**
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SpaceElements") && Input.GetMouseButtonDown(0))
            {
                //Debug.Log("🟡 Clicked on a SpaceElement: " + hit.collider.gameObject.name);

                // ✅ Show RedCircle at the clicked position
                redCircle.transform.position = hit.point;
                redCircle.SetActive(true);
                greenCircle.SetActive(false);

                // ✅ Try to display the message if this object has a SpaceElement component
                SpaceElement spaceElement = hit.collider.GetComponent<SpaceElement>();
                if (spaceElement != null)
                {
                    spaceElement.DisplayMessage();
                }
                else
                {
                    Debug.LogError("❌ SpaceElement NOT found on: " + hit.collider.gameObject.name);
                    Debug.Log($"🔍 Checking Parent: {hit.collider.transform.parent?.gameObject.name}");
                }

                return; // ✅ Prevent NPC movement
            }

            // **🛠️ Update indicator positions (Restore Pointer)**
            redCircle.transform.position = hit.point;
            greenCircle.transform.position = hit.point;

            // **🛠️ Step 3: Check if it's an obstacle**
            if (hit.collider.CompareTag("Object"))
            {
                redCircle.SetActive(true);
                greenCircle.SetActive(false);
            }
            else
            {
                redCircle.SetActive(false);
            }

            // **🛠️ Step 4: Check if it's a walkable area**
            if (hit.collider.CompareTag("Ground"))
            {
                greenCircle.SetActive(true);
                redCircle.SetActive(false);

                if (Input.GetMouseButtonDown(0)) // ✅ If left-clicked, move NPC
                {
                    agent.SetDestination(hit.point);
                    yellowCircle.transform.position = hit.point;
                    yellowCircle.SetActive(true);
                    isTraveling = true; // ✅ Movement is properly handled
                }
            }
            else
            {
                greenCircle.SetActive(false);
            }
        }

        // **🛠️ Step 5: Check if NPC reached the destination**
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
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

    /// <summary>
    /// **Checks if the mouse is over a UI element to prevent unintended movement.**
    /// </summary>
    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
           // Debug.Log($"🖱️ UI Raycast Hit: {result.gameObject.name}");
        }

        // ✅ Ignore NPC_Model colliders and prioritize UI elements
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("NPC_ClickableIcon") || result.gameObject.CompareTag("NPC_Text"))
            {
               // Debug.Log("🟡 Clicked on UI Element (NPC) - Ignoring movement!");
            }
        }

        return false; // ✅ Allow movement when clicking WalkableGround
    }
}