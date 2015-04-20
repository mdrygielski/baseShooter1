using UnityEngine;
using System.Collections;


public class GunPistol : MonoBehaviour
{

    public int Damage;
    public float SPS;           //ShootsPerSecond
    public int AmmoCapacity;    //Pojemnosc magazynka
    public float shootDistance;
    public float reloadTime;
    
    float shootTime;
    public int BulletsLeft;

    
    // Use this for initialization
    void Start()
    {
        BulletsLeft = AmmoCapacity;
    }


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && BulletsLeft > 0 && Time.time > shootTime)
        {
            Vector3 direction = transform.TransformDirection(Vector3.forward * shootDistance);
            GameObject objBullet = (GameObject)Network.Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, Quaternion.LookRotation(direction), 0);
            //objBullet.transform.parent = transform;
            //objBullet.transform.rotation = transform.rotation;
            Bullet bullet = objBullet.GetComponent<Bullet>();
            bullet.setDamage(Damage);
            BulletsLeft--;
            
            Debug.Log("Bullets left: " + BulletsLeft);
            if (BulletsLeft <= 0)
            {
                shootTime = Time.time + (reloadTime );
                Debug.Log("Reloading: " + reloadTime + " seconds.");
                BulletsLeft = AmmoCapacity;
            }
            else
                shootTime = Time.time + 1/SPS;
        }
    }
}
