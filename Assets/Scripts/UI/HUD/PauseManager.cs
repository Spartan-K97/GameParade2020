using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    bool isPaused = false;

    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            SwitchPause();
        }
    }

    public void SwitchPause()
    {
        if (isPaused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            AudioListener.pause = false;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            AudioListener.pause = true;
            Cursor.lockState = CursorLockMode.Confined;
            isPaused = true;
        }
    }

    public void ToMainMenu()
    {

        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
        AudioListener.pause = true;
        SceneManager.LoadScene("Menu");
    }



}
