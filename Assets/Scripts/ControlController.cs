﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! \brief This class will control the controls of the Picture hunt game.
public class ControlController : MonoBehaviour {
    // The bulletspawner which shoots the paintball bullets
    public BulletSpawner bulletSpawner;
    // The watershooter which shoots the water blast
    public Watergun watershooter;
    // PopUpWindow used for disabling popUp messages when shooting
    public PopUpWindow popUp;
	
	//! \brief Update is called once per frame
    //! It checks if a key is pressed and handles when needed.
    //! \return void
	void Update () {
        // Check if a key is pressed
        if (Input.GetKeyDown(KeyCode.PageUp) || Input.GetButtonDown("Submit"))
        {
            // Shoot paintball bullet
            bulletSpawner.Shoot();
            // Disable popupmessage
            popUp.disablePopUp();
        } else if (Input.GetKeyDown(KeyCode.PageDown) || Input.GetButtonDown("Fire1"))
        {
            // Shoot a waterblast
            watershooter.Shoot();
            // Disable popupmessage
            popUp.disablePopUp();
        }
	}
}
