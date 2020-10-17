using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class DefaultAIAgent : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;

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
    }
    #endregion

    #region Update
    private void Update()
    {
        SafeUpdate();
    }

    protected virtual void SafeUpdate()
    {

    }

    #endregion

    #region Wander

    protected bool wanderEnded = false;
    protected IEnumerator Wander()
    {
        wanderEnded = false;
        Vector3 newPos = GetRandomMapPosition();
        agent.SetDestination(newPos);

        yield return new WaitForFixedUpdate();

        while (Vector3.Distance(transform.position, newPos) > 1f)
        {
            yield return new WaitForFixedUpdate();
        }
        wanderEnded = true;
    }

    protected Vector3 GetRandomMapPosition()
    {
        Vector3 positionVector = Vector3.zero;

        positionVector.x = (Random.value * localMap.x * 2) - localMap.x;
        positionVector.z = (Random.value * localMap.z * 2) - localMap.z;

        positionVector += localOffset;

        NavMeshHit navHit;
        NavMesh.SamplePosition(positionVector, out navHit, 30.0f, NavMesh.AllAreas);

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
