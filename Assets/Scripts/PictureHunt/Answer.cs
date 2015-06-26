using UnityEngine;
using System.Collections;

// This class represents the answer parts which should be filled in to the question answer.
public class Answer: MonoBehaviour{
    // The description of the answer
    public string answerDescription = "Default answer";

    // Constructor to set the description
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

    // Reset method reactive the gameObject
    public void reset()
    {
        gameObject.SetActive(true);
    }
    // Answer description will be returned
    // 
    // @return answerDescription
    public string getAnswerDescription()
    {
        return answerDescription;
    }

    // Remove bullet impacts which are on this object.
    public void removeImpacts()
    {
        // Bullet impacts are added as childs
        // Removing impacts, can be done by removing the childs
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
