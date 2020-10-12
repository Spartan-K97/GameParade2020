using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerHandler : PlayerHandler
{
    public bool vulnerable = false;


    protected override void OnInLight()
    {
        vulnerable = true;
    }

    protected override void OnOutLight()
    {
        vulnerable = false;
    }
}
