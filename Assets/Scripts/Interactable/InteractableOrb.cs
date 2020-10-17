using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOrb : Interactable, ISurfaceShuffle
{
	public override string GetInteractMessage(Interactor interact)
    {
        if (interact.isRunner)
        {
            return "Pick Up Orb";
        }
        return "";
    }

	public override void Interact(Interactor interact)
    {
        if (interact.isRunner)
        {
            LevelManager.instance.playerHasOrb = true;
            FindObjectOfType<HumanMovement>().SetOrbHeld(true);
            Destroy(gameObject);
        }
    }
}
