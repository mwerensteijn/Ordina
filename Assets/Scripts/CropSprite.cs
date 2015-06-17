using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;

public class CropSprite : MonoBehaviour 
{

//	Reference for sprite which will be cropped and it has BoxCollider or BoxCollider2D
	public GameObject spriteToCrop;
    public GameObject plane;

    public float minCropHeight = 5;
    public float minCropWidth = 5;

    private bool hallo = true;
	private Vector3 startPoint, endPoint;
	private bool isMousePressed;
    private int cropCounter = 0;
    private List<Rect> croppedRects = new List<Rect>();
//	For sides of rectangle. Rectangle that will display cropping area
	private LineRenderer leftLine, rightLine, topLine, bottomLine;

    public static Texture2D LoadPNG(string filePath) {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath)) {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    public void saveTexture()
    {
        GetComponent<dbController>().insertPicture(spriteToCrop.GetComponent<SpriteRenderer>().sprite.texture);
    }

    public void SetPlane() {
        plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
        plane.transform.localScale = spriteToCrop.GetComponent<SpriteRenderer>().bounds.size;
        plane.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        plane.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white);
        plane.transform.position = new Vector3(0, 0, 9);
    }

	IEnumerator Start () {
        if (FileBrowser.selectedFile != "") {
            WWW file = new WWW("file://" + FileBrowser.selectedFile);
            yield return file;

            Texture2D tex = file.texture;
            Rect rec = new Rect(0, 0, tex.width, tex.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite newPlanet = Sprite.Create(tex, rec, pivot);

            spriteToCrop.GetComponent<SpriteRenderer>().sprite = newPlanet;
        }

        SetPlane();
        spriteToCrop.AddComponent<BoxCollider2D>();

		isMousePressed = false;
//		Instantiate rectangle sides
		leftLine = createAndGetLine("LeftLine");
		rightLine = createAndGetLine("RightLine");
		topLine = createAndGetLine("TopLine");
		bottomLine = createAndGetLine("BottomLine");

        saveTexture();
	}

    //	Creates line through LineRenderer component
	private LineRenderer createAndGetLine (string lineName) {
		GameObject lineObject = new GameObject(lineName);
		LineRenderer line = lineObject.AddComponent<LineRenderer>();
		line.SetWidth(0.03f,0.03f);
		line.SetVertexCount(2);
		return line;
	}

	void Update () {
        if (croppedRects.Count >= 4)
            StartCoroutine(generateTexturesFromList(croppedRects));
		if(Input.GetMouseButtonDown(0) && isSpriteTouched(spriteToCrop)) {
			isMousePressed = true;
			startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		} else if(Input.GetMouseButtonUp(0)) {
            if (isMousePressed)
                StartCoroutine(cropSprite());
			isMousePressed = false;
		}

		if(isMousePressed && isSpriteTouched(spriteToCrop)) {
			endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			drawRectangle();
		}
	}
//	Following method draws rectangle that displays cropping area
	private void drawRectangle() {
		leftLine.SetPosition(0, new Vector3(startPoint.x, endPoint.y, 0));
		leftLine.SetPosition(1, new Vector3(startPoint.x, startPoint.y, 0));

		rightLine.SetPosition(0, new Vector3(endPoint.x, endPoint.y, 0));
		rightLine.SetPosition(1, new Vector3(endPoint.x, startPoint.y, 0));

		topLine.SetPosition(0, new Vector3(startPoint.x, startPoint.y, 0));
		topLine.SetPosition(1, new Vector3(endPoint.x, startPoint.y, 0));

		bottomLine.SetPosition(0, new Vector3(startPoint.x, endPoint.y, 0));
		bottomLine.SetPosition(1, new Vector3(endPoint.x, endPoint.y, 0));
	}
	//	Following method crops as per displayed cropping area
	private IEnumerator cropSprite() {

        yield return new WaitForEndOfFrame();

        //float[] leftBottomCorner, rightUpperCorner;
        //leftBottomCorner = new float[2];
        //rightUpperCorner = new float[2];

//		Calculate topLeftPoint and bottomRightPoint of drawn rectangle
		Vector3 topLeftPoint = startPoint, bottomRightPoint=endPoint;
		if((startPoint.x > endPoint.x)) {
			topLeftPoint = endPoint;
			bottomRightPoint = startPoint;
		}

		if(bottomRightPoint.y > topLeftPoint.y) {
			float y = topLeftPoint.y;
			topLeftPoint.y = bottomRightPoint.y;
			bottomRightPoint.y = y;
		}

		SpriteRenderer spriteRenderer = spriteToCrop.GetComponent<SpriteRenderer>();
		Sprite spriteToCropSprite = spriteRenderer.sprite;
		Texture2D spriteTexture = spriteToCropSprite.texture;
		Rect spriteRect = spriteToCrop.GetComponent<SpriteRenderer>().sprite.textureRect;

		int pixelsToUnits = 100; // It's PixelsToUnits of sprite which would be cropped

        //GameObject croppedSpriteObj = new GameObject("CroppedSprite");
        Rect croppedSpriteRect = spriteRect;
		croppedSpriteRect.width = (Mathf.Abs(bottomRightPoint.x - topLeftPoint.x)*pixelsToUnits)* (1/spriteToCrop.transform.localScale.x);
		croppedSpriteRect.x = (Mathf.Abs(topLeftPoint.x - (spriteRenderer.bounds.center.x-spriteRenderer.bounds.size.x/2)) *pixelsToUnits)* (1/spriteToCrop.transform.localScale.x);
		croppedSpriteRect.height = (Mathf.Abs(bottomRightPoint.y - topLeftPoint.y)*pixelsToUnits)* (1/spriteToCrop.transform.localScale.y);
		croppedSpriteRect.y = ((topLeftPoint.y - (spriteRenderer.bounds.center.y - spriteRenderer.bounds.size.y/2))*(1/spriteToCrop.transform.localScale.y))* pixelsToUnits - croppedSpriteRect.height;//*(spriteToCrop.transform.localScale.y);

        if(croppedSpriteRect.width > minCropWidth && croppedSpriteRect.height > minCropHeight) {
            croppedRects.Add(croppedSpriteRect);
            //		Crop sprite
            GameObject g = new GameObject("Question");
            BoxCollider2D b = g.AddComponent<BoxCollider2D>();
            b.size = new Vector2(bottomRightPoint.x - topLeftPoint.x, topLeftPoint.y - bottomRightPoint.y);
            Vector3 pos = topLeftPoint - ((topLeftPoint - bottomRightPoint) / 2);
            g.transform.position = new Vector3(pos.x, pos.y, 0);
            ImageAnswer ia = g.AddComponent<ImageAnswer>();
            ia.leftLine = leftLine;
            ia.topLine = topLine;
            ia.rightLine = rightLine;
            ia.bottomLine = bottomLine;

            leftLine = createAndGetLine("LeftLine");
            rightLine = createAndGetLine("RightLine");
            topLine = createAndGetLine("TopLine");
            bottomLine = createAndGetLine("BottomLine");
        }

        /*
        Debug.Log(croppedRects.Count);
        croppedRects.Add(croppedSpriteRect);
        Debug.Log(croppedRects.Count);

        //leftBottomCorner[0] = croppedSpriteRect.xMin;
        //leftBottomCorner[1] = croppedSpriteRect.yMin;

        //rightUpperCorner[0] = croppedSpriteRect.xMax;
        //rightUpperCorner[1] = croppedSpriteRect.yMax;

        Sprite croppedSprite = Sprite.Create(spriteTexture, croppedSpriteRect, new Vector2(0,1), pixelsToUnits);
		SpriteRenderer cropSpriteRenderer = croppedSpriteObj.AddComponent<SpriteRenderer>();	
		cropSpriteRenderer.sprite = croppedSprite;
		topLeftPoint.z = -1;

		croppedSpriteObj.transform.position = new Vector3(0,0,0);
		croppedSpriteObj.transform.parent = spriteToCrop.transform.parent;
		croppedSpriteObj.transform.localScale = spriteToCrop.transform.localScale;
        ImageAnswer imageAnswer = croppedSpriteObj.AddComponent<ImageAnswer>();
        imageAnswer.position = croppedSpriteRect;
        imageAnswer.HideOriginalAnswer(topLeftPoint, bottomRightPoint);

        Debug.Log(croppedSprite.texture.width + " " + croppedSprite.texture.height);

        Texture2D CroppedTexture = new Texture2D((int)croppedSprite.rect.width, (int)croppedSprite.rect.height, TextureFormat.RGB24, false);
        CroppedTexture.ReadPixels(croppedSprite.textureRect, 0, 0);
        Color[] pixels = croppedSprite.texture.GetPixels((int)croppedSprite.rect.x,
                                                            (int)croppedSprite.rect.y,
                                                            (int)croppedSprite.rect.width ,
                                                            (int)croppedSprite.rect.height);
        CroppedTexture.SetPixels(pixels);
        CroppedTexture.Apply();
        byte[] test = CroppedTexture.EncodeToPNG();
        cropCounter++;
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen" + cropCounter + ".png", test);
         */
    }

//	Following method checks whether sprite is touched or not. There are two methods for simple collider and 2DColliders. you can use as per requirement and comment another one.

//	For simple 3DCollider
//	private bool isSpriteTouched(GameObject sprite)
//	{
//		RaycastHit hit;
//		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//		if (Physics.Raycast (ray, out hit)) 
//		{
//			if (hit.collider != null && hit.collider.name.Equals (sprite.name)) 
//				return true;
//		}
//		return false;
//	}

//	For 2DCollider
	private bool isSpriteTouched(GameObject sprite) {
		Vector3 posFor2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit2D = Physics2D.Raycast(posFor2D, Vector2.zero);

		if (hit2D != null && hit2D.collider != null) {
			if(hit2D.collider.name.Equals(sprite.name))
				return true;
		}

		return false;
	}

    public IEnumerator generateTexturesFromList(List<Rect> croppedRects){
        yield return new WaitForEndOfFrame();
        int pixelsToUnits = 100;
        
        if (hallo == true)
        {
            for (int i = 0; i < croppedRects.Count; i++)
            {
                Debug.Log("Entered gekke moethode en forloopjeeessss");
                GameObject croppedSpriteObj = new GameObject("CroppedSprite");

                SpriteRenderer spriteRenderer = spriteToCrop.GetComponent<SpriteRenderer>();
                Sprite spriteToCropSprite = spriteRenderer.sprite;
                Texture2D spriteTexture = spriteToCropSprite.texture;
                Rect spriteRect = spriteToCrop.GetComponent<SpriteRenderer>().sprite.textureRect;

                Sprite croppedSprite = Sprite.Create(spriteTexture, croppedRects[i], new Vector2(0, 1), pixelsToUnits);
                SpriteRenderer cropSpriteRenderer = croppedSpriteObj.AddComponent<SpriteRenderer>();
                cropSpriteRenderer.sprite = croppedSprite;
                //topLeftPoint.z = -1;

                croppedSpriteObj.transform.position = new Vector3(0, 0, 0);
                croppedSpriteObj.transform.parent = spriteToCrop.transform.parent;
                croppedSpriteObj.transform.localScale = spriteToCrop.transform.localScale;

                Debug.Log(spriteTexture.height);
                Debug.Log(spriteTexture.width);
                //ImageAnswer imageAnswer = croppedSpriteObj.AddComponent<ImageAnswer>();
                //imageAnswer.position = croppedSpriteRect;
                //imageAnswer.HideOriginalAnswer(topLeftPoint, bottomRightPoint);
                
                
                Texture2D CroppedTexture = new Texture2D((int)croppedSprite.rect.width, (int)croppedSprite.rect.height, TextureFormat.RGB24, false);
                CroppedTexture.ReadPixels(croppedSprite.textureRect, 0, 0);
                Color[] pixels = croppedSprite.texture.GetPixels((int)croppedSprite.rect.x,
                                                                    (int)croppedSprite.rect.y,
                                                                    (int)croppedSprite.rect.width,
                                                                    (int)croppedSprite.rect.height);
                CroppedTexture.SetPixels(pixels);
                CroppedTexture.Apply();
                byte[] test = CroppedTexture.EncodeToPNG();
                cropCounter++;
                File.WriteAllBytes(Application.dataPath + "/../SavedScreen" + cropCounter + ".png", test);
                Debug.Log("Saved!");

                //GetComponent<dbController>().insertPicture(CroppedTexture);
                GetComponent<dbController>().insertRect((int)croppedSprite.rect.x, (int)croppedSprite.rect.y, (int)croppedSprite.rect.width, (int)croppedSprite.rect.height);
                 
            }
            hallo = false;
        }

        

    }
}
