using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic : MonoBehaviour
{
    [SerializeField] ScreenFade sf = null;
    [SerializeField] float fadeInDuration = 1;
    [SerializeField] float fadeOutDuration = 1;
    [SerializeField] string nextScene = "";
    bool fadedIn = false;
    void Start()
    {
        sf.FadeFromDefault(fadeInDuration, FadeInComplete);
    }
    void FadeInComplete()
    {
        fadedIn = true;
	}

	void Update()
	{
		if(fadedIn && Input.anyKeyDown)
        {
            sf.FadeToDefault(fadeOutDuration, FadeOutComplete);
		}
	}
    void FadeOutComplete()
    {
        SceneManager.LoadScene(nextScene);
	}
}
