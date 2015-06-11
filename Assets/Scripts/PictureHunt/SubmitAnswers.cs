using UnityEngine;
using System.Collections;

public class SubmitAnswers : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] m_Questions = new GameObject[3];
    

    void Start()
    {
    }

    void Submit()
    {
        foreach(GameObject question in m_Questions){
            if (question.GetComponent<PictureQuestion>().checkAnswer())
            {
                // Correct answer
            }else{
                // Wrong answer
                Debug.Log(question.GetComponent<PictureQuestion>().getDescription());
            }
        }
    }
}