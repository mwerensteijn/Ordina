using UnityEngine;
using System.Collections;

public class PictureQuestion : MonoBehaviour
{
    private Answer givenAnswer = null;
    public string answerDescription = "Default answer";
    private Renderer renderer;
    private Material defaultMaterial;

    public PictureQuestion(string answer)
    {
        answerDescription = answer;
    }

    void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultMaterial = renderer.material;
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
        if (givenAnswer != null)
        {
            givenAnswer.reset();
        }
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
        if (givenAnswer != null)
        {
            givenAnswer.reset();
        }
        givenAnswer = null;
        renderer.material = defaultMaterial;
    }

}