using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region singleton

    public static LevelManager instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public List<GameObject> chaserObjectives;

    public void RemoveChaserObjective(GameObject gameObject)
    {
        chaserObjectives.Remove(gameObject);
        if(chaserObjectives.Count == 0)
        {
            Debug.Log("Player can now be killed");
        }
    }

    public GameObject GetRandomObjective()
    {
        return chaserObjectives[Random.Range(0, chaserObjectives.Count -1)];
    }

}
