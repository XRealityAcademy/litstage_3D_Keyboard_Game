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
        // Prevent movement if clicking a UI button
        if (IsPointerOverUIElement())
        {
            return; // Prevent unintended movement
        }

        // Cast a Ray to Detect World Objects
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            // Print only when the left mouse button is pressed
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Clicked object: " + hit.collider.gameObject.name);
            }

            // SpaceElements (Floating Icons, Speech Bubbles)
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SpaceElements") && Input.GetMouseButtonDown(0))
            {
                Debug.Log("🟡 Clicked on a SpaceElement: " + hit.collider.gameObject.name);
                redCircle.transform.position = hit.point;
                redCircle.SetActive(true);
                greenCircle.SetActive(false);

                SpaceElement spaceElement = hit.collider.GetComponent<SpaceElement>();
                if (spaceElement != null)
                {
                    spaceElement.DisplayMessage();
                }
                else
                {
                    Debug.LogError("❌ SpaceElement NOT found on: " + hit.collider.gameObject.name);
                }
                return;
            }

            // Handle Collectible Items
            if (hit.collider.CompareTag("Collectible") && Input.GetMouseButtonDown(0))
            {
                CollectibleItem item = hit.collider.GetComponent<CollectibleItem>();
                if (item != null)
                {
                    item.TryCollect(transform);
                }
                return;
            }

            // Handle Walkable Areas (Ground & TriggerZone)
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

            // Handle Obstacles
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

        // Check if NPC Reached the Destination
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
                DialogInteractionV2 dialogInteractionV2 = result.gameObject.GetComponentInParent<DialogInteractionV2>();
                if (dialogInteractionV2 != null)
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