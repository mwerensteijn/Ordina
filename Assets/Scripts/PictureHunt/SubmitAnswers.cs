using UnityEngine;
using System.Collections;

public class SubmitAnswers : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] m_Questions = new GameObject[3];
    

    void Start()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Bullet") ){
            Submit();
        }    
    }

    void Submit()
    {
        bool check = true;
        Debug.Log("Submit");
        foreach (GameObject question in m_Questions)
        {
            if (!(question.GetComponent<PictureQuestion>().isAnswered()))
            {
                Debug.Log("Not all questions are answered");
                return;
            }
        }
        foreach(GameObject question in m_Questions){
            if (question.GetComponent<PictureQuestion>().checkAnswer())
            {
                // Correct answer
            }else{
                // Wrong answer
                Debug.Log("Wrong should have been: " + question.GetComponent<PictureQuestion>().getDescription());
                check = false;
            }
        }
        // Check if all answer were right
        if (check)
        {
            Debug.Log("Well done");
        }
    }
}