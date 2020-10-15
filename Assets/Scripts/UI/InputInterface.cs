using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInterface : MonoBehaviour
{

    [SerializeField] HUDManager hud;
    [SerializeField] PauseManager pause;
    private bool paused = false;

    #region singleton

    public static InputInterface instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Multiple InputInterfaces");
            Destroy(this);
        }
    }

    #endregion singleton

    // Update is called once per frame
    void Update()
    {
        //adjust for deeper pause sceens
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
            if(paused)
            {
                hud.gameObject.SetActive(false);
                pause.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                pause.gameObject.SetActive(false);
                hud.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
