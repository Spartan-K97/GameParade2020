using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultAIAgent : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    [SerializeField] Transform destination;
    // Start is called before the first frame update

    Vector3 localMap;
    Vector3 localOffset;


    void Start()
    {
        localMap = AIMapInfo.instance.mapSize;
        localOffset = AIMapInfo.instance.mapOffset;

        Wander();
    }

    Vector3 GetRandomMapPosition()
    {
        Vector3 positionVector = Vector3.zero;

        positionVector.x = (Random.value * localMap.x * 2) - localMap.x;
        positionVector.z = (Random.value * localMap.z * 2) - localMap.z;

        positionVector += localOffset;

        Debug.Log(positionVector);

        NavMeshHit navHit;

        NavMesh.SamplePosition(positionVector, out navHit, 30.0f, NavMesh.AllAreas);

        return navHit.position;
    }
    void Wander()
    {
        Vector3 newPos = GetRandomMapPosition();

        agent.SetDestination(newPos);
    }


}
