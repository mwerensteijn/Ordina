using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageOverview : MonoBehaviour {
    public GUISkin skin;
    public GameObject[] g;
    private GridLayoutGroup gridLayoutGroup;
    public int imagesAmount = 0;
    public GameObject meerAfbeeldingen;

	// Use this for initialization
	void Start () {
        List<Texture2D> textures = GetComponent<dbController>().getPicturesTexture(9);
        imagesAmount = textures.Count;

        if (imagesAmount > 9) {
            meerAfbeeldingen.SetActive(true);
        }

        g = new GameObject[imagesAmount];
        gridLayoutGroup = GameObject.FindGameObjectWithTag("ImageOverview").GetComponent<GridLayoutGroup>();

        for (int i = 0; i < imagesAmount; i++) {
            g[i] = new GameObject();
            g[i].AddComponent<ImageOverviewImage>();
            Image r = g[i].AddComponent<Image>();
            r.preserveAspect = true;

            // WWW file = new WWW("file:///C:/b.jpg");
            //yield return file;

            Texture2D tex = textures[i];
            Rect rec = new Rect(0, 0, tex.width, tex.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite newPlanet = Sprite.Create(tex, rec, pivot);

            r.sprite = newPlanet;

            g[i].transform.SetParent(gridLayoutGroup.transform, false);
            g[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            g[i].transform.localPosition = Vector3.zero;
            BoxCollider2D b = g[i].AddComponent<BoxCollider2D>();
            b.size = gridLayoutGroup.cellSize;
        }
	}

    public void AddImage() {
        Application.LoadLevel("FileBrowser");
    }

	// Update is called once per frame
	void Update () {

	}
}
