using UnityEngine;
using System.Collections;

public class ResizeTest : MonoBehaviour {
    private GameObject t;
    public Material material;
    public GameObject answer;
    private Rect rect = new Rect(9, 147, 258, 118);

	// Use this for initialization
	void Start () {
        ResizeTexture();
        SetQuestions();
        SetAnswers();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetQuestions() {
        t = GameObject.CreatePrimitive(PrimitiveType.Quad);
        t.name = "AntwoordPlaats";
        t.AddComponent<testUV>();
        t.AddComponent<PictureQuestion>();
        t.tag = "Question";

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;

        float textureWidth = texture.width;
        float textureHeight = texture.height;

        t.transform.localScale = new Vector3(transform.localScale.x / textureWidth * rect.width, transform.localScale.y / textureHeight * rect.height, 1);

        float newZPosition = transform.localScale.x / textureWidth * (rect.x + rect.width / 2) - transform.localScale.x / 2 + transform.position.z;
        float newYPosition = transform.position.y + transform.localScale.y / 2 - transform.localScale.y / textureHeight * (rect.y + rect.height / 2);
        
        //float newYPosition = transform.position.y - transform.localScale.y / 2 + transform.localScale.y / textureHeight * (rect.y + rect.height * 2);

        //float y = transform.position.y - (transform.localScale.y / 2) + localScale.;
        t.transform.position = new Vector3(6.273f, newYPosition, newZPosition);
        t.transform.rotation = Quaternion.Euler(0, 270, 0);
        t.GetComponent<MeshRenderer>().material = material;
    }

    public void SetAnswers() {
        answer.GetComponent<testUV>().texture = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        answer.GetComponent<testUV>().test = rect;
        answer.GetComponent<testUV>().UpdateUVs();
    }

    public void ResizeTexture() {
        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        float height = transform.localScale.y;
        float maxWidth = transform.localScale.x;
        float width = height * (textureWidth / textureHeight);

        if (width > maxWidth) {
            width = maxWidth;
            height = width * (textureHeight / textureWidth);
        }

        transform.localScale = new Vector3(width, height, 1f);
    }
}
