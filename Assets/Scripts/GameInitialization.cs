﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitialization : MonoBehaviour
{
    dbController database;

    public static int selectedSubjectID;
    public static int _userID;
    private string _userName;

    public GUISkin customSkin2;

    public Texture2D dropDownImage;

    public bool NoUserSelected;

    private Rect windowRect = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, Screen.height / 8);

    public float offset;

    public float topBannerWidth;
    public float topBannerHeight;

    public float buttonSizeWidth;
    public float buttonSizeHeight;

    public float buttonPos1;
    public float buttonPos2;
    public float buttonPos3;
    public float buttonPos4;

    public float bottomBannerWidth;
    public float bottomBannerHeight;
    public float bottomBannerPos;

    private Rect _currentSubjectPos;
    private bool _subjectChosen = false;
    private Rect _subjectWindow;
    private Rect _subjectButtonPos;

    private Rect _deleteCurrentSubjectButtonPos;

    private Rect _newSubjectPos;
    private Rect _inPutArea;
    private Rect _addNewSubjectButtonPos;

    private string Subject;
    private bool executeOnce;

    private Vector2 scrollViewVector = Vector2.zero;
    public static List<string> list = new List<string>();

    int indexNumber = 0;
    bool show = false;

    void Awake()
    {
        NoUserSelected = false;

        _userName = "";
        Subject = "";

        executeOnce = false;
        database = Camera.main.GetComponent<dbController>();

        list = database.getSubjects();
        if (list.Count > 0)
            Subject = list[0];

        _subjectWindow = new Rect(Screen.width / 3, Screen.height / 10, Screen.width / 3, Screen.height / 2);
        _subjectButtonPos = new Rect(_subjectWindow.x / 10, _subjectWindow.height / 5, _subjectWindow.width / 3, _subjectWindow.height / 15);
        _currentSubjectPos = new Rect(_subjectButtonPos.x, _subjectButtonPos.y - _subjectButtonPos.height, _subjectButtonPos.width, _subjectButtonPos.height);

    }

    void Update()
    {
        Screen.fullScreen = true;
        _subjectWindow = new Rect(Screen.width / 3, Screen.height / 10, Screen.width / 3, Screen.height / 2);
        _subjectButtonPos = new Rect(_subjectWindow.x / 10, _subjectWindow.height / 5, _subjectWindow.width / 2, _subjectWindow.height / 15);

        _currentSubjectPos = new Rect(_subjectButtonPos.x, _subjectButtonPos.y - _subjectButtonPos.height, Screen.width, _subjectButtonPos.height);
        _deleteCurrentSubjectButtonPos = new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 1.2f), _subjectButtonPos.y, _subjectButtonPos.width * 0.5f, _subjectButtonPos.height * 2f);

        _newSubjectPos = new Rect(_subjectButtonPos.x, _subjectButtonPos.y + (_subjectButtonPos.height * 5), _subjectButtonPos.width, _subjectButtonPos.height);
        _inPutArea = new Rect(_newSubjectPos.x, _newSubjectPos.y + _newSubjectPos.height, _newSubjectPos.width, _newSubjectPos.height);
        _addNewSubjectButtonPos = new Rect(_inPutArea.x + (_inPutArea.width * 1.2f), _inPutArea.y, _inPutArea.width * 0.5f, _inPutArea.height);

        offset = Screen.width / 3;
        topBannerHeight = Screen.height / 3;
        topBannerWidth = offset;

        buttonSizeHeight = Screen.height / 10;
        buttonSizeWidth = offset;

        buttonPos1 = topBannerHeight + buttonSizeHeight * 2;
        buttonPos2 = topBannerHeight + buttonSizeHeight * 3;
        buttonPos3 = topBannerHeight + (buttonSizeHeight * 4);
        buttonPos4 = topBannerHeight + (buttonSizeHeight * 5);

        bottomBannerHeight = Screen.height;
        bottomBannerWidth = offset;
        bottomBannerPos = topBannerHeight + (buttonSizeHeight * 5);
    }
    void OnGUI()
    {
        GUI.skin = customSkin2;

        if (NoUserSelected)
            GUI.Window(2, windowRect, ShowPopup, "Vul een gebruikersnaam in a.u.b."); 
        else if (!_subjectChosen)
        {
            GUI.skin = customSkin2;
            GUI.Window(0, _subjectWindow, chooseSubjectAndUser, "Kies een onderwerp en vul een gebruikersnaam in");
        }
    }

    private void chooseSubjectAndUser(int windowID)
    {
        if (!NoUserSelected)
        {
            GUI.FocusWindow(0);
            GUI.skin = customSkin2;
            if (!executeOnce)
            {
                Subject = "Huidig onderwerp: " + Subject;
                executeOnce = true;
            }

            // Label with the current subject displayed as text
            GUI.Label(_currentSubjectPos, Subject);

            if (GUI.Button(_subjectButtonPos, ""))
            {
                if (!show)
                {
                    show = true;
                }
                else
                {
                    show = false;
                }
            }

            // texture dropDownImage is displayed on this label on the position of the subject button
            GUI.Label(new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 0.92f), _subjectButtonPos.y, _subjectButtonPos.width * 0.2f, _subjectButtonPos.height * 0.9f), dropDownImage);

            // This part takes care of the drop down menu
            //
            // A box with a verticall scrollbar will show up and the user can select a subject
            if (show)
            {
                scrollViewVector = GUI.BeginScrollView(new Rect(_subjectButtonPos.x, _subjectButtonPos.y + _subjectButtonPos.height, _subjectButtonPos.width, _subjectButtonPos.height * 3), scrollViewVector, new Rect(0, 0, 0, Mathf.Max(_subjectButtonPos.height, ((list.Count) * _subjectButtonPos.height))));

                if (list.Count == 0)
                    show = false;

                for (int index = 0; index < list.Count; index++)
                {
                    if (GUI.Button(new Rect(0, (index * _subjectButtonPos.height), _subjectButtonPos.width, _subjectButtonPos.height), ""))
                    {
                        show = false;
                        indexNumber = index;
                        Subject = "Huidig onderwerp: " + list[index];
                    }

                    GUI.Label(new Rect(10f, (index * _subjectButtonPos.height), _subjectButtonPos.width, _subjectButtonPos.height), list[index]);

                }

                GUI.EndScrollView();
            }
            else
            {
                if (list.Count != 0)
                    GUI.Label(new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 0.01f), _subjectButtonPos.y, _subjectButtonPos.width, _subjectButtonPos.height), list[indexNumber]);
                else
                    GUI.Label(new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 0.01f), _subjectButtonPos.y, _subjectButtonPos.width, _subjectButtonPos.height), "");
            }

            // Label with some text on position rect _newSubjectPos
            GUI.Label(_newSubjectPos, "Gebruikersnaam:");

            // Textarea for adding new subject on position rect _inputArea
            //
            // The text will be saved in variable called _newSubject which is a string variable
            _userName = GUI.TextArea(_inPutArea, _userName);

            // Checks if button "Doorgaan" is pressed. 
            //
            // If this button is pressed a bool will be set to true and the main menu window will be openend
            if (GUI.Button(new Rect(_subjectWindow.width / 3, _subjectWindow.height - (_subjectWindow.height / 5f), _subjectWindow.width / 3, _subjectWindow.height / 10), "Doorgaan"))
            {
                if (_userName != "")
                {
                    selectedSubjectID = database.getSubjectID(list[indexNumber]);
                    _userID = database.getPlayerID(_userName);
                    Application.LoadLevel("MainMenu");
                }
                else
                    NoUserSelected = true;
            }
        }
    }

    private void ShowPopup(int windowID)
    {
        GUI.FocusWindow(2);
        if (GUI.Button(new Rect(windowRect.width / 3, windowRect.height - (windowRect.height / 2), windowRect.width / 3, windowRect.height / 3), "Doorgaan"))
        {
            NoUserSelected = false;
        }
    }
}