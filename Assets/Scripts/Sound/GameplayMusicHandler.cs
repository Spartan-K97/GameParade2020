using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicHandler : MonoBehaviour
{
    [SerializeField] List<GamePlayMusicLayer> musicLayers;


    [SerializeField] GameObject runner;
    [SerializeField] GameObject chaser;


    private void Update()
    {
        float distance = Vector3.Distance(runner.transform.position, chaser.transform.position);

        foreach (GamePlayMusicLayer musicLayer in musicLayers)
        {
            if (musicLayer.layerDistance + musicLayer.blendDistance < distance)
            {
                musicLayer.layer.volume = 0;
            }
            else
            {
                if (musicLayer.layerDistance > distance)
                {
                    musicLayer.layer.volume = 1;
                }
                else
                {
                    musicLayer.layer.volume = (musicLayer.blendDistance - (distance - musicLayer.layerDistance)) / musicLayer.blendDistance;
                }
            }
        }
    }
}

[Serializable]
public struct GamePlayMusicLayer {

    public AudioSource layer;

    public float blendDistance;
    public float layerDistance;


}

