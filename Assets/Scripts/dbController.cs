using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.IO;
using System.Data;
using System.Collections.Generic;

public class dbController : MonoBehaviour {
	protected SqliteConnection dbconn;
    public Texture2D texture;
    //public GameObject plane;
    public byte[] imgByteArr;
	
	void Awake () {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        FileStream fs = new FileStream(Application.dataPath + "/database/DOGGOE.png", FileMode.Open, FileAccess.Read);
        imgByteArr = new byte[fs.Length];

        fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
        fs.Close();

        Texture2D tex = new Texture2D(2,2);

        tex.LoadImage(imgByteArr);

        //Debug things
        insertSubject("Wiskunde");
        int Subbbbje = getSubject("Wiskunde");
        insertQuestion("Wat is 1+1?", Subbbbje);
        List<int> lstr = getQuestionIDs(Subbbbje);
        Debug.Log(lstr[0]);
        insertAnswer("1+1 = 2", Convert.ToInt32(lstr[0]));

        dbconn.Close();

	}

    public int getAmountOfQuestions(string subject)
    {
        // TODO use database
        return 2;
    }

    public int getAmountOfSubImages(string subject, int questionID)
    {
        // TODO use database
        return 3;
    }

    public Vector2[] getSubImageCoordinates(string subject, int questionID, int subImageID)
    {
        // just a test not the real code
        if (subImageID == 1)
        {
            return new Vector2[] { new Vector2(65, 75), new Vector2(85, 80), new Vector2(65, 150), new Vector2(85, 155) };
        }
        else
        {
            return new Vector2[] { new Vector2(25, 75), new Vector2(85, 80), new Vector2(25, 150), new Vector2(85, 155) };
        }
    }

    public String getQuestionAnswer(string subject, int questionID, int subImageID)
    {
        if (subImageID == 1)
        {
            return "test1";
        }
        else
        {
            return "answer"+questionID+"_"+subImageID;
        }
    }

    public Texture2D getBackgroundImage(string subject, int questionID)
    {
        return texture;
    }

    public void insertPicture(Texture2D pic)
    {
        byte[] bytes = null;

        bytes = pic.EncodeToPNG();

        string dbLoc = "URI=file:" + Application.dataPath + "/database/Database.s3db";

        dbconn = new SqliteConnection(dbLoc);

        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        cmd.CommandText = "INSERT INTO Afbeelding(afbeelding) VALUES (@data)";
        cmd.Prepare();

        cmd.Parameters.Add("@data", DbType.Binary, bytes.Length);
        cmd.Parameters["@data"].Value = bytes;
        cmd.ExecuteNonQuery();

        if (cmd.Parameters["@data"].Value != bytes)
        {
            Debug.Log("Values corrupted!");
        }

        dbconn.Close();
    }

    public Texture2D getPicture()
    {
        Texture2D pic = new Texture2D(2,2);

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding FROM Afbeelding WHERE AfbeeldingID=39";
        byte[] data = (byte[])cmd.ExecuteScalar();

        if (data != null && data != imgByteArr)
        {
            pic.LoadImage(data);
        }
        else
        {
            Debug.Log("Entry in tabel met gegeven ID nummer NIET gevonden...");
        }

        dbconn.Close();

        return pic;
    }

    public void insertRect(Rect rect)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        cmd.CommandText = "SELECT MAX(AfbeeldingID) FROM Afbeelding";
        long lastRowID = (long)cmd.ExecuteScalar();
        Debug.Log(lastRowID);

        cmd.CommandText = "INSERT INTO Rechthoek(AfbeeldingID, XCoordinaat, YCoordinaat, Breedte, Hoogte) VALUES(@lastRowID, @x, @y, @width, @height)";
        
        cmd.Parameters.Add(new SqliteParameter("@lastRowID", lastRowID));
        cmd.Parameters.Add(new SqliteParameter("@x", rect.x));
        cmd.Parameters.Add(new SqliteParameter("@y", rect.y));
        cmd.Parameters.Add(new SqliteParameter("@width", rect.width));
        cmd.Parameters.Add(new SqliteParameter("@height", rect.height));
        
        cmd.ExecuteNonQuery();

        dbconn.Close();
    }

    public List<Rect> getRect(int imgID)
    {
        List<Rect> lrect = new List<Rect>();
        Rect rect = new Rect(0,0,0,0);
        
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT * FROM Rechthoek WHERE AfbeeldingID=" + imgID;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            rect.x = Convert.ToSingle(reader[2] + ""); rect.y = Convert.ToSingle(reader[3]+"");
            rect.width = Convert.ToSingle(reader[4] + ""); rect.height = Convert.ToSingle(reader[5] + "");
            lrect.Add(rect);
        }

        dbconn.Close();
        return lrect;
    }

    public void insertQuestion(string question, int subjectID)
    {
        //int subjectID = 0;
        try
        {
            dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
            dbconn.Open();

            SqliteCommand cmd = new SqliteCommand();

            cmd.Connection = dbconn;
            //cmd.CommandText = "SELECT OnderwerpID FROM Onderwerp WHERE Onderwerp=" + subject;
            //subjectID = (int)((Int64)cmd.ExecuteScalar());

            cmd.CommandText = "INSERT INTO Vraag(Vraag, OnderwerpID) VALUES(@vraag, @subject)";
            cmd.Parameters.Add(new SqliteParameter("@vraag", question));
            cmd.Parameters.Add(new SqliteParameter("@subject", subjectID));
            cmd.ExecuteNonQuery();

            dbconn.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public List<int> getQuestionIDs(int subjectID)
    {
        List<int> lstr = new List<int>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT * FROM Vraag WHERE OnderwerpID=" + subjectID;
        SqliteDataReader reader = cmd.ExecuteReader();

        int index = 0;
        while (reader.Read())
        {
            lstr.Add(Convert.ToInt32(reader[0]+""));
            index++;
        }

        dbconn.Close();

        return lstr;


    }

    public List<string> getQuestions(int subjectID)
    {
        List<string> lstr = new List<string>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT * FROM Vraag WHERE OnderwerpID=" + subjectID;
        SqliteDataReader reader = cmd.ExecuteReader();

        int index = 0;
        while (reader.Read())
        {
            lstr.Add(reader[3]+"");
            index++;
        }

        dbconn.Close();

        return lstr;
    }

    public void insertAnswer(string answer, int questionID)
    {
        //int questionID;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        //cmd.CommandText = "SELECT VraagID FROM Vraag WHERE Vraag=" + question;
        //questionID = (int)((Int64)cmd.ExecuteScalar());


        cmd.CommandText = "INSERT INTO Antwoord(Antwoord, VraagID) VALUES(@antwoord, @vraagID)";

        cmd.Parameters.Add(new SqliteParameter("@antwoord", answer));
        cmd.Parameters.Add(new SqliteParameter("@vraagID", questionID));
        cmd.ExecuteNonQuery();

        dbconn.Close();
    }

    public List<string> getAnswer(int questionID)
    {
        List<string> answers = new List<string>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT * FROM Antwoord WHERE VraagID=" + questionID;
        SqliteDataReader reader = cmd.ExecuteReader();

        int index = 0;
        while (reader.Read())
        {
            answers.Add(reader[2] + "");
            index++;
        }

        dbconn.Close();

        return answers;
    }

    public void insertSubject(string subject)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "INSERT INTO Onderwerp(Subject) VALUES(@subj)";

        cmd.Parameters.Add(new SqliteParameter("@subj", subject));
        cmd.ExecuteNonQuery();

        dbconn.Close();
    }

    public int getSubject(string subject)
    {
        int answerID = -1;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT OnderwerpID FROM Onderwerp WHERE Subject='" + subject + "'";// + subject;
        answerID = (int)((Int64)cmd.ExecuteScalar());

        dbconn.Close();

        return answerID;
    }

}