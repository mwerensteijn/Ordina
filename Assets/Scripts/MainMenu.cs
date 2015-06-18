using UnityEngine;
using System.Collections;


public class MainMenu : MonoBehaviour
{

    public GUISkin customSkin1;
    public GUISkin customSkin2;
    public GUISkin customSkin3;

    public float offset;

    public float topBannerWidth;
    public float topBannerHeight;

    public float buttonSizeWidth;
    public float buttonSizeHeight;

    public float buttonPos1;
    public float buttonPos2;
    public float buttonPos3;

    public float bottomBannerWidth;
    public float bottomBannerHeight;
    public float bottomBannerPos;

    void Update()
    {
        offset = Screen.width/3;
        topBannerHeight = Screen.height / 3;
        topBannerWidth = offset;

        buttonSizeHeight = Screen.height / 10;
        buttonSizeWidth = offset;

        buttonPos1 = topBannerHeight + buttonSizeHeight* 2;
        buttonPos2 = topBannerHeight + buttonSizeHeight * 3;
        buttonPos3 = topBannerHeight + (buttonSizeHeight * 4);

        bottomBannerHeight = Screen.height;
        bottomBannerWidth = offset;
        bottomBannerPos = topBannerHeight + (buttonSizeHeight * 5);
    }
    void OnGUI()
    {
        GUI.skin = customSkin1;

        GUI.Box(new Rect(offset, buttonSizeHeight, topBannerWidth, topBannerHeight), "");

        GUI.skin = customSkin2;

        if (GUI.Button(new Rect(offset, buttonPos1, buttonSizeWidth, buttonSizeHeight), "Meerkeuze vragen invoeren"))
        {
            Debug.Log("Meerkeuze button ingedrukt");
            Application.LoadLevel("EnterQuestions");
        }

        if (GUI.Button(new Rect(offset, buttonPos2, buttonSizeWidth, buttonSizeHeight), "Foto's bewerken"))
        {
            Debug.Log("Foto's knippen button ingedrukt");
            Application.LoadLevel("CMS");
        }

        if (GUI.Button(new Rect(offset, buttonPos3, buttonSizeWidth, buttonSizeHeight), "Exit"))
        {
            Application.Quit();
        }

        GUI.skin = customSkin3;

        //GUI.Box(new Rect(offset, bottomBannerPos, bottomBannerWidth, buttonSizeHeight), "Made by MJ");
        GUI.Box(new Rect(0, Screen.height - (Screen.height / 20), bottomBannerWidth, buttonSizeHeight), "Ordina the Game 1.0 made by MJ");
    }
    /*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
     */
}
