using UnityEngine;
using System.Collections;

public class BoundCheck : MonoBehaviour {

    public static int worldSizeX;
    public static int worldSizeZ;
    int sizeX;
    int sizeY;
    int matchCount;
    int moveToCenterCounter;
    
	// Use this for initialization
    void Awake()
    {
        matchCount = 0;
        moveToCenterCounter = 0;
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

	void Start () {
        



	}
	
	// Update is called once per frame
	void Update () {

        if (moveToCenterCounter <3)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 0), 1000 * Time.deltaTime);
        }
              


        if (!checkWorldBound())
        {
            if (matchCount > 3)
            {
                //Destroy(gameObject);
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 0), 1000*Time.deltaTime);
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z));
            matchCount++;
            //gameObject.transform.position = new Vector3(transform.position.x + Random.Range(-5, 5), 0, transform.position.z + Random.Range(-5, 5));
        }
        
	}
    public void setSize(int x,int z){
        worldSizeX = x;
        worldSizeZ = z;
    }

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("Collision Detected"); 
        
    }

    bool checkWorldBound()
    {
        Collider col = gameObject.GetComponent<Collider>();
        Bounds b = col.bounds;
        Vector3 min = b.min;
        Vector3 max = b.max;
        if (worldSizeX < max.x)
            return false;

        if (worldSizeZ < max.z)
            return false;

        if (-worldSizeX > min.x)
            return false;

        if (-worldSizeZ > min.z)
            return false;
        
        return true;
    }

    void OnTriggerEnter(Collider col)
    {
        moveToCenterCounter++;

        if (!col.gameObject.CompareTag("OPlayer"))
        {
            //Collider c = gameObject.GetComponent<Collider>();
            //Debug.Log("obj:" + c.bounds + "collider:" + col.bounds + "min:" + col.bounds.min + "max:" + col.bounds.max);
            col.gameObject.GetComponent<Renderer>().material.color = Color.red;
            //Destroy(gameObject);
            
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (!col.gameObject.CompareTag("OPlayer"))
        {
            Vector3 b = col.bounds.extents;
            b *= 3;
            
            gameObject.transform.position = new Vector3(Mathf.RoundToInt(transform.position.x + Random.Range(-b.x, b.x)), 0, Mathf.RoundToInt(transform.position.z + Random.Range(-b.z, b.z)));
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (!col.gameObject.CompareTag("OPlayer"))
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
            Destroy(gameObject.GetComponent<BoundCheck>());
        }
    }

    
}
