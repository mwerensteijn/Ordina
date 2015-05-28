using UnityEngine;
using System.Collections;

public class Watergun : MonoBehaviour {

    public GameObject waterGun;

	// Use this for initialization
	void Start () {
        waterGun.GetComponent<EllipsoidParticleEmitter>().emit = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonUp("Fire2"))
        {
            waterGun.GetComponent<EllipsoidParticleEmitter>().emit = false;
        } 
	}

    public void Shoot()
    {
        waterGun.GetComponent<EllipsoidParticleEmitter>().emit = true;

    }
}
