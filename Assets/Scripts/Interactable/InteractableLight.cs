using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class InteractableLight : Interactable
{
    [SerializeField] GameObject pointLight = null;
    public float lightDistance;
    public bool lightIsOn;

	public override string GetInteractMessage(Interactor interact)
	{
        if(lightIsOn && !interact.isRunner)
        {
            return "Snuff Out Light";
		}
        else if(!lightIsOn && interact.isRunner)
        {
            if(LevelManager.instance.PlayerHasMatch())
            {
                return "Turn On Light";
			}
            else
            {
                return "Needs Match";
			}
		}
        return "";
	}

	public override void Interact(Interactor interact)
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

