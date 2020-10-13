using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : IMovement
{
    [SerializeField] Transform human;
    [SerializeField] Animator anim;
    [SerializeField] Transform controller;

    int animLayerForward;
    int animLayerStrafe;
    int animBoolCrouch;
    int animBoolRun;
    int animBoolOrb;
    int animFloatDir;

    private void Start()
    {
        animLayerForward = anim.GetLayerIndex("Forward");
        animLayerStrafe = anim.GetLayerIndex("Strafe");
        animBoolRun = Animator.StringToHash("Running");
        animBoolCrouch = Animator.StringToHash("Crouching");
        animBoolOrb = Animator.StringToHash("HasOrb");
        animFloatDir = Animator.StringToHash("Direction");
    }

    public override void Move(float forwardSpeed, float strafeSpeed, bool LockFacingDir) // -1 to 1 values
    {
        float totalSpeed = Mathf.Sqrt((forwardSpeed * forwardSpeed) + (strafeSpeed * strafeSpeed));
        anim.SetLayerWeight(animLayerForward, totalSpeed);
        if (LockFacingDir)
        {
            float dir = (forwardSpeed >= 0) ? 1 : -1;
            Quaternion rot = (totalSpeed == 0) ? Quaternion.identity : Quaternion.LookRotation(new Vector3(strafeSpeed, 0, forwardSpeed) * dir, new Vector3(0, 1, 0));

            human.rotation = Quaternion.RotateTowards(human.rotation, controller.rotation * rot, 360 * Time.deltaTime);
            anim.SetFloat(animFloatDir, dir);
            anim.SetLayerWeight(animLayerForward, totalSpeed);
        }
        else
        {
            Quaternion rot = (totalSpeed == 0) ? Quaternion.identity : Quaternion.LookRotation(new Vector3(strafeSpeed, 0, forwardSpeed), new Vector3(0, 1, 0));
            human.rotation = Quaternion.RotateTowards(human.rotation, rot, 360 * Time.deltaTime);
        }
        controller.position = human.position;
    }
    public override void Sprint(bool yes)
    {
        anim.SetBool(animBoolRun, yes);
    }
    public override void Crouch(bool yes)
    {
        anim.SetBool(animBoolCrouch, yes);
    }
    public override void SetOrbHeld(bool yes)
    {
        anim.SetBool(animBoolOrb, yes);
    }
}
