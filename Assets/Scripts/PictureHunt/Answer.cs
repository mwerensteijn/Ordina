using UnityEngine;
using System.Collections;

public class Answer: MonoBehaviour{

    public string answerDescription = "Default answer";


    public Answer(string description){
        answerDescription = description;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void reset()
    {
        gameObject.SetActive(true);
    }
    public string getAnswerDescription()
    {
        return answerDescription;
    }
}
