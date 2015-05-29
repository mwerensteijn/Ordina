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
        /*if (Input.GetButtonUp("Submit"))
        {
            waterGun.GetComponent<EllipsoidParticleEmitter>().emit = false;
        } */
	}

    public void Shoot()
    {
        Debug.Log("Enter shoot method");
        waterGun.GetComponent<EllipsoidParticleEmitter>().emit = true;
        Invoke("Start", 0.3f);
    }

    void OnParticleCollision(GameObject obj)
    {
        //Debug.Log("haha, hit!");
        if (obj.tag == "Paintspot")
            Destroy(obj);
    }
}
