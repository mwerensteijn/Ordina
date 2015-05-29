using UnityEngine;
using System.Collections;

public class Question {
    public string question;
    public string[] answers = new string[3];
    public string correctAnswer;
    
    public static void Randomize(string[] texts) {
        // Knuth shuffle algorithm
        for (int t = 0; t < texts.Length; t++) {
            string tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }
}
