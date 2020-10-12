using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//provides details to playerStats
public class PlayerHandler : MonoBehaviour
{
    [SerializeField] PlayerPreset preset;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerInteraction playerInteraction;

    private void Start()
    {
        if (preset != null)
        {
            ApplyPreset();
        }
        else
        {
            Debug.Log("Player has not recieved a preset!");
        }
    }

    void ApplyPreset()
    {
        playerMovement.speed = preset.playerSpeed;

        playerInteraction.interactionDistance = preset.interactionDistance;
    }

    private void Update()
    {
        LightChecker.instance.performLightCheck(gameObject);
    }



}
