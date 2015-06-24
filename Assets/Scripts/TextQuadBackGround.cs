using UnityEngine;
using System.Collections;

public class TextQuadBackGround : MonoBehaviour {

    public TextMesh tm;
    public float xScaleFactor = 1.7f;
    public float yScaleFactor = 1.7f;
	// Use this for initialization
	void Start () {
	
	}

    public void UpdateTextQuadBackGroundSize() 
    {
        var x = tm.GetComponent<Renderer>().bounds.size.x * xScaleFactor;
        var y = tm.GetComponent<Renderer>().bounds.size.y * yScaleFactor;
        transform.localScale = new Vector3(x, y, 1);
    }
}
