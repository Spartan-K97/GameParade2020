using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PositionalMovement : MonoBehaviour
{
    [Serializable]
    public enum MovementType
    {
        Looping,
        Swing,
        Random
    };

    //The positions the platform will stop at
    [SerializeField] List<Vector3> positions = new List<Vector3>();
    [SerializeField] int position = 0;


    //when the platform reaches it's destination, does it stop for a while
    [SerializeField] float holdTime = 0;
    bool holding = false;
    //How long does it take for the platform to arrive
    [SerializeField] float moveTime = 1.0f;
    float movingTime = 0.0f;

    //The movement type
    [SerializeField] private MovementType moveType = MovementType.Looping;
    //For the looping movementType
    int swingDir = 1;

    //The positions the object move between
    Vector3 from, to;

    // Start is called before the first frame update
    void Start()
    {
        if (positions.Count == 0)
        {
            this.enabled = false;
        }

        //Turn each position to world coordinates
        for (int i = 0; i < positions.Count; i++)
        {
            positions[i] = transform.position + positions[i];
        }


        NextPosition();
    }

    //Move To next position depending on moveType
    void NextPosition()
    {
        from = positions[position];

        switch (moveType)
        {
            case MovementType.Looping:
                {
                    position++;
                    if (position > positions.Count - 1)
                    {
                        position = 0;
                    }
                }
                break;
            case MovementType.Swing:
                {
                    position += swingDir;
                    //swing down
                    if (position > positions.Count - 1)
                    {
                        swingDir = -1;
                        position -= 2;
                    }

                    if (position < 0)
                    {
                        swingDir = 1;
                        position += 2;
                    }

                }
                break;
            case MovementType.Random:
                position = (int)UnityEngine.Random.value * positions.Count;
                break;
            default:
                break;
        }

        to = positions[position];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!holding)
        {
            MovePosition();
        }
    }

    void MovePosition()
    {
        movingTime += Time.fixedDeltaTime;
        float distance = movingTime / moveTime;
        if (distance > 1)
        {
            if (holdTime > 0)
            {
                movingTime = 0;
                StartCoroutine(HoldPosition(holdTime));
                transform.position = to;
                NextPosition();
            }
            else
            {
                movingTime = 0;
                NextPosition();
                distance -= 1;
                transform.position = Vector3.Lerp(from, to, distance);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(from, to, distance);
        }        
    }

    IEnumerator HoldPosition(float waitTime)
    {
        holding = true;
        yield return new WaitForSecondsRealtime(waitTime);
        holding = false;
    }

}
