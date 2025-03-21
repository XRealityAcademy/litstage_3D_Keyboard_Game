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
        // ✅ **Step 1: Prevent movement if clicking a UI button**
        if (IsPointerOverUIElement())
        {
            return; // Prevent unintended movement
        }

        // ✅ **Step 2: Cast a Ray to Detect World Objects**
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            // ✅ **Step 3: Detect SpaceElements (Floating Icons, Speech Bubbles)**
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SpaceElements") && Input.GetMouseButtonDown(0))
            {
                Debug.Log("🟡 Clicked on a SpaceElement: " + hit.collider.gameObject.name);

                // ✅ Show RedCircle at the clicked position
                redCircle.transform.position = hit.point;
                redCircle.SetActive(true);
                greenCircle.SetActive(false);

                // ✅ Try to display the message
                SpaceElement spaceElement = hit.collider.GetComponent<SpaceElement>();
                if (spaceElement != null)
                {
                    spaceElement.DisplayMessage();
                }
                else
                {
                    Debug.LogError("❌ SpaceElement NOT found on: " + hit.collider.gameObject.name);
                }

                return; // ✅ Prevent movement
            }

            // ✅ **Step 4: Handle Walkable Area (Ground & TriggerZone)**
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("TriggerZone"))
            {
                greenCircle.transform.position = hit.point;
                greenCircle.SetActive(true);
                redCircle.SetActive(false);

                if (Input.GetMouseButtonDown(0))
                {
                    agent.SetDestination(hit.point);
                    yellowCircle.transform.position = hit.point;
                    yellowCircle.SetActive(true);
                    isTraveling = true;
                }
            }
            else
            {
                greenCircle.SetActive(false);
            }

            // ✅ **Step 5: Handle Obstacles**
            if (hit.collider.CompareTag("Object"))
            {
                redCircle.transform.position = hit.point;
                redCircle.SetActive(true);
                greenCircle.SetActive(false);
            }
            else
            {
                redCircle.SetActive(false);
            }
        }

        // ✅ **Step 6: Check if NPC Reached the Destination**
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
    /// ✅ **Prevents movement if clicking UI Elements (NPC Buttons, Dialog, Close)**
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
            if ((result.gameObject.CompareTag("NPC_ClickableIcon") ||
                result.gameObject.CompareTag("NPC_Text") ||
                result.gameObject.CompareTag("NPC_CloseButton")) && Input.GetMouseButtonDown(0))
            {
                DialogInteraction dialogInteraction = result.gameObject.GetComponentInParent<DialogInteraction>();
                DialogInteractionV2 dialogInteractionV2 = result.gameObject.GetComponentInParent<DialogInteractionV2>();

                if (dialogInteraction != null)
                {
                    dialogInteraction.OnNPCIconClick();
                    return true;
                }
                else if (dialogInteractionV2 != null)
                {
                    dialogInteractionV2.OnNPCIconClick(); // ✅ Now correctly calls OnNPCIconClick()
                    return true;
                }
                else
                {
                    Debug.LogError("❌ No valid DialogInteraction found on: " + result.gameObject.name);
                }
            }
        }

        return false;
    }
}