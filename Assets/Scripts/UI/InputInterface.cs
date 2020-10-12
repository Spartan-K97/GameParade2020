using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInterface : MonoBehaviour
{

    public List<UIScreen> uiScreen;

    UIShown currentUI = UIShown.HUD;

    [SerializeField] public HUDManager hudManager;

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
            switch (currentUI)
            {
                case UIShown.HUD://we pause the game
                    SwitchUIShown(UIShown.Pause);
                    break;

                //resume the game
                case UIShown.Pause:
                    SwitchUIShown(UIShown.HUD);
                    break;
                default:
                    break;
            }

        }
    }

    void SwitchUIShown(UIShown ui)
    {
        currentUI = ui;

        foreach (var screen in uiScreen)
        {
            if (screen.uiShown == ui)
            {
                HandleTime(screen.stopTimeOnScreen);
                screen.SwitchScreen(screen.uiShown == ui);
                Cursor.lockState = screen.cursorMode;
            }
            else
            {
                screen.SwitchScreen(false);
            }
        }
    }

    //
    public void ExitToHUD()
    {
        SwitchUIShown(UIShown.HUD);
    }

    void HandleTime(bool timeStop)
    {
        if (timeStop)
        {
            Debug.Log("tstop");
            Time.timeScale = 0;
        }
        else
        {
            Debug.Log("tgo");

            Time.timeScale = 1;
        }
    }

}
