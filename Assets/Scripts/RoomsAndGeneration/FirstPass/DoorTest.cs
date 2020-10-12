using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
    [SerializeField] Door d;
    // Start is called before the first frame update
    void Start()
    {
        d.FinaliseDoorType();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
