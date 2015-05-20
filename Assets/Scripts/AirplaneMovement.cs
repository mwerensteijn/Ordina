using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	private enum Lane { Left, Middle, Right };
	private Lane currentLane = Lane.Middle;

	private Lane newLane = Lane.Middle;
	private Lane lookLane = Lane.Middle;

	private float movementSpeed = 10;
	private float sideMovementSpeed = 15;
	public GameObject head;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = transform.position + Camera.main.transform.forward * movementSpeed * Time.deltaTime;
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z + movementSpeed * Time.deltaTime;

		if(head.transform.rotation.y < -0.3f) { 	  // Look left
			lookLane = 0;


			if(currentLane == Lane.Middle) {
				newLane = Lane.Left;
			} else if(currentLane == Lane.Right) {
				newLane = Lane.Middle;
			}
		} else if(head.transform.rotation.y > 0.3f) { // Look Right
			if(currentLane == Lane.Middle) {
				newLane = Lane.Right;
			} else if(currentLane == Lane.Left) {
				newLane = Lane.Middle;
			}
		}



		x += sideMovementSpeed * Time.deltaTime * head.transform.rotation.y * 3.3f;

		transform.position = new Vector3(x, y, z);

		Debug.Log (currentLane);
	}
}
