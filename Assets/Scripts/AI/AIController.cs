using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] IMovement movement = null;

    private void Start()
    {
        movement.controller = transform;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 60 * Time.deltaTime, 0));
        movement.Move(1, 0, true);
    }
}
