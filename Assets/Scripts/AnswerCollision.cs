using UnityEngine;
using System.Collections;

// This script detects if the airplane collides with an answer.
// If so, send a message to the WorldMovement script.
public class AnswerCollision : MonoBehaviour {
	// The WorldMovement script.
	private GameObject worldMovementScript;

	// Initialization
	void Start () {
		// Find the WorldMovement script.
		worldMovementScript = GameObject.FindGameObjectWithTag("WorldMovement");
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.tag == "LeftLane") { // The player has chosen the left answer.
			worldMovementScript.SendMessage ("AnswerCollision", AirplaneMovement.AnswerPosition.Left);
		} else if(other.tag == "MiddleLane") { // The player has chosen the middle answer.
			worldMovementScript.SendMessage ("AnswerCollision", AirplaneMovement.AnswerPosition.Middle);
		} else if(other.tag == "RightLane") { // The player has chosen the right answer.
			worldMovementScript.SendMessage ("AnswerCollision", AirplaneMovement.AnswerPosition.Right);
		}
	}
}
