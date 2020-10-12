using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class InteractableLight : Interactable
{
    [SerializeField] GameObject pointLight;
    public float lightDistance;
    public bool lightIsOn;

    public override void Interact(PlayerInteraction interact)
    {
        if(interact.isRunner)
        {
            lightIsOn = true;
            pointLight.SetActive(true);
        }
        else
        {
            lightIsOn = false;
            pointLight.SetActive(false);
        }
    }

}

