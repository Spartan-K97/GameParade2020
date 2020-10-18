using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableExit : Interactable
{
    private bool isUnlocked = false;
    private bool exitTriggered = false;

	public override string GetInteractMessage(Interactor interact)
	{
        if(exitTriggered) { return ""; }
        if(interact.isRunner)
        {
            if (isUnlocked)
            {
                if(LevelManager.instance.playerHasOrb)
                {
                    return "Leave With Orb";
				}
                else
                {
                    return "Leave Without Orb";
				}
            }
            else
            {
                if (LevelManager.instance.PlayerHasKey())
                {
                    return "Use Key";
                }
                return "Needs Key";
            }
		}
        return "";
	}

	public override void Interact(Interactor interact)
    {
        if (interact.isRunner)
        {
            if (isUnlocked)
            {
                exitTriggered = true;
                LevelManager.instance.freeze = true;
                if(LevelManager.instance.playerHasOrb)
                {
                    FindObjectOfType<ScreenFade>().FadeToWhite(2, () => SceneManager.LoadScene("Outro Hero"));
				}
                else
                {
                    FindObjectOfType<ScreenFade>().FadeToWhite(2, () => SceneManager.LoadScene("Outro Coward"));
                }
            }
            else
            {
                if(LevelManager.instance.UseKey())
                {
                    isUnlocked = true;
				}
			}
        }
    }

    //private void EndGameCoward()
    //{
    //    SceneManager.LoadScene("Outro Coward");
    //}
    //private void EndGameHero()
    //{
    //    SceneManager.LoadScene("Outro Hero");
    //}
}
