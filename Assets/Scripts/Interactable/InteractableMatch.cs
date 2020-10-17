using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMatch : Interactable, ISurfaceShuffle
{
    [SerializeField] int numMatches = 1;

	public override string GetInteractMessage(Interactor interact)
	{
		if (interact.isRunner)
		{
			if (numMatches == 1)
			{
				return "Pick up Match";
			}
			return "Pick up Matches";
		}
		return "";
	}

	public override void Interact(Interactor interact)
	{
		if(interact.isRunner)
		{
			LevelManager.instance.AddMatches(numMatches);
			Destroy(gameObject);
		}
	}
}
