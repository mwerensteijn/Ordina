using UnityEngine;
using System.Collections;

public class TriggerMenu : MonoBehaviour {
	private GameObject selectedButton;

	public float selectTime;
	private float currentTime = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(selectedButton != null) {
			currentTime += Time.deltaTime;

			if(currentTime >= selectTime) {
				if(selectedButton.name == "Start") {
					Application.LoadLevel (1);
				} else if(selectedButton.name == "Afsluiten") {
					Application.Quit();
				}
			}
		}
	}

	public void OnGazeEnter(GameObject button) {
		selectedButton = button;
	}

	public void OnGazeExit() {
		selectedButton = null;

		currentTime = 0;
	}
}
