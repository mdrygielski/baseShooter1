using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LightBlinking : MonoBehaviour {

	// Use this for initialization
    
    public bool isBlinking;
    
    float intensivity;
    Light lightToBlink;
    List<GameObject> childrens;
    float delta;
    float timer;

	void Start () {
        delta = Random.Range(0.1f, .3f);
        timer = 0;

        if (Random.Range(0, 100) > 90)
        {
            
            isBlinking = true;

            childrens = new List<GameObject>();
            foreach (Transform child in transform)
            {
                childrens.Add(child.gameObject);
            }
            GameObject lightObj = childrens[Random.Range(0, childrens.Count - 1)];
            lightToBlink = lightObj.GetComponent<Light>();
            intensivity = lightToBlink.intensity;

            
        }
        else
        {
            isBlinking = false;
        }


	
	}
	
	// Update is called once per frame
	void Update () {

        if ( isBlinking)
        {
            if (timer > delta)
            {
                timer = 0;
                lightToBlink.intensity = Random.Range(0.4f, intensivity);
                delta = Random.Range(0.2f, .5f);
            }
            timer += Time.deltaTime;
            
            

        }
	}
}
