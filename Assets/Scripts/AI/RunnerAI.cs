using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerAI : DefaultAIAgent
{
    public float hearingRadius = 5f;
    public float visualDistance = 15f;
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

        StartCoroutine(ObjectiveLoop());
    }

    #region ObjectiveLoop

    bool secondWander = false;
    bool objectiveCollected = false;

    IEnumerator ObjectiveLoop()
    {
        yield return StartCoroutine(FindObjectives());
        // Find Exit
        objectiveFound = false;
        while (!objectiveFound)
        {
            GoToPosition(GetRandomMapPosition());
            while(!ReachedDestination() && !objectiveFound)
            {
                AttemptDetectObjective(exit.gameObject);
                yield return null;
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
        yield return null;
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
                objectiveFound = false;
                StopMoving();
                Debug.Log("Going for Objective");
                yield return StartCoroutine(PathfindPos(detectedObjective.transform.position));
                Interactable i = detectedObjective.GetComponent<Interactable>();
                if(i == null) { Debug.LogError("Invalid Objective Targeted"); }
                else { i.Interact(interactor); }
                objectives.Remove(i);
            }

            objectiveCollected = false;
        }
    }


    #endregion ObjectiveLoop
}

