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

    //List<int> questionsList = new List<int>();
    //List<string> questionsList;
    List<int> questionsListID;
    List<GameObject> pictureQuestionObjects;
    PictureQuestionController(string subject)
    {
        this.subject = subject;
    }

    // Use this for initialization
	void Start () {
        dbControl = new dbController();
        subjectID = dbControl.getSubject(subject);
        //subjectID = 1;

        questionsListID = dbControl.getQuestionIDs(subjectID);
        amountOfQuestions = questionsListID.Count;

        spawnQuestion();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Almost not ugly method for generating a question list needed to get "random" questions just once
    /*void generateQuestionsList()
    {
        questionsList = new List<int>();
        for(int i = 0; i < amountOfQuestions; i++){
            questionsList.Add(i);
        }
    }*/

    public int findRandomNextQuestion()
    {
        int question = -1;
        if (questionsListID.Count > 0)
        {
            int index = Random.Range(0, questionsListID.Count);
            question = questionsListID[index];
            questionsListID.RemoveAt(index);
        }
        return question;
    }

    public void spawnQuestion()
    {
        pictureQuestionObjects = new List<GameObject>();
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
            Rect coordinates = rects[subImage];


            string answer = ""+subImage;

            // Spawn pictureQuestion object
            GameObject pictureQuestionGO = Instantiate(pictureQuestion, new Vector3(4, 3, calculatePosition(amountOfSubImages, subImage, -5, 10)), new Quaternion(0, 0, 0, 0)) as GameObject;
            PictureQuestion pq = pictureQuestionGO.GetComponent<PictureQuestion>();
            pq.answerDescription = answer;

            submit.addQuestion(pq);
            pictureQuestionObjects.Add(pictureQuestionGO);
            

            // Spawn answer object
            GameObject answerGO = Instantiate(pictureAnswer, new Vector3(calculatePosition(amountOfSubImages, subImage, 10, 4), 3,  -6), Quaternion.Euler(90,0,0)) as GameObject;
            answerGO.GetComponent<Answer>().answerDescription = answer;
            pictureQuestionObjects.Add(answerGO);

            //questionTexture = dbControl.getPicture(question);
            //answerGO.GetComponent<Renderer>().material.SetTexture("test", questionTexture);
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

    public void removeQuestion()
    {
        for(int i = 0; i < pictureQuestionObjects.Count; i++){
            Destroy(pictureQuestionObjects[i]);
        }
    }
}
