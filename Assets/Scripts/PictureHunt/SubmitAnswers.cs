using UnityEngine;
using System.Collections;
using Assets.Scripts.HighScore;
using System.Collections.Generic;
using System;

public class SubmitAnswers : MonoBehaviour, IScore
{
    // Save questions (missing parts of the image), used to check if they are answered and are correct
    public List<PictureQuestion> m_Questions = new List<PictureQuestion>();
    public dbController db;
    // When game is over the scoreScreen will be shown
    public ScoreScreen scoreScreen;
    public Canvas canvas;
    private GameManager gameManager;
    public DigitalClock gameTimer;
    public PopUpWindow popUp;
    private int elapsedTime = 0;

    // Used to spawn the next question
    public PictureQuestionController questionController;


    // Used for calculation and keeping track of the score
    [SerializeField]
    private int answerScoreWeigth = 0;
    public int AnswerScoreWeigth { get { return answerScoreWeigth; } set { answerScoreWeigth = value; } }

    private int totalAskedQuestions = 0;
    public int TotalAskedQuestions { get { return totalAskedQuestions; } set { totalAskedQuestions = value; } }

    private int totalCorrectQuestions = 0;
    public int TotalCorrectQuestions { get { return totalCorrectQuestions; } set { totalCorrectQuestions = value; } }

    //! \brief Use this for initialization
    //! \return void
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    public void addQuestion(PictureQuestion question)
    {
        m_Questions.Add(question);
    }

    //! \brief Submit answers
    //! \return void
    public void Submit()
    {
            // Check if every question is answered
            foreach (PictureQuestion question in m_Questions) {
                if (!(question.isAnswered())) {
                    popUp.enablePopUp("Niet alle onderdelen zijn beantwoord");
                    Debug.Log("Not all questions are answered");
                    return;
                }
            }

            // Check if the questions are answered correctly
            TotalAskedQuestions = m_Questions.Count;
            foreach (PictureQuestion question in m_Questions) {
                if (question.checkAnswer()) {
                    TotalCorrectQuestions += 1;
                    // Correct answer
                } else {
                    // Wrong answer
                    //Debug.Log("Wrong should have been: " + question.GetComponent<PictureQuestion>().getDescription());
                }
            }
            
            //Save and show answered question info
            int totalSeconds = gameTimer.GetTotalSeconds();
            SaveScore(CalculateScore(), totalSeconds - elapsedTime);
            elapsedTime = totalSeconds;
            popUp.enablePopUp(totalCorrectQuestions + " onderdelen goed beantwoord");

            // Remove old question with answers
            foreach (PictureQuestion question in m_Questions) {
                question.removeFromScene();
            }
            m_Questions = new List<PictureQuestion>();

            // Check if there no new questions
            if (!questionController.spawnQuestion()) {
                // End game
                popUp.disablePopUp();
                scoreScreen.ShowScoreScreen(CalculateScore(), gameTimer.GetFormatedTime());
                canvas.gameObject.SetActive(false);
            }
        
    }

    //! \brief Calculate the score
    //! \return int score
    public int CalculateScore()
    {
        return AnswerScoreWeigth * TotalCorrectQuestions;

    }
    //! \brief Save the score in the database
    //! \return void
    public void SaveScore(int totalScore, int totalTimeSeconds)
    {
        try
        {
            db.insertScore(gameManager.getPlayerName(), gameManager.getSubject(), gameManager.getSpelID(), totalScore, totalTimeSeconds, TotalAskedQuestions, TotalCorrectQuestions);
        }
        catch (Exception e) { Debug.Log("foutmelding: " + e.Message); }
    }
}