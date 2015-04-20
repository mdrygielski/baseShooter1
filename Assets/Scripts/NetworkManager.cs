using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    //unique name of our game to recognize it by master server
    string registeredGameName = "serverName-baseShoter";

    public static NetworkView networkView;
    HostData[] hostData;


    //helpers
    bool isRefreshing = false;
    float refreshLength = 3f; //time for refresh server list
    public int mapSeed;

    void Start()
    {
        
        //initialize network View and set type of synchronization
        
        networkView = gameObject.AddComponent<NetworkView>();
        networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
        Debug.Log(networkView.stateSynchronization);
    }

    private void StartServer()
    {
        /*
        string ip = "127.0.0.1";
        //string ip = "192.168.0.152";
        MasterServer.ipAddress = ip;
        MasterServer.port = 23466;
        Network.natFacilitatorIP = ip;
        Network.natFacilitatorPort = 50005;
        Network.connectionTesterIP = ip;
        Network.connectionTesterPort = 10737;
        */
        //set seed for initialize map
        mapSeed = Random.Range(0,99999);

        //register server on masterserver
        Network.InitializeServer(2, 25002, false);
        MasterServer.RegisterHost(registeredGameName, "baseShoter", "comment");

        //create map for server side player
        GameManager.createMap(mapSeed);
    }



    public IEnumerator RefreshHostList()
    {
        //fill hostData informations about servers with was found
        MasterServer.ClearHostList();
        Debug.Log("Refreshing...");
        MasterServer.RequestHostList(registeredGameName);

        float timeStarted = Time.time;
        float timeEnd = Time.time + refreshLength;

        while (Time.time < timeEnd)
        {

            hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }

        if (hostData == null || hostData.Length == 0)
        {
            Debug.Log("No servers found");
        }
        else
        {
            Debug.Log(hostData.Length+" servers has been found");
            
        }
    }

    void OnServerInitialized()
    {
        //thing to do when server is ready
        Debug.Log("Server initialazed");
        
        //already - only for quick test, just spawn player. After that player have to press a button to spwan his character
        SpawnPlayer();
    }

    void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        //handle envents


        //if server is registered succeeded
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
        {
            Debug.Log("Server registered");
        }
    }


    public void OnGUI()
    {
        //Just GUI to manage all this sh* ... stuff

        if (Network.isServer)
        {
            GUILayout.Label("Its a server");
        }
        if (Network.isClient)
        {
            GUILayout.Label("Its a client");
        }
        GUI.Box(new Rect(100f, 0f, 150f, 30f), "Map seed:"+mapSeed);
        

        if (GUI.Button(new Rect(25f, 25f, 150f, 30f), "Start new server"))
        {
            //start server
            StartServer();
        }

        if (GUI.Button(new Rect(25f, 65f, 150f, 30f), "Refresh list"))
        {
            //refresh list
            StartCoroutine("RefreshHostList");
        }

        if (GUI.Button(new Rect(25f, 105f, 150f, 30f), "Spawn Me"))
        {
            //spawn player
            SpawnPlayer();
        }

        if (hostData != null)
        {
            for (int i = 0; i < hostData.Length; i++)
            {
                if (GUI.Button(new Rect(Screen.width / 2, 65f + (30f * i), 300f, 30f), hostData[i].gameName))
                {
                    Network.Connect(hostData[i]);

                }
            }
        }
    }

    
    



    private void SpawnPlayer()
    {
        //create player prefab on sceen, thats all for now
        Debug.Log("Spawning player");
        Network.Instantiate(Resources.Load("Prefabs/Player"),new Vector3(0,1,0),Quaternion.identity,0);
    }






    void OnPlayerDisconnected(NetworkPlayer player)
    {
        //Do this stuff when player is disconnected form server, doesn't matter why.

        Debug.Log("Player disconnected from" + player.ipAddress + ":" + player.port);

        //remove RPC of player
        Network.RemoveRPCs(player);

        //remove player's objects
        Network.DestroyPlayerObjects(player);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        //Do this suff when connection with server is established
        

        //If I'am a server, and any player will connect, then I set seed for him, and I command him to call createMap function
        if (Network.isServer)
        {
            networkView.RPC("setSeed", player, mapSeed);
            networkView.RPC("createMap", player, mapSeed);

        }


        //This spawn will be done after tests
        Debug.Log("Client Connected OnPlayerConnected");
        //SpawnPlayer();
        
    }


    void OnApplicationQuit()
    {
        //Do this job when window will be closed

        if (Network.isServer)
        {
            Network.Disconnect(200);
            MasterServer.UnregisterHost();

        }
        if (Network.isClient)
        {
            Network.Disconnect(200);

        }
    }



    [RPC]
    public void setSeed(int num)
    {
        //Simple setter for mapSeed
        mapSeed = num;
    }


    public int getSeed()
    {
        //Simple getter for mapSeed
        return mapSeed;
    }


    [RPC]
    public void createMap(int seed)
    {
        //Call map generator
        GameManager.createMap(seed);
    }



}
