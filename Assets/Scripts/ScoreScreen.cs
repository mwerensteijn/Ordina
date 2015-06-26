using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour {

    private Canvas _ScoreScreen;
    public Text TotalScore;
    public Text TotalTime;

    //! \brief Start is called on the frame when a script is enabled.
    //! Initialize the variables.
    //! \return void
	void Start () 
    {
        _ScoreScreen = GetComponent<Canvas>();
        _ScoreScreen.enabled = false;
	}

    //! \brief ShowScoreScreen is called to enable the scorescreen.
    //! Show the scorescreen and show the score and time.
    //! \return void
    public void ShowScoreScreen(int score, string formatedTime) 
    {
        _ScoreScreen.enabled = true;
        TotalScore.text = "Score: " + score;
        TotalTime.text =  "Time:  " + formatedTime;
    }

    //! \brief Return to the mainmenu.
    //! \return void
    public void returnToMainMenu()
    {
        // level 0 is main menu
        Application.LoadLevel("MainMenu2d");
    }
}

