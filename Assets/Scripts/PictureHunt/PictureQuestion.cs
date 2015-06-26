using UnityEngine;
using System.Collections;

//!\brief The missing parts of the question are PictureQuestion objects
//! This class saves the given answer and checks if it is correct
public class PictureQuestion : MonoBehaviour
{
    private Answer givenAnswer = null;
    public string answerDescription = "Default answer";
    private Renderer renderer;
    private Material defaultMaterial;

    //! \brief constructor from PictureQuestion
    //! \param answer the answer to this question
    public PictureQuestion(string answer)
    {
        answerDescription = answer;
    }

    //! \brief Start is called when starting
    //! \return void
    void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultMaterial = renderer.material;
    }

    //! \brief Get the description of the answer
    //! \return answerDescription String with answer described
    public string getDescription()
    {
        return answerDescription;
    }

    //! \brief Set the answer
    //! \param answer The answer object which should be given to this question
    //! \return void
    public void setAnswer(Answer answer)
    {
        if (givenAnswer != null)
        {
            givenAnswer.reset();
        }
        givenAnswer = answer;
    }
    //! \brief Check if the answer is correct
    //! \return bool return true if an answer is given and correct.
    public bool checkAnswer()
    {
        if (givenAnswer == null)
        {
            return false;
        }
        return givenAnswer.getAnswerDescription().Equals(answerDescription);
    }

    //! \brief Check if an answer is given
    //! \return void
    public bool isAnswered()
    {
        return givenAnswer != null;
    }

    //! \brief Reset material and answer
    //! \return void
    public void reset() {
        if (givenAnswer != null)
        {
            givenAnswer.reset();
        }
        // Remove 
        givenAnswer = null;
        // change material back
        renderer.material = defaultMaterial;
    }

    //! \brief Destroy question with answer object
    //! \return void
    public void removeFromScene()
    {
        Destroy(givenAnswer.gameObject);
        Destroy(gameObject);
    }

}