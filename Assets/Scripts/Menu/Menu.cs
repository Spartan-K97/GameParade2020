using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	[SerializeField] ScreenFade sf = null;
	[SerializeField] string lightScene = "";
	[SerializeField] string darkScene = "";

	void Start()
	{
		sf.FadeFromDefault(1, null);
		Cursor.lockState = CursorLockMode.Confined;
	}

	public void StartLight()
	{
		sf.FadeToBlack(1, StartLightFaded);
	}

	public void StartDark()
	{
		sf.FadeToBlack(1, StartDarkFaded);
	}
	public void Quit()
	{
		Application.Quit();
	}
	void StartLightFaded()
	{
		SceneManager.LoadScene(lightScene);
		Cursor.lockState = CursorLockMode.Locked;
	}
	void StartDarkFaded()
	{
		SceneManager.LoadScene(darkScene);
		Cursor.lockState = CursorLockMode.Locked;
	}
}
