using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlController : MonoBehaviour {
    
    public BulletSpawner bulletSpawner;
    public Watergun watershooter;
    public PopUpWindow popUp;

    public List<GameObject> paintSpots;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.PageUp) || Input.GetButtonDown("Submit"))
        {
            bulletSpawner.Shoot();
            popUp.disablePopUp();
        } else if (Input.GetKeyDown(KeyCode.PageDown) || Input.GetButtonDown("Fire1"))
        {
            watershooter.Shoot();
            popUp.disablePopUp();
        }
	}
}
