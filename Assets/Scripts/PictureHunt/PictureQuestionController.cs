﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//!\brief This class controls the questions from the Picture hunt game
public class PictureQuestionController : MonoBehaviour {
    private int subjectID = -1;
    private int amountOfQuestions = 0; // Total amount of questions
    public dbController dbControl; // Database contoller
    public ProgressBar progressBar;

    private GameManager gameManager;

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
    private float maxHeight = 0;

    public float offset = 0.2f;

    List<int> questionsListID = new List<int>();

    //! \brief Use this for initialization
    //! \return void
	void Start () {
        lastPositionWidth = startingPositionWidth;
        lastPositionHeight = startingPositionHeight;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        subjectID = dbControl.getSubjectID(gameManager.getSubject());
        
        questionsListID = dbControl.getQuestionIDs(subjectID, true);
        amountOfQuestions = questionsListID.Count;
        Debug.Log("Total amount of questions found: " + amountOfQuestions);
        progressBar.SetMaxAwnsers(amountOfQuestions);
        spawnQuestion();
        
	}

    //! \brief Find the next random question from the already selected questions
    //! \return int questionID
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

    //! \brief Spawn the next question in the game
    //! this function will spawn all answers and missing question parts
    //! \return bool return if a new questions is found and spawned
    public bool spawnQuestion()
    {
        Texture2D questionTexture;
        progressBar.UpdateProgressBar(getAmountOfQuestionsAnswered());

        int question = findRandomNextQuestion();
        resetPosition();
        if (question <= -1)
        {
            Debug.Log("NO NEW QUESTIONS FOUND!");
            return false;
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
            float answerWidth = mainPictureQuestion.transform.localScale.x / questionTexture.width * r.width;
            float answerHeight = mainPictureQuestion.transform.localScale.y / questionTexture.height * r.height;
            GameObject answerGO = Instantiate(pictureAnswer, calculatePostionAnswers(answerWidth, answerHeight), Quaternion.Euler(0, 180, 0)) as GameObject;
            answerGO.GetComponent<Answer>().answerDescription = answer;
            answerGO.transform.localScale = new Vector3(answerWidth, answerHeight, 1f);
            // Change UV to fit the texture
            changeUV(answerGO, questionTexture, r);
        }
        return true;
    }

    //! \brief Calculate the position of the answer
    //! \return Vector3 contains the position of the answer
    private Vector3 calculatePostionAnswers(float width, float height ){
        bool first = false;
        if (lastPositionWidth == startingPositionWidth)
        {
            //first in the row
            first = true;
        }
        lastPositionWidth += (width + offset);
        if (height > maxHeight)
        {
            maxHeight = height;
        }
        if(lastPositionWidth > maxWidthAnswer){
            // + width because return - width
            lastPositionWidth = startingPositionWidth + width;
            lastPositionHeight += maxHeight += offset;
            maxHeight = 0;
        }
        if (first)
        {
            lastPositionWidth -= offset;
        }

        return new Vector3(lastPositionWidth - width, lastPositionHeight, defaultZPosition);
    }

    //! \brief Reset the position for calculate answers 
    //! This is used when a new question is spawned
    //! \return void
    private void resetPosition()
    {
        lastPositionHeight = startingPositionHeight;
        lastPositionWidth = startingPositionWidth;
        maxHeight = 0;
    }


    //! \brief Resize the questionObject to texture size
    //! \return void
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

    //! \brief Change the UV of the gameObject to scale the texture to the right position and size
    //! \return void
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

    //! \brief Get the total amount of questions which will be asked
    //! \return int width total amount of questions
    public int getTotalAmountOfQuestions()
    {
        return amountOfQuestions;
    }

    //! \brief Get the amount of questions which are answered
    //! \return int total amount of questions answered
    public int getAmountOfQuestionsAnswered()
    {
        return amountOfQuestions - questionsListID.Count;
    }
}
