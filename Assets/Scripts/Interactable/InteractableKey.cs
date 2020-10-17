using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : Interactable, ISurfaceShuffle
{
    [SerializeField] int numKeys = 1;

	public override string GetInteractMessage(Interactor interact)
	{
		if(interact.isRunner)
		{
			if(numKeys == 1)
			{
				return "Pick up Key";
			}
			return "Pick up Keys";
		}
		return "";
	}

	public override void Interact(Interactor interact)
	{
		if(interact.isRunner)
		{
			LevelManager.instance.AddKeys(numKeys);
			Destroy(gameObject);
		}
	}
}
