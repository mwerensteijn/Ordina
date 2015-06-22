using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PictureQuestionController : MonoBehaviour {
    public string subject = ""; // Chosen subject
    private int subjectID = -1;
    private int amountOfQuestions = 0; // Total amount of questions
    private dbController dbControl; // Database contoller
    public ProgressBar progressBar;

    public GameObject pictureQuestion;
    public GameObject pictureAnswer;
    public GameObject mainPictureQuestion;
    public SubmitAnswers submit;

    public float startingPositionWidth = 10;
    public float startingPositionHeight = 2;
    public float maxWidthAnswer = 14;
    public float defaultZPosition = -6;

    private float lastPositionWidth;
    private float lastPositionHeight;

    

    List<int> questionsListID = new List<int>();

    PictureQuestionController(string subject)
    {
        this.subject = subject;
    }

    // Use this for initialization
	void Start () {
        dbControl = new dbController();

        lastPositionWidth = startingPositionWidth;
        lastPositionHeight = startingPositionHeight;
        subjectID = dbControl.getSubjectID(subject);
        //subjectID = 101;

        questionsListID = dbControl.getQuestionIDs(subjectID);
        amountOfQuestions = questionsListID.Count;
        Debug.Log("Total amount of questions found: " + amountOfQuestions);
        spawnQuestion();
        progressBar.SetMaxAwnsers(amountOfQuestions);
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
        progressBar.UpdateProgressBar(getAmountOfQuestionsAnswered());
        int question = findRandomNextQuestion();
        resetPosition();
        if (question <= -1)
        {
            Debug.Log("NO NEW QUESTIONS FOUND!");
            return;
        }

        int pictureID = dbControl.getPictureID(question);
        Debug.Log(pictureID);
        questionTexture = dbControl.getPicture(pictureID);
        mainPictureQuestion.GetComponent<Renderer>().material.mainTexture = questionTexture;
        Debug.Log("Texture set");
        //int pictureID = 264;
        
        List<dbController.Rectangle> rects = dbControl.getRect(pictureID);
        int amountOfSubImages = rects.Count;
        Debug.Log("Spawn question with id: " + question);
        Debug.Log("The question needs "+ amountOfSubImages + " answers");

        // Max amount of subimages
        // if(amountOfSubImages > 5){ amountOfSubImages = 5; }

        for (int subImage = 0; subImage < amountOfSubImages; subImage++)
        {
            Rect r = rects[subImage].rect;
            r.y = questionTexture.height - r.y - r.height;

            string answer = ""+subImage;

            // Spawn pictureQuestion object
            int tHeight = questionTexture.height;
            int tWidth = questionTexture.width;

            float newZPosition = transform.localScale.x / tWidth * (r.x + r.width / 2) - transform.localScale.x / 2 + transform.position.z;
            float newYPosition = transform.position.y + transform.localScale.y / 2 - transform.localScale.y / tHeight * (r.y + r.height / 2);

         
            GameObject pictureQuestionGO = Instantiate(pictureQuestion, new Vector3(mainPictureQuestion.transform.position.x + 0.01f, newYPosition, newZPosition), Quaternion.Euler(0,270,0)) as GameObject;
            submit.addQuestion(pictureQuestionGO.GetComponent<PictureQuestion>()); 
            pictureQuestionGO.GetComponent<PictureQuestion>().answerDescription = answer;
            pictureQuestionGO.transform.localScale = new Vector3(mainPictureQuestion.transform.localScale.x / questionTexture.width * r.width, mainPictureQuestion.transform.localScale.y / questionTexture.height * r.height, 1);
            
            // Spawn answer object
//GameObject answerGO = Instantiate(pictureAnswer, new Vector3(calculatePosition(amountOfSubImages, subImage, 10, 4), 3,  -6), Quaternion.Euler(0,180,0)) as GameObject;
            float answerWidth = mainPictureQuestion.transform.localScale.x / questionTexture.width * r.width;
            float answerHeight = mainPictureQuestion.transform.localScale.y / questionTexture.height * r.height;
            GameObject answerGO = Instantiate(pictureAnswer, calculatePostionAnswers(answerWidth, answerHeight), Quaternion.Euler(0, 180, 0)) as GameObject;
            answerGO.GetComponent<Answer>().answerDescription = answer;
            //answerGO.GetComponent<Transform>().localScale = new Vector3(0.1f, rects[subImage].height / 100, rects[subImage].width / 100);
            //answerGO.transform.localScale = new Vector3(mainPictureQuestion.transform.localScale.x / questionTexture.width * rects[subImage].width, mainPictureQuestion.transform.localScale.y / questionTexture.height * rects[subImage].height, 1);
            answerGO.transform.localScale = new Vector3(answerWidth, answerHeight, 1f);
            changeUV(answerGO, questionTexture, r);
        }
    }

    private float calculatePosition(int maxQuestions, int question, float startingPosition, float width){
            float value = width / maxQuestions * question + startingPosition;
            return value;
    }

    private Vector3 calculatePostionAnswers(float width, float height ){
        Debug.Log("Calc pos: " + lastPositionWidth);
        lastPositionWidth += width;
        if(lastPositionWidth > maxWidthAnswer){
            // + width because return - width
            lastPositionWidth = startingPositionWidth + width;
            lastPositionHeight += height;
        }

        return new Vector3(lastPositionWidth - width, lastPositionHeight, defaultZPosition);
    }
    private void resetPosition()
    {
        lastPositionHeight = startingPositionHeight;
        lastPositionWidth = startingPositionWidth;
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

    public void changeUV(GameObject gObject, Texture2D texture, Rect test)
    {
        if (texture != null)
        {

            int tHeight = texture.height;
            int tWidth = texture.width;
            float rectHeight = test.height;
            float rectWidth = test.width;
            float yOffset = test.y / tHeight;

            float leftX = test.x / tWidth;
            float rightX = leftX + rectWidth / tWidth;
            float topY = (1 - test.y / texture.height);
            float bottomY = 1 - (test.y + test.height) / texture.height;

            Vector2[] newUV = new Vector2[]{
            // quad
                new Vector2(leftX,  bottomY),  // left bottom
                new Vector2(rightX, topY),   // right top
                 new Vector2(rightX,  bottomY), // right bottom
                new Vector2(leftX, topY), //  left Top */
            };

            gObject.GetComponent<MeshFilter>().mesh.uv = newUV;
            gObject.GetComponent<Renderer>().material.mainTexture = texture;
        }
    }

    public int getTotalAmountOfQuestions()
    {
        return amountOfQuestions;
    }

    public int getAmountOfQuestionsAnswered()
    {
        return amountOfQuestions - questionsListID.Count;
    }
}
