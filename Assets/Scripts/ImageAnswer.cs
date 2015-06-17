using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageAnswer : MonoBehaviour {
    public LineRenderer leftLine, topLine, rightLine, bottomLine;
    
    public void Start() {
        
    }

    public void OnMouseDown() {
        Destroy(leftLine.gameObject);
        Destroy(topLine.gameObject);
        Destroy(rightLine.gameObject);
        Destroy(bottomLine.gameObject);
        Destroy(gameObject);
    }
}
