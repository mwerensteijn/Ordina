using UnityEngine;
using System.Collections;

public class AnswerRow {
    public string A { set { answerAText.text = value; } }
    public string B { set { answerBText.text = value; } }
    public string C { set { answerCText.text = value; } }

    private TextMesh answerAText;
    private TextMesh answerBText;
    private TextMesh answerCText;

    public Material answerA;
    public Material answerB;
    public Material answerC;

    public Transform transform;

	// Use this for initialization
	public AnswerRow(GameObject row) {
        transform = row.transform;

        answerAText = row.transform.FindChild("A").FindChild("Answer").GetComponent<TextMesh>();
        answerBText = row.transform.FindChild("B").FindChild("Answer").GetComponent<TextMesh>();
        answerCText = row.transform.FindChild("C").FindChild("Answer").GetComponent<TextMesh>();

        answerA = row.transform.FindChild("A").GetComponent<MeshRenderer>().material;
        answerB = row.transform.FindChild("B").GetComponent<MeshRenderer>().material;
        answerC = row.transform.FindChild("C").GetComponent<MeshRenderer>().material;
	}
}
