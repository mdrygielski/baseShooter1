/*
 * Manage structures needed to generate maze
 * 
 * 
 * */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour {


    public static List<Vector3> roomsPosition;
    public static List<GameObject> rooms;
    public static int RoomLimit;
    static Transform rootParent;
    public static List<List<GameObject>> corridors;

    //number of ways going out from init room
    public static int ways;


    bool roomsGenerated;
    bool roomsDecorated;


	void Start () {

        roomsGenerated = false;
        roomsDecorated = false;

        //find Root object to carry all rooms;
        rootParent = gameObject.transform;

        //RoomLimit = 100;
        roomsPosition = new List<Vector3>();
        rooms = new List<GameObject>();

        GameObject firstRoom = (GameObject)Instantiate(Resources.Load("Prefabs/Room"));
        GameObject playerSpawn = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/playerSpawn"));

        

        //initialize initRoom parameters;

        Room initRoom = firstRoom.GetComponent<Room>();
        initRoom.setDistance(0); //distance from init room;
        initRoom.setWayId(-1); //wayId, for init room its always -1;

        firstRoom.transform.position = new Vector3(0, 0, 0);
        addRoom(firstRoom);
        playerSpawn.transform.parent = firstRoom.transform;

        //initialization of GameObject 'ground' its useless couse its done by inspector;
        /*
        GameObject ground = (GameObject.CreatePrimitive(PrimitiveType.Cube));
        ground.name = "Ground";
        ground.transform.position = firstRoom.transform.position;
        Vector3 scale = firstRoom.transform.localScale*RoomLimit/2;
        ground.transform.localScale = new Vector3(scale.x,0.1f,scale.z);
        Renderer r = ground.GetComponent<Renderer>();
        r.material = (Material)Resources.Load("Materials/Space");
        */
	}


    public static bool addRoom(GameObject room)
    {
        if (rooms.Count < RoomLimit)
        {

                room.transform.parent = rootParent;
                rooms.Add(room);
                room.name = "Room"+(rooms.Count-1);
                roomsPosition.Add(room.transform.position);
                return true;

        }

        return false;

    }


    public static int roomsIsExists(GameObject room)
    {
        for (int i = 0; i < roomsPosition.Count; i++)
        {
            if (Mathf.RoundToInt(roomsPosition[i].x) == Mathf.RoundToInt(room.transform.position.x) && Mathf.RoundToInt(roomsPosition[i].z) == Mathf.RoundToInt(room.transform.position.z))
            {
                return i;
            }
        }

        return -1;
    }
    public static int roomsIsExists(Vector3 position)
    {
        for (int i = 0; i < roomsPosition.Count; i++)
        {
            if (Mathf.RoundToInt(roomsPosition[i].x) == Mathf.RoundToInt(position.x) && Mathf.RoundToInt(roomsPosition[i].z) == Mathf.RoundToInt(position.z))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool limitReached()
    {
        if (rooms.Count < RoomLimit)
        {
            
            return false;
        }
        else
        {
            
            return true;
        }
    }

    public static void setWays(int _ways)
    {
        //This method initialize array of arrays which length depends on numbers ways outgoing from init room

        ways = _ways;
        corridors = new List<List<GameObject>>();
        for (int i = 0; i < ways; i++)
        {
            corridors.Add(new List<GameObject>());
        }
        //Debug.Log("setWays call:" + _ways+" corridoes:"+corridors.Count);
    }

    
    public static void setRoomWay(GameObject parent, GameObject _room, int numberOfWay)
    {
        //This method build metric for every room, and fill corridors array of arrays, which carry informations about all parts of maze 

        Room parentRoom = parent.GetComponent<Room>();
        Room room = _room.GetComponent<Room>();
        room.parent = parent;

        if (parentRoom.isInitRoom)
        {
            //Debug.Log("Parent room set way call");
            room.setWayId(numberOfWay);
           
        }
        else
        {
            room.setWayId(parentRoom.getWayId());
            //Debug.Log("NOT Parent room set way call");
        }
        room.setDistance(parentRoom.distance+1);

        //Debug.Log("WayId:" + room.getWayId());
        corridors[room.getWayId()-1].Add(_room);

    }
    

    //Just for drawing areas on scene preview
    void OnDrawGizmos()
    {
        for (int i = 0; i < corridors.Count; i++)
        {
            if(i==0){
                Gizmos.color = Color.red;
            }
            if(i==1){
                Gizmos.color = Color.green;
            }
            if(i==2){
                Gizmos.color = Color.blue;
            }

            for (int j = 0; j < corridors[i].Count;j++ )
            {

                Gizmos.DrawCube(corridors[i][j].transform.position, new Vector3(15f, 0.1f, 15f));
            }


        }

            


    }

    void insertConnectors()
    {
        for (int i = 0; i < corridors.Count; i++)
        {

            List<GameObject> corridor = corridors[i];

            for (int j = 0; j < corridor.Count; j++)
            {
                GameObject roomObj = corridor[j];
                Room room = roomObj.GetComponent<Room>();
                GameObject[] neighbours = room.neighbours;
                for (int n = 0; n < neighbours.Length; n++)
                {
                    if (neighbours[n] != null)
                    {
                        Room neighbourRoom = neighbours[n].GetComponent<Room>();
                        
                        if (room.floorType!=neighbourRoom.floorType)
                        {
                            //if neighbour have other type floor material
                            //put at this edge connector

                            GameObject roomConnector = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/roomConnector"));
                            roomConnector.transform.parent = roomObj.transform;

                            GameObject laserBeam = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/laserBeam"));
                            laserBeam.transform.parent = roomObj.transform;

                            //random side for laserBeam, even or odd
                            bool beamLeftSide = Random.Range(0,100)%2==0?true:false;


                            if (n == 0)
                            {
                                //put connector
                                roomConnector.transform.localPosition = new Vector3(0,0.1f,0.5f);
                                roomConnector.transform.Rotate(Vector3.up, -90f);


                                //put laser beam
                                if (beamLeftSide)
                                {
                                    laserBeam.transform.localPosition = new Vector3(-.41f, 1, .5f);
                                    laserBeam.transform.Rotate(Vector3.up, 90f);
                                }
                                else
                                {
                                    laserBeam.transform.localPosition = new Vector3(.41f, 1, .5f);
                                    laserBeam.transform.Rotate(Vector3.up, -90f);
                                }
                            }else
                            if (n == 1)
                            {
                                //put connector
                                roomConnector.transform.localPosition = new Vector3(0.5f, 0.1f, 0);


                                //put laser beam
                                if (beamLeftSide)
                                {
                                    laserBeam.transform.localPosition = new Vector3(.5f, 1, -.41f);
                                }
                                else
                                {
                                    laserBeam.transform.localPosition = new Vector3(.5f, 1, .41f);
                                    laserBeam.transform.Rotate(Vector3.up, 180f);
                                }
                            }else
                            if (n == 2)
                            {
                                //put connector
                                roomConnector.transform.localPosition = new Vector3(0, 0.1f, -0.5f);
                                roomConnector.transform.Rotate(Vector3.up, -90f);


                                //put laser beam
                                if (beamLeftSide)
                                {
                                    laserBeam.transform.localPosition = new Vector3(-.41f, 1, -.5f);
                                    laserBeam.transform.Rotate(Vector3.up, 90f);
                                }
                                else
                                {
                                    laserBeam.transform.localPosition = new Vector3(.41f, 1, -.5f);
                                    laserBeam.transform.Rotate(Vector3.up, -90f);
                                }
                            }else
                            if (n == 3)
                            {
                                //put connector
                                roomConnector.transform.localPosition = new Vector3(-0.5f, 0.1f, 0);


                                //put laser beam
                                if (beamLeftSide)
                                {
                                    laserBeam.transform.localPosition = new Vector3(-.5f, 1, -.41f);
                                }
                                else
                                {
                                    laserBeam.transform.localPosition = new Vector3(-.5f, 1, .41f);
                                    laserBeam.transform.Rotate(Vector3.up, 180f);
                                }
                            }

                        }

                       


                    }
                }
            }

        }

        



    }


    void decorateFloor()
    {
        for (int i = 0; i < corridors.Count; i++)
        {

            List<GameObject> corridor = corridors[i];

            int randomRoom = Random.Range(1, corridor.Count-1);

            for (int j = 0; j < randomRoom; j++)
            {
                GameObject roomObj = corridor[Random.Range(1, corridor.Count - 1)];
                Room room = roomObj.GetComponent<Room>();
                
                int floorType = Random.Range(1,3);
                room.floorType = floorType;
                Material floorMaterial = (Material)Instantiate(Resources.Load("Materials/Scenery/floor" +floorType ));
                while (room.distance > 0)
                {
                    roomObj.GetComponent<Renderer>().material = floorMaterial;
                    roomObj = room.parent;
                    room = roomObj.GetComponent<Room>();
                    room.floorType = floorType;
                    

                }

            }

        }
        

    }


    void spawnCrates(GameObject room)
    {

        for (int i = -2; i < 2; i++)
        {
            for (int j = -2; j < 2; j++)
            {
                int chance = Random.Range(0,100);
                if (chance > 75)
                {
                    int crateType = Random.Range(1, 9);
                    GameObject crate = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/Crates/Crate" + crateType));
                    crate.transform.parent = room.transform;
                    crate.transform.localPosition = new Vector3(i*.16f,1,j*.16f);


                   
                    

                        int[] types = new int[2] { 3, 7 };
                        for (int t = 0; t < types.Length; t++)
                        {
                            if (types[t].Equals(crateType))
                            {

                                if ((i == 0 && j != 0) || (i != 0 && j == 0))
                                {
                                    //if its close floor light
                                    crate.transform.Rotate(Vector3.up, 90);
                                }
                                else
                                {
                                    //otherwise select some rotation
                                    if (Random.Range(1, 9) > 4)
                                    {
                                        crate.transform.Rotate(Vector3.up, 90);
                                    }
                                }

                            }
                        }

                        if (i == 0 && j == 0)
                        {
                            Destroy(crate.gameObject);
                        }

                }

                

            }


        }

    }



    void spawnLights()
    {
        
        
        for (int i = 0; i < corridors.Count; i++){
            List<GameObject> corridor = corridors[i];


            for (int r = 0; r < corridor.Count; r++)
            {
                //for every room
                GameObject roomObj = corridor[r];
                Room room = roomObj.GetComponent<Room>();
                GameObject parentObj = room.parent;
                Room roomParent = parentObj.GetComponent<Room>();
                

                //take every wall
                spawnCrates(roomObj);



                
                for (int w = 0; w < room.wallsArray.Length; w++)
                {
                    GameObject wall = room.wallsArray[w];
                    if (wall.activeSelf)
                    {
                        //if taken wall is active

                        if (w.Equals(0))
                        {
                            //Wall N

                            //wideLights
                            int lightsNumber = Random.Range(0, 3);
                            for (int e = 0; e < lightsNumber; e++)
                            {
                                GameObject lights = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/Lights/lights"));
                                lights.transform.parent = roomObj.transform;

                                if (e % 2 == 0)
                                {
                                    lights.transform.localPosition = new Vector3(0.22f, 1.6f, 0.43f);
                                    lights.transform.Rotate(Vector3.up, 90);
                                }
                                else
                                {
                                    lights.transform.localPosition = new Vector3(-0.22f, 1.6f, 0.43f);
                                    lights.transform.Rotate(Vector3.up, 90);
                                }
                            }

                            //shortLight
                            if (lightsNumber < 3)
                            {
                                GameObject lights = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/Lights/lightWhiteSmall"));
                                lights.transform.parent = roomObj.transform;
                                lights.transform.localPosition = new Vector3(0, 0.5f, 0.39f);
                            }
                            


                        }
                        if(w.Equals(1))
                        {
                            //Wall E

                            //wideLights
                            int lightsNumber = Random.Range(0, 3);
                            for (int e = 0; e < lightsNumber; e++)
                            {
                                GameObject lights = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/Lights/lights"));
                                lights.transform.parent = roomObj.transform;

                                if (e % 2 == 0)
                                {
                                    lights.transform.localPosition = new Vector3(0.43f, 1.6f, 0.22f);
                                }
                                else
                                {
                                    lights.transform.localPosition = new Vector3(0.43f, 1.6f, -0.22f);
                                }
                            }
                            //shortLight
                            if (lightsNumber < 3)
                            {
                                GameObject lights = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/Lights/lightWhiteSmall"));
                                lights.transform.parent = roomObj.transform;
                                lights.transform.localPosition = new Vector3(0.39f, 0.5f, 0);
                                lights.transform.Rotate(Vector3.up, 90);
                            }
                           


                        }
                        if (w.Equals(2))
                        {
                            //Wall S

                            //wideLights
                            int lightsNumber = Random.Range(0, 3);
                            for (int e = 0; e < lightsNumber; e++)
                            {
                                GameObject lights = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/Lights/lights"));
                                lights.transform.parent = roomObj.transform;

                                if (e % 2 == 0)
                                {
                                    lights.transform.localPosition = new Vector3(0.22f, 1.6f, -0.43f);
                                    lights.transform.Rotate(Vector3.up,90);
                                }
                                else
                                {
                                    lights.transform.localPosition = new Vector3(-0.22f, 1.6f, -0.43f);
                                    lights.transform.Rotate(Vector3.up, 90);
                                }
                            }
                            //shortLight
                            if (lightsNumber < 3)
                            {
                                GameObject lights = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/Lights/lightWhiteSmall"));
                                lights.transform.parent = roomObj.transform;
                                lights.transform.localPosition = new Vector3(0, 0.5f, -0.39f);
                            }
                            




                        }


                        if (w.Equals(3))
                        {
                            //Wall W

                            //wideLights
                            int lightsNumber = Random.Range(0, 3);
                            for (int e = 0; e < lightsNumber; e++)
                            {
                                GameObject lights = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/Lights/lights"));
                                lights.transform.parent = roomObj.transform;

                                if (e % 2 == 0)
                                {
                                    lights.transform.localPosition = new Vector3(-0.43f, 1.6f, 0.22f);
                                }
                                else
                                {
                                    lights.transform.localPosition = new Vector3(-0.43f, 1.6f, -0.22f);
                                }
                            }
                            //shortLight
                            if (lightsNumber < 3)
                            {
                                GameObject lights = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/Lights/lightWhiteSmall"));
                                lights.transform.parent = roomObj.transform;
                                lights.transform.localPosition = new Vector3(-0.39f, 0.5f, 0);
                                lights.transform.Rotate(Vector3.up, 90);
                            }
                            


                        }


                    }
                    
                }
                
                


            }


        }

        
    }


    void insertKeyAndExit()
    {
        //take lengths of all corridors
        int[] corridorsSize = new int[corridors.Count];
        for (int i = 0; i < corridors.Count; i++)
        {
            corridorsSize[i] = corridors[i].Count;
        }

        //sort lengths from longest to shortest corridor
        for (int s = 0; s < corridors.Count - 1; s++)
        {
            for (int i = 0; i < corridors.Count - 1; i++)
            {
                if (corridors[i].Count < corridors[i + 1].Count)
                {
                    List<GameObject> temp = corridors[i + 1];
                    corridors[i + 1] = corridors[i];
                    corridors[i] = temp;
                    
                }
            }

        }

        //corridorsSize, 0 and 1 elements are the longest;
        //0 is for exit
        //1 is for key

        GameObject exitRoom = corridors[0][corridors[0].Count-1];
        GameObject keyRoom = corridors[1][corridors[1].Count - 1];
        Debug.Log("exit:" + exitRoom + "key:" + keyRoom);
        cleanRoom(exitRoom);

        GameObject exitPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/mazePortal"));
        exitPrefab.transform.parent = exitRoom.transform;
        exitPrefab.transform.localPosition = new Vector3(0, .2f, 0);

        GameObject keyPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Scene/card"));
        keyPrefab.transform.parent = keyRoom.transform;
        keyPrefab.transform.localPosition = new Vector3(0, 1f, 0);

        Material floorMaterial = (Material)Instantiate(Resources.Load("Materials/Scenery/floor3"));
        exitRoom.GetComponent<Renderer>().material = floorMaterial;
        keyRoom.GetComponent<Renderer>().material = floorMaterial;

        Room exit = exitRoom.GetComponent<Room>();
        Room parentExit = exit.parent.GetComponent<Room>();
        exit.floorType = 3;
        parentExit.floorType = 3;
        exit.parent.GetComponent<Renderer>().material = floorMaterial;

        

        /*
        foreach (GameObject neighbour in exit.neighbours)
        {
            if (neighbour != null)
            {
                neighbour.GetComponent<Renderer>().material = floorMaterial;
                
            }
        }
        */

    }

    void cleanRoom(GameObject room)
    {
        
        int c = 0;
        foreach (Transform child in room.transform)
        {
            
            if (c > 7)
            {
                
                Destroy(child.gameObject);
            }
            c++;
        }

    }

    void decorate()
    {
        //lights & crates
        spawnLights();
        decorateFloor();
        insertKeyAndExit();

        insertConnectors();

    }



    void Update()
    {
        if (limitReached())
        {
            
            if (!roomsGenerated)
            {
                Debug.Log("Rooms generated");
                if (!roomsDecorated)
                {
                    Debug.Log("Rooms decorated");
                    //here call decoration function
                    decorate();
                    
                }
                roomsDecorated = true;
            }
            roomsGenerated = true;
            
            
        }

    }
}
