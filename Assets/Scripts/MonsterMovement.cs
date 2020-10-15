using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : IMovement
{
    [SerializeField] Transform monster;
    [SerializeField] Transform controller;

	private void Start()
    {
        Transform spawnPos = GameObject.Find("Monster Spawn Location").transform;
        controller.position = monster.position = spawnPos.position;
        controller.rotation = monster.rotation = spawnPos.rotation;
    }

	public override void Move(float forwardSpeed, float strafeSpeed, bool LockFacingDir) // -1 to 1 values
    {
        forwardSpeed *= Time.deltaTime * 2;
        strafeSpeed *= Time.deltaTime * 2;
        float totalSpeed = Mathf.Sqrt((forwardSpeed * forwardSpeed) + (strafeSpeed * strafeSpeed));
        if (LockFacingDir)
        {
            float dir = (forwardSpeed >= 0) ? 1 : -1;
            Quaternion rot = (totalSpeed == 0) ? Quaternion.identity : Quaternion.LookRotation(new Vector3(strafeSpeed, 0, forwardSpeed) * dir, new Vector3(0, 1, 0));

            monster.rotation = Quaternion.RotateTowards(monster.rotation, controller.rotation * rot, 360 * Time.deltaTime);
        }
        else
        {
            Quaternion rot = (totalSpeed == 0) ? Quaternion.identity : Quaternion.LookRotation(new Vector3(strafeSpeed, 0, forwardSpeed), new Vector3(0, 1, 0));
            monster.rotation = Quaternion.RotateTowards(monster.rotation, rot, 360 * Time.deltaTime);
        }
        monster.position += controller.rotation * new Vector3(strafeSpeed, 0, forwardSpeed);
        controller.position = monster.position;
    }
}
