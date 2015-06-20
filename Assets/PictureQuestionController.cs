using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PictureQuestionController : MonoBehaviour {
    public string subject = ""; // Chosen subject
    private int subjectID = -1;
    private int amountOfQuestions = 0; // Total amount of questions
    dbController dbControl; // Database contoller

    public GameObject pictureQuestion;
    public GameObject pictureAnswer;
    public GameObject mainPictureQuestion;
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
        //subjectID = 101;

        questionsListID = dbControl.getQuestionIDs(subjectID);
        amountOfQuestions = questionsListID.Count;
        Debug.Log("Total amount of questions found: " + amountOfQuestions);
        spawnQuestion();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public int findRandomNextQuestion()
    {
        Debug.Log("RandomQuestions: " + questionsListID.Count);
        int question = -1;
        if (questionsListID.Count > 0){
            int index = Random.Range(0, questionsListID.Count);
            question = questionsListID[index];
            questionsListID.Remove(question);
        }
        return question;
    }

    public void spawnQuestion()
    {
        Texture2D questionTexture;
        int question = findRandomNextQuestion();
        if (question <= -1)
        {
            Debug.Log("NO NEW QUESTIONS FOUND!");
            return;
        }
        questionTexture = dbControl.getPicture(question);
        mainPictureQuestion.GetComponent<Renderer>().material.mainTexture = questionTexture;
        Debug.Log("Texture set");
        int pictureID = dbControl.getPictureID(questionTexture);
        //int pictureID = 264;
        List<Rect> rects = dbControl.getRect(pictureID);
        int amountOfSubImages = rects.Count;
        Debug.Log("Spawn question with id: " + question);
        Debug.Log("The question needs "+ amountOfSubImages + " answers");

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
            float yOffset = rects[subImage].y / tHeight;

            // Used to scale the texture on the answer QUAD
            Vector2[] newUV = new Vector2[]{
                new Vector2(xOffset,  yOffset),  // left bottom
                new Vector2(xOffset + rectWidth/tWidth, yOffset + rectHeight/tHeight),   // right top
                new Vector2(xOffset + rectWidth/tWidth,  yOffset), // right bottom
                new Vector2(xOffset, yOffset + rectHeight/tHeight), //  left Top
           };
            answerGO.GetComponent<MeshFilter>().mesh.uv = newUV;
            pictureQuestionGO.GetComponent<MeshFilter>().mesh.uv = newUV;

            answerGO.GetComponent<Renderer>().material.mainTexture = questionTexture;
            pictureQuestionGO.GetComponent<Renderer>().material.mainTexture = questionTexture;
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



    private void resizeQuestionImage(Texture questionTexture)
    {
        float textureWidth = questionTexture.width;
        float textureHeight = questionTexture.height;

        float height = mainPictureQuestion.transform.localScale.y;
        float maxWidth = mainPictureQuestion.transform.localScale.x;
        float width = height * (textureWidth / textureHeight);

        if (width > maxWidth) {
            width = maxWidth;
            height = width * (textureHeight / textureWidth);
        }

        mainPictureQuestion.transform.localScale = new Vector3(width, height, 1f);
    }
}
