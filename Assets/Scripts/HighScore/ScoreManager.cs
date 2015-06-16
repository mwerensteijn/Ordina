using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.HighScore;
using System;

/// <summary>

/// High score manager.

/// Local highScore manager

/// this is a singleton class.  to access these functions, use HighScoreManager._instance object.

/// eg: HighScoreManager._instance.SaveHighScore("boon",1232);

/// No need to attach this to any game object, thought it would not create errors attaching.

/// </summary>

public class ScoreManager : MonoBehaviour, IScoreManager
{

    private static ScoreManager m_instance;

 //   private List<Score> Scores = new List<Score>();
    private int score = 0;

    public static ScoreManager _instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("HighScoreManager").AddComponent<ScoreManager>();
            }
            return m_instance;
        }
    }

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public List<HighScore> GetPlayerHighScores(int playerId) 
    {
        //database connectie hier.
        return null;
    }

    public void SaveHighScore(Enum minigameType, int totalScore, int playerId)
    {
        //database connectie hier.
    }
}
