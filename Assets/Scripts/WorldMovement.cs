using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.HighScore;

public class WorldMovement : MonoBehaviour, IScore {
	// The states available in this MultipleChoice game.
	public enum State
	{
		Init,
		Idle,
		CheckAnswer,
		ChangeQuestion,
        ScoreScreen
	}
	
	// Holds the current state.
	public State currentState;
	// The movement speed of the gaming world.
	public static float movementSpeed = 20f;

    // answerRowFront always is the row with answers in the front of the player
    public AnswerRow answerRowFront;
    // answerRowBack always is the row with answers in the back of the game
    public AnswerRow answerRowBack;

	// If a row with answers is not visible anymore for the player, it will respawn at this Z position.
	private float appearPositionZ;
	// Defines at which position a row with answers will disappear.
	public float disappearPositionZ;

	// The answer, given by the player.
	private Transform givenAnswer;

    private Question[] questions;
    private int currentQuestion = 0;

    private TextMesh questionText;

    private bool minigameFinshed = false;

    [SerializeField]
    private int answerScoreWeigth = 0;
    public int AnswerScoreWeigth { get { return answerScoreWeigth; } set { answerScoreWeigth = value; } }

    [SerializeField]
    private int timeScoreWeigth = 0;
    public int TimeScoreWeigth { get { return timeScoreWeigth; } set { timeScoreWeigth = value; } }

    private int totalAskedQuestions = 0;
    public int TotalAskedQuestions { get { return totalAskedQuestions; } set { totalAskedQuestions = value; } }

    private int totalCorrectQuestions = 0;
    public int TotalCorrectQuestions { get { return totalCorrectQuestions; } set { totalCorrectQuestions = value; } }


	// Initialization
	void Start () {
        answerRowFront = new AnswerRow(GameObject.FindGameObjectWithTag("Answer1"));
        answerRowBack = new AnswerRow(GameObject.FindGameObjectWithTag("Answer2"));
		
        // Set the appear position.
        appearPositionZ = answerRowBack.transform.position.z;
        currentState = WorldMovement.State.Init;
        
        questionText = GameObject.FindGameObjectWithTag("Question").GetComponent<TextMesh>();

		// Start finite state machine.
		StartCoroutine("FSM");
	}
	
	// Update
	void Update () {
		// Cache the x and y position of a row.
        float x = answerRowFront.transform.position.x;
        float y = answerRowBack.transform.position.y;
        
		// Move the rows towards the player.
        answerRowFront.transform.position = new Vector3(x, y, answerRowFront.transform.position.z + -movementSpeed * Time.deltaTime);
        answerRowBack.transform.position = new Vector3(x, y, answerRowBack.transform.position.z + -movementSpeed * Time.deltaTime);

        


        //Debug.Log("Distance: " + Vector3.Distance(transform.Find("Hitbox").transform.position, answerRowFront.transform.position));

		// If a row is not visible anymore, respawn it at the appearing position.
        if (answerRowFront.transform.position.z <= disappearPositionZ) {
            answerRowFront.transform.position = new Vector3(x, y, appearPositionZ);

            Color a = new Color(1f / 255 * 231, 1f / 255 * 155, 1f / 255 * 19);

            answerRowFront.answerA.color = a;
            answerRowFront.answerB.color = a;
            answerRowFront.answerC.color = a;

            AnswerRow temp = answerRowFront;
            answerRowFront = answerRowBack;
            answerRowBack = temp;
            currentState = WorldMovement.State.ChangeQuestion;
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
            case WorldMovement.State.ScoreScreen:
                ShowScoreScreen();
                StopCoroutine("FSM");
                break;
			}
             yield return null;
		}
	}

    private void LoadQuestionsFromFile(string fileLocation) {
        // Load all text from file.
        string text = System.IO.File.ReadAllText(fileLocation);
        // Split the string by new lines.
        string[] data = data = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        
        int amountOfQuestions = data.Length / 4;
        questions = new Question[amountOfQuestions];

        for (int i = 0; i < amountOfQuestions; i++) {
            questions[i] = new Question();
            questions[i].question = data[i * 4];
            questions[i].answers[0] = data[i * 4 + 1];
            questions[i].answers[1] = data[i * 4 + 2];
            questions[i].answers[2] = data[i * 4 + 3];
            questions[i].correctAnswer = questions[i].answers[0];
            Question.Randomize(questions[i].answers);
        }
    }

	private void Init() {
		// load questions
        LoadQuestionsFromFile("Assets\\data.txt");

        // Set question and answer text
        questionText.text = questions[currentQuestion].question;
        answerRowFront.A = questions[currentQuestion].answers[0];
        answerRowFront.B = questions[currentQuestion].answers[1];
        answerRowFront.C = questions[currentQuestion].answers[2];

        //voor nu om te controleren of het tekstbestand eindigt.
        if (currentQuestion + 1 < questions.Length)
        {
            answerRowBack.A = questions[currentQuestion + 1].answers[0];
            answerRowBack.B = questions[currentQuestion + 1].answers[1];
            answerRowBack.C = questions[currentQuestion + 1].answers[2];
        }
        currentState = WorldMovement.State.Idle;
	}

    private void CheckAnswer()
    {
        if (givenAnswer.FindChild("Answer").GetComponent<TextMesh>().text.Equals(questions[currentQuestion].correctAnswer))
        {
            givenAnswer.GetComponent<MeshRenderer>().material.color = Color.green;
            totalCorrectQuestions += 1;
        }
        else
        {
            givenAnswer.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        totalAskedQuestions += 1;
        Debug.Log("aantal vragen: " + questions.Length + " + gestelde vragen: " + totalAskedQuestions);

        if (questions.Length == totalAskedQuestions)
        {
            currentState = WorldMovement.State.ScoreScreen;
            return;
        }

        currentState = WorldMovement.State.Idle;
    }

    private void ChangeQuestion()
    {
        if (currentQuestion < questions.Length)
        {
            questionText.text = questions[++currentQuestion].question;

            answerRowBack.A = questions[currentQuestion].answers[0];
            answerRowBack.B = questions[currentQuestion].answers[1];
            answerRowBack.C = questions[currentQuestion].answers[2];

            currentState = WorldMovement.State.Idle;
        }
    }

	// Called by AnswerCollision script.
	public void AnswerCollision(Transform answer) {
        givenAnswer = answer;
		currentState = WorldMovement.State.CheckAnswer;
	}

    public void ShowScoreScreen() 
    {
        int totalScore = CalculateScore();
        SaveScore(totalScore);
    }

    public int CalculateScore()
    {
        Debug.Log("answerscoreweight " + answerScoreWeigth);
        Debug.Log("totalcorrect questions" + totalCorrectQuestions);
        return (AnswerScoreWeigth * TotalCorrectQuestions) + (timeScoreWeigth * 1);
        
    }
    public void SaveScore(int totalScore)
    {
        int playerId = 0;
        Debug.Log("saving score: " + totalScore);
        ScoreManager._instance.SaveHighScore(MiniGameEnum.Minigames.Vliegtuig, totalScore, playerId);
    }
}
