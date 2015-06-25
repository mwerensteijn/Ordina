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

	private Vector3 startPoint, endPoint;
	private bool isMousePressed;
    private int cropCounter = 0;
    private static List<ImageAnswer> answers = new List<ImageAnswer>();
    private static List<ImageAnswer> removeFromDatabase = new List<ImageAnswer>();
    
//	For sides of rectangle. Rectangle that will display cropping area
	private LineRenderer leftLine, rightLine, topLine, bottomLine;

    //! \brief This method removes selected sprite from database.
    //! \param ImageAnswer i.
    //! \return void
    public static void RemoveAnswer(ImageAnswer i) {
        if (i.SavedInDatabase) {
            removeFromDatabase.Add(i);
        }

        answers.Remove(i);
    }

    //! \brief This method will resize the texture
    //! \return void
    public void ResizeTexture() {
        SpriteRenderer sr = spriteToCrop.GetComponent<SpriteRenderer>();
        if (sr == null)
            return;

        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f * 0.8f;
        float maxWorldScreenWidth = worldScreenHeight / Screen.height * Screen.width * 0.8f;
        float worldScreenWidth = worldScreenHeight * (width / height);

        if (worldScreenWidth > maxWorldScreenWidth) {
            worldScreenWidth = maxWorldScreenWidth;
            worldScreenHeight = worldScreenWidth * (height / width);
        }

        float newWidth = worldScreenWidth / width;
        float newHeight = worldScreenHeight / height;

        spriteToCrop.transform.localScale = new Vector3(newWidth, newHeight);
    }

    //! \brief This method will load a PNG image
    //! This method will create a Texture2D object and applies PNG image to it
    //! \param filePath. String contains filepath
    //! \return void
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

    //! \brief This method will save the texture in the database
    //! \return void
    public void saveTexture() {
        if (FileBrowser.selectedPictureID == -1) {
            FileBrowser.selectedPictureID = GetComponent<dbController>().insertPicture(spriteToCrop.GetComponent<SpriteRenderer>().sprite.texture, MainMenu.selectedSubjectID);
        }
           
        StartCoroutine(generateTexturesFromList(answers));
    }
    

    public void SetPlane() {
        plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
        plane.transform.localScale = spriteToCrop.GetComponent<SpriteRenderer>().bounds.size;
        plane.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        plane.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white);
        plane.transform.position = new Vector3(0, 0, 9);
    }

    //! \brief This method removes selected image from database
    //! \return void
    public void DeleteImage() {
        if (FileBrowser.selectedPictureID != -1) {
            dbController db = GetComponent<dbController>();
            db.deletePicture(FileBrowser.selectedPictureID);
        }
        Application.LoadLevel("ImageOverview");
    }

	IEnumerator Start () {
        if (FileBrowser.selectedPictureID != -1) {
            dbController db = GetComponent<dbController>();

            Texture2D tex = db.getPicture(FileBrowser.selectedPictureID);
            Rect rec = new Rect(0, 0, tex.width, tex.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite newPlanet = Sprite.Create(tex, rec, pivot);

            spriteToCrop.GetComponent<SpriteRenderer>().sprite = newPlanet;
            ResizeTexture();
            LoadRects();
        } else if (FileBrowser.selectedFile != "") {
            WWW file = new WWW("file://" + FileBrowser.selectedFile);
            yield return file;

            Texture2D tex = file.texture;
            Rect rec = new Rect(0, 0, tex.width, tex.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite newPlanet = Sprite.Create(tex, rec, pivot);

            spriteToCrop.GetComponent<SpriteRenderer>().sprite = newPlanet;

            ResizeTexture();
        }

        SetPlane();
        spriteToCrop.AddComponent<BoxCollider2D>();

		isMousePressed = false;
//		Instantiate rectangle sides
		leftLine = createAndGetLine("LeftLine");
		rightLine = createAndGetLine("RightLine");
		topLine = createAndGetLine("TopLine");
		bottomLine = createAndGetLine("BottomLine");

        //saveTexture();
	}

    //! \brief Creates line through LineRenderer component
    //! \return void
	private LineRenderer createAndGetLine (string lineName) {
		GameObject lineObject = new GameObject(lineName);
		LineRenderer line = lineObject.AddComponent<LineRenderer>();
		line.SetWidth(0.03f,0.03f);
		line.SetVertexCount(2);
		return line;
	}

    //! \brief Load rects from database
    //! \return void
    public void LoadRects() {
        dbController db = GetComponent<dbController>();
        List<dbController.Rectangle> rects = db.getRect(FileBrowser.selectedPictureID);

        SpriteRenderer spriteRenderer = spriteToCrop.GetComponent<SpriteRenderer>();

        for (int i = 0; i < rects.Count; i++) {
            leftLine = createAndGetLine("LeftLine");
		    rightLine = createAndGetLine("RightLine");
		    topLine = createAndGetLine("TopLine");
		    bottomLine = createAndGetLine("BottomLine");
            
            float xStart = (spriteRenderer.sprite.bounds.center.x - (spriteRenderer.transform.localScale.x * spriteRenderer.sprite.bounds.size.x / 2f)) + (spriteRenderer.transform.localScale.x * spriteRenderer.sprite.bounds.size.x / spriteRenderer.sprite.texture.width * rects[i].rect.x);
            float yStart = (spriteRenderer.sprite.bounds.center.y - (spriteRenderer.transform.localScale.y * spriteRenderer.sprite.bounds.size.y / 2f)) + (spriteRenderer.transform.localScale.y * spriteRenderer.sprite.bounds.size.y / spriteRenderer.sprite.texture.height * rects[i].rect.y);
            float xEnd = (spriteRenderer.sprite.bounds.center.x - (spriteRenderer.transform.localScale.x * spriteRenderer.sprite.bounds.size.x / 2f)) + (spriteRenderer.transform.localScale.x * spriteRenderer.sprite.bounds.size.x / spriteRenderer.sprite.texture.width * (rects[i].rect.x + rects[i].rect.width));
            float yEnd = (spriteRenderer.sprite.bounds.center.y - (spriteRenderer.transform.localScale.y * spriteRenderer.sprite.bounds.size.y / 2f)) + (spriteRenderer.transform.localScale.y * spriteRenderer.sprite.bounds.size.y / spriteRenderer.sprite.texture.height * (rects[i].rect.y + rects[i].rect.height));

            startPoint = new Vector3(xStart, yStart, 0);
            endPoint = new Vector3(xEnd, yEnd, 0);

            drawRectangle();

            GameObject g = new GameObject("Question");
            BoxCollider2D b = g.AddComponent<BoxCollider2D>();
            b.size = new Vector2(endPoint.x - startPoint.x, endPoint.y - startPoint.y);
            Vector3 pos = startPoint - ((startPoint - endPoint) / 2);
            g.transform.position = new Vector3(pos.x, pos.y, 0);
            ImageAnswer ia = g.AddComponent<ImageAnswer>();
            ia.leftLine = leftLine;
            ia.topLine = topLine;
            ia.rightLine = rightLine;
            ia.bottomLine = bottomLine;

            ia.rect = rects[i].rect;
            ia.SavedInDatabase = true;
            ia.rectID = rects[i].rectID;
        }
    }

    //! \brief Update is called every frame.
    //! \return void
	void Update () {
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

    //! \brief Following method crops as per displayed cropping areae.
    //! Since the GUI is resizeable every frame some rects need to be 
    //! recalculated
    //! \return void
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

            ia.rect = croppedSpriteRect;
            answers.Add(ia);
        }
    }

    //! \brief For 2DCollider.
    //! \return void
	private bool isSpriteTouched(GameObject sprite) {
		Vector3 posFor2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit2D = Physics2D.Raycast(posFor2D, Vector2.zero);

		if (hit2D != null && hit2D.collider != null) {
			if(hit2D.collider.name.Equals(sprite.name))
				return true;
		}

		return false;
	}

    //! \brief This method generature the texture which he will recieve from the database
    //! \param List<ImageAnswer> answers.
    //! \return void
    public IEnumerator generateTexturesFromList(List<ImageAnswer> answers){
        yield return new WaitForEndOfFrame();
        
        dbController db = GetComponent<dbController>();
        Debug.Log(answers.Count);
        for (int i = 0; i < answers.Count; i++) {
            answers[i].rectID = db.insertRect(answers[i].rect, FileBrowser.selectedPictureID);
        }

        answers.Clear();

        for (int i = 0; i < removeFromDatabase.Count; i++) {
            db.deleteRect(removeFromDatabase[i].rectID);
        }

        Application.LoadLevel("ImageOverview");
    }
}
