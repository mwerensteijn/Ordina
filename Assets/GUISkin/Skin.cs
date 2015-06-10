using UnityEngine;
using System.Collections;

public class Skin : MonoBehaviour {
    public GUISkin skin;

    public int selectedGridInt = 0;
    public string[] selStrings = new string[] { "hallo1", "hallo2", "hallo3", "hallo2", "hallo3", "hallo2", "hallo3", "hallo2", "hallo3", "hallo2", "hallo3" };

	// Use this for initialization
	void Start () {
	    
	}

    public Vector2 scrollPosition = Vector2.zero;

    void OnGUI() {
        GUI.skin = skin;
        

        GUI.Box(new Rect(0, 0, 500, 300), "Open file");
        GUI.Button(new Rect(477, 3, 21, 21), "X");
        GUI.Label(new Rect(20, 50, 100, 20), "Look in:");
        GUI.TextField(new Rect(120, 50, 200, 20), "C:\\");

        scrollPosition = GUI.BeginScrollView(new Rect(100, 80, 300, 100), scrollPosition, new Rect(0, 0, 300, 300));
        
        GUI.Button(new Rect(0, 0, 100, 20), "Top-left");
        GUI.Button(new Rect(120, 0, 100, 20), "Top-right");
        GUI.Button(new Rect(0, 180, 100, 20), "Bottom-left");
        GUI.Button(new Rect(120, 180, 100, 20), "Bottom-right");
        GUI.EndScrollView();


/*
        //GUILayout.BeginVertical("hallo", GUILayout.MaxHeight(50));
        GUILayout.BeginScrollView(new Vector2(100, 100));
        selectedGridInt = GUILayout.SelectionGrid(selectedGridInt, selStrings, 1, GUILayout.MaxHeight(2));
        if (GUILayout.Button("Start")) {
            Debug.Log("You choose: " + selStrings[selectedGridInt]);
        }

        GUILayout.EndScrollView();
        //GUILayout.EndVertical();
 * */
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
