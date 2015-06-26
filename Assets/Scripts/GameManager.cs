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
    //! \brief SetPlayerName
    //! \param string playername
    //! \return void
    public void setPlayerName(string name)
    {
        //playername inserten in de dll
        playerName = name;
    }

    //! \brief GetPlayerName
    //! \return string playerName
    public string getPlayerName()
    {
        return playerName;
    }

    //! \brief SetSubject
    //! \param string subject
    //! \return void
    public void setSubject(string subject)
    {
        this.subject = subject;
    }
    //! \brief GetSubject
    //! \return string subject
    public string getSubject()
    {
        return subject;
    }

    //! \brief SetSpelID
    //! \param int id
    //! \return void
    public void setSpelID(int id)
    {
        spelID = id;
    }
    //! \brief GetPlayerName
    //! \return int spelID
    public int getSpelID()
    {
        return spelID;
    }

    //! \brief Start PictureHunt minigame
    //! \return void
    public void StartPictureHunt() {
        setSpelID(CreateNewSpel());
        Application.LoadLevel("Basic");
    }

    //! \brief Start Fly-High minigame
    //! \return void
    public void StartMultipleChoice() {
        setSpelID(CreateNewSpel());
        Application.LoadLevel("MultipleChoice");
    }

    //! \brief CreateNewSpel
    //! \return int spelID
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
