using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PictureQuestionController : MonoBehaviour {
    public string subject = "y u no pick subject?"; // Chosen subject
    public int subjectID = -1;
    private int amountOfQuestions = 0; // Total amount of questions
    dbController dbControl; // Database contoller

    public GameObject pictureQuestion;
    public GameObject pictureAnswer;
    public SubmitAnswers submit;

    List<int> questionsListID = new List<int>();

    PictureQuestionController(string subject)
    {
        this.subject = subject;
    }

    // Use this for initialization
	void Start () {
        dbControl = new dbController();

        subjectID = dbControl.getSubjectID(subject);
        //subjectID = 1;

        questionsListID = dbControl.getQuestionIDs(subjectID);
        amountOfQuestions = questionsListID.Count;


        //amountOfQuestions = dbControl.getAmountOfQuestions(1);
        //subjectID = dbControl.
        //amountOfQuestions = dbControl.getAmountOfQuestions(1);
        generateQuestionsList();
        spawnQuestion();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Almost not ugly method for generating a question list needed to get "random" questions just once
    void generateQuestionsList()
    {
        questionsListID = new List<int>();
        for(int i = 0; i < amountOfQuestions; i++){
            questionsListID.Add(i);
        }
    }

    public int findRandomNextQuestion()
    {
        int question = -1;
        if (questionsListID.Count > 0)
        {
            int index = Random.Range(0, questionsListID.Count);
            question = questionsListID[index];
            questionsListID.Remove(index);
        }
        return question;
    }

    public void spawnQuestion()
    {
        Texture2D questionTexture;
        int question = findRandomNextQuestion();
        if (question <= -1)
        {
            return;
        }
        List<Rect> rects = dbControl.getRect(question);
        int amountOfSubImages = rects.Count;

        Debug.Log(amountOfQuestions);
        Debug.Log(question);
        Debug.Log(amountOfSubImages);

        // Max amount of subimages
        // if(amountOfSubImages > 5){ amountOfSubImages = 5; }

        for (int subImage = 0; subImage < amountOfSubImages; subImage++)
        {

            string answer = ""+subImage;

            // Spawn pictureQuestion object
            GameObject pictureQuestionGO = Instantiate(pictureQuestion, new Vector3(4, 3, calculatePosition(amountOfSubImages, subImage, -5, 10)), new Quaternion(0, 0, 0, 0)) as GameObject;
            pictureQuestionGO.GetComponent<PictureQuestion>().answerDescription = answer;
            

            // Spawn answer object
            GameObject answerGO = Instantiate(pictureAnswer, new Vector3(calculatePosition(amountOfSubImages, subImage, 10, 4), 3,  -6), Quaternion.Euler(90,0,0)) as GameObject;
            answerGO.GetComponent<Answer>().answerDescription = answer;
         
            questionTexture = dbControl.getPicture(question);
            answerGO.GetComponent<Renderer>().material.SetTexture("test", questionTexture);
        }
    }

    private float calculatePosition(int maxQuestions, int question, float startingPosition, float width){
       /* if (maxQuestions % 2 == 0)
        {
            // even number
            
        }
        else
        {*/
            // odd number
            float value = width / maxQuestions * question + startingPosition;

     //   }
            return value;
    }
}
