using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] string levelToLoad = "";

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
		}
        Shuffle(surfaceShuffle);
        Shuffle(wallShuffle);
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

	[SerializeField] HUDManager hud = null;
    [SerializeField] PauseManager pause = null;
    [SerializeField] ScreenFade sf = null;
    private bool paused = false;
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
	
    private int playerNumKeys = 0;
    private int playerNumKeysUsed = 0;
    private int playerNumMatches = 0;
    public bool playerHasOrb = false;
    private bool canSprint = false;
    private int numKeysInGame = 0;
    [SerializeField] int maxKeys = 5;
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
    public bool PlayerHasKey()
    {
        return playerNumKeys >= 1;
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
    public bool PlayerHasMatch()
    {
        return playerNumMatches >= 1;
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
        public List<GameObject> chaserObjectives;
        
        


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
