using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMonsterObjective : Interactable, IWallShuffle
{
    public bool InteractedWith = false;

	public override string GetInteractMessage(Interactor interact)
	{
        if(!interact.isRunner && !InteractedWith)
        {
            return "Destroy talisman";
		}
        return "";
	}

	public override void Interact(Interactor interact)
    {
        if (!interact.isRunner)
        {
            Debug.Log("Objective is Interacted with");
            InteractedWith = true;
            LevelManager.instance.RemoveChaserObjective(gameObject);
            // Destroy collider?
        }
    }
}
