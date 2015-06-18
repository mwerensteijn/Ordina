﻿using UnityEngine;
using System.Collections;

public class AirplaneMovement : MonoBehaviour {
	// Position of answers
	public enum AnswerPosition { Left, Middle, Right };
	// Holds the position, the player is looking at
	public AirplaneMovement.AnswerPosition lookingPosition = AnswerPosition.Middle;
    private AirplaneMovement.AnswerPosition lockedPosition = AnswerPosition.Middle;

	// X positions answers
	private float leftLaneX;
	private float middleLaneX;
	private float rightLaneX;
    
    public bool disableMovement = false;
	// The movement speed of the airplane
	private float sideMovementSpeed = 30f;

    public AnswerRow answerRowFront;

	// Initialization
	void Start () {
		// Save the x positions for the answers
		leftLaneX = GameObject.FindGameObjectWithTag("LeftLane").transform.position.x;
		middleLaneX = GameObject.FindGameObjectWithTag("MiddleLane").transform.position.x;
		rightLaneX = GameObject.FindGameObjectWithTag("RightLane").transform.position.x;
        answerRowFront = new AnswerRow(GameObject.FindGameObjectWithTag("Answer1"));
	}
	
	// Update is called once per frame
	void Update () {
        // Save the current position of the airplane
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        if (!disableMovement)
        {

            //z < answerRowFront.transform.position.z &&
            //Vector3.Dot(transform.position, answerRowFront.transform.position) < 0
            float distance = Vector3.Distance(transform.position, answerRowFront.transform.position);
            Debug.Log("distance " + distance);
            if (distance < 50 && distance > 20)
            {
                Debug.Log("Movement disabled!");
                disableMovement = true;        
                lockedPosition = lookingPosition;
                Debug.Log("distance < 50 tot ring");
            }

                // nog te doen
            // verwijderen 2de rij ringen
            // afstand berekene en userinput locken, vliegtuig op dat moment in de collidende baan laten vliegen (binnen de afstand van ring en vliegtuig goed poisitioneren)

            else
            {

                if (lookingPosition == AnswerPosition.Left)
                { // If looking to the left answer
                    // Move the airplane to the left lane
                    if (x > leftLaneX)
                    {
                        x -= sideMovementSpeed * Time.deltaTime;

                        if (x < leftLaneX)
                        {
                            x = leftLaneX;
                        }
                    }
                }
                else if (lookingPosition == AnswerPosition.Middle)
                { // If looking to the middle answer
                    // Move the airplane to the middle lane
                    if (x < middleLaneX)
                    {
                        x += sideMovementSpeed * Time.deltaTime;

                        if (x > middleLaneX)
                        {
                            x = middleLaneX;
                        }
                    }
                    else if (x > middleLaneX)
                    {
                        x -= sideMovementSpeed * Time.deltaTime;

                        if (x < middleLaneX)
                        {
                            x = middleLaneX;
                        }
                    }
                }
                else
                { // If looking to the right answer
                    // Move the airplane to the right lane
                    if (x < rightLaneX)
                    {
                        x += sideMovementSpeed * Time.deltaTime;

                        if (x > rightLaneX)
                        {
                            x = rightLaneX;
                        }
                    }
                }
            }
        }
        else if (disableMovement) 
        {
            if (lockedPosition == AnswerPosition.Left)
            {
                if (x > leftLaneX)
                {
                    x -= sideMovementSpeed * Time.deltaTime;

                    if (x < leftLaneX)
                    {
                        x = leftLaneX;
                    }
                }
            }
            else if (lockedPosition == AnswerPosition.Middle)
            { // If looking to the middle answer
                // Move the airplane to the middle lane
                if (x < middleLaneX)
                {
                    x += sideMovementSpeed * Time.deltaTime;

                    if (x > middleLaneX)
                    {
                        x = middleLaneX;
                    }
                }
                else if (x > middleLaneX)
                {
                    x -= sideMovementSpeed * Time.deltaTime;

                    if (x < middleLaneX)
                    {
                        x = middleLaneX;
                    }
                }
            }
            else if (lockedPosition == AnswerPosition.Right)
            { // If looking to the right answer
                // Move the airplane to the right lane
                if (x < rightLaneX)
                {
                    x += sideMovementSpeed * Time.deltaTime;

                    if (x > rightLaneX)
                    {
                        x = rightLaneX;
                    }
                }
            }
        }
        // Update the position of the airplane.
        transform.position = new Vector3(x, y, z);
	}

	void OnTriggerEnter(Collider other) {
        if (!disableMovement)
        {
            if (other.tag == "LeftLane")
            { // The player is looking at the left lane.
                lookingPosition = AirplaneMovement.AnswerPosition.Left;
            }
            else if (other.tag == "MiddleLane")
            { // The player is looking at the middle lane.
                lookingPosition = AirplaneMovement.AnswerPosition.Middle;
            }
            else if (other.tag == "RightLane")
            { // The player is looking at the right lane.
                lookingPosition = AirplaneMovement.AnswerPosition.Right;
            }
        }
	}
}
