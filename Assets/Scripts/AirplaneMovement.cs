using UnityEngine;
using System.Collections;

public class AirplaneMovement : MonoBehaviour {
	public enum AnswerPosition { Left, Middle, Right };
	public AirplaneMovement.AnswerPosition answer = AnswerPosition.Middle;

	private float leftLaneX;
	private float middleLaneX;
	private float rightLaneX;

	private float sideMovementSpeed = 30f;

	// Use this for initialization
	void Start () {
		leftLaneX = GameObject.FindGameObjectWithTag("LeftLane").transform.position.x;
		middleLaneX = GameObject.FindGameObjectWithTag("MiddleLane").transform.position.x;
		rightLaneX = GameObject.FindGameObjectWithTag("RightLane").transform.position.x;

		Debug.Log (leftLaneX);
	}
	
	// Update is called once per frame
	void Update () {
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		if(answer == AnswerPosition.Left) {
			if(x > leftLaneX) {
				x -= sideMovementSpeed * Time.deltaTime;

				if(x < leftLaneX) {
					x = leftLaneX;
				}
			}
		} else if(answer == AnswerPosition.Middle) {
			if(x < middleLaneX) {
				x += sideMovementSpeed * Time.deltaTime;

				if(x > middleLaneX) {
					x = middleLaneX;
				}
			} else if(x > middleLaneX) {
				x -= sideMovementSpeed * Time.deltaTime;
			
				if(x < middleLaneX) {
					x = middleLaneX;
				}
			}
		} else {
			if(x < rightLaneX) {
				x += sideMovementSpeed * Time.deltaTime;

				if(x > rightLaneX) {
					x = rightLaneX;
				}
			}
		}

		transform.position = new Vector3(x, y, z);
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "LeftLane") {
			answer = AirplaneMovement.AnswerPosition.Left;
		} else if(other.tag == "MiddleLane") {
			answer = AirplaneMovement.AnswerPosition.Middle;
		} else if(other.tag == "RightLane") {
			answer = AirplaneMovement.AnswerPosition.Right;
		}
	}
}
