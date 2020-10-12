using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] HUDManager hudManager;

    public bool isRunner = false;
    public float interactionDistance = 5.0f;
    //The interactable object the player is looking at
    Interactable interactHold;

    void Update()
    {
        if (hudManager == null)
        {
            Debug.Log("No interface found in PlayerInteraction");
        }
        if (Input.GetButtonDown("Interact")) 
        {
            if(interactHold != null)
            {
                interactHold.Interact(this);
                ForgetInteractable();//reset the UI string
            }
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (!interactable == interactHold)
                {
                    HoldInteractable(interactable);
                }
            }
            else 
            {
                if(interactHold != null)
                {
                    ForgetInteractable();
                }
            }
        }
        else 
        {
            ForgetInteractable();
        }
    }

    void HoldInteractable(Interactable interactable)
    {
        interactHold = interactable;
        //give the ui the interaction text
        string iText = interactHold.interactText;
        //Debug.Log(iText);
        hudManager.SetInteractionText(iText);
    }

    void ForgetInteractable()
    {
        interactHold = null;
        //erase text from ui
        hudManager.SetInteractionText("");
    }

}
