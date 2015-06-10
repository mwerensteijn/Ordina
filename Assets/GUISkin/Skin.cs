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
        

        GUI.Box(new Rect(0, 0, 500, 340), "Open file");
        GUI.Button(new Rect(477, 3, 21, 21), "X");
        GUI.Label(new Rect(20, 50, 100, 20), "Look in:");
        GUI.TextField(new Rect(120, 50, 200, 20), "C:\\");
        
        GUI.Box(new Rect(20, 80, 465, 200), "");
        scrollPosition = GUI.BeginScrollView(new Rect(20, 80, 465, 200), scrollPosition, new Rect(0, 0, 200, 200), false, true);

        GUI.Button(new Rect(0, 0, 445, 20), "Top-left");
        GUI.Button(new Rect(0, 21, 445, 20), "Top-left");
        GUI.Button(new Rect(0, 43, 445, 20), "Top-left");
        GUI.Button(new Rect(0, 65, 445, 20), "Top-left");
        GUI.EndScrollView();


        GUI.Button(new Rect(360, 310, 120, 20), "Select image");
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
