using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : Interactor
{
    [SerializeField] HUDManager hudManager = null;

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
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, interactionDistance, LayerMask.NameToLayer("Interactable")))
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
        string iText = interactHold.GetInteractMessage(this);
        //Debug.Log(iText);
        hudManager.SetInteractionText(iText);
    }

    void ForgetInteractable()
    {
        if (interactHold != null)
        {
            interactHold = null;
            //erase text from ui
            hudManager.SetInteractionText("");
        }
    }

}
