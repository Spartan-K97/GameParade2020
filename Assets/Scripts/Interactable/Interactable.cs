using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for interactable things
/// </summary>
public class Interactable : MonoBehaviour
{
    public string interactText = "Interaction Text Not Set";

    public virtual void Interact(PlayerInteraction interact)
    {
        Debug.Log("NO INTERACTION SET", gameObject);
    }
}
