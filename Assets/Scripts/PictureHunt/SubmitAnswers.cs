using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubmitAnswers : MonoBehaviour
{
     
    public List<PictureQuestion> m_Questions = new List<PictureQuestion>();

    private int amountOfCorrectAnswers = 0;
    private int amountOfWrongAnswers = 0;

    public PictureQuestionController questionController;
    

    void Start()
    {

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
        bool check = true;
        Debug.Log("Submit");
        foreach (PictureQuestion question in m_Questions)
        {
            if (!(question.isAnswered()))
            {
                Debug.Log("Not all questions are answered");
                return;
            }
        }
        foreach(PictureQuestion question in m_Questions){
            if (question.checkAnswer())
            {
                amountOfCorrectAnswers += 1;
                // Correct answer
            }else{
                // Wrong answer
                amountOfWrongAnswers += 1;
                Debug.Log("Wrong should have been: " + question.GetComponent<PictureQuestion>().getDescription());
                check = false;
            }
        }
        // Check if all answer were right
        if (check)
        {
            Debug.Log("Well done");
        }

        questionController.removeQuestion();
        questionController.spawnQuestion();
    }
}