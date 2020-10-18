using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserAI : DefaultAIAgent
{
    public float hearingRadius = 5f;
    public float visualDistance = 15f;

    private List<Interactable> monsterObjectives;
    [SerializeField] GameObject target = null;
    GameObject exit = null;

    //[SerializeField] bool inFOV = false;



    protected override void SafeStart()
    {
        base.SafeStart();
        interactor = gameObject.GetComponent<Interactor>();
        monsterObjectives = new List<Interactable>(FindObjectsOfType<InteractableMonsterObjective>());
        exit = FindObjectOfType<InteractableExit>().gameObject;
        //agent = movementController.gameObject.AddComponent<NavMeshAgent>();

        StartCoroutine(ObjectiveLoop());
    }

    #region ObjectiveLoop

    bool secondWander = false;
    //bool objectiveCollected = false;

    IEnumerator ObjectiveLoop()
    {

        yield return null;

       //StartCoroutine(DetectObjectives());


        while (monsterObjectives.Count > 0)
        {
            Coroutine detect = StartCoroutine(DetectObjectives(monsterObjectives));
            GoToPosition(GetRandomMapPosition());
            //StartCoroutine(Wander());
            
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
                    detectedObjective = monsterObjectives[Random.Range(0, monsterObjectives.Count)].gameObject;
                    //detectedObjective = LevelManager.instance.GetRandomObjective();
                    //GoToClosestMapPosition(detectedObjective.transform.position);
                }
            }

            if (objectiveFound)
            {
                secondWander = false;
                objectiveFound = false;
                StopMoving();
                yield return StartCoroutine(PathfindPos(detectedObjective.transform.position));
                Interactable i = detectedObjective.GetComponent<Interactable>();
                if(i == null) { Debug.LogError("Invalid Objective Targeted"); }
                else { i.Interact(interactor); }
                //LevelManager.instance.RemoveChaserObjective(detectedObjective);
                monsterObjectives.Remove(i);
                //StartCoroutine(ObjectiveInteractAttempt(detectedObjective));

                //yield return new WaitUntil(() => objectiveCollected);
                //if (LevelManager.instance.chaserObjectives.Count > 0)
                //{
                //    StartCoroutine(DetectObjectives());
                //}
            }

            //objectiveCollected = false;
        }

        //NO OBJECTIVES LEFT, KILL PLAYER
        StartCoroutine(KillLoop());
    }


    //IEnumerator ObjectiveInteractAttempt(GameObject _objective)
    //{
    //    Debug.Log("DuplicateCheck");
    //    while(!objectiveCollected)
    //    {
    //        yield return new WaitForFixedUpdate();
    //        objectiveCollected = CheckObjectiveInRange(detectedObjective, interactionRadius);
    //    }
    //}
    //
    //bool CheckObjectiveInRange(GameObject _gameObject, float range)
    //{
    //    if (ObjectiveIsCloseEnough(_gameObject, range))
    //    {
    //        //interact with objective
    //        Interactable interact = _gameObject.GetComponent<Interactable>();
    //        
    //        if(interact != null)
    //        {
    //            interact.Interact(interactor);
    //            return true;
    //        }
    //        else 
    //        {
    //            Debug.Log("OBJECTIVE IS NOT INTERACTABLE");
    //            return false;
    //        }
    //    }
    //    return false;
    //}
    //
    //bool ObjectiveIsCloseEnough(GameObject _gameObject, float _range)
    //{
    //    float distance = Vector3.Distance(_gameObject.transform.position, transform.position);
    //    if (distance < _range)
    //    {
    //        return true;
    //    }
    //    return false;
    //}



    #endregion ObjectiveLoop

    #region KillLoop

    //bool patrolling = false;
    IEnumerator KillLoop()
    {
        yield return new WaitForFixedUpdate();

        bool patrolToDoor = false;

        //while player isnt dead
        while (true)
        {
            Coroutine detect = StartCoroutine(DetectRunner());
            GoToPosition((patrolToDoor)? exit.transform.position : GetRandomMapPosition());
            yield return new WaitUntil(() => ReachedDestination() || objectiveFound);
            StopCoroutine(detect);
            if(objectiveFound) { Debug.Log("Start Chase Sequence"); }
            while (objectiveFound) // Chase sequence
            {
                objectiveFound = false;
                StopMoving();
                yield return StartCoroutine(PathfindPos(target.transform.position));
                AttemptDetectObjective(target);
            }
            patrolToDoor = !patrolToDoor;

            //StartCoroutine(Patrol());
            //yield return new WaitUntil(() => ReachedDestination() || objectiveFound);
            //yield return new WaitForEndOfFrame();
            //if player is seen, chase

            //if (objectiveFound)
            //{
            //    Debug.Log("objectiveFound");
            //    objectiveFound = false;
            //}

        }
    }

    //patrol from the door to a random map position
    //IEnumerator Patrol()
    //{
    //    Debug.Log("Patrol Started");
    //    Vector3 newPos = GetRandomMapPosition();
    //    Debug.Log(newPos);
    //    yield return StartCoroutine(PathfindPos(newPos));
    //
    //    if (!objectiveFound)
    //    {
    //        //go to door
    //        GoToClosestMapPosition(exit.transform.position);
    //        //yield return new WaitUntil(() => (Vector3.Distance(transform.position, exit.transform.position) > 2f) || objectiveFound);
    //        while ((Vector3.Distance(transform.position, exit.transform.position) > 2f) || objectiveFound)
    //        {
    //            yield return new WaitForFixedUpdate();
    //        }
    //        wanderEnded = true;
    //        Debug.Log("Patrol to door Finished");
    //    }
    //}


    #region DetectRunner
    IEnumerator DetectRunner()
    {
        if (objectiveFound)
        {
            Debug.Log("runner detection ended prematurely");
        }
        while (true)
        {
            yield return null;
           
            AttemptDetectObjective(target);
        }
        //Debug.Log("RunnerDetection ended");
    }
    #endregion
    
    //On detection warn the player
    //void AttemptDetectRunner()
    //{
    //    if(ObjectIsInDistance(target, hearingRadius))
    //    {
    //        //is the player quiet
    //        //else
    //        //investigate the noise
    //    }
    //}
    #endregion KillLoop
}

