using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageAnswer : MonoBehaviour {
    public static List<ImageAnswer> imageAnswers = new List<ImageAnswer>();
    public static float screenHeight = Camera.main.orthographicSize * 2f;
    public static float screenWidth = screenHeight * Camera.main.aspect / Screen.width * 380;
    public static float objectHeight;

    // Holds the left bottom corner of the image and the width & height.
    public Rect position;

    public GameObject hideAnswer;

    public void HideOriginalAnswer(Vector3 topLeftPoint, Vector3 bottomRightPoint) {

        // Create a gameobject to hide the answer.
        hideAnswer = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hideAnswer.transform.localScale = GetComponent<SpriteRenderer>().bounds.size;
        hideAnswer.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        hideAnswer.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white);
        hideAnswer.transform.position = topLeftPoint - ((topLeftPoint - bottomRightPoint) / 2);
    
    }

    public void Start() {
        gameObject.AddComponent<BoxCollider2D>();
        imageAnswers.Add(this);

        UpdatePositions();
    }

    public void UpdatePositions() {
        for (int i = 1; i <= imageAnswers.Count; i++) {
            imageAnswers[imageAnswers.Count - i].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(10, Camera.main.pixelHeight * (1f / imageAnswers.Count * i), 1));
            float width = imageAnswers[imageAnswers.Count - i].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            float height = imageAnswers[imageAnswers.Count - i].GetComponent<SpriteRenderer>().sprite.bounds.size.y;

            imageAnswers[imageAnswers.Count - i].transform.localScale = new Vector3(screenWidth / width, screenHeight / height / imageAnswers.Count);
        }
    }

    public void Update() {

    }

    public void OnMouseDown() {
        imageAnswers.Remove(this);

        Destroy(gameObject);
        Destroy(hideAnswer);

        UpdatePositions();
    }
}
