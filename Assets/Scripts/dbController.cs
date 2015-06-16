using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.IO;
using System.Data;

public class dbController : MonoBehaviour {
	protected SqliteConnection dbconn;
    public Texture2D texture;
	void Awake () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			dbconn = new SqliteConnection("URI=file:" +Application.dataPath + "/../Database.s3db");
		} else {
			dbconn = new SqliteConnection("URI=file:" +"C:/Users/daan/Documents/GitHub/Ordina/Assets/database/Database.s3db");	
		}
		
		dbconn.Open();
		

			/*SqliteCommand cmd = new SqliteCommand("SELECT naam, Geb_ID FROM Gebruiker", dbconn);
			SqliteDataReader reader = cmd.ExecuteReader();
			
			if (reader.HasRows) {
				while(reader.Read()) {
					string naam = reader.GetString (0);
					string Geb_ID = reader.GetString (1);
					
					Debug.Log ( naam + Geb_ID );
				}
			}
			
			reader.Close();
			reader = null;
			cmd.Dispose();
			cmd = null;
			dbconn.Close();
			dbconn = null;*/
	
		
		SqliteCommand cmd = new SqliteCommand();
		cmd.CommandText = @"INSERT INTO Gebruiker(naam) VALUES(@naam)";
		cmd.Connection = dbconn;
		cmd.Parameters.Add (new SqliteParameter ("@naam", "Chanan"));
		//cmd.Parameters.Add (new SqliteParameter ("@Geb_ID", "99"));
			
		int i = cmd.ExecuteNonQuery ();
		if (i == 1)
			Debug.Log ("Query ok");

		
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
}