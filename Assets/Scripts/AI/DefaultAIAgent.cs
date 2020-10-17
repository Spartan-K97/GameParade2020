using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class DefaultAIAgent : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent = null;
    [SerializeField] protected IMovement movementController = null;

    //vision
    [SerializeField] protected float degreesOfVision;
    [SerializeField] LayerMask ignoreLayers = 0;

    Vector3 localMap;
    Vector3 localOffset;

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

    protected bool wanderEnded = false;
    protected IEnumerator Wander()
    {
        wanderEnded = false;
        Vector3 newPos = GetRandomMapPosition();
        //agent.SetDestination(newPos);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(movementController.transform.position, newPos, 1 << NavMesh.GetAreaFromName("Walkable"), path);

        for(int x = 0; x < path.corners.Length; ++x)
        {
            Debug.Log("Moving to " + path.corners[x].ToString(), this);
            while (Vector3.Distance(movementController.transform.position, path.corners[x]) > 0.3f)
            {
                Vector3 dir = Vector3.Normalize(path.corners[x] - movementController.transform.position);
                movementController.Move(dir.z, dir.x, false);
                yield return true;
            }
        }

        //yield return true;
        //
        //while (Vector3.Distance(transform.position, newPos) > 2f)
        //{
        //    Debug.Log(agent.desiredVelocity);
        //    monsterMovement.Move(agent.desiredVelocity.z, agent.desiredVelocity.x, false);
        //    yield return true;
        //}
        wanderEnded = true;
    }

    protected Vector3 GetRandomMapPosition()
    {
        Vector3 positionVector = Vector3.zero;

        positionVector.x = Random.Range(-localMap.x, localMap.x);
        positionVector.z = Random.Range(-localMap.z, localMap.z);

        positionVector += localOffset;

        NavMeshHit navHit;
        Debug.Log(NavMesh.SamplePosition(positionVector, out navHit, 30.0f, 1 << NavMesh.GetAreaFromName("Walkable")));
        Debug.Log("Navigation Target: " + positionVector.ToString());

        return navHit.position;
    }

    #endregion


    protected void GoToClosestMapPosition(Vector3 _position)
    {
        NavMeshHit navHit;

        NavMesh.SamplePosition(_position, out navHit, 10.0f, NavMesh.AllAreas);

        agent.SetDestination(navHit.position);
    }


    protected void FollowObjectLastPosition(GameObject _object)
    {
        agent.SetDestination(_object.transform.position);
    }

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





}
