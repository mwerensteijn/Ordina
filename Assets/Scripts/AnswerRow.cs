using UnityEngine;
using System.Collections;

public class AnswerRow {
    // The multiple choice answers
    public string A { set { answerAText.text = value; } }
    public string B { set { answerBText.text = value; } }
    public string C { set { answerCText.text = value; } }

    // Answer textmeshes
    private TextMesh answerAText;
    private Plane answerAPlane;
    private TextMesh answerBText;
    private TextMesh answerCText;

    // Answer materials
    public Material answerA;
    public Material answerB;
    public Material answerC;

    public Transform transform;
    public GameObject row;

	// Use this for initialization
	public AnswerRow(GameObject row) {
        transform = row.transform;
        this.row = row;
        answerAText = row.transform.FindChild("A").FindChild("Answer").GetComponent<TextMesh>();
        answerBText = row.transform.FindChild("B").FindChild("Answer").GetComponent<TextMesh>();
        answerCText = row.transform.FindChild("C").FindChild("Answer").GetComponent<TextMesh>();

        answerA = row.transform.FindChild("A").GetComponent<MeshRenderer>().material;
        answerB = row.transform.FindChild("B").GetComponent<MeshRenderer>().material;
        answerC = row.transform.FindChild("C").GetComponent<MeshRenderer>().material;
	}

    //! \brief This function resizes the clouds behind the answers.
    public void SizePlane() 
    {        
        row.transform.FindChild("A").FindChild("Answer").GetComponentInChildren<TextQuadBackGround>().UpdateTextQuadBackGroundSize();
        row.transform.FindChild("B").FindChild("Answer").GetComponentInChildren<TextQuadBackGround>().UpdateTextQuadBackGroundSize();
        row.transform.FindChild("C").FindChild("Answer").GetComponentInChildren<TextQuadBackGround>().UpdateTextQuadBackGroundSize();
    }

    //! \brief This function cuts the text to fit it in the cloud.
    public void SizeTextMesh() 
    {
        row.transform.FindChild("A").FindChild("Answer").GetComponent<SmartTextMesh>().UpdateTextLayOut();
        row.transform.FindChild("B").FindChild("Answer").GetComponent<SmartTextMesh>().UpdateTextLayOut();
        row.transform.FindChild("C").FindChild("Answer").GetComponent<SmartTextMesh>().UpdateTextLayOut();
    }

    //! \brief This function hides the answers.
    public void HideAnswersText() 
    {
        answerAText.GetComponent<MeshRenderer>().enabled = false;
        answerBText.GetComponent<MeshRenderer>().enabled = false;
        answerCText.GetComponent<MeshRenderer>().enabled = false;
    }
}
