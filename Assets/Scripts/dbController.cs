using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.IO;
using System.Data;

public class dbController : MonoBehaviour {
	protected SqliteConnection dbconn;
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