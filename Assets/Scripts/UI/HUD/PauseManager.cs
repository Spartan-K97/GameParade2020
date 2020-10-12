using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

using UnityEngine.UI;

public class PauseManager : UIScreen
{
    PauseManager()
    {
        uiShown = UIShown.Pause;
        cursorMode = CursorLockMode.Confined;
        stopTimeOnScreen = true;
    }
}
