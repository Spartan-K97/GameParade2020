using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public void StartLight()
	{
		SceneManager.LoadScene("Scenes/PlayAsLight");
	}

	public void StartDark()
	{
		SceneManager.LoadScene("Scenes/PlayAsDark");
	}
}
