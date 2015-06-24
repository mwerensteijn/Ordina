using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerInput : MonoBehaviour {
    public MainMenuController controller;
    public PopUpWindow popUp;
    public InputField playerName;
    public GameManager gameManager;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private bool checkPlayerName(string name)
    {
        // email
        return name.Contains("@") && name.Contains(".");
        // something
        //return name != "";
    }

    public void Next()
    {
        if (checkPlayerName(playerName.text))
        {
            gameManager.setPlayerName(playerName.text);
            controller.selectSubject();
        }
        else
        {
            Debug.Log(playerName.text);
            // Popup give a legit name/email please
            popUp.enablePopUp("Please fill in an emailaddress");
        }
        
    }
}
