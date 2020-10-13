using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] bool playAsHuman;
    [SerializeField] ScreenFade sf;

    // Start is called before the first frame update
    void Start()
    {
        // Once loading is complete
        sf.FadeFromDefault(1, null);
    }
}
