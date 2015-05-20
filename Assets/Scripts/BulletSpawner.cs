using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour {
    public Rigidbody bullet;
    public float speed = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Shoot()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, transform.position, transform.rotation);
        bulletClone.velocity = transform.forward * speed;
    }
}
