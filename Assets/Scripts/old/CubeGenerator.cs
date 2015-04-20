using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CubeGenerator : MonoBehaviour {



    public class Graph
    {

    }

    public int sizeX;
    public int sizeZ;
    public int cubeAmount;
    public int minRoomSize;
    public int maxRoomSize;
    public List<GameObject> roomList;
    bool roomListIsSet;
    //bool graphHasBeenBuild;
    public List<Edge> graph;
    public List<Edge> neighbours;
    public List<List<float>> weightGraph;
    public List<GameObject> spanningTree;
    //Edge testEdge;
    float maxDistance;
    List<Edge> graphPrim;


    int z;
    int minX;
    int maxX;
    int minZ;
    int maxZ; 




	// Use this for initialization
    void Awake()
    {
        maxDistance = (sizeX + sizeZ) * 2;

        //graphHasBeenBuild = false;
        roomListIsSet = false;
        minX = -sizeX;
        maxX = sizeX;

        minZ = -sizeZ;
        maxZ = sizeZ;

        neighbours = new List<Edge>();
        spanningTree = new List<GameObject>();
        weightGraph = new List<List<float>>();
        //testEdge = new Edge();
        graph = new List<Edge>();
        roomList = new List<GameObject>();
        GameObject dungeon = new GameObject("DungeonContainer");
        

        int x, z;
        for (int i = 0; i < cubeAmount; i++)
        {
            x = Random.Range(minX, maxX);
            z = Random.Range(minZ, maxZ);
            Vector3 pos = new Vector3((float)x, (float)0, (float)z);
            GameObject obj = (GameObject.CreatePrimitive(PrimitiveType.Cube));

            obj.transform.position = pos;
            obj.name = "v" + i;

            int objWidth = Random.Range(minRoomSize, maxRoomSize);
            int objHeight = Random.Range(minRoomSize, maxRoomSize);
            obj.transform.localScale = new Vector3(objWidth, 1, objHeight);

            //int maxSize = objWidth>objHeight?objWidth:objHeight;
            //Collider objCol = obj.GetComponent<Collider>();
            //objCol.transform.localScale = new Vector3(maxSize,0,maxSize);

            BoundCheck bc = obj.AddComponent<BoundCheck>();
            bc.setSize(sizeX,sizeZ);


            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;
            Collider col = obj.GetComponent<Collider>();
            col.isTrigger = true;

            if (obj.transform.position.x >= maxX || obj.transform.position.x <= minX || obj.transform.position.z >= maxZ || obj.transform.position.z <= minZ)
            {
                //Debug.Log("Obj deleted");
                //Destroy(obj);

            }


            obj.transform.parent = dungeon.transform;

            roomList.Add(obj);
            
        }

        //testEdge.setEdge(roomList[0], roomList[1]);

        
        //createGraph

        for (int i = 0; i < roomList.Count; i++)
        {
            for (int j = i; j < roomList.Count; j++)
            {
                if (i != j)
                {
                    Edge e = new Edge(roomList[i], roomList[j]);
                    e.setEdge(roomList[i], roomList[j]);
                    graph.Add(e);

                }
            }

        }






           // Debug.Log("Graph primGraphSize:" + weightGraph.Count + "TestRow:" + weightGraph[0][0]);










        //size graph
        
        Debug.Log("Graph size:" + graph.Count);



        
        roomListIsSet = true;

    }
    void Start()
    {
        
    }
    int minKey(float[] key, bool[] mstSet)
    {
        // Initialize min value
        int min_index = 0;
        float min = maxDistance;
 
        for (int v = 0; v < cubeAmount; v++)
            if (mstSet[v] == false && key[v] < min){
                min = key[v];
                min_index = v;
            }
 
        return min_index;
    }

    int getMinimumIndex(List<float> row)
    {
        float minimum = maxDistance;
        int minimumIndex = 0;
        for (int i = 0; i < row.Count-1; i++)
        {
            if (row[i] < minimum)
            {
                minimum = row[i];
                minimumIndex = i;
            }
        }
        return minimumIndex;
    }

    IEnumerator Kruskal(List<List<float>> graf)
    {
        int[] parent = new int[graf.Count];
        //int u = 0;
        //int v = 0;
        int noOfEdges = 1;
        //float min;

        for (int i = 0; i < graf.Count; i++)
        {
            parent[i] = 0;
        }
        
        while (noOfEdges < graf.Count)
        {
            //min = maxDistance;

            
            for (int i = 0; i < graf.Count; i++)
            {
                
                
                for (int j = 0; j < graf.Count; j++)
                {
                    //if (graf[i][j] < min)
                    //{
                        //min = graf[i][j];
                    //}
                }
                
            }
            

            noOfEdges++;
        }

        Debug.Log("done");
        yield return new WaitForSeconds(8);

       // graphHasBeenBuild = true;
    }

    void findNeighbours(List<List<float>> g)
    {


        for (int i = 0; i < g.Count; i++)
        {
            
            //Edge e = new Edge(roomList[i], roomList[getMinimumIndex(g[i])]);
            //neighbours.Add(e);
        }

    }

    void Update()
    {
        //testEdge = graph[0];
        //Debug.Log("testObjPosition:" + testEdge.getStart().transform.position + "startName:" + testEdge.getStart().name+"endName:" + testEdge.getEnd().name+"distance:"+testEdge.getDistance());
        //sort Graph

        /*
        for (int i = 0; i < graph.Count; i++)
        {

           graph[i].getStart().name = graph[i].startName;
           graph[i].getEnd().name = graph[i].endName;
            
        }

        */
        graph.Sort(delegate(Edge e1, Edge e2)
        {
            return e1.getDistance().CompareTo(e2.getDistance());
        });
        
        //debug info about graph
        //Debug.Log("testedge:" + testEdge.ToString() + "distance:" + testEdge.getDistance());
        string dist = "";
        for (int i = 0; i<3; i++)
        {
            dist += graph[i].getDistance() + "  ";
        }
        //Debug.Log(dist);
        graphPrim = new List<Edge>();
        /*
        
        graph[0].getStart().name = "main";
        graph[0].getEnd().name = "main";
        //graphPrim.Add(graph[0]);
        for (int j = 1; j < 2; j++)
        {
            for (int i = 0; i < graph.Count; i++)
            {
                if (!(graph[i].getStart().name.Equals("main") && graph[i].getEnd().name.Equals("main")))
                {

                    graph[i].getStart().name = graph[i].getEnd().name;

                }
                if ((graph[i].getStart().name.Equals("main") && !graph[i].getEnd().name.Equals("main")) || (!graph[i].getStart().name.Equals("main") && graph[i].getEnd().name.Equals("main")))
                {
                    graphPrim.Add(graph[i]);
                    graph[i].getStart().name = "main";
                    graph[i].getEnd().name = "main";
                }
                if (graph[i].getStart().name.Equals(graph[i].getEnd().name))
                {
                    graphPrim.Add(graph[i]);
                }

            }
        }

        Debug.Log(graphPrim.Count);
        */

        if (roomListIsSet)
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
                weightGraph.Add(row);
                
            }
            //Debug.Log(weightGraph[0][2]);
            //Kruskal(weightGraph);
            //if (!graphHasBeenBuild && roomListIsSet)
            //{
            //    StartCoroutine("Kruskal", weightGraph);
           // }
            for (int i = 0; i < weightGraph.Count; i++)
            {
                //Debug.Log(i);
                //Edge e = new Edge(roomList[i], roomList[getMinimumIndex(g[i])]);
                //neighbours.Add(e);
            }
            //findNeighbours(weightGraph);
            //Debug.Log(weightGraph.Count);
        }
    }
 

    void OnDrawGizmos()
    {
        //draw Map border
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(maxX*2, 0.1f, maxZ*2));
        
        Gizmos.color = Color.black;

        //draw debug Edges

        if (roomListIsSet)
        {
            /*
            for (int i = 0; i < roomList.Count; i++)
            {
                for (int j = i; j < roomList.Count; j++)
                {
                    if (i != j)
                    {
                        //Gizmos.DrawLine(roomList[i].transform.position, roomList[j].transform.position);
                    }
                }

            }
           */
            for (int i = 0; i < graphPrim.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(graphPrim[i].getStart().transform.position, graphPrim[i].getEnd().transform.position);

            }
            


            //draw testEdge
            //Gizmos.color = Color.red;
            
            //Gizmos.DrawLine(testEdge.getStart().transform.position, testEdge.getEnd().transform.position);
        }



    }

}
