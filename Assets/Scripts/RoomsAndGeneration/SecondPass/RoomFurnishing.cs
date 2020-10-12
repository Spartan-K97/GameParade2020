using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds furnishing presets for the room
/// </summary>
public class RoomFurnishing : MonoBehaviour
{
    [SerializeField]List<RoomFurnishPreset> presets;

    //Has it Weighd the chance of each room, only do once
    static bool processed = false;
    static float fullWeight = 0;

    /*
    public void Furnish()
    {
        if (presets.Count == 0)
        {
            Debug.Log("No furniture");
        }
        else
        {
            if(!processed)
            {
                genInfo = DungeonInstancing.instance.chosenQuest.generationInfo;
                ProcessWeighting();
                processed = true;

                Instantiate(SelectPreset().preset, gameObject.transform);
            }
            else
            {
                //Debug.Log("WeightingProcessed");
                GameObject g = Instantiate(SelectPreset().preset, gameObject.transform);
                //Debug.Log(g);
            }
        }
        
    }

    #region WeighRooms
    void ProcessWeighting()
    {
        foreach (RoomFurnishPreset preset in presets)
        {
            preset.chance = WeighPreset(preset);
            fullWeight += preset.chance;
        }
    }

    float WeighPreset(RoomFurnishPreset preset)
    {
        float weight = 0;
        if(preset.styles.Contains(genInfo.furnishingStyle))
        {
            weight += 10;
        }
        if(preset.encounterType == genInfo.encounterType)
        {
            weight += 10;
        }
        if(preset.lightingAmount == genInfo.lightingAmount)
        {
            weight += 10;
        }


        return weight;
    }

    #endregion WeighRooms

    RoomFurnishPreset SelectPreset()
    {
        float chosenWeight = Random.value * fullWeight;

        foreach (RoomFurnishPreset preset in presets)
        {
            chosenWeight -= preset.chance;
            if(chosenWeight <= 0)
            {
                return preset;
            }
        }

        Debug.Log("Room Furniture preset selection failed");
        return null;

    }

    */
}


