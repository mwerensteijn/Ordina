using UnityEngine;
using System.Collections;

public class Question {
    public string question;
    public string[] answers = new string[3];
    public string correctAnswer;

    //! \brief Randomize is called to randomize the position of the answers.
    //! The answers will be randomized using the knuth / fisher-yates algorithm.
    //! \return void
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
