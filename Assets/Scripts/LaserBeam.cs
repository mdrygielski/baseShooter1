using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {

    public GameObject laser;
    LineRenderer laserBeam;
    public ParticleSystem laserParticles;

    public GameObject startEmiter;
    public GameObject endEmiter;
	// Use this for initialization
	void Start () {
        laserBeam = laser.GetComponent<LineRenderer>();
        laserBeam.SetWidth(.2f,.2f);
        laserParticles.startLifetime = Vector3.Distance(startEmiter.transform.position, endEmiter.transform.position) / laserParticles.startSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        laserBeam.SetPosition(0,startEmiter.transform.position);
        laserBeam.SetPosition(1, endEmiter.transform.position);
	}
}
