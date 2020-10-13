﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	[SerializeField] ScreenFade sf;
	[SerializeField] string lightScene;
	[SerializeField] string darkScene;

	void Start()
	{
		sf.FadeFromDefault(1, null);
	}

	public void StartLight()
	{
		sf.FadeToBlack(1, StartLightFaded);
	}

	public void StartDark()
	{
		sf.FadeToBlack(1, StartDarkFaded);
	}
	void StartLightFaded()
	{
		SceneManager.LoadScene(lightScene);
	}
	void StartDarkFaded()
	{
		SceneManager.LoadScene(darkScene);
	}
}