using UnityEngine;
using System.Collections;

public class AirplaneMovement : MonoBehaviour {
	public enum AnswerPosition { Left, Middle, Right };
	public AirplaneMovement.AnswerPosition answer = AnswerPosition.Middle;

	private float movementSpeed = 10;
	private float sideMovementSpeed = 15;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = transform.position + Camera.main.transform.forward * movementSpeed * Time.deltaTime;
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z + movementSpeed * Time.deltaTime;

		if(answer == AnswerPosition.Left) {
			if(x > -48) {
				x -= 30 * Time.deltaTime;
			}
		} else if(answer == AnswerPosition.Middle) {
			if(x < -22) {
				x += 30 * Time.deltaTime;
			} else if(x > -10) {
				x -= 30 * Time.deltaTime;
			}
		} else {
			if(x < 15) {
				x += 30 * Time.deltaTime;
			}
		}

		transform.position = new Vector3(x, y, z);
	}

	void OnTriggerEnter(Collider other) {
		if(other.name == "A") {
			answer = AirplaneMovement.AnswerPosition.Left;
		} else if(other.name == "B") {
			answer = AirplaneMovement.AnswerPosition.Middle;
		} else if(other.name == "C") {
			answer = AirplaneMovement.AnswerPosition.Right;
		}
	}
}
