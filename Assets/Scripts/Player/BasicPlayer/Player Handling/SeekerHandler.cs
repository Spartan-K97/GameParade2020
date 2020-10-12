using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerHandler : PlayerHandler
{
    public bool vulnerable = false;

    protected override void OnInLight()
    {
        vulnerable = false;
    }

    protected override void OnOutLight()
    {
        vulnerable = true;
    }

    protected override void SafeUpdate()
    {
        base.SafeUpdate();
        if(Input.GetButton("Sprint"))
        {
            playerMovement.speed = preset.alternateSpeed;
        }
        else
        {
            playerMovement.speed = preset.playerSpeed;
        }
    }

}
