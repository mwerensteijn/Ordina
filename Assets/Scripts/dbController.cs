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
        //testshit();
	}

    public void testshit()
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        //FileStream fs = new FileStream(Application.dataPath + "/database/DOGGOE.png", FileMode.Open, FileAccess.Read);
        //imgByteArr = new byte[fs.Length];

        //fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
        //fs.Close();

        Texture2D tex = new Texture2D(2, 2);

        //tex.LoadImage(imgByteArr);

        List<int> lsss = getPictureIDs(9);

        for (int i = 0; i < lsss.Count; i++)
        {
            Debug.Log(lsss[i]);
        }

            // List<Texture2D> test = getPictures(getSubject("Wiskunde"));

            //tex = test[0];

            //MeshRenderer rend = plane.GetComponent<MeshRenderer>();
            //rend.material.SetTexture(1, tex);

            //Debug things
            //insertSubject("Wiskunde");
            //int Subbbbje = getSubject("Wiskunde");
            //insertQuestion("Wat is 1+1?", Subbbbje);
            //List<int> lstr = getQuestionIDs(Subbbbje);
            //Debug.Log(lstr[0]);
            //insertAnswer("1+1 = 2", Convert.ToInt32(lstr[0]));

            dbconn.Close();
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

    public List<int> getPictureIDs(int subID)
    {
        List<int> pic = new List<int>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding.AfbeeldingID FROM Vraag Vraag, Afbeelding Afbeelding, Onderwerp Onderwerp WHERE Vraag.AfbeeldingID = Afbeelding.AfbeeldingID AND Vraag.OnderwerpID = " + subID;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            pic.Add(Convert.ToInt32(reader[0]));
        }

        dbconn.Close();

        return pic;
    }

    public List<Texture2D> getPictures(int subID)
    {
        List<Texture2D> pic = new List<Texture2D>();
        Texture2D tex = new Texture2D(2, 2);

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding.Afbeelding FROM Vraag Vraag, Afbeelding Afbeelding, Onderwerp Onderwerp WHERE Vraag.AfbeeldingID = Afbeelding.AfbeeldingID AND Vraag.OnderwerpID = " + subID;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read()) 
        {
            byte[] data = (byte[])reader[1];

            if (data != null)
            {
                tex.LoadImage(data);
                pic.Add(tex);
                Debug.Log("Entry is gevonden!");
            }
            else
            {
                Debug.Log("Entry in tabel met gegeven ID nummer NIET gevonden...");
            }
        }

        dbconn.Close();

        return pic;
    }

    public Texture2D getPicture(int questionID)
    {
        Texture2D pic = new Texture2D(2, 2);

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding.Afbeelding FROM Vraag Vraag, Afbeelding Afbeelding WHERE Vraag.AfbeeldingID = Afbeelding.AfbeeldingID AND Vraag.VraagID = " + questionID;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            pic.LoadImage((byte[])reader[1]);
        }

        dbconn.Close();

        return pic;
    }

    public int getPictureID(Texture2D img)
    {
        int picID = 0;
        byte[] bytes = null;

        bytes = img.EncodeToPNG();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding.Afbeelding FROM Afbeelding Afbeelding WHERE Afbeelding.Afbeelding=" + bytes;
        picID = (int)((Int32)cmd.ExecuteScalar());

        dbconn.Close();

        return picID;
    }

    public int getPictureID(byte[] img)
    {
        int picID = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding.Afbeelding FROM Afbeelding Afbeelding WHERE Afbeelding.Afbeelding=" + img;
        picID = (int)((Int32)cmd.ExecuteScalar());

        dbconn.Close();

        return picID;
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
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        //cmd.CommandText = "SELECT OnderwerpID FROM Onderwerp WHERE Onderwerp=" + subject;
        //subjectID = (int)((Int64)cmd.ExecuteScalar());

        cmd.CommandText = "INSERT INTO Vraag(Vraag, OnderwerpID) VALUES(@vraag,@subject)";
        cmd.Parameters.Add(new SqliteParameter("@vraag", question));
        cmd.Parameters.Add(new SqliteParameter("@subject", subjectID));
        cmd.ExecuteNonQuery();

        dbconn.Close();
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

        //int index = 0;
        while (reader.Read())
        {
            lstr.Add(Convert.ToInt32(reader[0]+""));
            //index++;
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
            lstr.Add(reader[3] + "");
            index++;
        }

        dbconn.Close();

        return lstr;
    }

    public int getQuestionID(string question)
    {
        int id = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT VraagID FROM Vraag WHERE Vraag='" + question + "'";

        id = Convert.ToInt32(cmd.ExecuteScalar());

        dbconn.Close();

        return id;
    }

    public string getQuestion(int questionID)
    {
        string question = "";

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT Vraag FROM Vraag WHERE VraagID=" + questionID;

        question = cmd.ExecuteScalar() + "";

        dbconn.Close();

        return question;
    }

    public void insertAnswer(string answer, int questionID, bool correct)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;

        cmd.CommandText = "INSERT INTO Antwoord(Antwoord, VraagID, Correct) VALUES(@antwoord, @vraagID, @correct)";

        cmd.Parameters.Add(new SqliteParameter("@antwoord", answer));
        cmd.Parameters.Add(new SqliteParameter("@vraagID", questionID));
        cmd.Parameters.Add(new SqliteParameter("@correct", correct));
        cmd.ExecuteNonQuery();

        dbconn.Close();
    }

    public List<string> getAnswers(int questionID)
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
            answers.Add(reader[1] + "");
            index++;
        }

        dbconn.Close();

        return answers;
    }

    public List<int> getAnswerIDs(string question)
    {
        List<int> answers = new List<int>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT * FROM Antwoord WHERE Vraag=" + question;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            answers.Add((int)((Int32)reader[0]));
        }

        dbconn.Close();

        return answers;
    }

    public bool getAnswerCorrect(int answerID)
    {
        bool answer = false;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT Correct FROM Antwoord WHERE AntwoordID=" + answerID;
        answer = (bool)cmd.ExecuteScalar();

        dbconn.Close();

        return answer;
    }

    public bool getAnswerCorrect(string answerStr)
    {
        bool answer = false;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT Correct FROM Antwoord WHERE Antwoord=" + answerStr;
        answer = (bool)cmd.ExecuteScalar();

        dbconn.Close();

        return answer;
    }

    public void insertSubject(string subject)
    {
        List<string> strl = getSubjects();
        bool insert = true;

        for (int i = 0; i < strl.Count; i++)
        {
            if (subject == strl[i])
            {
                insert = false;
            }
        }
        if (insert)
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
    }

    public List<string> getSubjects()
    {

        List<string> subjects = new List<string>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Subject FROM Onderwerp";
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            subjects.Add(reader[0] + "");
        }
        dbconn.Close();

        return subjects;
    }

    public int getSubjectID(string subject)
    {
        int answerID = -1;
        string bryanisboos;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();
        Debug.Log(subject);
        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT OnderwerpID FROM Onderwerp WHERE Subject ='"+ subject+ "'";
        bryanisboos = cmd.ExecuteScalar() + "";
        answerID = Convert.ToInt32(bryanisboos);
        dbconn.Close();
        return answerID;
    }

    public string getSubject(int subject)
    {
        string answer = "";

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT Subject FROM Onderwerp WHERE OnderwerpID='" + subject + "'";
        answer = cmd.ExecuteScalar() + "";

        dbconn.Close();

        return answer;
    }

}