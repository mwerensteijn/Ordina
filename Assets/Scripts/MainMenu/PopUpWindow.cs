using UnityEngine;
using System.Collections;

public class PopUpWindow : MonoBehaviour
{
    private Rect windowRect = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 2.5f, Screen.height / 8);
    private string text;
    private bool isActive = false;
    void OnGUI()
    {
        if (isActive)
        {
            windowRect = GUI.Window(0, windowRect, ShowGui, text);
        }
    }

    public void disablePopUp()
    {
        isActive = false;
    }

    private void ShowGui(int windowID)
    {
        if (GUI.Button(new Rect(windowRect.width / 3, windowRect.height - (windowRect.height / 2), windowRect.width / 3, windowRect.height / 3), "Doorgaan"))
        {
            isActive = false;
        }
        GUI.FocusWindow(0);
        GUI.DragWindow();
    }
    public void enablePopUp(string text)
    {
        this.text = text;
        isActive = true;
    }
}
