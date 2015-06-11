using UnityEngine;
using System.Collections;

public class PictureQuestion : MonoBehaviour
{
    private Answer givenAnswer = null;
    [SerializeField] private string answerDescription = "Default answer";

    public PictureQuestion(string answer)
    {
        answerDescription = answer;
    }

  /*  void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.tag;
        if (tag.Equals("Bullet"))
        {
            GameObject answer = collision.collider.GetComponent<Bullet>().getAnswer();
            Debug.Log("ANSWERRRR!!!!");
            if (answer != null)
            {
                givenAnswer = answer.GetComponent<Answer>();
                Debug.Log("ANSWERRRR!!!!");
            }
        }
    }*/

    public string getDescription()
    {
        return answerDescription;
    }

    public void setAnswer(Answer answer)
    {
        givenAnswer = answer;
    }

    public bool checkAnswer()
    {
        if (givenAnswer == null)
        {
            return false;
        }
        return givenAnswer.getAnswerDescription().Equals(answerDescription);
    }

    public bool isAnswered()
    {
        return givenAnswer != null;
    }

    public void reset()
    {
        givenAnswer = null;
    }

}