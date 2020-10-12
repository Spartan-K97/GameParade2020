using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Preset", menuName = "Player/Preset")]
public class PlayerPreset : ScriptableObject
{
    public float playerSpeed;
    public float alternateSpeed;


    public float interactionDistance;
}
