using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterMovement : IMovement
{
    [SerializeField] Transform human;

	private void Start()
    {
        Transform spawnPos = GameObject.Find("Monster Spawn Location").transform;
        controller.position = transform.position = spawnPos.position;
        controller.rotation = transform.rotation = spawnPos.rotation;
    }

	private void Update()
	{
        if (Vector3.Distance(transform.position, human.position) < 1)
        {
            LevelManager.instance.freeze = true;
            FindObjectOfType<ScreenFade>().FadeToBlack(2, () => SceneManager.LoadScene("Outro Death"));
        }
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

            transform.rotation = Quaternion.RotateTowards(transform.rotation, controller.rotation * rot, 360 * Time.deltaTime);
        }
        else
        {
            Quaternion rot = (totalSpeed == 0) ? Quaternion.identity : Quaternion.LookRotation(new Vector3(strafeSpeed, 0, forwardSpeed), new Vector3(0, 1, 0));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 360 * Time.deltaTime);
        }
        transform.position += controller.rotation * new Vector3(strafeSpeed, 0, forwardSpeed);
        controller.position = transform.position;
    }
}
