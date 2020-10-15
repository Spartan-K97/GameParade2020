using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
	void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Confined;
	}
	void OnDisable()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
}
