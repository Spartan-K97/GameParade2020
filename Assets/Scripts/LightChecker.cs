using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChecker : MonoBehaviour
{
    #region singleton

    public static LightChecker instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [SerializeField] List<InteractableLight> lights;
    [SerializeField] LayerMask ignoreLayers;
    public bool performLightCheck(GameObject objectToCheck)
    {
        foreach (InteractableLight safeLight in lights)
        {
            if (safeLight.lightIsOn)
            {
                if (Vector3.Distance(safeLight.transform.position, objectToCheck.transform.position) < safeLight.lightDistance)
                {
                    if (!Physics.Linecast(safeLight.transform.position, objectToCheck.transform.position, ignoreLayers))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

}
