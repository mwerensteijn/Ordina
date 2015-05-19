using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
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

		x += sideMovementSpeed * Time.deltaTime * head.transform.rotation.y * 3.3f;

		transform.position = new Vector3(x, y, z);

		//Debug.Log (head.transform.rotation.y);
	}
}
