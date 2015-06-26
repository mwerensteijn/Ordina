using UnityEngine;
using System.Collections;

//! \brief This class represents a watergun in the game. 
//! The watergun can remove the paintspots in the game 
//! and remove given anwers from the questions.
public class Watergun : MonoBehaviour {
    // waterGun object in the scene.
    public GameObject waterGun;

	//! \brief Use this for initialization
    //! \return void
	void Start () {
        // Disable the watergun
        waterGun.GetComponent<EllipsoidParticleEmitter>().emit = false;
	}

    //! \brief Shoot a water blast
    //! \return void
    public void Shoot()
    {
        // Enable  the watergun
        waterGun.GetComponent<EllipsoidParticleEmitter>().emit = true;
        // 
        Invoke("Start", 0.3f);
    }

    //! \brief When the the watergun hits a gameobject, this method will be called
    //! \return void
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
