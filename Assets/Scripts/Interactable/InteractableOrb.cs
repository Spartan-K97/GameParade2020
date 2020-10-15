using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOrb : Interactable, IShuffle
{
    [SerializeField] GameObject exit;
    public override void Interact(Interactor interact)
    {
        if (interact.isRunner)
        {
            exit.SetActive(true); // Supposed to be able to exit without orb
            FindObjectOfType<HumanMovement>().SetOrbHeld(true); // Slow, but only called once in game, so is fine
            Destroy(gameObject);
        }
    }
}
