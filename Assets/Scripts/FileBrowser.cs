using UnityEngine;
using System.Collections;
using System.IO;

public class FileBrowser : MonoBehaviour {
    public GUISkin skin;
    private Vector2 scrollPosition = Vector2.zero;

    private string path = "C:\\";
    private string[] fileEntries;
    private string[] directoryEntries;
    private int entries;
    private int selectedFileEntry = -1;

    public static string selectedFile = "";

	// Use this for initialization
	void Start () {
        ProcessPath();
	}

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

    void OnGUI() {
        GUI.skin = skin;

        GUI.Box(new Rect(0, 0, 500, 340), "Open file");
        GUI.Button(new Rect(477, 3, 21, 21), "X");
        GUI.Label(new Rect(20, 50, 100, 20), "Look in:");
        path = GUI.TextField(new Rect(120, 50, 200, 20), path);
        if (GUI.Button(new Rect(330, 50, 20, 20), "↑")) {
            int index = path.LastIndexOf('\\', path.Length - 2, path.Length - 2);

            if (index > 0) {
                path = path.Substring(0, index);
                ProcessPath();
            }
        }
        
        GUI.Box(new Rect(20, 80, 465, 200), "");

        if (Event.current.isKey) {
            switch (Event.current.keyCode) {
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    Event.current.Use();    // Ignore event, otherwise there will be control name conflicts!
                    ProcessPath();
                    break;
            }
        }

        int scrollBarHeight = 20 * entries;
        if (scrollBarHeight < 200) {
            scrollBarHeight = 200;
        }
        scrollPosition = GUI.BeginScrollView(new Rect(20, 80, 465, 200), scrollPosition, new Rect(0, 0, 200, scrollBarHeight), false, true);

        for (int i = 0; i < directoryEntries.Length; i++) {
            if (GUI.Button(new Rect(2, (i * 22), 447, 20), directoryEntries[i])) {
                path += directoryEntries[i];
                selectedFileEntry = -1;
                ProcessPath();
            }
        }

        GUIStyle selectedFileStyle = new GUIStyle(GUI.skin.button);
        selectedFileStyle.normal.textColor = new Color(1, 0.5568f, 0);

        for (int i = 0; i < fileEntries.Length; i++) {
            if (i == selectedFileEntry) {
                GUI.Button(new Rect(2, ((i + directoryEntries.Length) * 22), 447, 20), fileEntries[i], selectedFileStyle);
            } else {
                if (GUI.Button(new Rect(2, ((i + directoryEntries.Length) * 22), 447, 20), fileEntries[i])) {
                    selectedFileEntry = i;
                }
            }
            
        }

        GUI.EndScrollView();
        if (GUI.Button(new Rect(360, 310, 120, 20), "Select image")) {
            if (selectedFileEntry >= 0) {
                selectedFile = path + fileEntries[selectedFileEntry];

                Application.LoadLevel("CMS");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
