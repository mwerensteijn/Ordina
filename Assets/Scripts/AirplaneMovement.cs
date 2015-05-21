using UnityEngine;
using System.Collections;

public class AirplaneMovement : MonoBehaviour {
	public enum Position { Left, Middle, Right };
	public AirplaneMovement.Position answer = Position.Middle;

	private float movementSpeed = 10;
	private float sideMovementSpeed = 30;

	private float leftLanePositionX = -48;
	private float middleLanePositionX = -16;
	private float rightLanePositionX = 15;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z + movementSpeed * Time.deltaTime;

		if(answer == Position.Left) {
			if(x > leftLanePositionX) {
				x -= sideMovementSpeed * Time.deltaTime;
			}
		} else if(answer == Position.Middle) {
			if(x < middleLanePositionX - (sideMovementSpeed * Time.deltaTime)) {
				x += sideMovementSpeed * Time.deltaTime;
			} else if(x > middleLanePositionX + (sideMovementSpeed * Time.deltaTime)) {
				x -= sideMovementSpeed * Time.deltaTime;
			}
		} else {
			if(x < rightLanePositionX) {
				x += sideMovementSpeed * Time.deltaTime;
			}
		}

		transform.position = new Vector3(x, y, z);
	}

	void OnTriggerEnter(Collider other) {
		if(other.name == "A") {
			answer = AirplaneMovement.Position.Left;
		} else if(other.name == "B") {
			answer = AirplaneMovement.Position.Middle;
		} else if(other.name == "C") {
			answer = AirplaneMovement.Position.Right;
		}
	}
}
