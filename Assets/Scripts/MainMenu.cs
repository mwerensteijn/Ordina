using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    dbController database; 

    public GUISkin customSkin1;
    public GUISkin customSkin2;
    public GUISkin customSkin3;

    public Texture2D dropDownImage;

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

    private Rect _currentSubjectPos;
    private bool _subjectChosen = false;
    private Rect _subjectWindow;
    private Rect _subjectButtonPos;

    private Rect _deleteCurrentSubjectButtonPos;

    private Rect _newSubjectPos;
    private Rect _inPutArea;
    private Rect _addNewSubjectButtonPos;

    private string Subject, _newSubject;
    private bool executeOnce;

    private Vector2 scrollViewVector = Vector2.zero;
    public Rect dropDownRect = new Rect(Screen.width/3, 0f, 50f, 50f);
    public static List<string> list = new List<string>();

    int indexNumber;
    bool show = false;

    void Awake()
    {
        _newSubject = "";

        executeOnce = false;
        database = Camera.main.GetComponent<dbController>();

        list = database.getSubjects();
        if (list.Count >= 0)
            Subject = list[0];

        _subjectWindow = new Rect(Screen.width / 3, Screen.height / 10, Screen.width / 3, Screen.height / 2);
        _subjectButtonPos = new Rect(_subjectWindow.x / 10, _subjectWindow.height / 5, _subjectWindow.width / 3, _subjectWindow.height / 15);
        _currentSubjectPos = new Rect(_subjectButtonPos.x, _subjectButtonPos.y - _subjectButtonPos.height, _subjectButtonPos.width, _subjectButtonPos.height);

    }

    void Update()
    {
        _subjectWindow = new Rect(Screen.width / 3, Screen.height / 10, Screen.width / 3, Screen.height / 2);
        _subjectButtonPos = new Rect(_subjectWindow.x / 10, _subjectWindow.height / 5, _subjectWindow.width / 2, _subjectWindow.height / 15);

        _currentSubjectPos = new Rect(_subjectButtonPos.x, _subjectButtonPos.y - _subjectButtonPos.height, Screen.width, _subjectButtonPos.height);
        _deleteCurrentSubjectButtonPos = new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 1.2f), _subjectButtonPos.y, _subjectButtonPos.width * 0.5f, _subjectButtonPos.height * 2f);

        _newSubjectPos = new Rect(_subjectButtonPos.x, _subjectButtonPos.y + (_subjectButtonPos.height * 5), _subjectButtonPos.width, _subjectButtonPos.height);
        _inPutArea = new Rect(_newSubjectPos.x, _newSubjectPos.y + _newSubjectPos.height, _newSubjectPos.width, _newSubjectPos.height);
        _addNewSubjectButtonPos = new Rect(_inPutArea.x + (_inPutArea.width * 1.2f), _inPutArea.y, _inPutArea.width * 0.5f, _inPutArea.height);

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

        if (!_subjectChosen)
        {
            GUI.skin = customSkin2;
            GUI.Window(0, _subjectWindow, chooseSubject, "Kies onderwerp");
        }
        else
        {
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
            GUI.Box(new Rect(0, Screen.height - (Screen.height / 20), bottomBannerWidth, buttonSizeHeight), "Ordina the Game 1.0 made by MJ");
        }
    }

    private void chooseSubject(int windowID)
    {
        GUI.skin = customSkin2;
        if (!executeOnce)
        {
            Subject = "Huidig onderwerp: " + Subject;
            executeOnce = true;
        }
        GUI.Label(_currentSubjectPos, Subject);
        GUI.Label(new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 0.9f), _subjectButtonPos.y, _subjectButtonPos.width, _subjectButtonPos.height), dropDownImage);
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

        if (show)
        {
            //Debug.Log("SHOWWWW");
            scrollViewVector = GUI.BeginScrollView(new Rect(_subjectButtonPos.x, _subjectButtonPos.y + _subjectButtonPos.height, _subjectButtonPos.width, _subjectButtonPos.height * 3), scrollViewVector, new Rect(0, 0, 0, Mathf.Max(_subjectButtonPos.height, ((list.Count) * _subjectButtonPos.height))));
           
            Debug.Log(list.Count);
            //GUI.Box(new Rect(_subjectButtonPos.x, _subjectButtonPos.y + _subjectButtonPos.height, _subjectButtonPos.width, Mathf.Max(300, ((list.Count - 1) * _subjectButtonPos.height))), "");

            
            for (int index = 0; index < list.Count; index++)
            {

                if (GUI.Button(new Rect(0, (index * _subjectButtonPos.height), _subjectButtonPos.width, _subjectButtonPos.height), ""))
                {
                    show = false;
                    indexNumber = index;
                    Subject = "Huidig onderwerp: " + list[index];
                    Debug.Log(Subject);
                }

                GUI.Label(new Rect(10f, (index * _subjectButtonPos.height), _subjectButtonPos.width, _subjectButtonPos.height), list[index]);

            }
            
            GUI.EndScrollView();
        }
        else
        {
            GUI.Label(new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 0.01f), _subjectButtonPos.y, _subjectButtonPos.width, _subjectButtonPos.height), list[indexNumber]);
        }

        if(GUI.Button(_deleteCurrentSubjectButtonPos, "Huidig onderwerp verwijderen"))
        {
            Debug.Log("VERWIJDER ONDERWERP");
        }

        GUI.Label(_newSubjectPos, "Nieuw onderwerp toevoegen:");

        // Area for adding new subject
        _newSubject = GUI.TextArea(_inPutArea, _newSubject);

        // Button for adding new subject
        //
        // If this button is pressed the new subject will be add to the database
         if (GUI.Button(_addNewSubjectButtonPos, "Voeg toe"))
        {
            //list.Add(_newSubject);
            if (_newSubject != "")
            {
                database.insertSubject(_newSubject);
                list.Add(_newSubject);
                indexNumber = list.Count-1;
                Subject = "Huidig onderwerp: " + list[indexNumber];
            }
        }

        // Checks if button "Doorgaan" is pressed. 
        //
        // If this button is pressed a bool will be set to true and the main menu window will be openend
        if (GUI.Button(new Rect(_subjectWindow.width / 3, _subjectWindow.height - (_subjectWindow.height /5f), _subjectWindow.width / 3, _subjectWindow.height / 10), "Doorgaan"))
        {
            _subjectChosen = true;
        }
    }
}
