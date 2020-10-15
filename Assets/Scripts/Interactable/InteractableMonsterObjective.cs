using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMonsterObjective : Interactable
{
    public bool InteractedWith = false;

    public override void Interact(Interactor interact)
    {
        if (!interact.isRunner)
        {
            Debug.Log("Objective is Interacted with");
            InteractedWith = true;
            LevelManager.instance.RemoveChaserObjective(gameObject);
        }
    }
}
