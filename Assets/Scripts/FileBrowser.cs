using UnityEngine;
using System.Collections;
using System.IO;

public class FileBrowser : MonoBehaviour {
    public GUISkin skin;
    private Vector2 scrollPosition = Vector2.zero;

    private Rect windowRect;
    private Rect browserRect;

    public Texture2D _dropDownImage;
    public Texture2D _backButtonImage;

    private string path = "C:\\";
    private string[] fileEntries;
    private string[] directoryEntries;
    private int entries;
    private int selectedFileEntry = -1;

    public static string selectedFile = "";
    public static int selectedPictureID = -1;

    //! \brief Start is called on the fram when a script is enabled.
    //! This method will call a method called ProcessPath
    //! \return void
	void Start () {
        windowRect = new Rect(Screen.width / 3, Screen.height / 6, Screen.width / 3, Screen.height / 8);
        browserRect = new Rect(windowRect.x, windowRect.y + windowRect.height, Screen.width / 3, Screen.height / 3);
        ProcessPath();
	}

    //! \brief This method is literally the file browser code
    //! \return void
    public void ProcessPath() {
        if (File.Exists(path)) {
            Debug.Log("Error: the path is a file.");
            fileEntries = null;
            directoryEntries = null;

            entries = 0;
        } else if (Directory.Exists(path)) {
            if (!path.EndsWith("\\")) {
                path += "\\";
            }

            fileEntries = Directory.GetFiles(path);
            for (int i = 0; i < fileEntries.Length; i++) {
                fileEntries[i] = fileEntries[i].Substring(path.Length);
            }
            
            directoryEntries = Directory.GetDirectories(path);
            for (int i = 0; i < directoryEntries.Length; i++) {
                directoryEntries[i] = directoryEntries[i].Substring(path.Length);
            }
                
            entries = fileEntries.Length + directoryEntries.Length;
        }
    }

    //! \brief OnGUI is called for rendering and handling GUI events. 
    //! Creates GUI for the File Browser
    //! \return void
    void OnGUI() {
        GUI.skin = skin;

        //GUI.Box(new Rect(0, 0, 500, 340), "Open file");
        GUI.Box(windowRect, "Open file");
        
        //if(GUI.Button(new Rect(477, 3, 21, 21), _backButtonImage))
        if (GUI.Button(new Rect(windowRect.x + (windowRect.width * 0.94f), windowRect.y + (windowRect.height * 0.05f), windowRect.width * 0.05f, windowRect.height * 0.2f), _backButtonImage))
        {
            Application.LoadLevel("ImageOverview");
        }

        //GUI.Label(new Rect(20, 50, 100, 20), "Look in:");
        GUI.Label(new Rect(windowRect.x + (windowRect.width * 0.05f), windowRect.y + (windowRect.height * 0.6f), windowRect.width * 0.2f, windowRect.height * 0.4f), "Look in: ");

        //path = GUI.TextField(new Rect(120, 50, 200, 20), path);
        path = GUI.TextField(new Rect(windowRect.x + (windowRect.width * 0.3f), windowRect.y + (windowRect.height * 0.6f), windowRect.width * 0.4f, windowRect.height * 0.3f), path);

        
        //if (GUI.Button(new Rect(330, 50, 20, 20), _dropDownImage)) {
        if(GUI.Button(new Rect(windowRect.x + (windowRect.width * 0.75f), windowRect.y + (windowRect.height * 0.6f), windowRect.width * 0.05f, windowRect.height * 0.3f),  _dropDownImage)){
            int index = path.LastIndexOf('\\', path.Length - 2, path.Length - 2);

            if (index > 0) {
                path = path.Substring(0, index);
                ProcessPath();
            }
        }
        
        //GUI.Box(new Rect(20, 80, 465, 200), "");
        GUI.Box(browserRect, "");

        if (Event.current.isKey) {
            switch (Event.current.keyCode) {
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    Event.current.Use();    // Ignore event, otherwise there will be control name conflicts!
                    ProcessPath();
                    break;
            }
        }
        float scrollBarHeight = 22 * entries;
        if (scrollBarHeight < browserRect.height)
        {
            scrollBarHeight = browserRect.height;
        }

        //scrollPosition = GUI.BeginScrollView(new Rect(20, 80, 465, 200), scrollPosition, new Rect(0, 0, 200, scrollBarHeight), false, true);
        scrollPosition = GUI.BeginScrollView(browserRect, scrollPosition, new Rect(0, 0, browserRect.height, scrollBarHeight), false, true);

        for (int i = 0; i < directoryEntries.Length; i++) {
            if (GUI.Button(new Rect(2, (i * 22), browserRect.width * 0.96f, browserRect.height / 10f), directoryEntries[i]))
            {
                path += directoryEntries[i];
                selectedFileEntry = -1;
                ProcessPath();
            }
        }

        GUIStyle selectedFileStyle = new GUIStyle(GUI.skin.button);
        selectedFileStyle.normal.textColor = new Color(1, 0, 0);

        for (int i = 0; i < fileEntries.Length; i++) {
            if (i == selectedFileEntry) {
                GUI.Button(new Rect(2, ((i + directoryEntries.Length) * 22), browserRect.width * 0.96f, browserRect.height / 10f), fileEntries[i], selectedFileStyle);
            } else {
                if (GUI.Button(new Rect(2, ((i + directoryEntries.Length) * 22), browserRect.width * 0.96f, browserRect.height / 10f), fileEntries[i]))
                {
                    selectedFileEntry = i;
                }
            }
            
        }

        GUI.EndScrollView();

        if (GUI.Button(new Rect(browserRect.x + (browserRect.width * 0.8f), browserRect.y + (browserRect.height * 1.01f), browserRect.width * 0.2f, browserRect.height * 0.1f), "Select image")) {
            if (selectedFileEntry >= 0) {
                selectedFile = path + fileEntries[selectedFileEntry];
                selectedPictureID = -1;

                Application.LoadLevel("CMS");
            }
        }
    }

    //! \brief Update is called every frame.
    //! \return void
	void Update () {
        windowRect = new Rect(Screen.width / 3, Screen.height / 6, Screen.width / 3, Screen.height / 8);
	}
}
