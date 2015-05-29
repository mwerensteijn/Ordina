using UnityEngine;
using System.Collections;

public class TerrainMovement : MonoBehaviour {
    private GameObject terrain1;
    private GameObject terrain2;

    private float disappearPositionZ = -70;
    private float appearPositionZ;

	// Use this for initialization
    void Start() {
        terrain1 = GameObject.Find("Terrain1") as GameObject;
        terrain2 = GameObject.Find("Terrain2") as GameObject;

        appearPositionZ = terrain2.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        // Cache the x and y position of a row.
        float x = terrain1.transform.position.x;
        float y = terrain1.transform.position.y;

        // Move the rows towards the player.
        terrain1.transform.position = new Vector3(x, y, terrain1.transform.position.z + -WorldMovement.movementSpeed * Time.deltaTime);
        terrain2.transform.position = new Vector3(x, y, terrain2.transform.position.z + -WorldMovement.movementSpeed * Time.deltaTime);

        if (terrain1.transform.position.z <= disappearPositionZ) {
            terrain1.transform.position = new Vector3(x, y, appearPositionZ);
        } else if (terrain2.transform.position.z <= disappearPositionZ) {
            terrain2.transform.position = new Vector3(x, y, appearPositionZ);
        }
	}
}
