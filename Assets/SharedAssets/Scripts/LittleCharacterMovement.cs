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
           // Debug.Log("🟡 Clicked on UI Element - Ignoring movement!");
            return; // ✅ Prevent movement if clicking UI
        }

        // **🛠️ Step 2: Cast a ray from the crosshair to detect world objects**
        ray = cam.ScreenPointToRay(crosshair.transform.position);

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log($"🖱️ Clicked on: {hit.collider.gameObject.name}");

            // **🛠️ If clicking an NPC speech bubble or icon, prevent movement**
            if (hit.collider.CompareTag("NPC_SpeechBubble") || hit.collider.CompareTag("NPC_ClickableIcon"))
            {
                //Debug.Log("🟡 Clicked on NPC UI - Ignoring movement!");
                return; // ✅ Prevent movement if clicking on UI
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
                //Debug.Log("✅ Clicked on GROUND at: " + hit.point);

                greenCircle.SetActive(true);
                redCircle.SetActive(false);

                if (Input.GetMouseButtonDown(0)) // ✅ If left-clicked, move NPC
                {
                    //Debug.Log("🟢 Moving NPC_LittleCharacter to: " + hit.point);
                    agent.SetDestination(hit.point);
                    yellowCircle.transform.position = hit.point;
                    yellowCircle.SetActive(true);
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
            Debug.Log($"🖱️ UI Raycast Hit: {result.gameObject.name}");
        }

        // ✅ Ignore NPC_Model colliders and prioritize UI elements
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("NPC_ClickableIcon") || result.gameObject.CompareTag("NPC_Text"))
            {
                Debug.Log("🟡 Clicked on UI Element (NPC) - Ignoring movement!");
                //return true; // ❌ Prevent movement
            }
        }

        return false; // ✅ Allow movement when clicking WalkableGround
    }
}