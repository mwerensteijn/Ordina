using UnityEngine;
using System.Collections;

public class Answer: MonoBehaviour{

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private Vector3 defaultScale;
    private GameObject question = null;

    public string answerDescription = "Default answer";


    public Answer(string description){
        answerDescription = description;
    }

    // Use this for initialization
    void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setQuestion(GameObject newQuestion){
        question = newQuestion;
        question.SetActive(false);
    }
    public void reset()
    {
        question.SetActive(true);
        question = null;
        resetToStartingPosition();
    }
    private void resetToStartingPosition()
    {
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
        transform.localScale = defaultScale;
    }

    public string getAnswerDescription()
    {
        return answerDescription;
    }
}
