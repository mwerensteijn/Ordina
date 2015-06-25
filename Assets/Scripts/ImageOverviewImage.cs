using UnityEngine;
using System.Collections;

public class ImageOverviewImage : MonoBehaviour {
    public int pictureID = -1;

    //! \brief OnMouseDown will remove the selected picture and load scene called "CMS".
    //! \return void
    public void OnMouseDown() {
        FileBrowser.selectedPictureID = pictureID;
        Application.LoadLevel("CMS");
    }
}
