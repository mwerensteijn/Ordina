using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlController : MonoBehaviour {
    
    public BulletSpawner bulletSpawner;
    public Watergun watershooter;

    public List<GameObject> paintSpots;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            bulletSpawner.Shoot();
        } else if (Input.GetButton("Fire2"))
        {
            watershooter.Shoot();
        }
	}
}
