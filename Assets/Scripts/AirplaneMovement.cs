using UnityEngine;
using System.Collections;

public class AirplaneMovement : MonoBehaviour {
	public enum Position { Left, Middle, Right };
	public AirplaneMovement.Position lookingPosition = Position.Middle;
	
	private float movementSpeed = 40f;
	private float sideMovementSpeed = 30f;
	
	private float leftLanePositionX = -50f;
	private float middleLanePositionX = -16f;
	private float rightLanePositionX = 18f;

	public GameObject answers;
	public float newAnswersPositionZ;
	public float questionDistance = 250f;

	// Use this for initialization
	void Start () {
		newAnswersPositionZ = transform.position.z + questionDistance;
	}
	
	// Update is called once per frame
	void Update () {
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z + movementSpeed * Time.deltaTime;
		
		if(lookingPosition == Position.Left) {
			if(x > leftLanePositionX) {
				x -= sideMovementSpeed * Time.deltaTime;
				
				if(x < leftLanePositionX) {
					x = leftLanePositionX;
				}
			}
		} else if(lookingPosition == Position.Middle) {
			if(x < middleLanePositionX) {
				x += sideMovementSpeed * Time.deltaTime;
				
				if(x > middleLanePositionX) {
					x = middleLanePositionX;
				}
			} else if(x > middleLanePositionX) {
				x -= sideMovementSpeed * Time.deltaTime;
				
				if(x < middleLanePositionX) {
					x = middleLanePositionX;
				}
			}
		} else {
			if(x < rightLanePositionX) {
				x += sideMovementSpeed * Time.deltaTime;
				
				if(x > rightLanePositionX) {
					x = rightLanePositionX;
				}
			}
		}
		
		transform.position = new Vector3(x, y, z);

		if(transform.position.z >= newAnswersPositionZ) {
			newAnswersPositionZ += questionDistance;
			answers.transform.position = answers.transform.position + new Vector3(0, 0, questionDistance);
		}
		
		answers.transform.FindChild("A");
		answers.transform.FindChild("B");
		answers.transform.FindChild("C");
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.name == "A") {
			lookingPosition = AirplaneMovement.Position.Left;
		} else if(other.name == "B") {
			lookingPosition = AirplaneMovement.Position.Middle;
		} else if(other.name == "C") {
			lookingPosition = AirplaneMovement.Position.Right;
		}
	}
}