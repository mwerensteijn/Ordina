using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnterQuestions : MonoBehaviour {

    public Texture2D texture;
    public Texture2D dropDownImage;
    public GUISkin CustomSkin2;

    dbController database;

    private string _submitString;
    private string _question, _answer1, _answer2, _answer3, _oldQuestion, _oldAnswer1, _oldAnswer2, _oldAnswer3;
    private string _subject, _selectedSubject;

    private bool _changeExistingQuestion, _emptyTextArea, _checkBox, _popupWindowOpened, _succesFullySend, _succesFullyUpdated, _show = false;
    private bool _answer1IsRight, _answer2IsRight, _answer3IsRight, _oldAnswer1IsRight, _oldAnswer2IsRight, _oldAnswer3IsRight;

    private float _labelAlignLeft, _labelWidth, _labelHeight;

    private Rect _newButtonPos, _changeButtonPos;
    private Rect _changeQuestionLabelPos, _changeQuestionButtonPos;
    private Rect _questionPos, _answer1Pos, _answer2Pos, _answer3Pos;
    private Rect _checkBox1Pos, _checkBox2Pos, _checkBox3Pos;
    private Rect _submitButtonPos, _backButtonPos;
    private Rect _boxRect, _QuestionsWindow;
    private Rect _dropDownRect;

    public int ScreenShotInt2 = 1;

    GUIContent checkBox1 = new GUIContent();
    GUIContent checkBox2 = new GUIContent();
    GUIContent checkBox3 = new GUIContent();

    private Rect _windowRect = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3f, Screen.height / 8);

    private Vector2 scrollViewVector = Vector2.zero;

    public static List<string> list = new List<string>();

    private List<string> _questionsPerSubject;

    private int indexNumber;

    //! \brief Start is called on the frame when a script is enabled.
    //! Initialize the database and assign subjects to a list
    //! \return void
    void Start()
    {
        database = Camera.main.GetComponent<dbController>();

        list = database.getSubjects();
        if (list.Count >= 0)
            _subject = database.getSubject(MainMenu.selectedSubjectID);

        _question = "";
        _answer1 = "";
        _answer2 = "";
        _answer3 = "";
        _submitString = "";
        _selectedSubject = "";
    }

    //! \brief Update is called every frame.
    //! Since the GUI is resizeable every frame some rects need to be 
    //! recalculated
    //! \return void
    void Update()
    {
        _QuestionsWindow = new Rect(Screen.width / 4, Screen.height / 10, Screen.width / 2, Screen.height / 2);
        
        _questionsPerSubject = database.getQuestions(MainMenu.selectedSubjectID);

        _changeQuestionLabelPos = new Rect(_QuestionsWindow.width / 8f, 0 + (_QuestionsWindow.height * 0.25f), _changeButtonPos.width, _changeButtonPos.height);
        _changeQuestionButtonPos = new Rect(_changeQuestionLabelPos.x + (_QuestionsWindow.width / 5), _changeQuestionLabelPos.y, _changeQuestionLabelPos.width * 2f, _changeQuestionLabelPos.height);

        _changeButtonPos = new Rect(_changeQuestionLabelPos.x, 0 + (_QuestionsWindow.height / 10), _QuestionsWindow.width / 5, _QuestionsWindow.height / 12f);
        _newButtonPos = new Rect(_changeButtonPos.x + (_changeButtonPos.width * 3f), _changeButtonPos.y, _changeButtonPos.width, _changeButtonPos.height);

        _labelAlignLeft = 10f;
        _labelWidth = _changeQuestionLabelPos.width;
        _labelHeight = _changeQuestionLabelPos.height;

        _questionPos = new Rect(_changeQuestionLabelPos.x, _changeQuestionLabelPos.y + (_labelHeight * 5f), _labelWidth, _labelHeight);
        _answer1Pos = new Rect(_questionPos.x, _questionPos.y + (_questionPos.height * 2f), _questionPos.width, _questionPos.height);
        _answer2Pos = new Rect(_questionPos.x, _answer1Pos.y + _questionPos.height, _questionPos.width, _questionPos.height);
        _answer3Pos = new Rect(_questionPos.x, _answer2Pos.y + _questionPos.height, _questionPos.width, _questionPos.height);

        _backButtonPos = new Rect(0 + _questionPos.x, _answer3Pos.y + (_answer3Pos.height * 2f), _newButtonPos.width, _newButtonPos.height);
        _submitButtonPos = new Rect(_backButtonPos.x + (_backButtonPos.width * 3f), _backButtonPos.y, _backButtonPos.width, _backButtonPos.height);

        _boxRect = new Rect(Screen.width / 4.3f, Screen.height / 6, Screen.width / 1.9f, Screen.height / 1.5f);

        _checkBox1Pos = new Rect(_changeQuestionButtonPos.x + (_changeQuestionButtonPos.width * 1.38f), _answer1Pos.y, _changeQuestionButtonPos.width / 9f, _changeQuestionButtonPos.height);
        _checkBox2Pos = new Rect(_changeQuestionButtonPos.x + (_changeQuestionButtonPos.width * 1.38f), _answer2Pos.y, _changeQuestionButtonPos.width / 9f, _changeQuestionButtonPos.height);
        _checkBox3Pos = new Rect(_changeQuestionButtonPos.x + (_changeQuestionButtonPos.width * 1.38f), _answer3Pos.y, _changeQuestionButtonPos.width / 9f, _changeQuestionButtonPos.height);

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Application.CaptureScreenshot("Screenshot" + ScreenShotInt2 + ".png");
            ScreenShotInt2++;
        }
    }

    //! \brief OnGUI is called for rendering and handling GUI events. 
    //! Creates GUI based on different booleans
    //! \return void
    void OnGUI()
    {
        GUI.skin = CustomSkin2;
        if (!_popupWindowOpened)
        {
            GUI.Window(4, _boxRect, createWindow, "Vragen toevoegen/wijzigen");
        }
  
        if (_emptyTextArea)
        {
            _windowRect = GUI.Window(0, _windowRect, ShowGui, "Vul alle velden in a.u.b.");
            _popupWindowOpened = true;
        } else if (_checkBox)
        {
            _windowRect = GUI.Window(0, _windowRect, ShowGui, "Zet een vinkje bij het correcte antwoord");
            _popupWindowOpened = true;
        }

        if (_succesFullySend)
        {
            _popupWindowOpened = true;
            _windowRect = GUI.Window(1, _windowRect, ShowSuccesFullySend, "Gelukt. De vraag staat nu in de database");
        }
        if (_succesFullyUpdated)
        {
            _popupWindowOpened = true;
            _windowRect = GUI.Window(2, _windowRect, ShowSuccesFullyUpdated, "Gelukt. De geupdate vraag staat nu in de database");
        }
    }

    //! \brief This method will draw the interface to enter questions.
    //! \param windowID. ID number for the window
    //! \return void
    private void createWindow(int windowID)
    {
        GUI.FocusWindow(4);

        _questionsPerSubject = database.getQuestions(MainMenu.selectedSubjectID);

        if (!_popupWindowOpened)
        {
            if (GUI.Button(_changeButtonPos, "Vragen aanpassen"))
            {
                _changeExistingQuestion = true;
                _submitString = "Wijzig vraag";
            }
            if (GUI.Button(_newButtonPos, "Nieuwe vraag toevoegen"))
            {
                _changeExistingQuestion = false;
                _submitString = "Voeg toe";
                _question = "";
                _answer1 = "";
                _answer2 = "";
                _answer3 = "";
                checkBox1.image = null;
                checkBox2.image = null;
                checkBox3.image = null;
            }

            if (_changeExistingQuestion && _questionsPerSubject.Count != 0)
            {
                GUI.Label(_changeQuestionLabelPos, "Selecteer vraag");
                if (GUI.Button(_changeQuestionButtonPos, _selectedSubject))
                {
                    if (!_show)
                    {
                        _show = true;
                    }
                    else
                    {
                        _show = false;
                    }
                }
                // texture dropDownImage is displayed on this label on the position of the subject button
                GUI.Label(new Rect(_changeQuestionButtonPos.x + (_changeQuestionButtonPos.width * 0.88f), _changeQuestionButtonPos.y, _changeQuestionButtonPos.width * 0.2f, _changeQuestionButtonPos.height * 0.9f), dropDownImage);
                if (_show)
                {
                    scrollViewVector = GUI.BeginScrollView(new Rect(_changeQuestionButtonPos.x, _changeQuestionButtonPos.y + _changeQuestionButtonPos.height, _changeQuestionButtonPos.width, _changeQuestionButtonPos.height * 3), scrollViewVector, new Rect(0, 0, 0, Mathf.Max(_changeQuestionButtonPos.height, ((_questionsPerSubject.Count ) * _changeQuestionButtonPos.height))));
                    if (_questionsPerSubject.Count == 0)
                        _show = false;
                    Debug.Log(_questionsPerSubject.Count);
                    for (int index = 0; index < _questionsPerSubject.Count; index++)
                    {
                        if (GUI.Button(new Rect(0, (index * _changeQuestionButtonPos.height), _changeQuestionButtonPos.width, _changeQuestionButtonPos.height), ""))
                        {
                            _show = false;
                            indexNumber = index;
                            _selectedSubject = _questionsPerSubject[index];
                            GUI.Button(_changeQuestionButtonPos, _selectedSubject);
                            changeQuestion(_selectedSubject);
                        }
                        GUI.Label(new Rect(10f, (index * _changeQuestionButtonPos.height + (_changeQuestionButtonPos.height * 0.01f)), _changeQuestionButtonPos.width, _changeQuestionButtonPos.height), _questionsPerSubject[index]);
                    }
                    GUI.EndScrollView();
                }

            }
            _UserInsertQuestions();
            _CheckSelectedCheckbox();


            if (GUI.Button(_backButtonPos, "Terug"))
            {
                MainMenu._subjectChosen = true;
                Application.LoadLevel("GUI");
            }
            
            if (GUI.Button(_submitButtonPos, _submitString))
            {
                if (_question == "" || _answer1 == "" || _answer2 == "" || _answer3 == "")
                {
                    _emptyTextArea = true;
                }
                else if ((_answer1IsRight || _answer2IsRight || _answer3IsRight) == false)
                    _checkBox = true;
                else if (!_changeExistingQuestion)
                {
                    _succesFullySend = true;
                }
                else if (_changeExistingQuestion)
                    _succesFullyUpdated = true;
            }
        }
    }

    //! \brief This method will draw the interface if user has pressed Submit button.
    //! This interface has one button and if pressed current scene will be reloaded
    //! \param windowID. ID number for the window
    //! \return void
    private void ShowSuccesFullySend(int windowID)
    {
        if (GUI.Button(new Rect(_windowRect.width / 3, _windowRect.height - (_windowRect.height / 2), _windowRect.width / 3, _windowRect.height / 3), "Doorgaan"))
        {
            _emptyTextArea = _checkBox = _popupWindowOpened = _succesFullySend = false;

            database.insertQuestion(_question, database.getSubjectID(_subject)); //TODO - De string moet ingevuld worden met de string!
            database.insertAnswer(_answer1, database.getQuestionID(_question), _answer1IsRight); //TODO - Een functie maken voor het getten van een questionID aan de hand van de ingevulde question string!
            database.insertAnswer(_answer2, database.getQuestionID(_question), _answer2IsRight); //TODO - Een functie maken voor het getten van een questionID aan de hand van de ingevulde question string!
            database.insertAnswer(_answer3, database.getQuestionID(_question), _answer3IsRight); //TODO - Een functie maken voor het getten van een questionID aan de hand van de ingevulde question string!

            Application.LoadLevel("EnterQuestions");

        }
        GUI.FocusWindow(1);
        GUI.DragWindow();
    }

    //! \brief This method will draw the interface if user has pressed Change button
    //! This interface has one button and if pressed current scene will be reloaded
    //! \param windowID. ID number for the window
    //! \return void
    private void ShowSuccesFullyUpdated(int windowID)
    {
        if (GUI.Button(new Rect(_windowRect.width / 3, _windowRect.height - (_windowRect.height / 2), _windowRect.width / 3, _windowRect.height / 3), "Doorgaan"))
        {
            _emptyTextArea = _checkBox = _popupWindowOpened = _succesFullyUpdated = false;
            Debug.Log("Vraag is nu: " + _question);

            List<int> lint = database.getAnswerIDs(_oldQuestion);

            database.updateQuestion(database.getQuestionID(_oldQuestion), _question);
            database.updateAnswer(lint[0], _answer1, _answer1IsRight);
            database.updateAnswer(lint[1], _answer2, _answer2IsRight);
            database.updateAnswer(lint[2], _answer3, _answer3IsRight);

            Application.LoadLevel("EnterQuestions");
        }
        GUI.FocusWindow(2);
        GUI.DragWindow();
    }

    //! \brief This method will draw the interface if user has not filled in all fields required for a question
    //! This interface has one button and if pressed the user will return to previous screen
    //! \param windowID. ID number for the window
    //! \return void
    private void ShowGui(int windowID)
    {
        if (GUI.Button(new Rect(_windowRect.width / 3, _windowRect.height - (_windowRect.height / 2), _windowRect.width / 3, _windowRect.height / 3), "Doorgaan"))
        {
            _emptyTextArea = _checkBox = _popupWindowOpened = false;
        }
        GUI.FocusWindow(0);
        GUI.DragWindow();
    }

    //! \brief This method will set a texture on one of the three buttons
    //! \return void
    private void _CheckSelectedCheckbox()
    {
        if (GUI.Button(_checkBox1Pos, checkBox1.image))
        {
            if (_answer1IsRight == false)
            {
                checkBox1.image = texture;
                checkBox2.image = checkBox3.image = null;
                _answer2IsRight = _answer3IsRight = false;
                _answer1IsRight = true;
            }
            else
            {
                _answer1IsRight = false;
                checkBox1.image = null;
            }
        }
        if (GUI.Button(_checkBox2Pos, checkBox2.image))
        {
            if (_answer2IsRight == false)
            {
                checkBox2.image = texture;
                checkBox1.image = checkBox3.image = null;
                _answer1IsRight = _answer3IsRight = false;
                _answer2IsRight = true;
            }
            else
            {
                _answer2IsRight = false;
                checkBox2.image = null;
            }
        }
        if (GUI.Button(_checkBox3Pos, checkBox3.image))
        {
            if (_answer3IsRight == false)
            {
                checkBox3.image = texture;
                checkBox1.image = checkBox2.image = null;
                _answer1IsRight = _answer2IsRight = false;
                _answer3IsRight = true;
            }
            else
            {
                _answer3IsRight = false;
                checkBox3.image = null;
            }
        }
    }

    //! \brief This method will set a texture on one of the three buttons
    //! \param changed
    //! \return void
    private void _CheckSelectedCheckbox(bool changed)
    {
        if (_answer1IsRight)
        {
            checkBox1.image = texture;
            checkBox2.image = checkBox3.image = null;
        }
        else if (_answer2IsRight)
        {
            checkBox2.image = texture;
            checkBox1.image = checkBox3.image = null;
        }
        else if (_answer3IsRight)
        {
            checkBox3.image = texture;
            checkBox1.image = checkBox2.image = null;
        }
    }

    //! \brief This method get all the data from the selected question.
    //! \param selectedQuestion. This string contains the question the user want to change
    //! \return void
    private void changeQuestion(string selectedQuestion)
    {
        _changeExistingQuestion = true;
        _question = database.getQuestion(database.getQuestionID(selectedQuestion));

        List<int> answers = database.getAnswerIDs(selectedQuestion);

        _answer1 = database.getAnswer(answers[0]);
        _answer2 = database.getAnswer(answers[1]);
        _answer3 = database.getAnswer(answers[2]);

        _answer1IsRight = database.getAnswerCorrect(_answer1);
        _answer2IsRight = database.getAnswerCorrect(_answer2);
        _answer3IsRight = database.getAnswerCorrect(_answer3);

        _oldQuestion = _question;
        _oldAnswer1 = _answer1;
        _oldAnswer2 = _answer2;
        _oldAnswer3 = _answer3;

        _oldAnswer1IsRight = _answer1IsRight;
        _oldAnswer2IsRight = _answer2IsRight;
        _oldAnswer3IsRight = _answer3IsRight;

        _CheckSelectedCheckbox(_changeExistingQuestion);
        _UserInsertQuestions();

    }

    //! \brief This method takes care of the labels and text area for the questions
    //! \return void
    private void _UserInsertQuestions()
    {
        GUI.Label(_questionPos, "Vraag");
        _question = GUI.TextArea(new Rect(_questionPos.x + _questionPos.width, _questionPos.y, _changeQuestionButtonPos.width, _changeQuestionButtonPos.height), _question);

        GUI.Label(_answer1Pos, "Antwoord A");
        _answer1 = GUI.TextArea(new Rect(_questionPos.x + _questionPos.width, _answer1Pos.y, _changeQuestionButtonPos.width, _changeQuestionButtonPos.height), _answer1);

        GUI.Label(_answer2Pos, "Antwoord B");
        _answer2 = GUI.TextArea(new Rect(_questionPos.x + _questionPos.width, _answer2Pos.y, _changeQuestionButtonPos.width, _changeQuestionButtonPos.height), _answer2);

        GUI.Label(_answer3Pos, "Antwoord C");
        _answer3 = GUI.TextArea(new Rect(_questionPos.x + _questionPos.width, _answer3Pos.y, _changeQuestionButtonPos.width, _changeQuestionButtonPos.height), _answer3);

    }
}
