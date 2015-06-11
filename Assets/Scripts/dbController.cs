﻿using UnityEngine;
using Mono.Data.Sqlite;
using System;

public class dbController : MonoBehaviour {
	protected SqliteConnection dbconn;
	
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
	
}