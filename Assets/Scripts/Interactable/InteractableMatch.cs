﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMatch : Interactable
{
    [SerializeField] int numMatches = 1;

	public override void Interact(Interactor interact)
	{
		if(interact.isRunner)
		{
			LevelManager.instance.AddMatches(numMatches);
		}
	}
}
