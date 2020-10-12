using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//default player handler, neither a seeker or a hunter
public class PlayerHandler : MonoBehaviour
{
    [SerializeField] protected PlayerPreset preset;

    [SerializeField] protected PlayerMovement playerMovement;
    [SerializeField] protected PlayerInteraction playerInteraction;

    bool inLight = false;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if (preset != null)
        {
            ApplyPreset();
        }
        else
        {
            Debug.Log("Player has not recieved a preset!");
        }

        //perform initial light test
        if (inLight = LightChecker.instance.performLightCheck(gameObject))
        {
            OnInLight();
        }
        else
        {
            OnOutLight();
        }
    }

    void ApplyPreset()
    {
        playerMovement.speed = preset.playerSpeed;

        playerInteraction.interactionDistance = preset.interactionDistance;
    }

    //unity doesnt like overriden updates
    private void Update()
    {
        SafeUpdate();
    }

    protected virtual void SafeUpdate()
    {
        PerformLightTest();

    }


    void PerformLightTest()
    {
        bool inLightState = LightChecker.instance.performLightCheck(gameObject);

        if (inLightState != inLight)
        {
            inLight = inLightState;
            if (inLightState)
            {
                OnInLight();
            }
            else
            {
                OnOutLight();
            }
        }
    }

    protected virtual void OnInLight()
    {
        Debug.Log("CHILD CLASS NOT APPLIED, lighthit");
    }

    protected virtual void OnOutLight()
    {
        Debug.Log("CHILD CLASS NOT APPLIED, lightmiss");
    }

}
