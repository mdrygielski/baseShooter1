using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MapGenerator : MonoBehaviour {

    public int mapSizeX;
    public int mapSizeZ;
    public int minRoomWallSize;
    public int maxRoomWallSize;
    public int amountOfRooms;
    public bool showGrid;
    public int mapSeed;

    int roomsPerRow;
    float maxDistance;

    List<GameObject> roomList;
    List<List<float>> distanceList;
    List<Edge> edgeList;


    bool edgesCalculated = false;

    void Awake()
    {
        Random.seed = mapSeed;
    }

	// Used for initialization
	void Start () {
        

        //Initiation list of rooms
        roomList = new List<GameObject>();


        //Initiation 2 dimensional list of distances between rooms;
        distanceList = new List<List<float>>();


        //Initiation list of edges between rooms;
        edgeList = new List<Edge>();


        //Calculate maximum amount of rooms per row
        roomsPerRow = Mathf.RoundToInt(Mathf.Sqrt((float)amountOfRooms));


        //Be sure to keep minimum amount of rooms not lees then half of amountOfRooms;
        while (CreateRooms() < Mathf.RoundToInt(amountOfRooms / 2))
        {
            RemoveRooms();
        }

        //Fill distance list;
        CreateDistanceList();


        //Based on distanceList, create edgeList contains minimum distances between rooms;
        CreateEdgeList();

        Debug.Log("Rooms:" + roomList.Count + "Edges:" + edgeList.Count);
        for (int i = 0; i < edgeList.Count;i++)
        {
            //Debug.Log(edgeList[i].ToString());
        }
        Debug.Log(Random.seed);

	
	}

    void CreateEdgeList()
    {
        for (int i = 0; i < distanceList.Count; i++)
        {
            Edge e = new Edge(roomList[i],roomList[getMinimumIndex(distanceList[i])]);
            edgeList.Add(e);
        }
        edgesCalculated = true;
    }


    int getMinimumIndex(List<float> row)
    {
        float minimum = maxDistance;
        int minimumIndex = 0;
        for (int i = 0; i < row.Count - 1; i++)
        {
            if (row[i] < minimum)
            {
                minimum = row[i];
                minimumIndex = i;
            }
        }
        return minimumIndex;
    }

    void CreateDistanceList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            List<float> row = new List<float>();
            for (int j = 0; j < roomList.Count; j++)
            {
                if (i != j)
                {
                    row.Add(Vector3.Distance(roomList[i].transform.position, roomList[j].transform.position));
                }
                else
                {
                    row.Add(maxDistance);
                }

            }

            distanceList.Add(row);
        }
    }


    void RemoveRooms()
    {
        //Just destroy all gameObjects in roomList
        foreach (GameObject room in roomList)
        {
            Destroy(room);
        }

    }

    int CreateRooms()
    {

        //calculate cell size;
        int cellSize = Mathf.RoundToInt(maxRoomWallSize * 2f);

        //This keeps reflection of existing rooms in array
        //bool[] cells = new bool[roomsPerRow * roomsPerRow];

        //Initiation max posibble distance between rooms;
        maxDistance = cellSize * maxRoomWallSize;


        //room wall size;
        int objWidth;
        int objHeight;

        //center cell coordinates
        int centerRoomX;
        int centerRoomZ;

        //center room coordinates; 
        int x;
        int z;


        for (int i = 0; i < roomsPerRow; i++)
        {

            for (int j = 0; j < roomsPerRow; j++)
            {


                //Probability to create room in cell
                if (Random.Range(0, 1f) < 0.8f)
                {

                    //Draw size of room
                    objWidth = Random.Range(minRoomWallSize, maxRoomWallSize);
                    objHeight = Random.Range(minRoomWallSize, maxRoomWallSize);

                    //based on cell size, calculate position to not intersect with cell border
                    //calculate center of cell
                    centerRoomX = i * cellSize;
                    centerRoomZ = j * cellSize;

                    x = Mathf.RoundToInt(Random.Range(centerRoomX - minRoomWallSize * 0.5f, centerRoomX + maxRoomWallSize * 0.5f));
                    z = Mathf.RoundToInt(Random.Range(centerRoomZ - minRoomWallSize * 0.5f, centerRoomZ + maxRoomWallSize * 0.5f));
                    Vector3 pos = new Vector3((float)x, (float)0, (float)z);

                    GameObject obj = (GameObject.CreatePrimitive(PrimitiveType.Cube));
                    obj.transform.position = pos;
                    obj.transform.localScale = new Vector3(objWidth, 1, objHeight);
                    obj.name = "room" + ((i * roomsPerRow) + j);
                    roomList.Add(obj);
                }
            }
        }

        return roomList.Count;
    }







    //Below only gizmos and debug informations are created;
    //---------------------------------------------------------------------
    void OnDrawGizmos()
    {
        int cellSize = maxRoomWallSize * 2;

        if (showGrid)
        {
            for (int i = 0; i < roomsPerRow; i++)
            {

                for (int j = 0; j < roomsPerRow; j++)
                {
                    if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                    {
                        Gizmos.color = new Color(1, 0, 0, .5f);
                    }
                    else
                    {
                        Gizmos.color = new Color(1, 1, 1, .5f);
                    }
                    //Gizmos.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.5F);
                    Gizmos.DrawCube(new Vector3(i * cellSize, 0, j * cellSize), new Vector3(cellSize, .5f, cellSize));
                }
            }
        }

        if (edgesCalculated)
        {
            for (int i = 0; i < edgeList.Count; i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1f);
                Gizmos.DrawLine(edgeList[i].startPosition, edgeList[i].endPosition);
            }
        }



    }
	// Update is called once per frame
	void Update () {
	
	}
}
