using UnityEngine;
using System.Collections;

//! \brief BulletSpawner class shoots the paintball bullet
public class BulletSpawner : MonoBehaviour {
    public GameObject bullet;
    public float speed = 10f;
    public float shotDelay = 0.5f;

    private float lastShotTime = 0;
    private Rigidbody bulletClone;

   

	//! \brief Use this for initialization
	void Start () {
        GameObject test = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        test.gameObject.GetComponent<MeshRenderer>().enabled = false;
	}

    
    //! \brief Shoot the paintball bullet
    //! \return void
    public void Shoot()
    {
        // Time limit so the player can't spawn any bullets
        if ((Time.time - lastShotTime) < shotDelay)
        {
            return;
        }
        lastShotTime = Time.time;


        // Reset bullet position and reuse it
        if (bulletClone != null)
        {
            bulletClone.GetComponent<MeshRenderer>().enabled = true;
            bulletClone.position = transform.position;
            bulletClone.velocity = transform.forward * speed;
        }
        // No bullet available, so spawn a new one
        else
        {
            GameObject bulletCloneGO = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
            bulletClone = bulletCloneGO.GetComponent<Rigidbody>();
            bulletClone.velocity = transform.forward * speed;
        }
    }
}
