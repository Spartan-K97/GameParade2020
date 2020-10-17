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

    private List<InteractableLight> lights = null;
    [SerializeField] LayerMask ignoreLayers = 0;

    private void Start()
    {
        lights = new List<InteractableLight>(FindObjectsOfType<InteractableLight>());
    }

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
