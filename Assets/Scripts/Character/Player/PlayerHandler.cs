using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//provides details to playerStats
public class PlayerHandler : MonoBehaviour
{
    #region Singleton
    public static PlayerHandler instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion Singleton


    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] bool buttonUsed = false;

    private void Update()
    {
        if(buttonUsed)
        {
            buttonUsed = false;
        }
    }



}
