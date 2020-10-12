using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class InteractableLight : Interactable
{
    [SerializeField] GameObject pointLight;
    public override void Interact(PlayerInteraction interact)
    {
        pointLight.SetActive(false);
    }

}

