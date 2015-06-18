using UnityEngine;
using System.Collections;

public class testUV : MonoBehaviour {
    public Rect test;
    public Texture2D texture;

     public Rect uvsFront = new Rect( 0.0f, 1.0f, 0.125f, 0.125f );
 public Rect uvsBack = new Rect( 0.125f, 0.875f, 0.125f, 0.125f );
 public Rect uvsLeft = new Rect( 0.25f, 0.75f, 0.125f, 0.125f );
 public Rect uvsRight = new Rect( 0.375f, 0.625f, 0.125f, 0.125f );
 public Rect uvsTop = new Rect( 0.5f, 0.5f, 0.125f, 0.125f );
 public Rect uvsBottom = new Rect( 0.625f, 0.375f, 0.125f, 0.125f );
	// Use this for initialization
	void Start () {
        int tHeight = texture.height;
        int tWidth = texture.width;
        float rectHeight = test.height;
        float rectWidth = test.width;
        float xOffset = test.x/tWidth;
        float yOffset = test.y/tHeight;
        Vector2[] newUV = new Vector2[]{
            // quad
                new Vector2(xOffset,  yOffset),  // left bottom
                new Vector2(xOffset + rectWidth/tWidth, yOffset + rectHeight/tHeight),   // right top
                 new Vector2(xOffset + rectWidth/tWidth,  yOffset), // right bottom
                new Vector2(xOffset, yOffset + rectHeight/tHeight), //  left Top */

            /*   new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f), //  left Top*/
                
                 new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f), //  left Top

                  new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f), //  left Top

                  new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f), //  left Top

                  new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f), //  left Top

                  new Vector2(0.0f,  0.0f),  // left bottom
                new Vector2(1.0f, 1.0f),   // right top
                 new Vector2(1.0f,  0.0f), // right bottom
                new Vector2(0.0f, 1.0f) //  left Top*/
            };



    Vector2[] theUVs = newUV;
    /*  //FRONT    2    3    0    1
     theUVs[2].x = uvsFront.x;
     theUVs[2].y = uvsFront.y;
     theUVs[3] = new Vector2( uvsFront.x + uvsFront.width, uvsFront.y );
     theUVs[0] = new Vector2( uvsFront.x, uvsFront.y - uvsFront.height );
     theUVs[1] = new Vector2( uvsFront.x + uvsFront.width, uvsFront.y - uvsFront.height );
     
     // BACK    6    7   10   11
     theUVs[6] = new Vector2(uvsBack.x, uvsBack.y);
     theUVs[7] = new Vector2(uvsBack.x + uvsBack.width, uvsBack.y);
     theUVs[10] = new Vector2(uvsBack.x, uvsBack.y - uvsBack.height);
     theUVs[11] = new Vector2(uvsBack.x + uvsBack.width, uvsBack.y - uvsBack.height);
     
     // LEFT   19   17   16   18
     theUVs[19] = new Vector2(uvsLeft.x, uvsLeft.y);
     theUVs[17] = new Vector2(uvsLeft.x + uvsLeft.width, uvsLeft.y);
     theUVs[16] = new Vector2(uvsLeft.x, uvsLeft.y - uvsLeft.height);
     theUVs[18] = new Vector2(uvsLeft.x + uvsLeft.width, uvsLeft.y - uvsLeft.height);
     
     // RIGHT   23   21   20   22
     theUVs[23] = new Vector2(uvsRight.x, uvsRight.y);
     theUVs[21] = new Vector2(uvsRight.x + uvsRight.width, uvsRight.y);
     theUVs[20] = new Vector2(uvsRight.x, uvsRight.y - uvsRight.height);
     theUVs[22] = new Vector2(uvsRight.x + uvsRight.width, uvsRight.y - uvsRight.height);
     
     // TOP    4    5    8    9
     theUVs[4] = new Vector2(uvsTop.x, uvsTop.y);
     theUVs[5] = new Vector2(uvsTop.x + uvsTop.width, uvsTop.y);
     theUVs[8] = new Vector2(uvsTop.x, uvsTop.y - uvsTop.height);
     theUVs[9] = new Vector2(uvsTop.x + uvsTop.width, uvsTop.y - uvsTop.height);
     
     // BOTTOM   15   13   12   14
     theUVs[15] = new Vector2(uvsBottom.x, uvsBottom.y);
     theUVs[13] = new Vector2(uvsBottom.x + uvsBottom.width, uvsBottom.y);
     theUVs[12] = new Vector2(uvsBottom.x, uvsBottom.y - uvsBottom.height);
     theUVs[14] = new Vector2(uvsBottom.x + uvsBottom.width, uvsBottom.y - uvsBottom.height);
        */

        GetComponent<MeshFilter>().mesh.uv = theUVs;
        GetComponent<Renderer>().material.mainTexture = texture;
	
	}
	
	// Update is called once per frame
	void Update () {

      
	}
}
