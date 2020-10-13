using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMapInfo : MonoBehaviour
{
    #region Singleton

    public static AIMapInfo instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    #endregion

    public Vector3 mapSize;
    public Vector3 mapOffset;
}
