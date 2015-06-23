using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string playerName = "DefaultPlayerName";
    [SerializeField]
    private string subject = "DefaultSubject";
    [SerializeField]
    private int spelID = -1; // Default value

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
