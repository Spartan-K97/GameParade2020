﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOrb : Interactable
{
    [SerializeField] GameObject exit;
    public override void Interact(Interactor interact)
    {
        if (interact.isRunner)
        {
            exit.SetActive(true);

            Destroy(gameObject);
        }
    }
}
