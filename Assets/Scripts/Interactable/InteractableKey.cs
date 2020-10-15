﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : Interactable
{
    [SerializeField] int numKeys = 1;

	public override void Interact(Interactor interact)
	{
		if(interact.isRunner)
		{
			LevelManager.instance.AddKeys(numKeys);
		}
	}
}
