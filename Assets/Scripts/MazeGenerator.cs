using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour {
    public static GameObject roomManager;
    public int MazeSize;


	// Use this for initialization
	void Start () {
        MazeSize = 50;
        //Random.seed = 1234567890;
       
        GameObject roomManager = new GameObject("RoomManager");
        roomManager.transform.position = new Vector3(0,0,0);
        roomManager.AddComponent<RoomManager>();
        RoomManager.RoomLimit = MazeSize;
        roomManager.transform.parent = gameObject.transform;
	}


    public static GameObject getRoomManager()
    {
        return roomManager;
    }
	// Update is called once per frame
	void Update () {
	
	}


}
