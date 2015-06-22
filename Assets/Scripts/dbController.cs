using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.IO;
using System.Data;
using System.Collections.Generic;


/*
 * Default doxygen documentation per method:
 *  //! \brief xxx
    //! \param imgID xxx
    //! \return stringX xxx
 */

public class dbController : MonoBehaviour
{
    protected SqliteConnection dbconn;
    public Texture2D texture;
    //public GameObject plane;
    public byte[] imgByteArr;

    private string dbLoc;

    public struct Picture {
        public int pictureID;
        public Texture2D texture;
    }

    public struct Rectangle {
        public int rectID;
        public Rect rect;
    }

    void Awake()
    {
        dbLoc = "URI=file:" + Application.dataPath + "/database/Database.s3db";
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

    //! \brief This function puts a given texture2D object in the linked database.
    //! \param pic the Texture2D object
    //! \param onderwerpID the ID of the subject you are currently handling within the game.
    //! \return lastRowID The ID of the last picture in the database
    public int insertPicture(Texture2D pic, int onderwerpID)
    {
        byte[] bytes = null;

        bytes = pic.EncodeToPNG();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");

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

        cmd.CommandText = "SELECT MAX(AfbeeldingID) FROM Afbeelding";
        long lastRowID = (long)cmd.ExecuteScalar();

        cmd.CommandText = "INSERT INTO Vraag(AfbeeldingID, OnderwerpID) VALUES(@pictureID, @subjectID)";

        cmd.Parameters.Add(new SqliteParameter("@pictureID", lastRowID));
        cmd.Parameters.Add(new SqliteParameter("@subjectID", onderwerpID));
        cmd.ExecuteNonQuery();

        dbconn.Close();

        return (int) lastRowID;
    }

    //! \brief This function deletes a picture associated to the given imageID or questionID from the database.
    //! \param imgID This represents the imageID, which is default 0.
    //! \param questionID this represents the questionID, which is default 0.
    //! \return void
    public void deletePicture(int imgID = 0, int questionID = 0)
    {
        if (imgID != 0)
        {

            dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
            dbconn.Open();

            SqliteCommand cmd = new SqliteCommand(dbconn);

            deleteRects(imgID);
            cmd.CommandText = "DELETE FROM Afbeelding WHERE AfbeeldingID=" + imgID;
            cmd.ExecuteScalar();

            dbconn.Close();
        }
        else if (questionID != 0)
        {
            dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
            dbconn.Open();

            SqliteCommand cmd = new SqliteCommand(dbconn);
            cmd.CommandText = "SELECT * FROM Vraag WHERE VraagID=" + questionID;
            SqliteDataReader reader = cmd.ExecuteReader();

            int imgID2 = Convert.ToInt32(reader[1]);
            reader.Close();
            try
            {
                deleteRects(imgID2);
            }
            catch (Exception e)
            {
                Debug.Log("Exception thrown: " + e.Message);
            }

            
            cmd.CommandText = "DELETE FROM Afbeelding WHERE AfbeeldingID=" + imgID2;
            cmd.ExecuteScalar();

            dbconn.Close();
        }
    }

    //! \brief This function retrieves all imageID's associated to a given subject ID.
    //! \param subID This represents the subject ID.
    //! \return List<int> This list contains all the ID's of the pictures associated to the given subject ID.
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

    public List<Picture> getPictures(int subID)
    {
        List<Picture> picList = new List<Picture>();
        Picture pic;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding.AfbeeldingID, Afbeelding.Afbeelding FROM Vraag Vraag, Afbeelding Afbeelding WHERE Vraag.AfbeeldingID = Afbeelding.AfbeeldingID AND Vraag.OnderwerpID = " + subID;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            pic = new Picture();
            pic.pictureID = Convert.ToInt32(reader[0]);
            byte[] data = (byte[])reader[1];

            if (data != null)
            {
                pic.texture = new Texture2D(2, 2);
                pic.texture.LoadImage(data);
                picList.Add(pic);
                Debug.Log("Entry is gevonden!");
            }
            else
            {
                Debug.Log("Entry in tabel met gegeven ID nummer NIET gevonden...");
            }
        }

        dbconn.Close();

        return picList;
    }

    public Texture2D getPicture(int pictureID)
    {
        Texture2D pic = new Texture2D(2, 2);

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding.Afbeelding FROM Afbeelding Afbeelding WHERE Afbeelding.AfbeeldingID = " + pictureID;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            pic.LoadImage((byte[])reader[0]);
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
        cmd.CommandText = "SELECT Afbeelding.AfbeeldingID FROM Afbeelding Afbeelding WHERE Afbeelding.Afbeelding=" + bytes;
        picID = Convert.ToInt32(cmd.ExecuteScalar());

        dbconn.Close();

        return picID;
    }

    public int getPictureID(byte[] img)
    {
        int picID = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding.AfbeeldingID FROM Afbeelding Afbeelding WHERE Afbeelding.Afbeelding=" + img;
        picID = Convert.ToInt32(cmd.ExecuteScalar());

        dbconn.Close();

        return picID;
    }

   /* public int getPictureIDs(int subjectID)
    {
        int picID = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Afbeelding.AfbeeldingID FROM Afbeelding, Vraag  WHERE Vraag.OnderwerpID=" + subjectID + " AND Vraag.AfbeeldingID = Afbeelding.AfbeeldingID";
        picID = Convert.ToInt32(cmd.ExecuteScalar());

        dbconn.Close();

        return picID;
    }*/

    public int getPictureID(int questionID)
    {
        int picID = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);
        cmd.CommandText = "SELECT Vraag.AfbeeldingID FROM Vraag  WHERE Vraag.VraagID=" + questionID;
        picID = Convert.ToInt32(cmd.ExecuteScalar());

        dbconn.Close();

        return picID;
    }

    public int insertRect(Rect rect, int pictureID)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        cmd.CommandText = "INSERT INTO Rechthoek(AfbeeldingID, XCoordinaat, YCoordinaat, Breedte, Hoogte) VALUES(@pictureID, @x, @y, @width, @height)";

        if (pictureID == -1) {
            cmd.CommandText = "SELECT MAX(AfbeeldingID) FROM Afbeelding";
            long lastPictureID = (long)cmd.ExecuteScalar();

            cmd.Parameters.Add(new SqliteParameter("@pictureID", lastPictureID));
        } else {
            cmd.Parameters.Add(new SqliteParameter("@pictureID", pictureID));
        }

        cmd.Parameters.Add(new SqliteParameter("@x", rect.x));
        cmd.Parameters.Add(new SqliteParameter("@y", rect.y));
        cmd.Parameters.Add(new SqliteParameter("@width", rect.width));
        cmd.Parameters.Add(new SqliteParameter("@height", rect.height));

        cmd.ExecuteNonQuery();

        cmd.CommandText = "SELECT MAX(RechthoekID) FROM Rechthoek";
        long lastRowID = (long)cmd.ExecuteScalar();

        dbconn.Close();

        return (int) lastRowID;
    }

    public void deleteRect(int rectID) {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        cmd.CommandText = "DELETE FROM Rechthoek WHERE RechthoekID=" + rectID;
        cmd.ExecuteScalar();

        dbconn.Close();
    }

    public void deleteRects(int imgID)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        cmd.CommandText = "DELETE FROM Rechthoek WHERE AfbeeldingID=" + imgID;
        cmd.ExecuteScalar();

        dbconn.Close();
    }

    public List<dbController.Rectangle> getRect(int imgID)
    {
        List<dbController.Rectangle> lrect = new List<dbController.Rectangle>();
        Rect rect = new Rect(0, 0, 0, 0);

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT * FROM Rechthoek WHERE AfbeeldingID=" + imgID;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            rect.x = Convert.ToSingle(reader[2] + ""); 
            rect.y = Convert.ToSingle(reader[3] + "");
            rect.width = Convert.ToSingle(reader[4] + ""); 
            rect.height = Convert.ToSingle(reader[5] + "");

            dbController.Rectangle r = new dbController.Rectangle();
            r.rect = rect;
            r.rectID = Convert.ToInt32(reader[0]);
            lrect.Add(r);
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

    public void deleteQuestion(string question)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        deleteAnswer(getQuestionID(question));
        deletePicture(questionID:getQuestionID(question));
        cmd.CommandText = "DELETE FROM Vraag WHERE Vraag='" + question + "'";
        cmd.ExecuteScalar();

        dbconn.Close();
    }

    public void deleteQuestion(int questionID)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        deleteAnswer(questionID);
        deletePicture(questionID:questionID);
        cmd.CommandText = "DELETE FROM Vraag WHERE VraagID=" + questionID;
        cmd.ExecuteScalar();

        dbconn.Close();
    }

    public List<int> getQuestionIDs(int subjectID, bool isImageQuestion)
    {
        List<int> lstr = new List<int>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");

        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        if (isImageQuestion) {
            cmd.CommandText = "SELECT * FROM Vraag WHERE Vraag IS NULL AND OnderwerpID=" + subjectID;
        } else {
            cmd.CommandText = "SELECT * FROM Vraag WHERE AfbeeldingID IS NULL AND OnderwerpID=" + subjectID;
        
        }
        SqliteDataReader reader = cmd.ExecuteReader();

        //int index = 0;
        while (reader.Read())
        {
            lstr.Add(Convert.ToInt32(reader[0] + ""));
            //index++;
        }

        dbconn.Close();

        return lstr;
    }

    public List<int> getQuestionIDs(int subjectID) {
        List<int> lstr = new List<int>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");

        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT * FROM Vraag WHERE OnderwerpID=" + subjectID;
 
        SqliteDataReader reader = cmd.ExecuteReader();

        //int index = 0;
        while (reader.Read()) {
            lstr.Add(Convert.ToInt32(reader[0] + ""));
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
        if (correct == true) 
            cmd.Parameters.Add(new SqliteParameter("@correct", 1));
        else
            cmd.Parameters.Add(new SqliteParameter("@correct", 0));
        cmd.ExecuteNonQuery();

        dbconn.Close();
    }

    public void deleteAnswer(int questionID)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        cmd.CommandText = "DELETE FROM Antwoord WHERE VraagID=" + questionID;
        cmd.ExecuteScalar();

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

    public List<int> getAnswerIDs(int questionId)
    {
        List<int> answers = new List<int>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT * FROM Antwoord WHERE VraagID=" + questionId;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            answers.Add(Convert.ToInt32(reader[0]));
        }

        dbconn.Close();

        return answers;
    }

    public string getAnswer(int answerId)
    {
        string answer = "";

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT * FROM Antwoord WHERE AntwoordID=" + answerId;
        SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            answer = (string)reader[1];
        }

        dbconn.Close();

        return answer;
    }

    public bool getAnswerCorrect(int answerID)
    {
        bool answer = false;
        int answerINT = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT Correct FROM Antwoord WHERE AntwoordID=" + answerID;
        object result = cmd.ExecuteScalar();
       
        if (result == DBNull.Value) {
            answerINT = 0;
        } else {
            answerINT = Convert.ToInt32(result);
        }
        
        if (answerINT > 0)
            answer = true;

        dbconn.Close();

        return answer;
    }

    public bool getAnswerCorrect(string answerStr)
    {
        bool answer = false;
        int answerINT = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT Correct FROM Antwoord WHERE Antwoord=" + answerStr;
        answerINT = Convert.ToInt32(cmd.ExecuteScalar());

        if (answerINT > 0)
            answer = true;

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

    public void deleteSubject(string subj)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        List<int> lint = getQuestionIDs(getSubjectID(subj));
        for (int i = 0; i < lint.Count; i++)
        {
            deleteQuestion(lint[i]);
        }
        
        cmd.CommandText = "DELETE FROM Onderwerp WHERE Subject='" + subj + "'";
        cmd.ExecuteScalar();

        dbconn.Close();
    }

    public void deleteSubject(int subjID)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        List<int> lint = getQuestionIDs(subjID);
        for (int i = 0; i < lint.Count; i++)
        {
            deleteQuestion(lint[i]);
        }

        cmd.CommandText = "DELETE FROM Onderwerp WHERE OnderwerpID=" + subjID;
        cmd.ExecuteScalar();

        dbconn.Close();
    }

    public List<string> getSubjects()
    {

        List<string> subjects = new List<string>();

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        Debug.Log("URI=file:" + Application.dataPath + "/database/Database.s3db");
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

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT OnderwerpID FROM Onderwerp WHERE Subject ='" + subject + "'";
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

    public void insertScore(string spelerNaam, string onderwerp, int spelID, int score, int tijd, int aantalGesteldeVragen, int correctBeantwoorddeVragen)
    {
        int spelerID = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "INSERT INTO Score(SpelerID, SpelID, Onderwerp, BehaaldeScore, AantalGesteldeVragen, CorrectBeantwoorddeVragen, Tijd) VALUES(@player, @SpelID, @Onderwerp, @BehaaldeScore, @AGV, @CBV, @tijd)";

        spelerID = getPlayerID(spelerNaam);

        cmd.Parameters.Add(new SqliteParameter("@player", spelerID));
        cmd.Parameters.Add(new SqliteParameter("@SpelID", spelID));
        cmd.Parameters.Add(new SqliteParameter("@Onderwerp", onderwerp));
        cmd.Parameters.Add(new SqliteParameter("@BehaaldeScore", score));
        cmd.Parameters.Add(new SqliteParameter("@AGV", aantalGesteldeVragen));
        cmd.Parameters.Add(new SqliteParameter("@CBV", correctBeantwoorddeVragen));
        cmd.Parameters.Add(new SqliteParameter("@tijd", tijd));
        cmd.ExecuteNonQuery();

        dbconn.Close();
    }

    public void insertScore(int spelerID, string onderwerp, int spelID, int score, int tijd, int aantalGesteldeVragen, int correctBeantwoorddeVragen)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "INSERT INTO Score(SpelerID, SpelID, Onderwerp, BehaaldeScore, AantalGesteldeVragen, CorrectBeantwoorddeVragen, Tijd) VALUES(@player, @SpelID, @Onderwerp, @BehaaldeScore, @AGV, @CBV, @tijd)";

        cmd.Parameters.Add(new SqliteParameter("@player", spelerID));
        cmd.Parameters.Add(new SqliteParameter("@SpelID", spelID));
        cmd.Parameters.Add(new SqliteParameter("@Onderwerp", onderwerp));
        cmd.Parameters.Add(new SqliteParameter("@BehaaldeScore", score));
        cmd.Parameters.Add(new SqliteParameter("@AGV", aantalGesteldeVragen));
        cmd.Parameters.Add(new SqliteParameter("@CBV", correctBeantwoorddeVragen));
        cmd.Parameters.Add(new SqliteParameter("@tijd", tijd));
        cmd.ExecuteNonQuery();

        dbconn.Close();
    }

    public int getScore(int playerID)
    {
        int score = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT BehaaldeScore FROM Score WHERE SpelerID='" + playerID + "'";
        score = Convert.ToInt32(cmd.ExecuteScalar() + "");

        dbconn.Close();

        return score;
    }

    public int getScore(string player)
    {
        int score = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT BehaaldeScore FROM Score WHERE SpelerNaam='" + player + "'";
        score = Convert.ToInt32(cmd.ExecuteScalar() + "");

        dbconn.Close();

        return score;
    }

    public void insertPlayerData(string playerName)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "INSERT INTO Speler(SpelerNaam) VALUES(@player)";

        cmd.Parameters.Add(new SqliteParameter("@player", playerName));
        cmd.ExecuteNonQuery();

        dbconn.Close();
    }

    public void deletePlayer(string playerName)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        cmd.CommandText = "DELETE FROM Gebruiker WHERE Naam='" + playerName + "'";
        cmd.ExecuteScalar();

        dbconn.Close();
    }

    public void deletePlayer(int playerID)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        cmd.CommandText = "DELETE FROM Gebruiker WHERE Geb_ID=" + playerID;
        cmd.ExecuteScalar();

        dbconn.Close();
    }

    public string getPlayer(int playerID)
    {
        string player = "";

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT SpelerNaam FROM Speler WHERE SpelerID='" + playerID + "'";
        player = cmd.ExecuteScalar() + "";

        dbconn.Close();

        return player;
    }

    public int getPlayerID(string playerName)
    {
        int player = 0;

        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand();

        cmd.Connection = dbconn;
        cmd.CommandText = "SELECT SpelerID FROM Speler WHERE SpelerNaam='" + playerName + "'";
        player = Convert.ToInt32(cmd.ExecuteScalar() + "");

        dbconn.Close();

        return player;
    }
}