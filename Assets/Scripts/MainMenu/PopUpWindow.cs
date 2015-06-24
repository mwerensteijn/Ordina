using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopUpWindow : MonoBehaviour
{
    public GameObject popUpPanel;
    public Text popUpText;

    public void disablePopUp()
    {
        popUpPanel.SetActive(false);
    }

    public void enablePopUp(string text)
    {
        popUpText.text= text;
        popUpPanel.SetActive(true);
    }
}
