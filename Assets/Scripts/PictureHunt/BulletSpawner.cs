using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour {
    public GameObject bullet;
    public float speed = 10f;
    public float shotDelay = 0.5f;

    private float lastShotTime = 0;
    private Rigidbody bulletClone;

   

	// Use this for initialization
	void Start () {
        GameObject test = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        test.gameObject.GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Shoot()
    {
        if ((Time.time - lastShotTime) < shotDelay)
        {
            return;
        }
        lastShotTime = Time.time;

        if (bulletClone != null)
        {
            bulletClone.GetComponent<MeshRenderer>().enabled = true;
            bulletClone.position = transform.position;
            bulletClone.velocity = transform.forward * speed;
        }
        else
        {
            GameObject bulletCloneGO = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
            bulletClone = bulletCloneGO.GetComponent<Rigidbody>();
            bulletClone.velocity = transform.forward * speed;
        }
    }
}
