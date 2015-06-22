using UnityEngine;
using System.Collections;
using Assets.Scripts.HighScore;
using System.Collections.Generic;
using System;

public class SubmitAnswers : MonoBehaviour, IScore
{
     
    public List<PictureQuestion> m_Questions = new List<PictureQuestion>();
    public dbController db = new dbController();
    private DigitalClock gameTimer;
    private int elapsedTime = 0;

    public GameManager gameManager;

    public PictureQuestionController questionController;

    [SerializeField]
    private int answerScoreWeigth = 0;
    public int AnswerScoreWeigth { get { return answerScoreWeigth; } set { answerScoreWeigth = value; } }

    private int totalAskedQuestions = 0;
    public int TotalAskedQuestions { get { return totalAskedQuestions; } set { totalAskedQuestions = value; } }

    private int totalCorrectQuestions = 0;
    public int TotalCorrectQuestions { get { return totalCorrectQuestions; } set { totalCorrectQuestions = value; } }

    void Start()
    {
        gameTimer = new DigitalClock();

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Bullet") ){
            Submit();
        }    
    }

    public void addQuestion(PictureQuestion question)
    {
        m_Questions.Add(question);
    }

    
    void Submit()
    {
       // bool check = true;
        Debug.Log("Submit");
        foreach (PictureQuestion question in m_Questions)
        {
            if (!(question.isAnswered()))
            {
                Debug.Log("Not all questions are answered");
                return;
            }
        }
        TotalAskedQuestions = m_Questions.Count;
        foreach(PictureQuestion question in m_Questions){
            if (question.checkAnswer())
            {
                TotalCorrectQuestions += 1;
                // Correct answer
            }else{
                // Wrong answer
                Debug.Log("Wrong should have been: " + question.GetComponent<PictureQuestion>().getDescription());
                //check = false;
            }
        }

        // Check if all answer were right
       /* if (check)
        {
            Debug.Log("Well done");
        }*/
        int totalSeconds = gameTimer.GetTotalSeconds();
        SaveScore(CalculateScore(), totalSeconds - elapsedTime);
        //db.insertScore(gameManager.getPlayerName(), gameManager.getSubject(), gameManager.getSpelID(), score,  totalSeconds - elapsedTime, TotalAskedQuestions, TotalCorrectQuestions);
        elapsedTime = totalSeconds;
        foreach(PictureQuestion question in m_Questions){
            question.removeFromScene();
        }
        m_Questions = new List<PictureQuestion>();
        questionController.spawnQuestion();
    }

    public int CalculateScore()
    {
        Debug.Log("answerscoreweight " + answerScoreWeigth);
        Debug.Log("totalcorrect questions" + totalCorrectQuestions);
        return AnswerScoreWeigth * TotalCorrectQuestions;

    }
    public void SaveScore(int totalScore, int totalTimeSeconds)
    {
        //database connectie en opslag nodig.
        try
        {
            db.insertScore(gameManager.getPlayerName(), gameManager.getSubject(), gameManager.getSpelID(), totalScore, totalTimeSeconds, TotalAskedQuestions, TotalCorrectQuestions);
        }
        catch (Exception e) { Debug.Log("foutmelding: " + e.Message); }
        Debug.Log("total score: " + totalScore);
        Debug.Log("total time in seconds: " + totalTimeSeconds);
    }
}