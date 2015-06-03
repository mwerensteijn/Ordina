using UnityEngine;
using System.Collections;

public class ImageAnswer : MonoBehaviour {
    // Holds the left bottom corner of the image and the width & height.
    public Rect position;

    public GameObject hideAnswer;

    public void Update() {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(100, 100, 1));
        //transform.position += new Vector3(10 * Time.deltaTime, 0, 0);
    }

    public void OnMouseDown() {
        Destroy(gameObject);
        Destroy(hideAnswer);
    }
}
