using UnityEngine;
using System.Collections;
using System;

public class TextQuadBackGround : MonoBehaviour {

    public TextMesh tm;
    public float xScaleFactor = 1.0f;
    public float yScaleFactor = 1.0f;
    public bool IgnoreTextLength = false;
    public SmartTextMesh smTm;
	// Use this for initialization
	void Start () {

	}

    //! \brief UpdateTextQuadBackGroundSize is called to scale the cloud.
    //! Scale the cloud to the size of the answer.
    //! \return void
    public void UpdateTextQuadBackGroundSize() 
    {
        float x = 0f;
        float y = 0f;

        if (IgnoreTextLength)
        {
            x = tm.GetComponent<Renderer>().bounds.size.x * xScaleFactor;
            y = tm.GetComponent<Renderer>().bounds.size.y * yScaleFactor;
        }
        else if (!IgnoreTextLength)
        {
            float lineHigh = (float)Math.Ceiling(tm.text.Length / smTm.MaxWidth);

            x = (tm.GetComponent<Renderer>().bounds.size.x + (smTm.MaxWidth * 0.25f)) * xScaleFactor;
            y = (tm.GetComponent<Renderer>().bounds.size.y + (lineHigh * 1.5f)) * yScaleFactor;
        }

        transform.localScale = new Vector3(x, y, 1);
    }
}
