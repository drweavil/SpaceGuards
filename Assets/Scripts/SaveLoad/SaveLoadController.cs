using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadController : MonoBehaviour {
	public static void SaveGameSessionData(GameSessionData data){
		FileStream file;
		BinaryFormatter bf = new BinaryFormatter ();
		CreateDirectoryIfNotExist (Application.persistentDataPath + "/Resources/");
		string path = Application.persistentDataPath + "/Resources/currentGameData.gameData";
		if (File.Exists (path)) {
			File.Delete (path);
		}
		file = File.Create (path);
		bf.Serialize (file, data);
		file.Close ();

	}



	public static GameSessionData LoadGameSessionData(){
		string path = Application.persistentDataPath + "/Resources/currentGameData.gameData";
		GameSessionData loadedInfo = new GameSessionData();
		FileStream file;BinaryFormatter bf = new BinaryFormatter ();
		if (File.Exists (path)) {
			file = File.Open (path, FileMode.Open);
			loadedInfo = (GameSessionData)bf.Deserialize (file);
			file.Close ();
		}
		return loadedInfo;
	}


	public static bool GameSessionDataExist(){
		return File.Exists (Application.persistentDataPath + "/Resources/currentGameData.gameData");
	}


	public static void CreateDirectoryIfNotExist (string path){
		if (!Directory.Exists (path)) {
			Directory.CreateDirectory (path);
		}
	}
}
