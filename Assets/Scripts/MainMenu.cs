using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    dbController database;

    GameObject Script;

    public static int selectedSubjectID;

    public GUISkin customSkin1;
    public GUISkin customSkin2;
    public GUISkin customSkin3;

    public Texture2D dropDownImage;

    public static bool activaEnterQuestionScript = false;

    public bool NoSubject;

    private Rect windowRect = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, Screen.height / 8);
    private Rect changeSubjectRect = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, Screen.height / 7);

    public float offset;

    public float topBannerWidth;
    public float topBannerHeight;

    public float buttonSizeWidth;
    public float buttonSizeHeight;

    public float buttonPos1;
    public float buttonPos2;
    public float buttonPos3;
    public float buttonPos4;
    public float buttonPos5;

    public float bottomBannerWidth;
    public float bottomBannerHeight;
    public float bottomBannerPos;

    private Rect _currentSubjectPos;
    public static bool _subjectChosen = false;
    private bool _changeSubjectName = false;
    private Rect _subjectWindow;
    private Rect _subjectButtonPos;

    private Rect _deleteCurrentSubjectButtonPos;

    private Rect _newSubjectPos;
    private Rect _inPutArea;
    private Rect _addNewSubjectButtonPos;

    private string Subject, _newSubject, changeSubjectName = "", currentSubject = "";
    private bool executeOnce;

    private Vector2 scrollViewVector = Vector2.zero;
    public Rect dropDownRect = new Rect(Screen.width/3, 0f, 50f, 50f);
    public static List<string> list = new List<string>();

    int indexNumber = 0;
    bool show = false;

    void Start()
    {
        database = Camera.main.GetComponent<dbController>();

        list = database.getSubjects();
        if (list.Count > 0)
            Subject = list[0];
    }

    void Awake()
    {
        _newSubject = "";
        Subject = "";

        executeOnce = false;

        _subjectWindow = new Rect(Screen.width / 3, Screen.height / 10, Screen.width / 3, Screen.height / 2);
        _subjectButtonPos = new Rect(_subjectWindow.x / 10, _subjectWindow.height / 5, _subjectWindow.width / 3, _subjectWindow.height / 15);
        _currentSubjectPos = new Rect(_subjectButtonPos.x, _subjectButtonPos.y - _subjectButtonPos.height, _subjectButtonPos.width, _subjectButtonPos.height);

    }

    void Update()
    {
        Screen.fullScreen = true;
        _subjectWindow = new Rect(Screen.width / 3, Screen.height / 4, Screen.width / 3, Screen.height / 2);
        _subjectButtonPos = new Rect(_subjectWindow.x / 10, _subjectWindow.height / 5, _subjectWindow.width / 2, _subjectWindow.height / 15);

        _currentSubjectPos = new Rect(_subjectButtonPos.x, _subjectButtonPos.y - _subjectButtonPos.height, Screen.width, _subjectButtonPos.height);
        _deleteCurrentSubjectButtonPos = new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 1.2f), _subjectButtonPos.y, _subjectButtonPos.width * 0.5f, _subjectButtonPos.height * 2f);

        _newSubjectPos = new Rect(_subjectButtonPos.x, _subjectButtonPos.y + (_subjectButtonPos.height * 5), _subjectButtonPos.width, _subjectButtonPos.height);
        _inPutArea = new Rect(_newSubjectPos.x, _newSubjectPos.y + _newSubjectPos.height, _newSubjectPos.width, _newSubjectPos.height);
        _addNewSubjectButtonPos = new Rect(_inPutArea.x + (_inPutArea.width * 1.2f), _inPutArea.y, _inPutArea.width * 0.5f, _inPutArea.height);

        offset = Screen.width/3;
        topBannerHeight = Screen.height / 3;
        topBannerWidth = offset;

        buttonSizeHeight = Screen.height / 12;
        buttonSizeWidth = offset;

        buttonPos1 = topBannerHeight + buttonSizeHeight* 2;
        buttonPos2 = topBannerHeight + buttonSizeHeight * 3;
        buttonPos3 = topBannerHeight + (buttonSizeHeight * 4);
        buttonPos4 = topBannerHeight + (buttonSizeHeight * 5);
        buttonPos5 = topBannerHeight + (buttonSizeHeight * 6);

        bottomBannerHeight = Screen.height;
        bottomBannerWidth = offset;
        bottomBannerPos = topBannerHeight + (buttonSizeHeight * 5);
    }
    void OnGUI()
    {
        Screen.fullScreen = true;
        GUI.skin = customSkin2;
        if(NoSubject)
            GUI.Window(1, windowRect, ShowPopup, "Voeg een onderwerp toe a.u.b.");
        if (!_subjectChosen)
        {
            GUI.skin = customSkin2;
            if(!NoSubject)
                GUI.Window(0, _subjectWindow, chooseSubject, "Kies onderwerp");
        }
        else
        {
            GUI.Box(new Rect(offset, buttonSizeHeight, topBannerWidth, topBannerHeight), "");

            GUI.skin = customSkin2;

            GUIStyle g = new GUIStyle(GUI.skin.label);
            g.fontSize = 18;
            g.normal.textColor = new Color(1, 1, 1);
            GUI.Label(new Rect(offset, buttonPos1 - buttonSizeHeight / 2, buttonSizeWidth, buttonSizeHeight / 2), "Huidige onderwerp: " + currentSubject, g);

            if (GUI.Button(new Rect(offset, buttonPos1, buttonSizeWidth, buttonSizeHeight), "Meerkeuze vragen invoeren"))
            {
                Application.LoadLevel("EnterQuestions");
            }

            if (GUI.Button(new Rect(offset, buttonPos2, buttonSizeWidth, buttonSizeHeight), "Foto's bewerken"))
            {
                Application.LoadLevel("ImageOverview");
            }
            if (GUI.Button(new Rect(offset, buttonPos3, buttonSizeWidth, buttonSizeHeight), "Onderwerpnaam aanpassen"))
            {
                _changeSubjectName = true;
            }
            if (GUI.Button(new Rect(offset, buttonPos4, buttonSizeWidth, buttonSizeHeight), "Onderwerp kiezen"))
            {
                _subjectChosen = false;
                Application.LoadLevel("GUI");
            }
            if (GUI.Button(new Rect(offset, buttonPos5, buttonSizeWidth, buttonSizeHeight), "Exit"))
            {
                Application.Quit();
            }

            GUI.skin = customSkin3;
            GUI.Box(new Rect(0, Screen.height - (Screen.height / 20), bottomBannerWidth, buttonSizeHeight), "Ordina the Game 1.0 made by Hogeschool Utrecht");
        }

        if (_changeSubjectName) {
            GUI.skin = customSkin2;
            GUI.Window(3, changeSubjectRect, ChangeSubjectName, "Onderwerpnaam aanpaasen");
        }
    }

    private void chooseSubject(int windowID)
    {
        if (!NoSubject)
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
        
            // Button with the current subject displayed on it
            //
            // If the database contains more than zero subjects
            // a boolean called show will be set to true
            // and a drop down menu will be displayed with all the subject
            // in the database
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
                    GUI.Label(new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 0.01f), _subjectButtonPos.y + (_subjectButtonPos.height * 0.01f), _subjectButtonPos.width, _subjectButtonPos.height), list[indexNumber]);
                else
                    GUI.Label(new Rect(_subjectButtonPos.x + (_subjectButtonPos.width * 0.01f), _subjectButtonPos.y + (_subjectButtonPos.height * 0.01f), _subjectButtonPos.width, _subjectButtonPos.height), "");
            }

            // Button to remove a subjet from the database
            //
            // If this button is pressed the selected subject will be deleted from
            // the database and the list with all the subjects will be updated automatically
            if (GUI.Button(_deleteCurrentSubjectButtonPos, "Huidig onderwerp verwijderen"))
            {
                if (list.Count > 0)
                {
                    database.deleteSubject(list[indexNumber]);
                    list.Remove(list[indexNumber]);
                    if (list.Count != 0)
                    {
                        if (list.Count == indexNumber)
                            Subject = "Huidig onderwerp: " + list[indexNumber - 1];
                        else
                            Subject = "Huidig onderwerp: " + list[indexNumber];
                    }
                    else
                    {
                        Subject = "Huidige onderwerp: ";
                    }
                    if (list.Count == indexNumber)
                        indexNumber--;
                }
            }

            // Label with some text on position rect _newSubjectPos
            GUI.Label(_newSubjectPos, "Nieuw onderwerp toevoegen:");

            // Textarea for adding new subject on position rect _inputArea
            //
            // The text will be saved in variable called _newSubject which is a string variable
            _newSubject = GUI.TextArea(_inPutArea, _newSubject);

            // Button for adding new subject
            //
            // If this button is pressed the new subject will be add to the database
            // and the list with subjects will be updated  with the new subject
             if (GUI.Button(_addNewSubjectButtonPos, "Voeg toe"))
            {
                //list.Add(_newSubject);
                if (_newSubject != "")
                {
                    bool alreadyExist = false;
                    for (int i = 0; i < list.Count; i++ )
                    {
                        if (_newSubject == list[i])
                        {
                            alreadyExist = true;
                            break;
                        }
                    }
                    if (!alreadyExist)
                    {
                        database.insertSubject(_newSubject);
                        list.Add(_newSubject);
                        indexNumber = list.Count - 1;
                        Subject = "Huidig onderwerp: " + list[indexNumber];
                        _newSubject = "";
                    }    
                }
                show = false;
            }

            // Checks if button "Doorgaan" is pressed. 
            //
            // If this button is pressed a bool will be set to true and the main menu window will be openend
            if (GUI.Button(new Rect(_subjectWindow.width / 3, _subjectWindow.height - (_subjectWindow.height /5f), _subjectWindow.width / 3, _subjectWindow.height / 10), "Doorgaan"))
            {
                if (list.Count > 0)
                {
                    _subjectChosen = true;
                    NoSubject = false;
                    selectedSubjectID = database.getSubjectID(list[indexNumber]);
                    currentSubject = database.getSubject(selectedSubjectID);
                    changeSubjectName = currentSubject;
                }
                else
                {
                    NoSubject = true;
                } 
            }   
            }
    }

    private void ChangeSubjectName(int windowID) {
        GUI.FocusWindow(3);

        changeSubjectName = GUI.TextArea(new Rect(20, 20, windowRect.width - 40, 20), changeSubjectName);

        if (GUI.Button(new Rect(windowRect.width / 3, windowRect.height - (windowRect.height / 3), windowRect.width / 3, windowRect.height / 3), "Opslaan")) {
            _changeSubjectName = false;
            currentSubject = changeSubjectName;
        }
        //GUI.FocusWindow(0);
        //GUI.DragWindow();
    }

    private void ShowPopup(int windowID)
    {
        GUI.FocusWindow(1);
        if (GUI.Button(new Rect(windowRect.width / 3, windowRect.height - (windowRect.height / 2), windowRect.width / 3, windowRect.height / 3), "Doorgaan"))
        {
            NoSubject = false;
        }
        //GUI.FocusWindow(0);
        //GUI.DragWindow();
    }
}
