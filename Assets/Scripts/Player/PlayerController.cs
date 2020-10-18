using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] IMovement movement = null;

    bool isCrouching = false;

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip[] audioClips;

	private void Start()
	{
        movement.controller = transform;
	}

	private void Update()
    {
        if(LevelManager.instance.freeze) { return; }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        bool crouchPressed = Input.GetButtonDown("Crouch");
        bool running = Input.GetButton("Sprint");

        CheckAudio(x, z, running);

        if (crouchPressed)
        {
            movement.Crouch(isCrouching = !isCrouching);
        }

        movement.Sprint(running);
        movement.Move(z, x, true);
    }

    void CheckAudio(float x, float z, bool running)
    {
        if((x != 0) || (z != 0)) 
        {
            if(running)
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

    }


}
