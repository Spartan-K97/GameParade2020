using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Furnish Preset", menuName = "Room/FurnishPreset")]
public class RoomFurnishPreset : ScriptableObject
{
    public GameObject preset;
    [Space]

    public float chance;
    [Space]

    //
    public List<FurnishingStyle> styles;
    //[SerializeField] List<FurnishAge> ages;
    public EncounterType encounterType;
    //[SerializeField] LootAmount LootAmount;
    public LightingAmount lightingAmount;
}

public enum FurnishingStyle
{
    empty,
    basic,
    library,
    hall,

}

//add combat
public enum EncounterType
{
    none, exploration
}


public enum LightingAmount 
{
    WellLit, Lit, Dark, Pitch 
}



public enum FurnishAge
{
    recent,
    abandoned,
    neutral
}

public enum LootAmount
{
    none, low, medium, high
}
