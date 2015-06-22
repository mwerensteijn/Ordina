using UnityEngine;
using System.Collections;

public class ImageOverviewImage : MonoBehaviour {
    public int pictureID = -1;

    public void OnMouseDown() {
        FileBrowser.selectedPictureID = pictureID;
        Application.LoadLevel("CMS");
    }
}
