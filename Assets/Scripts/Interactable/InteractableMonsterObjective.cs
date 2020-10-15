﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMonsterObjective : Interactable
{
    public override void Interact(Interactor interact)
    {
        if (!interact.isRunner)
        {
            LevelManager.instance.RemoveChaserObjective(gameObject);
        }
    }
}
