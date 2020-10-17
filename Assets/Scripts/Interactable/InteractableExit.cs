using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableExit : Interactable
{
	public override string GetInteractMessage(Interactor interact)
	{
        if(interact.isRunner)
        {
            if(LevelManager.instance.PlayerHasKey())
            {
                return "Use Key";
			}
            return "Needs Key";
		}
        return "";
	}

	public override void Interact(Interactor interact)
    {
        if (interact.isRunner)
        {
            Debug.Log("Game WON");
        }
    }
}
