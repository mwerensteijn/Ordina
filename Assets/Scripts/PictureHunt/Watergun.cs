using UnityEngine;
using System.Collections;

// This class represents a watergun in the game. 
// The watergun can remove the paintspots in the game 
// and remove given anwers from the questions.
public class Watergun : MonoBehaviour {
    // waterGun object in the scene.
    public GameObject waterGun;

	// Use this for initialization
	void Start () {
        // Disable the watergun
        waterGun.GetComponent<EllipsoidParticleEmitter>().emit = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

    // Shoot a water blast
    public void Shoot()
    {
        // Enable  the watergun
        waterGun.GetComponent<EllipsoidParticleEmitter>().emit = true;
        // 
        Invoke("Start", 0.3f);
    }

    // When the the watergun hits a gameobject, this method will be called
    void OnParticleCollision(GameObject obj)
    {
        if (obj.tag == "Paintspot")
        {
            // Remove paintspot from the scene
            Destroy(obj);
        }

        if (obj.tag == "Question")
        {
            // Reset the question
            obj.GetComponent<PictureQuestion>().reset();
        }
    }
}
