using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnterQuestions : MonoBehaviour {

    public Texture2D texture;
    public Texture2D dropDownImage;
    public GUISkin CustomSkin;

    dbController database;

    public bool emptyTextArea, checkBox, popupWindowOpened, succesFullySend, changed, succesFullyUpdated = false;


    GUIContent checkBox1 = new GUIContent();
    GUIContent checkBox2 = new GUIContent();
    GUIContent checkBox3 = new GUIContent();

    Rect checkBoxRect1, checkBoxRect2, checkBoxRect3, _QuestionsWindow, _QuestionsListPos;

    bool answer1IsRight, answer2IsRight, answer3IsRight, oldAnswer1IsRight, oldAnswer2IsRight, oldAnswer3IsRight;

    public string Question, Answer1, Answer2, Answer3, Subject, oldQuestion, oldAnswer1, oldAnswer2, oldAnswer3;

    public float offset, checkBoxAlignLeft, checkBoxHeight;
    public float checkBoxWidth, checkBox1Pos, checkBox2Pos, checkBox3Pos;
    public float questionAreaPos, questionWidth, questionHeight;
    public float topicAreaPos, topicWidth, topicHeight;
    public float answer1AreaPos, answer2AreaPos, answer3AreaPos;
    public float labelAlignLeft, labelWidth, labelHeight, answerAlignLeft, answersWidth, answerHeight;
    public float backAlignLeft, backButtonWidth, backButtonHeight;
    public float submitAlignLeft, submitButtonWidth, submitButtonHeight;

    private Rect windowRect = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 2.5f, Screen.height / 8);


    private Vector2 scrollViewVector = Vector2.zero;
    public Rect dropDownRect;
    public static List<string> list = new List<string>();

    private List<string> _questionsPerSubject;
    private string _selectedSubject;

    int indexNumber;
    bool show = false;

    void Start()
    {
        database = Camera.main.GetComponent<dbController>();

        list = database.getSubjects();
        if (list.Count >= 0)
            Subject = database.getSubject(MainMenu.selectedSubjectID);
    }

    void Awake()
    {
        Question = "";
        Answer1 = "";
        Answer2 = "";
        Answer3 = "";
    }

    void Update()
    {
        _QuestionsWindow = new Rect(Screen.width / 3, Screen.height / 10, Screen.width / 3, Screen.height / 2);
        _QuestionsListPos = new Rect(_QuestionsWindow.x / 10, _QuestionsWindow.height / 5, _QuestionsWindow.width / 2, _QuestionsWindow.height / 15);

        _questionsPerSubject = database.getQuestions(MainMenu.selectedSubjectID);

        offset = Screen.width / 3;
        checkBoxAlignLeft = Screen.width - (Screen.width / 10);
        checkBoxHeight = 30f;
        checkBoxWidth = 30f;
        checkBox1Pos = offset + checkBoxHeight;
        checkBox2Pos = offset + (checkBoxHeight * 2);
        checkBox3Pos = offset + (checkBoxHeight * 3);

        labelAlignLeft = 10f;
        labelWidth = Screen.width / 6;
        labelHeight = checkBoxHeight;

        questionAreaPos = checkBox1Pos - (checkBoxHeight * 2);
        questionWidth = Screen.width / 1.5f;
        questionHeight = 30f;

        topicAreaPos = questionAreaPos - checkBoxHeight;
        topicWidth = Screen.width / 1.5f;
        topicHeight = 30f;

        answerAlignLeft = labelAlignLeft + labelWidth;
        answersWidth = Screen.width / 1.5f;
        answerHeight = 30f;
        answer1AreaPos = checkBox1Pos;
        answer2AreaPos = checkBox2Pos;
        answer3AreaPos = checkBox3Pos;

        checkBoxRect1 = new Rect(checkBoxAlignLeft, checkBox1Pos, checkBoxWidth, checkBoxHeight);
        checkBoxRect2 = new Rect(checkBoxAlignLeft, checkBox2Pos, checkBoxWidth, checkBoxHeight);
        checkBoxRect3 = new Rect(checkBoxAlignLeft, checkBox3Pos, checkBoxWidth, checkBoxHeight);

        backAlignLeft = 10f;
        backButtonWidth = 80f;
        backButtonHeight = 30f;

        submitAlignLeft = checkBoxAlignLeft - checkBoxWidth;
        submitButtonWidth = 80f;
        submitButtonHeight = 30f;

        dropDownRect = new Rect(answerAlignLeft, questionAreaPos - (topicHeight * 6), topicWidth, 30f);
    }


    void OnGUI()
    {
        GUI.skin = CustomSkin;
        _questionsPerSubject = database.getQuestions(MainMenu.selectedSubjectID);
        Debug.Log("Aantal vragen is: " + _questionsPerSubject.Count);
        if (!popupWindowOpened)
        {
            GUI.skin = CustomSkin;
            _ReadQuestionsFromDatabase();
            _CheckSelectedCheckbox();
            _UserInsertQuestions();
          
            if (GUI.Button(new Rect(submitAlignLeft, checkBox3Pos + (submitButtonHeight * 2), submitButtonWidth, submitButtonHeight), "Submit"))
            {
                if (Question == "" || Answer1 == "" || Answer2 == "" || Answer3 == "")
                {
                    emptyTextArea = true;
                }
                else if ((answer1IsRight || answer2IsRight || answer3IsRight) == false)
                    checkBox = true;
                else if (!changed)
                {
                    succesFullySend = true;
                }
                else if (changed)
                    succesFullyUpdated = true;
            }
            if (GUI.Button(new Rect(backAlignLeft, checkBox3Pos + (backButtonHeight * 2), backButtonWidth, backButtonHeight), "Back"))
            {
                MainMenu._subjectChosen = true;
                Application.LoadLevel("GUI");
            }
        }

        if (emptyTextArea)
        {
            windowRect = GUI.Window(0, windowRect, ShowGui, "Vul alle velden in a.u.b.");
            popupWindowOpened = true;
        } else if(checkBox)
        {
            windowRect = GUI.Window(0, windowRect, ShowGui, "Zet een vinkje bij het correcte antwoord");
            popupWindowOpened = true;
        } 
        if (succesFullySend)
        {
            popupWindowOpened = true;
            windowRect = GUI.Window(1, windowRect, ShowSuccesFullySend, "Gelukt. De vraag staat nu in de database");
        }
        if(succesFullyUpdated)
        {
            popupWindowOpened = true;
            windowRect = GUI.Window(2, windowRect, ShowSuccesFullyUpdated, "Gelukt. De geupdate vraag staat nu in de database");
        }
    }

    private void ShowSuccesFullySend(int windowID)
    {
        if (GUI.Button(new Rect(windowRect.width / 3, windowRect.height - (windowRect.height / 2), windowRect.width / 3, windowRect.height / 3), "Doorgaan"))
        {
            emptyTextArea = checkBox = popupWindowOpened = succesFullySend = false;

            database.insertQuestion(Question, database.getSubjectID(Subject)); //TODO - De string moet ingevuld worden met de string!
            database.insertAnswer(Answer1, database.getQuestionID(Question), answer1IsRight); //TODO - Een functie maken voor het getten van een questionID aan de hand van de ingevulde question string!
            database.insertAnswer(Answer2, database.getQuestionID(Question), answer2IsRight); //TODO - Een functie maken voor het getten van een questionID aan de hand van de ingevulde question string!
            database.insertAnswer(Answer3, database.getQuestionID(Question), answer3IsRight); //TODO - Een functie maken voor het getten van een questionID aan de hand van de ingevulde question string!

            Application.LoadLevel("EnterQuestions");

        }
        GUI.FocusWindow(1);
        GUI.DragWindow();
    }

    private void ShowSuccesFullyUpdated(int windowID)
    {
        if (GUI.Button(new Rect(windowRect.width / 3, windowRect.height - (windowRect.height / 2), windowRect.width / 3, windowRect.height / 3), "Doorgaan"))
        {
            emptyTextArea = checkBox = popupWindowOpened = succesFullyUpdated = false;
            Debug.Log("Vraag is nu: " + Question);

            List<int> lint = database.getAnswerIDs(oldQuestion);

            database.updateQuestion(database.getQuestionID(oldQuestion), Question);
            database.updateAnswer(lint[0], Answer1, answer1IsRight);
            database.updateAnswer(lint[1], Answer2, answer2IsRight);
            database.updateAnswer(lint[2], Answer3, answer3IsRight);
            //database.insertQuestion(Question, database.getSubjectID(Subject)); //TODO - De string moet ingevuld worden met de string!
            //database.insertAnswer(Answer1, database.getQuestionID(Question), answer1IsRight); //TODO - Een functie maken voor het getten van een questionID aan de hand van de ingevulde question string!
            //database.insertAnswer(Answer2, database.getQuestionID(Question), answer2IsRight); //TODO - Een functie maken voor het getten van een questionID aan de hand van de ingevulde question string!
            //database.insertAnswer(Answer3, database.getQuestionID(Question), answer3IsRight); //TODO - Een functie maken voor het getten van een questionID aan de hand van de ingevulde question string!

            Application.LoadLevel("EnterQuestions");

        }
        GUI.FocusWindow(2);
        GUI.DragWindow();
    }

    private void ShowGui(int windowID)
    {
        if(GUI.Button(new Rect(windowRect.width / 3, windowRect.height - (windowRect.height / 2), windowRect.width / 3, windowRect.height / 3), "Doorgaan"))
        {
            emptyTextArea = checkBox = popupWindowOpened = false;
        }
        GUI.FocusWindow(0);
        GUI.DragWindow();
    }
    private void _CheckSelectedCheckbox()
    {
        if (GUI.Button(checkBoxRect1, checkBox1.image))
        {
            if (answer1IsRight == false)
            {
                checkBox1.image = texture;
                checkBox2.image = checkBox3.image = null;
                answer2IsRight = answer3IsRight = false;
                answer1IsRight = true;
            }
            else
            {
                answer1IsRight = false;
                checkBox1.image = null;
            }
        }
        if (GUI.Button(checkBoxRect2, checkBox2.image))
        {
            if (answer2IsRight == false)
            {
                checkBox2.image = texture;
                checkBox1.image = checkBox3.image = null;
                answer1IsRight = answer3IsRight = false;
                answer2IsRight = true;
            }
            else
            {
                answer2IsRight = false;
                checkBox2.image = null;
            }
        }
        if (GUI.Button(checkBoxRect3, checkBox3.image))
        {
            if (answer3IsRight == false)
            {
                checkBox3.image = texture;
                checkBox1.image = checkBox2.image = null;
                answer1IsRight = answer2IsRight = false;
                answer3IsRight = true;
            }
            else
            {
                answer3IsRight = false;
                checkBox3.image = null;
            }
        }
    }

    private void _CheckSelectedCheckbox(bool changed)
    {
        if(answer1IsRight)
        {
            checkBox1.image = texture;
            checkBox2.image = checkBox3.image = null;
        }
        else if (answer2IsRight)
        {
            checkBox2.image = texture;
            checkBox1.image = checkBox3.image = null;
        }
        else if (answer3IsRight)
        {
            checkBox3.image = texture;
            checkBox1.image = checkBox2.image = null;
        }
    }

    private void _ReadQuestionsFromDatabase()
    {
        if (GUI.Button(_QuestionsListPos, ""))
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
        GUI.Label(new Rect(_QuestionsListPos.x + (_QuestionsListPos.width * 0.92f), _QuestionsListPos.y, _QuestionsListPos.width * 0.2f, _QuestionsListPos.height * 0.9f), dropDownImage);

        if (show)
        {
            scrollViewVector = GUI.BeginScrollView(new Rect(_QuestionsListPos.x, _QuestionsListPos.y + _QuestionsListPos.height, _QuestionsListPos.width, _QuestionsListPos.height * 3), scrollViewVector, new Rect(0, 0, 0, Mathf.Max(_QuestionsListPos.height, ((_questionsPerSubject.Count) * _QuestionsListPos.height))));
            if (_questionsPerSubject.Count == 0)
                show = false;
            Debug.Log(_questionsPerSubject.Count);
            for (int index = 0; index < _questionsPerSubject.Count; index++)
            {
                if (GUI.Button(new Rect(0, (index * _QuestionsListPos.height), _QuestionsListPos.width, _QuestionsListPos.height), ""))
                {
                    show = false;
                    indexNumber = index;
                    _selectedSubject = _questionsPerSubject[index];
                    changeQuestion(_selectedSubject);
                }

                GUI.Label(new Rect(10f, (index * _QuestionsListPos.height + (_QuestionsListPos.height * 0.01f)), _QuestionsListPos.width, _QuestionsListPos.height), _questionsPerSubject[index]);

            }

            GUI.EndScrollView();
        }
    }

    private void changeQuestion(string selectedQuestion)
    {
        changed = true;
        Question = database.getQuestion(database.getQuestionID(selectedQuestion));

        List<int> answers = database.getAnswerIDs(selectedQuestion);

        Answer1 = database.getAnswer(answers[0]);
        Answer2 = database.getAnswer(answers[1]);
        Answer3 = database.getAnswer(answers[2]);

        answer1IsRight = database.getAnswerCorrect(Answer1);
        answer2IsRight = database.getAnswerCorrect(Answer2);
        answer3IsRight = database.getAnswerCorrect(Answer3);

        oldQuestion = Question;
        oldAnswer1 = Answer1;
        oldAnswer2 = Answer2;
        oldAnswer3 = Answer3;

        oldAnswer1IsRight = answer1IsRight;
        oldAnswer2IsRight = answer2IsRight;
        oldAnswer3IsRight = answer3IsRight;

        _CheckSelectedCheckbox(changed);
        _UserInsertQuestions();

    }

    private void _UserInsertQuestions()
    {
        GUI.Label(new Rect(labelAlignLeft, checkBox1Pos - (checkBoxHeight * 2), labelWidth, labelHeight), "Vraag");
        Question = GUI.TextArea(new Rect(answerAlignLeft, questionAreaPos, questionWidth, questionHeight), Question);

        GUI.Label(new Rect(labelAlignLeft, checkBox1Pos, labelWidth, labelHeight), "Antwoord A");
        Answer1 = GUI.TextArea(new Rect(answerAlignLeft, answer1AreaPos, answersWidth, answerHeight), Answer1);

        GUI.Label(new Rect(labelAlignLeft, checkBox2Pos, labelWidth, labelHeight), "Antwoord B");
        Answer2 = GUI.TextArea(new Rect(answerAlignLeft, answer2AreaPos, answersWidth, answerHeight), Answer2);

        GUI.Label(new Rect(labelAlignLeft, checkBox3Pos, labelWidth, labelHeight), "Antwoord C");
        Answer3 = GUI.TextArea(new Rect(answerAlignLeft, answer3AreaPos, answersWidth, answerHeight), Answer3);

    }
}
