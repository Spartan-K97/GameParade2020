using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Door : MonoBehaviour {

    [Serializable]
    public enum DoorType
    {
        Door,
        Wall
    };

    //generation
    public List<GameObject> roomList = new List<GameObject>();

    // for location
    public GameObject parentRoom;

    [Header("Relative True Door Position")]
    public GameObject truePosition;

    //Door Type
    [SerializeField] private DoorType doorType = DoorType.Wall;

    [SerializeField] GameObject wall, door;

    private void Start()
    {
        if (door == null)
        {
            doorType = DoorType.Wall;
        }
    }

    //Choose A Room to spawn
    public GameObject ChooseRoom()
    {
        int chosenRoom = (int)Mathf.Floor(UnityEngine.Random.value * roomList.Count);

        return roomList[chosenRoom];
    }

    //is there a door
    public void FinaliseDoorType()
    {
        switch (doorType)
        {
            case DoorType.Door:
                wall.SetActive(false);
                door.SetActive(true);
                break;
            case DoorType.Wall:
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

}