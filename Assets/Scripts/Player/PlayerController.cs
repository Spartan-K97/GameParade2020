﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] IMovement movement;

    bool isCrouching = false;

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool crouchPressed = Input.GetButtonDown("Crouch");
        bool running = Input.GetButton("Sprint");

        if (crouchPressed)
        {
            movement.Crouch(isCrouching = !isCrouching);
        }

        movement.Sprint(running);
        movement.Move(z, x, true);
    }
}