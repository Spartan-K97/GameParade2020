using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMovement : MonoBehaviour
{
    [HideInInspector] public Transform controller;

    public virtual void Move(float forwardSpeed, float strafeSpeed, bool LockFacingDir) { } // -1 to 1 values
    public virtual void Sprint(bool yes) { }
    public virtual void Crouch(bool yes) { }
    public virtual void SetOrbHeld(bool yes) { }
}
