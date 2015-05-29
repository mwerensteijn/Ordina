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
		if(other.tag == "LeftLane" || other.tag == "MiddleLane" || other.tag == "RightLane") { // The player has chosen the left answer.
			worldMovementScript.SendMessage ("AnswerCollision", other.transform);
		}
	}
}
