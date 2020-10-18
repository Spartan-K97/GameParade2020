using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GlowyOrb : MonoBehaviour
{
    [SerializeField] Transform monster;
    [SerializeField] Light plight;
    [SerializeField] Renderer rend;
    private Color col;

	private void Start()
    {
        if (rend != null)
        {
            col = rend.material.color;
        }
	}

	void Update()
    {
        float glowyness = Mathf.Min((Vector3.Distance(transform.position, monster.position) - 5) / 15, 15);
        plight.intensity = glowyness * 0.5f;
        if(rend != null)
        {
            rend.material.color = col * glowyness;
		}
    }
}
