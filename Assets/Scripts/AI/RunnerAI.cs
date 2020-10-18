using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerAI : DefaultAIAgent
{
    public float hearingRadius = 5f;
    public float visualDistance = 15f;
    [SerializeField] GameObject avoid = null;
    private List<Interactable> objectives;
    private Interactable exit;

    //[SerializeField] bool inFOV = false;

    protected override void SafeStart()
    {
        base.SafeStart();
        interactor = gameObject.GetComponent<Interactor>();
        objectives = new List<Interactable>(FindObjectsOfType<InteractableKey>());
        objectives.Add(FindObjectOfType<InteractableOrb>());
        exit = FindObjectOfType<InteractableExit>();
        //agent = movementController.gameObject.AddComponent<NavMeshAgent>();

        mainLoop = StartCoroutine(ObjectiveLoop());
        StartCoroutine(detectMonster());
    }

    #region ObjectiveLoop

    Coroutine mainLoop = null;
    Coroutine findObjective = null;
    bool secondWander = false;
    //bool objectiveCollected = false;

    IEnumerator ObjectiveLoop()
    {
        findObjective = StartCoroutine(FindObjectives());
        yield return findObjective;
        // Find Exit
        while (!objectiveFound)
        {
            GoToPosition(GetRandomMapPosition());
            while(!ReachedDestination() && !objectiveFound)
            {
                AttemptDetectObjective(exit.gameObject);
                yield return new WaitForFixedUpdate();
            }
            StopMoving();
        }
        yield return StartCoroutine(PathfindPos(exit.transform.position));
        Interactable i = detectedObjective.GetComponent<Interactable>();
        if (i == null) { Debug.LogError("Invalid Objective Targeted"); }
        else { i.Interact(interactor); }
        Debug.Log("If the game hasn't ended, something is broken");
    }

    IEnumerator FindObjectives()
    {
        yield return new WaitForFixedUpdate();
        while (objectives.Count > 0)
        {
            Coroutine detect = StartCoroutine(DetectObjectives(objectives));
            GoToPosition(GetRandomMapPosition());
            
            yield return new WaitUntil(() => (ReachedDestination() || objectiveFound));
            //if nothing is detected and the AI has reached their destination

            if (ReachedDestination())
            {
                StopCoroutine(detect);
                if(!secondWander)
                {
                    secondWander = true;
                }
                else
                {
                    //give the ai a break and give it a objective location
                    objectiveFound = true;
                    detectedObjective = objectives[Random.Range(0, objectives.Count)].gameObject;
                }
            }

            if (objectiveFound)
            {
                secondWander = false;
                StopMoving();
                Debug.Log("Going for Objective");
                yield return StartCoroutine(PathfindPos(detectedObjective.transform.position));
                Interactable i = detectedObjective.GetComponent<Interactable>();
                if(i == null) { Debug.LogError("Invalid Objective Targeted"); }
                else { i.Interact(interactor); }
                objectives.Remove(i);
                objectiveFound = false;
            }

            //objectiveCollected = false;
        }
    }

    IEnumerator detectMonster()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if(ObjectIsInFOV(avoid, 180))
            {
                Debug.Log("Fleeing from monster");
                if(mainLoop != null) { StopCoroutine(mainLoop); mainLoop = null; }
                if(findObjective != null) { StopCoroutine(findObjective); findObjective = null; }
                StopMoving();
                movementController.Sprint(true);
                yield return StartCoroutine(PathfindPos(GetRandomPosAvoiding(avoid.transform.position)));

                if (!ObjectIsInFOV(avoid, 180))
                {
                    mainLoop = StartCoroutine(ObjectiveLoop());
                    movementController.Sprint(false);
                    Debug.Log("No longer fleeing");
                }
            }
            yield return new WaitForFixedUpdate();
		}
	}

    Vector3 GetRandomPosAvoiding(Vector3 avoidPos)
    {
        Vector3 randPos = GetRandomMapPosition();

        Vector3 currentPos = movementController.transform.position;
        Vector3 avoidDir = avoidPos - currentPos;
        for (int attempt = 0; attempt < 5; ++attempt)
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(currentPos, randPos, 1 << NavMesh.GetAreaFromName("Walkable"), path);
            if (Vector3.Dot(path.corners[0] - currentPos, avoidDir) <= 0) // Away from target
            {
                return randPos;
            }
            randPos = GetRandomMapPosition();
        }

        return randPos;
    }

    #endregion ObjectiveLoop
}

