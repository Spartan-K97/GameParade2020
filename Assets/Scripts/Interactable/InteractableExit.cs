﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableExit : Interactable
{
    public override void Interact(Interactor interact)
    {
        if (interact.isRunner)
        {
            Debug.Log("Game WON");
        }
    }
}
