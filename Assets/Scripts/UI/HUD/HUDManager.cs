using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the heads up display
/// </summary>
public class HUDManager : UIScreen
{
    HUDManager()
    {
        uiShown = UIShown.HUD;
        cursorMode = CursorLockMode.Locked;
        stopTimeOnScreen = false;
    }

    public Text interactionText;

    public void SetInteractionText(string iText) 
    {
        interactionText.text = iText;
    }

}
