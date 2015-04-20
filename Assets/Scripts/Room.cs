using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

    public enum Direction
    {
        NONE,
        N,
        E,
        S,
        W
        
    };

    //way number which means ID of maze part
    public int wayId;

    //carry how far is this room from init room
    public int distance;
    public GameObject parent;


    //public Direction entrance;
    //public Direction[] entrances = new Direction[4];
    //GameObject wallN, wallE, wallS, wallW, collumn0, collumn1, collumn2, collumn3;
    //bool roomsWasCreated;

    int roomsToCreate;
    Direction[] directionsToCreate;

    int roomsCreated;
    int roomToCreate;

    float roomStep;

    bool isWaiting;

    
    public bool isInitRoom;

    public bool[] walls;
    public GameObject[] wallsArray;
    public GameObject[] neighbours;
    public int floorType;


	// Use this for initialization

    void Awake()
    {
        //array keep walls activity
        walls = new bool[4] { true, true, true, true };
        wallsArray = new GameObject[4];
        neighbours = new GameObject[4] {null,null,null,null};
        floorType = 0;

        //someday make this array private to use it in removeWall function
        List<GameObject> childrens = new List<GameObject>();
        foreach (Transform child in transform)
        {
            childrens.Add(child.gameObject);
        }
        for (int i = 0; i < wallsArray.Length; i++)
        {
            wallsArray[i] = childrens[i];
        }
    }

	void Start () {
        
        

        isWaiting = false;

        roomStep = 15f;
        roomsCreated = 0;


        roomsToCreate = Random.Range(2, 4);


        //only if its init room, set number of outgoing ways
        if (wayId == -1)
        {
            isInitRoom = true;
            RoomManager.setWays(roomsToCreate);
        }
        else
        {
            isInitRoom = false;
        }

        directionsToCreate = new Direction[roomsToCreate];

        //random directions to create next room
        setDirections();


        


	}
	

	void Update () {
        if (!RoomManager.limitReached())
        {
            if (!myJobIsDone())
            {
                createRooms();
            }
            else
            {
                
            }
        }
        
	
	}

    void createRooms()
    {
        isWaiting = true;

            if (directionsToCreate[roomsCreated] == Direction.N)
            {
                
                Vector3 positionToCheck = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z + roomStep));
                int roomIndex = RoomManager.roomsIsExists(positionToCheck);
                if (roomIndex==-1)
                {
                    roomsCreated++;
                    GameObject roomObj = (GameObject)Instantiate(Resources.Load("Prefabs/Room"));
                    
                    roomObj.transform.position = positionToCheck;
                    Room room = roomObj.GetComponent<Room>();

                    removeWall(Direction.N, true);
                    room.removeWall(Direction.S);

                    


                    isWaiting = false;

                    RoomManager.addRoom(roomObj);
                    neighbours[0] = roomObj;


                    RoomManager.setRoomWay(gameObject, roomObj, roomsCreated);
                    

                }
                


            }else if (directionsToCreate[roomsCreated] == Direction.E)
            {
                
                Vector3 positionToCheck = new Vector3(Mathf.RoundToInt(transform.position.x + roomStep), 0, Mathf.RoundToInt(transform.position.z));
                int roomIndex = RoomManager.roomsIsExists(positionToCheck);
                if (roomIndex == -1)
                {
                    roomsCreated++;
                    GameObject roomObj = (GameObject)Instantiate(Resources.Load("Prefabs/Room"));
                    roomObj.transform.position = positionToCheck;
                    Room room = roomObj.GetComponent<Room>();

                    removeWall(Direction.E, true);
                    room.removeWall(Direction.W);


                    isWaiting = false;
                    RoomManager.addRoom(roomObj);
                    neighbours[1] = roomObj;

                    RoomManager.setRoomWay(gameObject, roomObj, roomsCreated);
                    
                }
            }else if (directionsToCreate[roomsCreated] == Direction.S)
            {
                
                Vector3 positionToCheck = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z - roomStep));
                int roomIndex = RoomManager.roomsIsExists(positionToCheck);
                if (roomIndex == -1)
                {
                    roomsCreated++;
                    GameObject roomObj = (GameObject)Instantiate(Resources.Load("Prefabs/Room"));
                    roomObj.transform.position = positionToCheck;
                    Room room = roomObj.GetComponent<Room>();
                    removeWall(Direction.S, true);
                    room.removeWall(Direction.N);


                    isWaiting = false;
                    RoomManager.addRoom(roomObj);
                    neighbours[2] = roomObj;

                    RoomManager.setRoomWay(gameObject, roomObj, roomsCreated);
                    
                }
            }else if (directionsToCreate[roomsCreated] == Direction.W)
            {
                
                Vector3 positionToCheck = new Vector3(Mathf.RoundToInt(transform.position.x - roomStep), 0, Mathf.RoundToInt(transform.position.z));
                int roomIndex = RoomManager.roomsIsExists(positionToCheck);
                if (roomIndex == -1)
                {
                    roomsCreated++;
                    GameObject roomObj = (GameObject)Instantiate(Resources.Load("Prefabs/Room"));
                    roomObj.transform.position = positionToCheck;
                    Room room = roomObj.GetComponent<Room>();
                    removeWall(Direction.W, true);
                    room.removeWall(Direction.E);


                    isWaiting = false;
                    RoomManager.addRoom(roomObj);
                    neighbours[3] = roomObj;

                    RoomManager.setRoomWay(gameObject, roomObj, roomsCreated);
                    
                }
            }
       
        if(isWaiting){
            setDirections();
        }

    }

    void setDirections()
    {
        directionsToCreate = new Direction[roomsToCreate];
        int idx = 0;
        Direction tmpDirection;
        while (idx < roomsToCreate)
        {
            tmpDirection = getDirectionByInt(Random.Range(0, 4));
            if (!directionIsInArray(directionsToCreate, tmpDirection))
            {
                directionsToCreate[idx] = tmpDirection;
                //Debug.Log("Rooms Directions:" + tmpDirection);
                idx++;
            }
        }
    }

    Direction getDirectionByInt(int num)
    {
        if (num > -1 && num < 4)
        {
            if (num == 0)
            {
                return Direction.N;
            }
            if (num == 1)
            {
                return Direction.E;
            }
            if (num == 2)
            {
                return Direction.S;
            }

            return Direction.W;
        }
        else
        {
            return Direction.NONE;
        }
    }


    public void removeWall(Direction _direction, bool keepCollums = false)
    {
        if (_direction.Equals(Direction.N))
        {
            
            List<GameObject> childrens = new List<GameObject>();
            foreach (Transform child in transform)
            {
                childrens.Add(child.gameObject);
            }
            //Destroy(childrens[0]);
            walls[0] = false;
            childrens[0].SetActive(false);
            if (!keepCollums)
            {
                //Destroy(childrens[4]);
                //Destroy(childrens[5]);
                childrens[4].SetActive(false);
                childrens[5].SetActive(false);
            }

        }
        else if (_direction.Equals(Direction.E))
        {

            List<GameObject> childrens = new List<GameObject>();
            foreach (Transform child in transform)
            {
                childrens.Add(child.gameObject);
            }
            //Destroy(childrens[1]);
            walls[1] = false;
            childrens[1].SetActive(false);
            if (!keepCollums)
            {
                //Destroy(childrens[5]);
                //Destroy(childrens[6]);
                childrens[5].SetActive(false);
                childrens[6].SetActive(false);
            }

        }else if (_direction.Equals(Direction.S))
        {

            List<GameObject> childrens = new List<GameObject>();
            foreach (Transform child in transform)
            {
                childrens.Add(child.gameObject);
            }
            //Destroy(childrens[2]);
           walls[2] = false;
            childrens[2].SetActive(false);
            if (!keepCollums)
            {
                //Destroy(childrens[6]);
                //Destroy(childrens[7]);
                childrens[6].SetActive(false);
                childrens[7].SetActive(false);
            }

        }else if (_direction.Equals(Direction.W))
        {

            List<GameObject> childrens = new List<GameObject>();
            foreach (Transform child in transform)
            {
                childrens.Add(child.gameObject);
            }
            //Destroy(childrens[3]);
            walls[3] = false;
            childrens[3].SetActive(false);
            if (!keepCollums)
            {
                //Destroy(childrens[4]);
                //Destroy(childrens[7]);
                childrens[4].SetActive(false);
                childrens[7].SetActive(false);
            }

        }
       
    }
    bool directionIsInArray(Direction[] array, Direction d)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == d)
            {
                return true;
            }
        }
        return false;
    }


    bool myJobIsDone()
    {
        if (roomsCreated < roomsToCreate)
        {
            return false;
        }
        else
        {
            return true;
            
        }
    }


    //simple setters & getters

    public void setWayId(int _way){
        wayId = _way;
    }
    public int getWayId()
    {
        return wayId;
    }
    public int getDistance()
    {
        return distance;
    }
    public void setDistance(int dist)
    {
        distance = dist;
    }
    public int getRoomsToCreate()
    {
        return roomsToCreate;
    }
}
