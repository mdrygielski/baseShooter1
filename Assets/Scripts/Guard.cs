using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour {

    public Transform target;
    public Transform target2;
    private GameObject objPlayer;
    NavMeshAgent agent;

    public float dist;
    float minDistance;
    public int health;
    public int damage;

    Vector3 startPosition;
    // Use this for initialization
    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

	void Start () {
        minDistance = 8f;
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        startPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        float p1Distance = Vector3.Distance(transform.position, target.position);
        if (p1Distance < minDistance)
        {
            if (p1Distance > dist)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                agent.SetDestination(transform.position);
            }
            
        }
        else
        {
            agent.SetDestination(startPosition);
        }

        /*
        float p2Distance = Vector3.Distance(transform.position, target2.position);

        if(p1Distance < minDistance || p2Distance < minDistance){

            if(p1Distance<p2Distance){
                agent.SetDestination(target.position);
            }else{
                agent.SetDestination(target2.position);
            }


        }else{
            agent.SetDestination(startPosition);
        }
         */
       

        
	}

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag.Equals("Bullet"))
    //        getHit(other.GetComponent<Bullet>().damage, gameObject);
    //    if (other.tag.Equals("Player"))
    //    {
            
    //        Debug.Log("Enemy is Killing player!");

    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bullet"))
        {
            Network.Instantiate(Resources.Load("Effects/hitEffect"), other.transform.position, Quaternion.identity, 0);
            getHit(other.GetComponent<Bullet>().damage, gameObject);
        }
    }
    

    void getHit(int dmg, GameObject obj)
    {
        Debug.Log(dmg);
        health -= dmg;
        if (health < 0)
            DestroyObject(obj);
    }
}
