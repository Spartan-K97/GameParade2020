using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterHandler : PlayerHandler
{
    protected override void OnInLight()
    {
        playerMovement.speed = preset.alternateSpeed;
    }

    protected override void OnOutLight()
    {
        playerMovement.speed = preset.playerSpeed;
    }

}
