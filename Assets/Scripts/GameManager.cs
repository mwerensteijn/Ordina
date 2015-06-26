using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string playerName = "DefaultPlayerName";
    [SerializeField]
    private string subject = "DefaultSubject";
    [SerializeField]
    private int spelID = -1; // Default value

    public dbController _dbcontroller;
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    public void setPlayerName(string name)
    {
        //playername inserten in de dll
        playerName = name;
    }
    public string getPlayerName()
    {
        return playerName;
    }

    public void setSubject(string subject)
    {
        this.subject = subject;
    }
    public string getSubject()
    {
        return subject;
    }

    public void setSpelID(int id)
    {
        spelID = id;
    }
    public int getSpelID()
    {
        return spelID;
    }

    public void StartPictureHunt() {
        setSpelID(CreateNewSpel());
        Application.LoadLevel("Basic");
    }

    public void StartMultipleChoice() {
        setSpelID(CreateNewSpel());
        Application.LoadLevel("MultipleChoice");
    }

    public int CreateNewSpel() 
    {       

        string playerIdString = _dbcontroller.getPlayerID(playerName).ToString();
        if (playerIdString.Trim() == "0") 
        {
            _dbcontroller.insertPlayerData(playerName);
            playerIdString = _dbcontroller.getPlayerID(playerName).ToString();
        }

        int subjectId = _dbcontroller.getSubjectID(subject);
        _dbcontroller.insertGameData(Convert.ToInt32(playerIdString), subjectId);
        return _dbcontroller.getGameID(Convert.ToInt32(playerIdString), subjectId);
    }
}
