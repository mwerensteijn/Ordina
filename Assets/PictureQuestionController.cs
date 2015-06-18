using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PictureQuestionController : MonoBehaviour {
    public string subject = ""; // Chosen subject
    public int subjectID = -1;
    private int amountOfQuestions = 0; // Total amount of questions
    dbController dbControl; // Database contoller

    public GameObject pictureQuestion;
    public GameObject pictureAnswer;
    public GameObject backgroundImage;
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
        spawnQuestion();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public int findRandomNextQuestion()
    {
        int question = -1;
        if (questionsListID.Count > 0){
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
            // 100 is a bad guess for now
            pictureQuestionGO.GetComponent<Transform>().localScale = new Vector3(0.1f, rects[subImage].height/100, rects[subImage].width/100);
            

            // Spawn answer object
            GameObject answerGO = Instantiate(pictureAnswer, new Vector3(calculatePosition(amountOfSubImages, subImage, 10, 4), 3,  -6), Quaternion.Euler(90,0,0)) as GameObject;
            answerGO.GetComponent<Answer>().answerDescription = answer;
            // 100 is a bad guess for now
            answerGO.GetComponent<Transform>().localScale = new Vector3(0.1f, rects[subImage].height / 100, rects[subImage].width / 100);
            questionTexture = dbControl.getPicture(question);
            //questionTexture.height is 1 (100%)
            //questionTexture.width is 1 (100%)

            int tHeight = questionTexture.height;
            int tWidth = questionTexture.width;
            float rectHeight = rects[subImage].height;
            float rectWidth = rects[subImage].width;
            float xOffset = rects[subImage].x / tWidth;
            float yOffset = rects[subImage].y/tHeight;
            Vector2[] newUV = new Vector2[]{
                 new Vector2(xOffset,  yOffset),  // left bottom
                new Vector2(xOffset + rectWidth/tWidth, yOffset + rectHeight/tHeight),   // right top
                 new Vector2(xOffset + rectWidth/tWidth,  yOffset), // right bottom
                new Vector2(xOffset, yOffset + rectHeight/tHeight), //  left Top
                
                 new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f), //  left Top

                  new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f), //  left Top

                  new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f), //  left Top

                  new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f), //  left Top

                  new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f) //  left Top 
            };
            answerGO.GetComponent<MeshFilter>().mesh.uv = newUV;
            pictureQuestionGO.GetComponent<MeshFilter>().mesh.uv = newUV;

            answerGO.GetComponent<Renderer>().material.mainTexture = questionTexture;
            pictureQuestionGO.GetComponent<Renderer>().material.mainTexture = questionTexture;
        }
        // Disabled because DB doesn't work yet
        //questionTexture = dbControl.getPicture(question);
        //backgroundImage.GetComponent<Renderer>().material.mainTexture = questionTexture;
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
