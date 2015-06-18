using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageOverview : MonoBehaviour {
    public GUISkin skin;
    public GameObject[] g;
    public GameObject imageOverview;
    public GridLayoutGroup gridLayoutGroup;

	// Use this for initialization
	IEnumerator Start () {
        g = new GameObject[9];
        gridLayoutGroup = GameObject.FindGameObjectWithTag("ImageOverview").GetComponent<GridLayoutGroup>();

        for (int i = 0; i < 9; i++) {
            g[i] = new GameObject();
            g[i].AddComponent<ImageOverviewImage>();
            Image r = g[i].AddComponent<Image>();
            r.preserveAspect = true;


            WWW file = new WWW("file:///C:/b.jpg");
            yield return file;

            Texture2D tex = file.texture;
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

	// Update is called once per frame
	void Update () {
	    
	}
}
