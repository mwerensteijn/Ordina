using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.IO;
using System.Data;

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

        //insertRectX(1, 1, 1, 1, 1);

        //insertPicture(tex);

        ////MeshRenderer rend = plane.GetComponent<MeshRenderer>();
        ////rend.material.SetTexture("_MainTex", extractPicture());
	}
	private void insertData(String ImagePath, String ImageNaam)
	{	
		try
		{
			FileStream fs = new FileStream(@ImagePath, FileMode.Open, FileAccess.Read);
			byte[] imgByteArr = new byte[fs.Length];
			fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
			fs.Close();
			SqliteCommand cmd = new SqliteCommand();
			cmd.CommandText  = @"INSERT INTO Afbeelding(name,img) VALUES(@Naam,@img)";
			cmd.Parameters.Add (new SqliteParameter ("@naam", ImageNaam));
			cmd.Parameters.Add(new SqliteParameter("@img", imgByteArr));
				int result = cmd.ExecuteNonQuery ();
					if (result == 1)
				Debug.Log ("Afbeelding sorted");
			}
		catch (Exception ex)
		{
            Debug.Log("PANIEK TIJDENS AFBEELDING OPSLAAN");
		}
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
        Texture2D tex = new Texture2D(2,2);
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

        //dbconn.Close();
    }

    public Texture2D extractPicture()
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

        //dbconn.Close();

        return pic;
    }

    public void insertRectX(int rectID, int x, int y, int length, int width)
    {
        dbconn = new SqliteConnection("URI=file:" + Application.dataPath + "/database/Database.s3db");
        dbconn.Open();

        SqliteCommand cmd = new SqliteCommand(dbconn);

        cmd.CommandText = @"select last_insert_rowid()";
        long lastRowID = (long)cmd.ExecuteScalar();
        Debug.Log(lastRowID);
    }
}