using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform facingDir;
    bool isCrouching = false;

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

	private void Update()
	{
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool crouchPressed = Input.GetButtonDown("Crouch");
        bool running = Input.GetButton("Sprint");

        if(crouchPressed)
        {
            Crouch(isCrouching = !isCrouching);
		}

        Sprint(running);
        Move(z, x);
    }

    void Move(float forwardSpeed, float strafeSpeed) // -1 to 1 values
    {
        float dir = (forwardSpeed >= 0)? 1 : -1;
        float totalSpeed = Mathf.Sqrt((forwardSpeed * forwardSpeed) + (strafeSpeed * strafeSpeed));
        Quaternion rot = (totalSpeed == 0)? Quaternion.identity : Quaternion.LookRotation(new Vector3(strafeSpeed, 0, forwardSpeed) * dir, new Vector3(0, 1, 0));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, facingDir.rotation * rot, 360 * Time.deltaTime);
        anim.SetFloat(animFloatDir, dir);
        anim.SetLayerWeight(animLayerForward, totalSpeed);
    }

    void Sprint(bool yes)
    {
        anim.SetBool(animBoolRun, yes);
    }
    void Crouch(bool yes)
    {
        anim.SetBool(animBoolCrouch, yes);
    }
}
