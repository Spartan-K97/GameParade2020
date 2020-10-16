﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserAI : DefaultAIAgent
{
    public float hearingRadius = 5f;
    public float visualDistance = 15f;


    bool objectiveFound = false;
    public float interactionRadius = 2f;


    [SerializeField] GameObject target;
    [SerializeField] GameObject exit;

    [SerializeField] bool inFOV = false;

    Interactor interactor;


    protected override void SafeStart()
    {
        base.SafeStart();
        interactor = gameObject.GetComponent<Interactor>();

        StartCoroutine(ObjectiveLoop());
    }

    protected override void SafeUpdate()
    {

    }

    #region ObjectiveLoop

    bool secondWander = false;
    bool objectiveCollected = false;
    GameObject detectedObjective = null;

    IEnumerator ObjectiveLoop()
    {

        yield return new WaitForFixedUpdate();

       StartCoroutine(DetectObjectives());


        while (LevelManager.instance.chaserObjectives.Count > 0)
        {

            StartCoroutine(Wander());
            
            yield return new WaitUntil(() => wanderEnded || objectiveFound);
            //if nothing is detected and the AI has reached their destination

            if (wanderEnded)
            {
                if(!secondWander)
                {
                    secondWander = true;
                }
                else
                {
                    //give the ai a break and give it a objective location
                    objectiveFound = true;
                    detectedObjective = LevelManager.instance.GetRandomObjective();
                    GoToClosestMapPosition(detectedObjective.transform.position);
                }
            }

            if (objectiveFound)
            {
                secondWander = false;
                StartCoroutine(ObjectiveInteractAttempt(detectedObjective));
                
                yield return new WaitUntil(() => objectiveCollected);
                objectiveFound = false;
                if (LevelManager.instance.chaserObjectives.Count > 0)
                {
                    StartCoroutine(DetectObjectives());
                }
            }

            objectiveCollected = false;
        }

        //NO OBJECTIVES LEFT, KILL PLAYER
        StartCoroutine(KillLoop());
    }

    #region DetectObjectives
    IEnumerator DetectObjectives()
    {
        if(objectiveFound)
        {
            Debug.Log("objective detection ended prematurely");
        }
        Debug.Log("Searching for objectives");
        while (!objectiveFound)
        {
            yield return new WaitForFixedUpdate();
            foreach (GameObject objective in LevelManager.instance.chaserObjectives)
            {
                AttemptDetectObjective(objective);
            }
        }
        Debug.Log("ObjectiveDetection ended");
    }
    void AttemptDetectObjective(GameObject _gameObject)
    {
        if (ObjectIsInFOV(_gameObject, degreesOfVision))
        {
            Debug.Log("Objective Found via detection");
            //go to objective
            detectedObjective = _gameObject;
            GoToClosestMapPosition(detectedObjective.transform.position);
            objectiveFound = true;
        }
    }

    #endregion

    IEnumerator ObjectiveInteractAttempt(GameObject _objective)
    {
        Debug.Log("DuplicateCheck");
        while(!objectiveCollected)
        {
            yield return new WaitForFixedUpdate();
            objectiveCollected = CheckObjectiveInRange(detectedObjective, interactionRadius);
        }
    }

    bool CheckObjectiveInRange(GameObject _gameObject, float range)
    {
        if (ObjectiveIsCloseEnough(_gameObject, range))
        {
            //interact with objective
            Interactable interact = _gameObject.GetComponent<Interactable>();
            
            if(interact != null)
            {
                interact.Interact(interactor);
                return true;
            }
            else 
            {
                Debug.Log("OBJECTIVE IS NOT INTERACTABLE");
                return false;
            }
        }
        return false;
    }

    bool ObjectiveIsCloseEnough(GameObject _gameObject, float _range)
    {
        float distance = Vector3.Distance(_gameObject.transform.position, transform.position);
        if (distance < _range)
        {
            return true;
        }
        return false;
    }



    #endregion ObjectiveLoop

    #region KillLoop

    bool patrolling = false;
    IEnumerator KillLoop()
    {
        yield return new WaitForFixedUpdate();

        StartCoroutine(DetectRunner());

        //while player isnt dead
        while (true)
        {
            StartCoroutine(Patrol());
            Debug.Log("KillloopStart");
            yield return new WaitUntil(() => wanderEnded || objectiveFound);
            yield return new WaitForEndOfFrame();
            wanderEnded = false;
            //if player is seen, chase

            if (objectiveFound)
            {
                Debug.Log("objectiveFound");
                objectiveFound = false;
            }
            
        }
    }

    //patrol from the door to a random map position
    IEnumerator Patrol()
    {
        Debug.Log("Patrol Started");
        Vector3 newPos = GetRandomMapPosition();
        Debug.Log(newPos);
        agent.SetDestination(newPos);
        while ((Vector3.Distance(transform.position, newPos) > 1f) || objectiveFound)
        {
            yield return new WaitForFixedUpdate();
        }

        if (!objectiveFound)
        {
            //go to door
            GoToClosestMapPosition(exit.transform.position);
            //yield return new WaitUntil(() => (Vector3.Distance(transform.position, exit.transform.position) > 2f) || objectiveFound);
            while ((Vector3.Distance(transform.position, exit.transform.position) > 2f) || objectiveFound)
            {
                yield return new WaitForFixedUpdate();
            }
            wanderEnded = true;
            Debug.Log("Patrol to door Finished");
        }
    }


    #region DetectRunner
    IEnumerator DetectRunner()
    {
        if (objectiveFound)
        {
            Debug.Log("runner detection ended prematurely");
        }
        while (!objectiveFound)
        {
            yield return new WaitForFixedUpdate();
           
            AttemptDetectObjective(target);
        }
        Debug.Log("RunnerDetection ended");
    }
    #endregion

    //On detection warn the player
    void AttemptDetectRunner()
    {
        if(ObjectIsInDistance(target, hearingRadius))
        {
            //is the player quiet
            //else
            //investigate the noise
        }
    }
    #endregion KillLoop
}
