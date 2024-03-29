﻿using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.HighScore;
using System.Collections.Generic;

public class WorldMovement : MonoBehaviour, IScore {
	// The states available in this MultipleChoice game.
	public enum State
	{
		Init,
		Idle,
		CheckAnswer,
		ChangeQuestion,
        FinishMiniGame,
        ScoreScreen
	}

    // To receive the chosen subjectID.
    private GameManager gameManager;

    // The game timer
    private DigitalClock gameTimer;
    // The progressbar
    private ProgressBar progressBar;
    // The scorescreen
    public ScoreScreen scoreScreen;
    public Canvas HUD;
    private dbController _dbController;
	// Holds the current state.
	public State currentState;
	// The movement speed of the gaming world.
	public static float movementSpeed = 5f;

    // answerRowFront always is the row with answers in the front of the player
    public AnswerRow answerRowFront;

    // The airplanemovement script
    public AirplaneMovement airplaneMovement;
    private ParticleAnimator airplaneEngine;

	// If a row with answers is not visible anymore for the player, it will respawn at this Z position.
	private float appearPositionZ;
	// Defines at which position a row with answers will disappear.
	//public float disappearPositionZ;

	// The answer, given by the player.
	private Transform givenAnswer;

    private int currentQuestion = 0;

    public TextMesh questionText;
    
    //alle vragen bijhouden
    private List<Question> _questions = new List<Question>();

    private bool minigameFinshed = false;

    [SerializeField]
    private int answerScoreWeigth = 0;
    public int AnswerScoreWeigth { get { return answerScoreWeigth; } set { answerScoreWeigth = value; } }

    private int totalAskedQuestions = 0;
    public int TotalAskedQuestions { get { return totalAskedQuestions; } set { totalAskedQuestions = value; } }

    private int totalCorrectQuestions = 0;
    public int TotalCorrectQuestions { get { return totalCorrectQuestions; } set { totalCorrectQuestions = value; } }

    public bool InitGameStartButtonPressed = false;
    private bool StartGameMovement = false;
	// Initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        answerRowFront = new AnswerRow(GameObject.FindGameObjectWithTag("Answers"));
        _dbController = GetComponentInParent<dbController>();        

        // Set the appear position.
        appearPositionZ = answerRowFront.transform.position.z;
        currentState = WorldMovement.State.Init;
        gameTimer = HUD.GetComponentInChildren<DigitalClock>();
        progressBar = HUD.GetComponentInChildren<ProgressBar>();

		// Start finite state machine.
		StartCoroutine("FSM");
	}
	
	// Update
	void Update () {
		// Cache the x and y position of a row.
        float x = answerRowFront.transform.position.x;
        float y = answerRowFront.transform.position.y;

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.Period)) && !airplaneMovement.disableMovement)
        {
            if (InitGameStartButtonPressed)
            {
                movementSpeed = 40f;
                airplaneMovement.SetSideMovementSpeed(80f);
                airplaneMovement.disableMovement = true;
                airplaneMovement.airplaneEngineParticleBeam.sizeGrow = -0.3f;
            }
            else 
            { 
                InitGameStartButtonPressed = true; 
                StartGameMovement = true; 
            }
        }
        if (StartGameMovement)
        {
            // Move the rows towards the player.
            answerRowFront.transform.position = new Vector3(x, y, answerRowFront.transform.position.z + -movementSpeed * Time.deltaTime);

            // If a row is not visible anymore, respawn it at the appearing position.
            if (answerRowFront.transform.position.z <= airplaneMovement.transform.position.z)
            {
                answerRowFront.transform.position = new Vector3(x, y, appearPositionZ);

                airplaneMovement.disableMovement = false;
                airplaneMovement.airplaneEngineParticleBeam.sizeGrow = -0.9f;
                airplaneMovement.SetSideMovementSpeed(10f);
                movementSpeed = 5f;

                Color a = new Color(1f / 255 * 231, 1f / 255 * 155, 1f / 255 * 19);

                answerRowFront.answerA.color = a;
                answerRowFront.answerB.color = a;
                answerRowFront.answerC.color = a;

                AnswerRow temp = answerRowFront;
                //answerRowFront = answerRowBack;
                //answerRowBack = temp;
                currentState = WorldMovement.State.ChangeQuestion;
            }
        }
	}

    private void DisableAnswering() {
    
    }

    private void EnableAnswering() {
    
    }

	// Finite state machine.
	private IEnumerator FSM() {
		while(true) {
			switch(currentState){ 
			case WorldMovement.State.Init:
				Init ();
				break;
			case WorldMovement.State.Idle:
				break;
			case WorldMovement.State.CheckAnswer:
				CheckAnswer ();
				break;
			case WorldMovement.State.ChangeQuestion:
				ChangeQuestion();
				break;
            case WorldMovement.State.FinishMiniGame:
                StopMovement();
                break;
            case WorldMovement.State.ScoreScreen:
                ShowScoreScreen();
                StopCoroutine("FSM");
                break;
			}
             yield return null;
		}
	}

    private void LoadQuestionsFromFile(string fileLocation)
    {
        // Load all text from file.
        string text = System.IO.File.ReadAllText(fileLocation);
        // Split the string by new lines.
        string[] data = data = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        int amountOfQuestions = data.Length / 4;
        
        for (int i = 0; i < amountOfQuestions; i++)
        {
            Question question = new Question();
            _questions.Add(question);
            _questions[i].question = data[i * 4];
            _questions[i].answers[0] = data[i * 4 + 1];
            _questions[i].answers[1] = data[i * 4 + 2];
            _questions[i].answers[2] = data[i * 4 + 3];
            _questions[i].correctAnswer = _questions[i].answers[0];
            Question.Randomize(_questions[i].answers);
        }
    }

    private void LoadQuestionsFromDB() 
    {
        //TODO
        //onderwerpId = getonderwip of iets..
        int subjectId = _dbController.getSubjectID(gameManager.getSubject());
        List<int> dbvragenIds = _dbController.getQuestionIDs(subjectId, false);

        foreach (int vraagid in dbvragenIds) 
        {
            Question question = new Question();

            string vraagString = _dbController.getQuestion(vraagid);
            question.question = vraagString;
            List<int> dbvraagAntwoordIds = _dbController.getAnswerIDs(vraagid);
            if (dbvraagAntwoordIds.Count >= 3) {
                for (int i = 0; i < 3; i++)
                {
                    string antwoordString = _dbController.getAnswer(dbvraagAntwoordIds[i]);
                    bool correctAnswer = _dbController.getAnswerCorrect(dbvraagAntwoordIds[i]);
                    question.answers[i] = antwoordString;
                    if (correctAnswer) { question.correctAnswer = antwoordString; }
                    
                }
                Question.Randomize(question.answers);
            }
            _questions.Add(question);
        }     
    }

	private void Init() {
		// load questions
        if (_dbController != null) { 
            LoadQuestionsFromDB();
        } else {
            LoadQuestionsFromFile("Assets\\data.txt");
        }

        // Set question and answer text
        questionText.text = _questions[currentQuestion].question;
        answerRowFront.A = _questions[currentQuestion].answers[0];
        answerRowFront.B = _questions[currentQuestion].answers[1];
        answerRowFront.C = _questions[currentQuestion].answers[2];
        answerRowFront.SizePlane();
        answerRowFront.SizeTextMesh();

        questionText.GetComponentInChildren<TextQuadBackGround>().UpdateTextQuadBackGroundSize();

        currentState = WorldMovement.State.Idle;
        progressBar.SetMaxAwnsers(_questions.Count);
	}

    private void CheckAnswer()
    {
        if (givenAnswer.FindChild("Answer").GetComponent<TextMesh>().text.ToString().Trim().Equals(_questions[currentQuestion].correctAnswer.ToString().Trim()))
        {
            givenAnswer.GetComponent<MeshRenderer>().material.color = Color.green;
            totalCorrectQuestions += 1;
        }
        else
        {
            givenAnswer.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        totalAskedQuestions += 1;
        progressBar.UpdateProgressBar(totalAskedQuestions);

        if (_questions.Count == totalAskedQuestions)
        {
            currentState = WorldMovement.State.FinishMiniGame;
            return;
        }

        currentState = WorldMovement.State.Idle;
    }

    private void ChangeQuestion()
    {
        if (currentQuestion < _questions.Count)
        {
            ++currentQuestion;
            questionText.text = _questions[currentQuestion].question;
            answerRowFront.A = _questions[currentQuestion].answers[0];
            answerRowFront.B = _questions[currentQuestion].answers[1];
            answerRowFront.C = _questions[currentQuestion].answers[2];
            answerRowFront.SizePlane();
            answerRowFront.SizeTextMesh();

            questionText.GetComponentInChildren<TextQuadBackGround>().UpdateTextQuadBackGroundSize();

            currentState = WorldMovement.State.Idle;
        }
    }

	// Called by AnswerCollision script.
	public void AnswerCollision(Transform answer) {
        givenAnswer = answer;
		currentState = WorldMovement.State.CheckAnswer;
	}

    public void StopMovement() 
    {
        movementSpeed = 0f;
        airplaneMovement.SetSideMovementSpeed(0f);
        airplaneMovement.disableMovement = true;
        airplaneMovement.airplaneEngineParticleBeam.sizeGrow = -0.9f;
        currentState = WorldMovement.State.ScoreScreen;
    }

    public void ShowScoreScreen() 
    {
        HUD.enabled = false;
        questionText.GetComponent<MeshRenderer>().enabled = false;
        answerRowFront.HideAnswersText();
        int totalScore = CalculateScore();

        //laat score screen zien.
        Vector3 scoreScreenPosition = airplaneMovement.transform.position;
        scoreScreenPosition.z += 1;
        scoreScreen.transform.position = scoreScreenPosition;
        scoreScreen.ShowScoreScreen(totalScore, gameTimer.GetFormatedTime());

        SaveScore(totalScore, gameTimer.GetTotalSeconds());
    }

    public int CalculateScore()
    {
        return AnswerScoreWeigth * TotalCorrectQuestions;   
    }
    public void SaveScore(int totalScore, int totalTimeSeconds)
    {
        try
        {
            _dbController.insertScore(gameManager.getPlayerName(), gameManager.getSubject(), gameManager.getSpelID(), totalScore, totalTimeSeconds, TotalAskedQuestions, TotalCorrectQuestions);
        }
        catch (Exception e) { Debug.Log("foutmelding: " + e.Message);} 
    }
}
