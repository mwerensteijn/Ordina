using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DigitalClock : MonoBehaviour {

	public Text timer;
	private float secondsTimer, minutesTimer;

    //! \brief Start is called on the frame when a script is enabled.
    //! Initialize the variables.
    //! \return void
	void Start () {
		timer = this.GetComponent<Text>();
		secondsTimer = 0;
		minutesTimer = 0;
		timer.text = minutesTimer.ToString("00") + ":" + secondsTimer.ToString("00");
	}

    //! \brief Update the digital timer.
	void FixedUpdate () {
		secondsTimer += Time.fixedDeltaTime;
		if (secondsTimer >= 60) {
			minutesTimer++;
			secondsTimer = 0;
		}
		timer.text = minutesTimer.ToString("00") + ":" + Math.Floor(secondsTimer).ToString("00");
	}

    //! \brief Returns the passed time in seconds
    //! \return int the time in seconds.
    public int GetTotalSeconds() 
    {
        int roundedSeconds = (int)Math.Round(secondsTimer, 0);
        int roundedMinuts = (int)Math.Round(minutesTimer, 0);
        return roundedSeconds + (roundedMinuts * 60);
    }

    //! \brief Get the time in the following format: 00:00:00
    //! \return string the time formatted as string
    public string GetFormatedTime() 
    {
        return minutesTimer.ToString("00") + ":" + Math.Floor(secondsTimer).ToString("00");
    }
}
