using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : IMovement
{
    Animator anim;
    [SerializeField] Transform firstPersonArm = null;
    [SerializeField] float armLiftDuration = 1;
    [SerializeField] GameObject orbLight = null;
    private Vector3 armUpPos; // Replace pos with Rotation when mesh added
    private Vector3 armDownPos;

    int animLayerForward;
    int animLayerStrafe;
    int animBoolCrouch;
    int animBoolRun;
    int animBoolOrb;
    int animFloatDir;

    void Start()
    {
        if (firstPersonArm != null)
        {
            armUpPos = firstPersonArm.localPosition;
            armDownPos = armUpPos - new Vector3(0, 1, 0);
            firstPersonArm.localPosition = armDownPos;
        }
        anim = GetComponent<Animator>();
        animLayerForward = anim.GetLayerIndex("Forward");
        animLayerStrafe = anim.GetLayerIndex("Strafe");
        animBoolRun = Animator.StringToHash("Running");
        animBoolCrouch = Animator.StringToHash("Crouching");
        animBoolOrb = Animator.StringToHash("HasOrb");
        animFloatDir = Animator.StringToHash("Direction");
        Transform spawnPos = GameObject.Find("Human Spawn Location").transform;
        controller.position = transform.position = spawnPos.position;
        controller.rotation = transform.rotation = spawnPos.rotation;
    }

    public override void Move(float forwardSpeed, float strafeSpeed, bool LockFacingDir) // -1 to 1 values
    {
        if (LevelManager.instance.freeze && (forwardSpeed != 0 || strafeSpeed != 0))
        {
            Move(0, 0, LockFacingDir);
            return;
        }
        float totalSpeed = Mathf.Sqrt((forwardSpeed * forwardSpeed) + (strafeSpeed * strafeSpeed));
        RaycastHit hit;
        if (rb != null && rb.SweepTest(Vector3.Normalize(new Vector3(strafeSpeed, 0, forwardSpeed)), out hit, (totalSpeed * Time.deltaTime) + 0.05f) && totalSpeed > 0)
        {
            totalSpeed = 0;
            Debug.Log("Human walking into wall");
        }
        anim.SetLayerWeight(animLayerForward, totalSpeed);
        if (LockFacingDir)
        {
            float dir = (forwardSpeed >= 0) ? 1 : -1;
            Quaternion rot = (totalSpeed == 0) ? Quaternion.identity : Quaternion.LookRotation(new Vector3(strafeSpeed, 0, forwardSpeed) * dir, new Vector3(0, 1, 0));

            transform.rotation = Quaternion.RotateTowards(transform.rotation, controller.rotation * rot, 360 * Time.deltaTime * 2);
            anim.SetFloat(animFloatDir, dir);
            anim.SetLayerWeight(animLayerForward, totalSpeed);
        }
        else
        {
            Quaternion rot = (totalSpeed == 0) ? Quaternion.identity : Quaternion.LookRotation(new Vector3(strafeSpeed, 0, forwardSpeed), new Vector3(0, 1, 0));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 360 * Time.deltaTime * 2);
        }
        controller.position = transform.position;
    }
    public override void Sprint(bool yes)
    {
        anim.SetBool(animBoolRun, yes);
    }
    public override void Crouch(bool yes)
    {
        //anim.SetBool(animBoolCrouch, yes);
    }
    public override void SetOrbHeld(bool yes)
    {
        anim.SetBool(animBoolOrb, yes);
        orbLight.SetActive(yes);
        if (firstPersonArm != null)
        {
            if (yes)
            {
                StartCoroutine(ShowArm());
            }
            else
            {
                StartCoroutine(HideArm());
            }
        }
    }
    IEnumerator ShowArm()
    {
        firstPersonArm.gameObject.SetActive(true);
        float time = 0;
        while(time < 1)
        {
            yield return true;
            time += Time.deltaTime * armLiftDuration;
            firstPersonArm.localPosition = Vector3.Lerp(armDownPos, armUpPos, time);
        }
        firstPersonArm.localPosition = armUpPos;
        firstPersonArm.gameObject.SetActive(true);
    }
    IEnumerator HideArm()
    {
        firstPersonArm.gameObject.SetActive(true);
        float time = 1;
        while (time > 0)
        {
            yield return true;
            time -= Time.deltaTime * armLiftDuration;
            firstPersonArm.localPosition = Vector3.Lerp(armDownPos, armUpPos, time);
        }
        firstPersonArm.localPosition = armDownPos;
        firstPersonArm.gameObject.SetActive(false);
    }
}
