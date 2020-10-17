using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for interactable things
/// </summary>
public class Interactable : MonoBehaviour
{
    public virtual string GetInteractMessage(Interactor interact) // Empty string = no interaction
    {
        // Reason: Different message for human/monster
        return "";
    }

    public virtual void Interact(Interactor interact)
    {
        Debug.Log("NO INTERACTION SET", gameObject);
    }
}
