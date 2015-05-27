using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextScript : MonoBehaviour {

    Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
        text.text = GetComponentInParent<Slider>().value.ToString() + "%" ;
        Debug.Log(text.text);
	}
}
