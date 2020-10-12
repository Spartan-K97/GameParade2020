using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General purpose UI manager for each screen
/// </summary>
public class UIScreen : MonoBehaviour
{
    [SerializeField] GameObject screen;

    public UIShown uiShown;
    public bool stopTimeOnScreen;
    public CursorLockMode cursorMode;


    public void SwitchScreen(bool setActive)
    {
        screen.SetActive(setActive);
    }

    public void ExitToUI()
    {
        InputInterface.instance.ExitToHUD();
    }
}

public enum UIShown
{
    HUD,
    Pause
}