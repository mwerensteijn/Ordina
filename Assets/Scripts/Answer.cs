using UnityEngine;
using System.Collections;

public class Answer : MonoBehaviour {
	public string answer;

	private TextMesh answerText;

	// Use this for initialization
	void Start () {
		answerText = transform.FindChild("Answer").GetComponent<TextMesh>();
		answerText.text = answer;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
