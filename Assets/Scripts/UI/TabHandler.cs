using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabHandler : MonoBehaviour
{
    public GameObject[] tabs;

    public void ActivateTab(GameObject tabToActivate)
    {

        Debug.Log("Tab pressed");

        foreach (GameObject tab in tabs)
        {
            if(tab != tabToActivate)
            {
                tab.SetActive(false);
            }
            else
            { 
                tab.SetActive(true);
            }
        }
    }

}
