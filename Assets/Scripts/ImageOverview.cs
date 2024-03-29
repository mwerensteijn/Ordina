﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageOverview : MonoBehaviour {
    public GUISkin skin;
    public GameObject[] g;
    private GridLayoutGroup gridLayoutGroup;
    public int imagesAmount = 0;
    public GameObject meerAfbeeldingen;

    //! \brief Start is called on the frame when a script is enabled.
    //! This method will get all the pictures of the selected subject from the
    //! database and the user will get a overview of that
    //! \return void
	void Start () {
        Scrollbar s = GameObject.FindGameObjectWithTag("ImageOverviewScrollbar").GetComponent<Scrollbar>();
        s.gameObject.SetActive(false);

        List<dbController.Picture> textures = GetComponent<dbController>().getPictures(MainMenu.selectedSubjectID);
        imagesAmount = textures.Count;

        if (imagesAmount > 9) {
            s.gameObject.SetActive(true);
        }

        g = new GameObject[imagesAmount];
        gridLayoutGroup = GameObject.FindGameObjectWithTag("ImageOverview").GetComponent<GridLayoutGroup>();

        for (int i = 0; i < imagesAmount; i++) {
            g[i] = new GameObject();
            g[i].AddComponent<ImageOverviewImage>().pictureID = textures[i].pictureID;
            Image r = g[i].AddComponent<Image>();
            r.preserveAspect = true;

            // WWW file = new WWW("file:///C:/b.jpg");
            //yield return file;

            Texture2D tex = textures[i].texture;
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

        s.value = 1;
	}

    //! \brief This method will change the scene to scene named "GUI"
    //! \return void
    public void Exit() {
        Application.LoadLevel("GUI");
    }

    //! \brief This method will change the scene to scene named "FileBrowser"
    //! \return void
    public void AddImage() {
        Application.LoadLevel("FileBrowser");
    }

    //! \brief Update is called every frame.
    //! \return void
	void Update () {

	}
}
