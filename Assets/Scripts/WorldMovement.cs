using UnityEngine;
using System.Collections;

public class WorldMovement : MonoBehaviour {
	// The states available in this MultipleChoice game.
	public enum State
	{
		Init,
		Idle,
		CheckAnswer,
		ChangeQuestion
	}
	
	// Holds the current state.
	public State currentState;
	// The movement speed of the gaming world.
	public float movementSpeed = 10f;

	// Holds the first row of answers.
	public GameObject answers1;
	// Holds the second row of answers.
	public GameObject answers2;

	// If a row with answers is not visible anymore for the player, it will respawn at this Z position.
	private float appearPositionZ;
	// Defines at which position a row with answers will disappear.
	public float disappearPositionZ;

	// The answer, given by the player.
	private AirplaneMovement.AnswerPosition givenAnswer;
	
	// Initialization
	void Start () {
		// Set the appear position.
		appearPositionZ = answers2.transform.position.z;
		currentState = WorldMovement.State.Init;

		// Start finite state machine.
		StartCoroutine("FSM");
	}
	
	// Update
	void Update () {
		// Cache the x and y position of a row.
		float x = answers1.transform.position.x;
		float y = answers1.transform.position.y;

		// Move the rows towards the player.
		answers1.transform.position = new Vector3(x, y, answers1.transform.position.z + -movementSpeed * Time.deltaTime);
		answers2.transform.position = new Vector3(x, y, answers2.transform.position.z + -movementSpeed * Time.deltaTime);

		// If a row is not visible anymore, respawn it at the appearing position.
		if(answers1.transform.position.z <= disappearPositionZ) {
			answers1.transform.position = new Vector3(x, y, appearPositionZ);
		} else if(answers2.transform.position.z <= disappearPositionZ) {
			answers2.transform.position = new Vector3(x, y, appearPositionZ);
		}
	}

	// Finite state machine.
	private IEnumerator FSM() {
		while(true) {
			switch(currentState){ 
			case WorldMovement.State.Init:
				Init ();
				break;
			case WorldMovement.State.Idle:
				break;
			case WorldMovement.State.CheckAnswer:
				CheckAnswer ();
				break;
			case WorldMovement.State.ChangeQuestion:
				ChangeQuestion();
				break;
			}
			
			yield return null;	
		}
	}

	private void Init() {
		// load questions
		// Set question and answer text

		currentState = WorldMovement.State.Idle;
	}

	private void CheckAnswer() {

		currentState = WorldMovement.State.ChangeQuestion;
	}

	private void ChangeQuestion() {

		currentState = WorldMovement.State.Idle;
	}

	// Called by AnswerCollision script.
	public void AnswerCollision(AirplaneMovement.AnswerPosition position) {
		currentState = WorldMovement.State.CheckAnswer;
	}
}
