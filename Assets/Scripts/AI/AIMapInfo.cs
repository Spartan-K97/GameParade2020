using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMapInfo : MonoBehaviour
{
    #region Singleton

    public static AIMapInfo instance;
    [SerializeField] Transform mapNegCorner = null;
    [SerializeField] Transform mapPosCorner = null;
    [HideInInspector] public Vector3 maxExtent;
    [HideInInspector] public Vector3 minExtent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            maxExtent = mapPosCorner.position;
            minExtent = mapNegCorner.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    #endregion
}
