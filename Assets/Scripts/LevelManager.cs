using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] string levelToLoad = "";
    public bool freeze = false;

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
            if (levelToLoad != "")
            {
                SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);
            }
		}
        else
        {
            Debug.LogError("Multiple LevelManagers found in scene, only one should be instantiated");
		}
	}

	#endregion

	#region Game Start
	private void Start()
    {
        // Once loading is complete
        sf.FadeFromDefault(1, null);
        List<Interactable> surfaceShuffle = new List<Interactable>();
        List<Interactable> wallShuffle = new List<Interactable>();
        foreach(Interactable i in FindObjectsOfType<Interactable>())
        {
            if (i is ISurfaceShuffle) { surfaceShuffle.Add(i); }
            if (i is IWallShuffle) { wallShuffle.Add(i); }
            if (i is InteractableKey) { ++numKeysInGame; }
            if (i is InteractableMonsterObjective) { ++numWardsInGame; }
		}
        Shuffle(surfaceShuffle);
        Shuffle(wallShuffle);
        hud.SetNumKeys(playerNumKeys);
        hud.SetNumMatches(playerNumMatches);
        hud.SetNumWards(numWardsInGame);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Shuffle(List<Interactable> toShuffle)
    {
        for (int x = 0; x < toShuffle.Count; ++x)
        {
            Transform a = toShuffle[x].transform;
            Transform b = toShuffle[Random.Range(0, toShuffle.Count)].transform;
            Vector3 tempPos = a.position;
            Quaternion tempRot = a.rotation;
            a.position = b.position;
            a.rotation = b.rotation;
            b.position = tempPos;
            b.rotation = tempRot;
        }
    }

	#endregion

	#region UI

    [SerializeField] ScreenFade sf = null;
	[SerializeField] HUDManager hud = null;
    //[SerializeField] PauseManager pause = null;
    //private bool paused = false;
	//void Update()
    //{
    //    //adjust for deeper pause sceens
    //    if (Input.GetButtonDown("Pause"))
    //    {
    //        paused = !paused;
    //        if (paused)
    //        {
    //            hud.gameObject.SetActive(false);
    //            pause.gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            pause.gameObject.SetActive(false);
    //            hud.gameObject.SetActive(true);
    //        }
    //    }
    //}
	#endregion

	#region human
	
    [SerializeField] int playerNumKeys = 0; // Default num can be set
    private int playerNumKeysUsed = 0;
    [SerializeField] int playerNumMatches = 0;
    public bool playerHasOrb = false;
    private bool canSprint = true;
    private int numKeysInGame = 0;
    [SerializeField] int maxKeys = 5;
    [SerializeField] int maxMatches = 5;
    [SerializeField] float maxSprintDuration = 2.0f;
    [SerializeField] float sprintCooldownDuration = 5.0f;

    public int AddKeys(int numKeys)
    {
        int keysLeft = Mathf.Max(0, playerNumKeys + numKeys - maxKeys);
        playerNumKeys = Mathf.Min(maxKeys, playerNumKeys + numKeys);
        hud.SetNumKeys(playerNumKeys);
        return keysLeft;
	}
    public bool UseKey() // Return true if the correct key is used
    {
        if(playerNumKeys <= 0) { return false; }
        --playerNumKeys;
        hud.SetNumKeys(playerNumKeys);
        ++playerNumKeysUsed;
        if(Random.Range(0, numKeysInGame) < playerNumKeysUsed) { return true; }
        return false;
	}
    public bool PlayerHasKey()
    {
        return playerNumKeys >= 1;
	}
    public int AddMatches(int numMatches)
    {
        int matchesLeft = Mathf.Max(0, playerNumMatches + numMatches - maxMatches);
        playerNumMatches = Mathf.Min(maxMatches, playerNumMatches + numMatches);
        hud.SetNumMatches(playerNumMatches);
        return matchesLeft;
    }
    public bool UseMatch() // Return false if no matches to use
    {
        if (playerNumMatches <= 0) { return false; }
        --playerNumMatches;
        hud.SetNumMatches(playerNumMatches);
        return true;
    }
    public bool PlayerHasMatch()
    {
        return playerNumMatches >= 1;
    }

    public float Sprint() // Returns time allowed to sprint, -1 = sprint unavailable
    {
        if (canSprint)
        {
            canSprint = false;
            StartCoroutine(SprintCooldown());
            return maxSprintDuration;
        }
        return -1;
    }

    private IEnumerator SprintCooldown()
    {
        yield return new WaitForSeconds(sprintCooldownDuration);
        canSprint = true;
	}

    #endregion

    #region monster

    private int numWardsInGame = 0;

    public void WardBroken()
    {
        --numWardsInGame;
        hud.SetNumWards(numWardsInGame);
	}
    public bool AllWardsDestroyed()
    {
        return numWardsInGame <= 0;
	}

    //public List<GameObject> chaserObjectives;
    //
    //public void RemoveChaserObjective(GameObject gameObject)
    //{
    //    chaserObjectives.Remove(gameObject);
    //    hud.SetNumWards(chaserObjectives.Count);
    //    if(chaserObjectives.Count == 0)
    //    {
    //        Debug.Log("Player can now be killed");
    //    }
    //}
    //
    //public GameObject GetRandomObjective()
    //{
    //    return chaserObjectives[Random.Range(0, chaserObjectives.Count)]; // max = EXCLUSIVE
    //}

    #endregion
}
