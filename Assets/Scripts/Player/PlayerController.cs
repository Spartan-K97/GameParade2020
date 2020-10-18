using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] IMovement movement = null;

    bool isCrouching = false;

    [SerializeField] AudioSource audioSource = null;
    [SerializeField] AudioSource secondSource = null;


    [SerializeField] AudioClip[] audioClips = null;
    bool sprinting = false;

	private void Start()
	{
        movement.controller = transform;
	}

    private void Update()
    {
        if (LevelManager.instance.freeze) { return; }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        bool crouchPressed = Input.GetButtonDown("Crouch");
        bool running = Input.GetButton("Sprint");


        if (crouchPressed)
        {
            movement.Crouch(isCrouching = !isCrouching);
        }

        if(audioClips.Length > 0)
        {
            CheckAudio(x, z, running);
            if (running)
            {
             float sprintTime = LevelManager.instance.Sprint();
            if(sprintTime > 0)
            {
                StartCoroutine(SprintUntil(sprintTime));
                sprinting = true;
            }
            running = sprinting;
        }
        movement.Sprint(running);
        }

        movement.Move(z, x, true);
    }

    private IEnumerator SprintUntil(float sprintTime)
    {
        yield return new WaitForSeconds(sprintTime - 1.5f);
        secondSource.Play();
        yield return new WaitForSeconds(1.5f);
        sprinting = false;
        secondSource.Pause();
    }


    void CheckAudio(float x, float z, bool running)
    {
        if((x != 0) || (z != 0)) 
        {
            if(sprinting)
            {
                
                if(!(audioSource.clip == audioClips[1] && audioSource.isPlaying))
                {
                    audioSource.clip = audioClips[1];
                    audioSource.Play();
                }
            }
            else
            {
                if (!(audioSource.clip == audioClips[0] && audioSource.isPlaying))
                {
                    audioSource.clip = audioClips[0];
                    audioSource.Play();
                }
            }
        }
        else
        {
            audioSource.Stop();
        }

    }
    
}
