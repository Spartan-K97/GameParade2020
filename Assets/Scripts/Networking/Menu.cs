using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	[SerializeField] Button lightButton;
	[SerializeField] Button darkButton;

	public void StartLight()
	{
		lightButton.interactable = false;
		darkButton.interactable = true;
		NetworkManager mngr = NetworkManager.GetInstance();
		mngr.Shutdown();
		mngr.StartServer();
	}

	public void StartDark()
	{
		lightButton.interactable = true;
		darkButton.interactable = false;
		NetworkManager mngr = NetworkManager.GetInstance();
		mngr.Shutdown();
		mngr.StartClient();
	}
}
