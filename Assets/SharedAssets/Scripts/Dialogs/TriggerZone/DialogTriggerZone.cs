using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTriggerZone : MonoBehaviour
{

    [SerializeField] private DialogInteraction dialogInteraction; // Reference to DialogInteraction

    private void Start()
    {
        // If not assigned, try to find it automatically
        if (dialogInteraction == null)
        {
            dialogInteraction = GetComponentInParent<DialogInteraction>();
        }

        if (dialogInteraction == null)
        {
            Debug.LogError("? No DialogInteraction found in parent! Ensure FloatingDialog is a child of NPC.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ✅ Only triggers when the Player enters
        {
            Debug.Log("🟢 Player entered DialogTriggerZone: " + gameObject.name);

            if (dialogInteraction != null)
            {
                Debug.Log("✅ Player Triggered Dialog!");
                dialogInteraction.OnNPCIconClick(); // ✅ Now only triggers via Player movement
            }
            else
            {
                Debug.LogWarning("❌ No DialogInteraction found on: " + gameObject.name);
            }
        }
    }
}
 