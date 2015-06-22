using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    string playerName = "DefaultPlayerName";
    string subject = "DefaultSubject";
    int spelID = -1; // Default value

    public void setPlayerName(string name)
    {
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
}
