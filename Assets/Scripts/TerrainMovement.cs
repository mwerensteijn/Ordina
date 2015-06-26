﻿using UnityEngine;
using System.Collections;

public class TerrainMovement : MonoBehaviour {
    private GameObject terrain1;
    private GameObject terrain2;

    private float dissapearPositionZ = 0;
    private float appearPositionZ;
    private float terrainDistance = 0;
    public Transform airPlanePosition;

    //! \brief Start is called on the frame when a script is enabled.
    //! Setup the terrain variables.
    //! \return void
    void Start() {
        terrain1 = GameObject.Find("Terrain1") as GameObject;
        terrain2 = GameObject.Find("Terrain2") as GameObject;

       // disappearPositionZ = terrain1.transform.position.z - terrain2.transform.position.z;
        terrainDistance = Vector3.Distance(terrain1.transform.position, terrain2.transform.position);
        appearPositionZ = (float)terrainDistance;
        //appearPositionZ = terrain2.transform.position.z + terrainDistance;
        dissapearPositionZ = airPlanePosition.position.z + (terrainDistance / 2) * -1;
	}

    //! \brief Update is called every frame.
    //! Move the terrains to create a seamless game world.
    //! \return void
	void Update () {
        // Cache the x and y position of a row.
        float x = terrain1.transform.position.x;
        float y = terrain1.transform.position.y;

        // Move the rows towards the player.
        terrain1.transform.position = new Vector3(x, y, terrain1.transform.position.z - WorldMovement.movementSpeed * Time.deltaTime);
        terrain2.transform.position = new Vector3(x, y, terrain2.transform.position.z - WorldMovement.movementSpeed * Time.deltaTime);

        float distance = Vector3.Distance(terrain1.transform.position, terrain2.transform.position); 

        if (terrain1.transform.position.z <= dissapearPositionZ) {
            terrain1.transform.position = new Vector3(x, y, terrain2.transform.position.z + appearPositionZ);
        } else if (terrain2.transform.position.z <= dissapearPositionZ) {
            terrain2.transform.position = new Vector3(x, y, terrain1.transform.position.z + appearPositionZ);
        }
	}
}
