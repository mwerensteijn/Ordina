using UnityEngine;
using System.Collections;

public class PictureQuestion : MonoBehaviour
{
    private Answer givenAnswer;
    [SerializeField] private string answerDescription = "Default answer";

    public PictureQuestion(string answer)
    {
        answerDescription = answer;
    }


    public string getDescription()
    {
        return answerDescription;
    }

    public bool checkAnswer()
    {
        return givenAnswer.getAnswerDescription().Equals(answerDescription);
    }
}