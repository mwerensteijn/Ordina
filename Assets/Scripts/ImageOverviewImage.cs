using UnityEngine;
using System.Collections;


public class ImageOverviewImage : MonoBehaviour {

    public void OnMouseDown() {
        FileBrowser.selectedTexture = GetComponent<UnityEngine.UI.Image>().sprite.texture;
        Application.LoadLevel("CMS");
    }
}
