﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DigitalClock : MonoBehaviour {

	public Text timer;
	private float secondsTimer, minutesTimer;

	// Use this for initialization
	void Start () {
		timer = this.GetComponent<Text>();
		secondsTimer = 0;
		minutesTimer = 0;
		Debug.Log (timer.text);
		timer.text = minutesTimer.ToString("00") + ":" + secondsTimer.ToString("00");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		secondsTimer += Time.fixedDeltaTime;
		if (secondsTimer >= 60) {
			minutesTimer++;
			secondsTimer = 0;
		}
		timer.text = minutesTimer.ToString("00") + ":" + Math.Floor(secondsTimer).ToString("00");
	}
}