using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class DefaultAIAgent : MonoBehaviour
{
    [SerializeField] protected IMovement movementController = null;

    //vision
    [SerializeField] protected float degreesOfVision;
    [SerializeField] LayerMask ignoreLayers = 0;
    //[SerializeField] protected float interactionRadius = 2f;
    [SerializeField] protected Interactor interactor;

    Vector3 localMap;
    Vector3 localOffset;
    protected bool objectiveFound = false;
    protected GameObject detectedObjective = null;

    #region Start
    void Start()
    {
        SafeStart();
    }

    protected virtual void SafeStart()
    {
        localMap = AIMapInfo.instance.mapSize;
        localOffset = AIMapInfo.instance.mapOffset;
        movementController.controller = transform;
    }
    #endregion

    #region Wander

    //protected bool wanderEnded = false;
    //protected IEnumerator Wander()
    //{
    //    wanderEnded = false;
    //    Vector3 newPos = GetRandomMapPosition();
    //
    //    yield return StartCoroutine(PathfindPos(newPos));
    //
    //    wanderEnded = true;
    //}

    Coroutine movement = null;
    Coroutine interact = null;
    protected void GoToPosition(Vector3 pos)
    {
        StopMoving();
        NavMeshHit navHit;
        NavMesh.SamplePosition(pos, out navHit, 30.0f, 1 << NavMesh.GetAreaFromName("Walkable"));
        movement = StartCoroutine(PathfindPos(pos));
    }
    protected void StopMoving()
    {
        if (movement != null) {
            StopCoroutine(movement);
            movement = null;
        }
        movementController.Move(0, 0, false);
    }
    protected bool ReachedDestination()
    {
        return movement == null;
	}

    bool escapeStuck = false;
    protected IEnumerator PathfindPos(Vector3 targetPos)
    {
        NavMeshHit navHit;
        NavMesh.SamplePosition(targetPos, out navHit, 30.0f, 1 << NavMesh.GetAreaFromName("Walkable"));

        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(movementController.transform.position, navHit.position, 1 << NavMesh.GetAreaFromName("Walkable"), path);

        for (int x = 0; x < path.corners.Length; ++x)
        {
            while (Vector3.Distance(movementController.transform.position, path.corners[x] - new Vector3(0, path.corners[x].y, 0)) > 0.3f)
            {
                Vector3 dir = path.corners[x] - movementController.transform.position; dir.y = 0;
                dir = Vector3.Normalize(dir);
                movementController.Move(dir.z, dir.x, false);
                yield return null;
            }
        }
        movement = null;
        movementController.Move(0, 0, false);
    }

    protected Vector3 GetRandomMapPosition()
    {
        Vector3 positionVector = Vector3.zero;

        positionVector.x = Random.Range(-localMap.x, localMap.x);
        positionVector.z = Random.Range(-localMap.z, localMap.z);

        positionVector += localOffset;

        NavMeshHit navHit;
        NavMesh.SamplePosition(positionVector, out navHit, 30.0f, 1 << NavMesh.GetAreaFromName("Walkable"));

        return navHit.position;
    }

    #endregion


    //protected void GoToClosestMapPosition(Vector3 _position)
    //{
    //    NavMeshHit navHit;
    //
    //    NavMesh.SamplePosition(_position, out navHit, 10.0f, NavMesh.AllAreas);
    //
    //    StartCoroutine(PathfindPos(navHit.position));
    //}


    //protected void FollowObjectLastPosition(GameObject _object)
    //{
    //    StartCoroutine(PathfindPos(_object.transform.position));
    //}

    protected bool ObjectIsInDistance(GameObject _object, float _distance)
    {
        float distance = Vector3.Distance(transform.position, _object.transform.position);
        if (distance < _distance)
        {
            Debug.Log(distance);
            if (!Physics.Linecast(transform.position, _object.transform.position, ignoreLayers))
            {
                return true;
            }
        }
        return false;
    }

    protected bool ObjectIsInFOV(GameObject _object, float _angle)
    {
        Vector3 targetDir = _object.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        if (angle < _angle)
        {
            if (!Physics.Linecast(_object.transform.position, transform.position, ignoreLayers))
            {
                return true;
            }
        }
        return false;
    }


    #region DetectObjectives
    protected IEnumerator DetectObjectives(List<Interactable> objectives)
    {
        if (objectiveFound)
        {
            Debug.Log("objective detection ended prematurely");
        }
        Debug.Log("Searching for objectives");
        while (!objectiveFound)
        {
            yield return new WaitForFixedUpdate();
            foreach (Interactable objective in objectives)
            {
                AttemptDetectObjective(objective.gameObject);
            }
        }
        Debug.Log("ObjectiveDetection ended");
    }
    public void AttemptDetectObjective(GameObject _gameObject)
    {
        if (ObjectIsInFOV(_gameObject, degreesOfVision))
        {
            Debug.Log("Objective Found via detection");
            //go to objective
            detectedObjective = _gameObject;
            //GoToClosestMapPosition(detectedObjective.transform.position);
            objectiveFound = true;
        }
    }

    #endregion



}
