using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserAI : DefaultAIAgent
{
    public float hearingRadius = 5f;
    public float visualDistance = 15f;


    bool objectiveFound = false;
    public float interactionRadius = 2f;


    [SerializeField]GameObject target;

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


        while (LevelManager.instance.chaserObjectives.Count > 0)
        {
            StartCoroutine(Wander());
            StartCoroutine(DetectObjectives());

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

            if(objectiveFound)
            {
                secondWander = false;
                StartCoroutine(ObjectiveInteractAttempt(detectedObjective));
                yield return new WaitUntil(() => objectiveCollected);
            }

            objectiveCollected = false;
            objectiveFound = false;
        }

        Debug.Log("CHASER CAN KILL");
        //NO OBJECTIVES LEFT, KILL PLAYER
        StartCoroutine(KillLoop());
    }

    #region DetectObjectives
    IEnumerator DetectObjectives()
    {
        while (!objectiveFound)
        {
            foreach (GameObject objective in LevelManager.instance.chaserObjectives)
            {
                objectiveFound = (AttemptDetectObjective(objective));
            }
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Objective Found");
    }
    bool AttemptDetectObjective(GameObject _gameObject)
    {
        if(ObjectIsInFOV(_gameObject, degreesOfVision))
        {
            //go to objective
            detectedObjective = _gameObject;
            GoToClosestMapPosition(detectedObjective.transform.position);
            return true;
        }
        return false;
    }

    #endregion

    IEnumerator ObjectiveInteractAttempt(GameObject _objective)
    {
        while(!objectiveCollected)
        {
            yield return new WaitForFixedUpdate();
            objectiveCollected = CheckObjectiveInRange(detectedObjective, interactionRadius);
        }
    }

    bool CheckObjectiveInRange(GameObject _gameObject, float range)
    {
        if (ObjectIsInDistance(target, range))
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

    #endregion ObjectiveLoop

    #region KillLoop

    IEnumerator KillLoop()
    {
        yield return new WaitForFixedUpdate();
        //if player is seen, chase

        //if player is heard, goto position

        ///////patrol
        //if player is not detected, wander
        //if wandered, return to the door

    }






    //On detection warn the player
    void AttemptDetectRunner()
    {

        //can the player be seen
        if(inFOV = ObjectIsInFOV(target, degreesOfVision))
        {
                //chase the player
        }
        else
        //can the player be heard
        if(ObjectIsInDistance(target, hearingRadius))
        {
            //is the player quiet
            //else
            //investigate the noise

        }
    }
    #endregion KillLoop

    //if objectivefound and moving towards objective
 

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(transform.position, hearingRadius);
    }
}

