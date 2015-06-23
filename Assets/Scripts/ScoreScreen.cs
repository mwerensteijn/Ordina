using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour {

    private Canvas _ScoreScreen;
    //public DigitalClock gameTimer;
    //public WorldMovement worldMovement;
    public Text TotalScore;
    public Text TotalTime;
    public AirplaneMovement airPlane;

	// Use this for initialization
	void Start () 
    {
        _ScoreScreen = GetComponent<Canvas>();
        _ScoreScreen.enabled = false;
	}

    public void ShowScoreScreen(int totalTimeInSeconds, string formatedTime) 
    {
        Vector3 scoreScreenPosition = airPlane.transform.position;
        scoreScreenPosition.z += 1;
        _ScoreScreen.transform.position = scoreScreenPosition;
        _ScoreScreen.enabled = true;
        TotalScore.text = "Score: " + totalTimeInSeconds;
        TotalTime.text =  "Time:  " +formatedTime;
    }
}
