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
<<<<<<< HEAD
//<<<<<<< HEAD
        amountOfQuestions = dbControl.getAmountOfQuestions(1);
//=======
        //subjectID = dbControl.
        amountOfQuestions = dbControl.getAmountOfQuestions(1);
//>>>>>>> origin/master
        generateQuestionsList();
=======
        //subjectID = dbControl.getSubject(subject);
        subjectID = 1;
        //questionsList = dbControl.getQuestions(subjectID);
        questionsListID = dbControl.getQuestionIDs(subjectID);
        amountOfQuestions = questionsListID.Count;
        //generateQuestionsList();
>>>>>>> origin/master
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
        if (question.Equals(""))
        {
            return;
        }
        int amountOfSubImages = dbControl.getAmountOfSubImages(subjectID, question);

        // Max amount of subimages
        // if(amountOfSubImages > 5){ amountOfSubImages = 5; }

        for (int subImage = 0; subImage < amountOfSubImages; subImage++)
        {
            Vector2[] coordinates = dbControl.getSubImageCoordinates(subjectID, question, subImage);
            // coordinates
            float maxX = 0;
            float maxY = 0;
            float minX = 0;
            float minY = 0;
            for (int i = 0; i < coordinates.Length; i++)
            {
                if (coordinates[i].x > maxX)
                {
                    maxX = coordinates[i].x;
                }
                else if (coordinates[i].x < minX)
                {
                    minX = coordinates[i].x;
                }
                if (coordinates[i].y > maxY)
                {
                    maxY = coordinates[i].y;
                }
                else if (coordinates[i].y < minY)
                {
                    minY = coordinates[i].y;
                }
            }

            float height = maxY - minY;
            float width = maxX - minX;

            //string answer = dbControl.getAnswers(question);
            string answer = dbControl.getQuestionAnswer(subjectID, question, subImage);
            


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

        }
        questionTexture = dbControl.getBackgroundImage(subjectID, question);
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
