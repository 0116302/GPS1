using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour {

	public static string saveFile = "save.dat";

	public static void Save () {
		// Save the player's progress into a file
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/" + saveFile);
		bf.Serialize(file, Game.current);
		file.Close();
	}

	public static void Load () {
		Debug.Log ("Attempting to load save file...");

		// Load the player's progress from a file
		if (File.Exists (Application.persistentDataPath + "/" + saveFile)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/" + saveFile, FileMode.Open);
			Game.current = (Game)bf.Deserialize (file);
			file.Close ();

			Debug.Log ("Save file loaded.");

		} else {
			Debug.Log ("Save file not found.");
		}
	}

	void Start () {
		Game.current = new Game ();
		Load ();
	}
}
