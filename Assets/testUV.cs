using UnityEngine;
using System.Collections;

public class testUV : MonoBehaviour {
    public Rect test;
    public Texture2D texture;
    public Vector2[] newUV;

	// Use this for initialization
	void Start () {
        UpdateUVs();
	}

    public void UpdateUVs() {
        if (texture != null) {

            int tHeight = texture.height;
            int tWidth = texture.width;
            float rectHeight = test.height;
            float rectWidth = test.width;
            float yOffset = test.y / tHeight;

            float leftX = test.x / tWidth;
            float rightX = leftX + rectWidth / tWidth;
            float topY = (1 - test.y / texture.height);
            float bottomY = 1 - (test.y + test.height) / texture.height;

           newUV = new Vector2[]{
            // quad
                new Vector2(leftX,  bottomY),  // left bottom
                new Vector2(rightX, topY),   // right top
                 new Vector2(rightX,  bottomY), // right bottom
                new Vector2(leftX, topY), //  left Top */
            };

            GetComponent<MeshFilter>().mesh.uv = newUV;
            GetComponent<Renderer>().material.mainTexture = texture;
        }
    }
	
	// Update is called once per frame
	void Update () {

      
	}
}
