using UnityEngine;
using System.Collections;

public class WorldMovement : MonoBehaviour {	
	public float movementSpeed = 10f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z + -movementSpeed * Time.deltaTime;

		transform.position = new Vector3(x, y, z);
	}
}
