using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour {

    private Canvas _ScoreScreen;
    public Text TotalScore;
    public Text TotalTime;

	// Use this for initialization
	void Start () 
    {
        _ScoreScreen = GetComponent<Canvas>();
        _ScoreScreen.enabled = false;
	}

    public void ShowScoreScreen(int totalTimeInSeconds, string formatedTime) 
    {
        _ScoreScreen.enabled = true;
        TotalScore.text = "Score: " + totalTimeInSeconds;
        TotalTime.text =  "Time:  " +formatedTime;
    }
}

