/*
 * Manage part of game actions
 * 
 * to do:
 * choose place to put key&door objects, when room decoration will be implemented  
 * 
 * 
 * */

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static NetworkManager networkManager;
    public static GameObject gameManager;


	// Use this for initialization
	void Start () {
        networkManager =  gameObject.AddComponent<NetworkManager>();
        
        
        
        GameObject ob = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Instantiate(ob);
        //this is instance of GameManager
        gameManager = gameObject;


	}
    
    public static int createMap(int seed){
        Debug.Log("Creating Map");
        Random.seed = seed;

        removeMap();
        Debug.Log("Removing old Map");
        gameManager.gameObject.AddComponent<MazeGenerator>();
        
        Debug.Log("Map has been created");
        return seed;
    }

    static void removeMap(){
        
        Destroy(gameManager.GetComponent<MazeGenerator>());
        
        GameObject gm = GameObject.Find("RoomManager");
        Destroy(gm);

    }
    

}
