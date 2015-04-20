using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int damage;
    public float moveSpeed = 10f;
    public float timeSpentAlive;

    private float distance;
    Vector3 direction;



	// Use this for initialization
    void Start()
    {
        //transform.Rotate(new Vector3(0, 90, 90));
    }
	// Update is called once per frame
	void Update () {
        timeSpentAlive -= Time.deltaTime;
        if (timeSpentAlive < 0) // if we have been traveling for more than onesecond remove the bullet
        {
            removeMe();
        }
        
    // move the bullet

        transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        //transform.position += Vector3.forward * moveSpeed * Time.deltaTime; // because the bullet has a rigid body we don't want it moving off it's Y axis
        //transform.position = Vector3.MoveTowards(direction,)
    }   
    void removeMe()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if(other.gameObject.layer.Equals(10) )
        {
            removeMe();
        }
                
        //if (!other.tag.Equals("Player"))
        //{
            //Debug.Log("DESTROY Collision detected: " + other.gameObject + other.tag);
            
            
        //}
    }
    
   
    public void setDamage(int dmg)
    {
        damage = dmg;
    }

    public void setDirection(Vector3 dir)
    {
        direction = dir;
    }
}
