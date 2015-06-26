using UnityEngine;
using System.Collections;

//! \brief This class represents the answer parts which should be filled in to the question answer.
public class Answer: MonoBehaviour{
    // The description of the answer
    public string answerDescription = "Default answer";

    //! \brief Constructor to set the description
    public Answer(string description){
        answerDescription = description;
    }

    //! \brief Use this for initialization
    //! \return void
    void Start()
    {

    }

    //! \brief Update is called once per frame
    //! \return void
    void Update()
    {
    }

    //! \brief Reset method reactive the gameObject
    //! \return void
    public void reset()
    {
        gameObject.SetActive(true);
    }
    //! \brief Answer description will be returned
    //! \return answerDescription
    public string getAnswerDescription()
    {
        return answerDescription;
    }

    //! \brief Remove bullet impacts which are on this object.
    //! \return void
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
