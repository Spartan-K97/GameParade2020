using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region singleton

    private static LevelManager privateInstance;
    public static LevelManager instance { get
    {
        return privateInstance;
    }}

	private void Awake()
	{
		if(privateInstance == null)
        {
            privateInstance = this;
		}
        else
        {
            Debug.LogError("Multiple LevelManagers found in scene, only one should be instantiated");
		}
	}

	#endregion

	#region UI
	[SerializeField] HUDManager hud;
    [SerializeField] PauseManager pause;
    [SerializeField] ScreenFade sf;
    private bool paused = false;

	private void Start()
	{
        // Once loading is complete
        sf.FadeFromDefault(1, null);
    }
	void Update()
    {
        //adjust for deeper pause sceens
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
            if (paused)
            {
                hud.gameObject.SetActive(false);
                pause.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                pause.gameObject.SetActive(false);
                hud.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
	#endregion

	#region human
	public List<GameObject> chaserObjectives;
    private int playerNumKeys = 0;
    private int playerNumKeysUsed = 0;
    private int playerNumMatches = 0;
    private bool canSprint = false;
    [SerializeField] int maxKeys = 5;
    [SerializeField] int numKeysInGame = 5;
    [SerializeField] int maxMatches = 5;
    [SerializeField] float maxSprintDuration = 2.0f;
    [SerializeField] float sprintCooldownDuration = 5.0f;

    public int AddKeys(int numKeys)
    {
        int keysLeft = Mathf.Max(0, playerNumKeys + numKeys - maxKeys);
        playerNumKeys = Mathf.Min(maxKeys, playerNumKeys + numKeys);
        hud.AddKey(numKeys - keysLeft);
        return keysLeft;
	}
    public bool UseKey() // Return true if the correct key is used
    {
        if(playerNumKeys <= 0) { return false; }
        --playerNumKeys;
        hud.RemoveKey(1);
        ++playerNumKeysUsed;
        if(Random.Range(0, numKeysInGame) < playerNumKeysUsed) { return true; }
        return false;
	}
    public int AddMatches(int numMatches)
    {
        int matchesLeft = Mathf.Max(0, playerNumMatches + numMatches - maxMatches);
        playerNumMatches = Mathf.Min(maxMatches, playerNumMatches + numMatches);
        hud.AddKey(numMatches - matchesLeft);
        return matchesLeft;
    }
    public bool UseMatch() // Return false if no matches to use
    {
        if (playerNumMatches <= 0) { return false; }
        --playerNumMatches;
        hud.RemoveMatch(1);
        return true;
    }
    public float Sprint() // Returns time allowed to sprint, -1 = sprint unavailable
    {
        if(canSprint) { return -1; }
        canSprint = false;
        StartCoroutine(SprintCooldown());
        return maxSprintDuration;
	}
    private IEnumerator SprintCooldown()
    {
        yield return new WaitForSeconds(sprintCooldownDuration);
        canSprint = true;
	}

	#endregion

	#region monster
	#endregion
	public void RemoveChaserObjective(GameObject gameObject)
    {
        chaserObjectives.Remove(gameObject);
        if(chaserObjectives.Count == 0)
        {
            Debug.Log("Player can now be killed");
        }
    }

    public GameObject GetRandomObjective()
    {
        return chaserObjectives[Random.Range(0, chaserObjectives.Count -1)];
    }

}
