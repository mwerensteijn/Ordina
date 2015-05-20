using UnityEngine;
using System.Collections;

public class ControlController : MonoBehaviour {
    public BulletSpawner bulletSpawner;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            bulletSpawner.Shoot();
        }
	}
}
