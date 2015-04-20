/*
 * Destroying particle system component after time of duration
 * 
 * 
 * */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]

public class EffectDestroyer : MonoBehaviour {

    float timer;
    ParticleSystem ps;
	// Use this for initialization
	void Start () {
        
       

        ps = GetComponent<ParticleSystem>();
        timer = ps.duration;
	}
	

	void Update () {
        timer -= 0.01f;
        if (timer < 0)
        {
            Destroy(gameObject);
        }

	}

}
