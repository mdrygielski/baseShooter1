using UnityEngine;
using System.Collections;

public class MazePortal : MonoBehaviour {

    public ParticleSystem emiter;
    public Light[] lights;
    public Light mainLight;
    public Light cardReaderLight;

    public bool isActive;

    int lightActive;

    float lightTimeDelay = 1;
    float lightTimeCounter = 0;


    float emiterTimeDelay = 3;
    float emiterTimeCounter = 0;

    int emiterStages = 6;
    int emiterStagesCounter = 0;

    float cardReaderLightDelay = .5f;
    float cardReaderLightCounter = 0;
    bool cardReaderLightOn = false;

	// Use this for initialization
	void Start () {
        deActivate();
	}
	
	// Update is called once per frame
    public void turnOn(bool active)
    {
        isActive = active;
    }

	void Update () {

        if (isActive)
        {
            activate();
        }
        else
        {
            deActivate();
        }
	
	}
    void activate()
    {
        mainLight.color = new Color(1,0.4f,0);
        cardReaderLight.color = Color.green;
        activateLight(lightActive);
        if (lightActive == lights.Length)
        {
            activeEmiter();

            if (emiterStages == emiterStagesCounter)
            {
                mainLight.color = Color.green;
                
            }
        }
    }

    void activateLight(int lightToOn)
    {
        
        if (lightToOn < lights.Length)
        {
            lightTimeCounter += Time.deltaTime;
            if(lightTimeCounter>lightTimeDelay)
            {
                lightTimeCounter = 0;
                lights[lightToOn].gameObject.SetActive(true);
                lightActive++;
                lightTimeDelay -= lightTimeDelay / 2;
            }
        }


    }

    void deActivate()
    {
        deActivateEmiter();
        foreach (Light light in lights)
        {
            light.gameObject.SetActive(false);
        }
        mainLight.color = Color.red;
        lightActive = 0;
        lightTimeDelay = 1;
        lightTimeCounter = 0;
    }

    void activeEmiter()
    {
        cardReaderOn(true);
        if (emiterStagesCounter < emiterStages)
        {
            emiterTimeCounter += Time.deltaTime;
            if (emiterTimeCounter > emiterTimeDelay)
            {
                emiterTimeCounter = 0;
                emiterStagesCounter++;
                emiter.emissionRate = emiterStagesCounter;
            }

        }

        
    }

    void deActivateEmiter()
    {
        cardReaderOn(false);
        emiterTimeDelay = 3;
        emiterTimeCounter = 0;
        
        emiterStages = 6;
        emiterStagesCounter = 0;
        emiter.emissionRate = 0;

    }

    void cardReaderOn(bool isOn)
    {
        if (!isOn)
        {
            cardReaderLight.color = Color.red;
            cardReaderLightCounter += Time.deltaTime;
            if (cardReaderLightCounter > cardReaderLightDelay)
            {
                cardReaderLightOn = !cardReaderLightOn;
                cardReaderLightCounter = 0;
            }

            if (cardReaderLightOn)
            {
                cardReaderLight.intensity = 1;
            }
            else
            {
                cardReaderLight.intensity = .2f;
            }


        }
        else
        {
            cardReaderLight.color = Color.green;
            cardReaderLight.intensity = 1;
        }

    }

}
