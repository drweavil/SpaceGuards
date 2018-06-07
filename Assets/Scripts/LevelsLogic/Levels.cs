using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Levels : MonoBehaviour {
	public const int locked = 0, open = 1, complete = 2;
	public const int planet_1_maximum_level = 8;
	public const int planet_2_maximum_level = 10;
	public const int planet_3_maximum_level = 12;
	public const int planet_4_maximum_level = 8;
	public const int planet_5_maximum_level = 10;
	public const int planet_6_maximum_level = 12;
	public const int maximumPlanetID = 6;
	public const float maximumHealthPool = 50f;
	public const float minimumHealthPool = 30f;
	static Dictionary<int, PlanetSettings> planetSettings = new Dictionary<int, PlanetSettings>();


	void Awake(){
		planetSettings.Clear ();
		SetPlanetSettings ();
	}

	public static Level GetNextLevel (Level currentLevel){
		//Debug.Log(currentLevel.levelID);
		//FieldInfo maximumLevelID = typeof(Levels).GetField ("planet_" + currentLevel.planetID.ToString() + "_maximum_level");
		//Debug.Log ((int)maximumLevelID.GetValue(null));
		if (planetSettings[currentLevel.planetID].maximumLevelID == currentLevel.levelID) {
			if (currentLevel.planetID == maximumPlanetID) {
				return new Level ();	
			} else {
				return GetLevel (currentLevel.planetID + 1, 1);
			}
		}else{
			return GetLevel (currentLevel.planetID, currentLevel.levelID + 1);
		}

	}

	/*public static Level GetLevel(int race, int levelID){
		MethodInfo method = typeof(Levels).GetMethod ("Race_" + race.ToString() + "_ID_" + levelID.ToString(), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		return (Level)method.Invoke (null, null);
	}*/



	public static Level GetLevel(int race, int levelID){
		string levelKey = race.ToString () + "_" + levelID.ToString ();
		if (LevelController.instance.gameSessionData.levels.ContainsKey (levelKey)) {
			if (LevelController.instance.gameSessionData.levels [levelKey].level == null) {
				LevelController.instance.gameSessionData.levels [levelKey].level = NewLevel (race, levelID);
			}
		} else {
			Level newLevel = NewLevel (race, levelID);
			LevelSaveData newLevelSaveData = new LevelSaveData ();
			newLevelSaveData.planetID = race;
			newLevelSaveData.levelID = levelID;
			newLevelSaveData.level = newLevel;
			LevelController.instance.gameSessionData.levels.Add (newLevel.GetLevelKey(), newLevelSaveData);
		}
		LevelController.instance.SaveGameSessionData ();
		return LevelController.instance.gameSessionData.levels [levelKey].level;
	}

	static Level NewLevel(int race, int levelID){
		Level newLevel = new Level ();
		newLevel.planetID = race;
		newLevel.levelID = levelID;

		List<int> shipIDs = new List<int> (new int[]{1,2,3,4,5,6});
		int shipsCount = levelID;
		if (shipsCount > 6) {
			shipsCount = 6;
		}
		for (int i = 0; i < shipsCount; i++) {
			if (race == 1) {
				newLevel.ships.Add (shipIDs [i]);
			} else {
				int randomIndex = Random.Range (0, shipIDs.Count);
				newLevel.ships.Add (shipIDs[randomIndex]);
				shipIDs.RemoveAt (randomIndex);
			}
		}

		newLevel.time = Random.Range(planetSettings [race].minimumLevelTime, planetSettings [race].maximumLevelTime); 
		newLevel.healthPool = GetHealthPool (race, levelID);

		if (newLevel.levelID == planetSettings [race].maximumLevelID) {
			if (race == 1) {
				newLevel.bosses = new List<int> (new int[]{ 2 });
			} else if (race == 2) {
				newLevel.bosses = new List<int> (new int[]{ 1 });
			} else if (race == 3) {
				newLevel.bosses = new List<int> (new int[]{ 3 });
			} else if (race == 4) {
				newLevel.bosses = new List<int> (new int[]{ 1 });
			} else if (race == 5) {
				newLevel.bosses = new List<int> (new int[]{ 2 });
			} else if (race == 6) {
				newLevel.bosses = new List<int> (new int[]{ 1,2,3 });
			}
		}
		return newLevel;
	}


	public static float GetHealthPool(int race, int levelID){
		//planetSettings [race].maximumLevelID - 1;
		return Mathf.Lerp (minimumHealthPool, maximumHealthPool, Mathf.InverseLerp(1, planetSettings [race].maximumLevelID, levelID));
	}



	void SetPlanetSettings(){
		PlanetSettings ps1 = new PlanetSettings ();
		ps1.minimumLevelTime = 120f;
		ps1.maximumLevelTime = 180f;
		ps1.maximumLevelID = 8;
		ps1.planetID = 1;
		planetSettings.Add (1, ps1);

		PlanetSettings ps2 = new PlanetSettings ();
		ps2.minimumLevelTime = 120f;
		ps2.maximumLevelTime = 180f;
		ps2.maximumLevelID = 10;
		ps2.planetID = 2;
		planetSettings.Add (2, ps2);

		PlanetSettings ps3 = new PlanetSettings ();
		ps3.minimumLevelTime = 210f;
		ps3.maximumLevelTime = 270f;
		ps3.maximumLevelID = 12;
		ps3.planetID = 3;
		planetSettings.Add (3, ps3);

		PlanetSettings ps4 = new PlanetSettings ();
		ps4.minimumLevelTime = 210f;
		ps4.maximumLevelTime = 270f;
		ps4.maximumLevelID = 8;
		ps4.planetID = 4;
		planetSettings.Add (4, ps4);

		PlanetSettings ps5 = new PlanetSettings ();
		ps5.minimumLevelTime = 270f;
		ps5.maximumLevelTime = 350f;
		ps5.maximumLevelID = 10;
		ps5.planetID = 5;
		planetSettings.Add (5, ps5);

		PlanetSettings ps6 = new PlanetSettings ();
		ps6.minimumLevelTime = 270f;
		ps6.maximumLevelTime = 350f;
		ps6.maximumLevelID = 12;
		ps6.planetID = 6;
		planetSettings.Add (6, ps6);
	}


	static Level Race_1_ID_1(){
		Level level = new Level ();
		level.planetID = 1;
		level.levelID = 1;
		level.ships = new List<int> (new int[]{1,2});
		level.bosses = new List<int> (new int[]{});
		level.time = 60f;
		return level;
	}

	static Level Race_1_ID_2(){
		Level level = new Level ();
		level.planetID = 1;
		level.levelID = 2;
		level.ships = new List<int> (new int[]{1,2,5});
		level.bosses = new List<int> (new int[]{2});
		level.time = 60f;
		return level;
	}


	static Level Race_2_ID_1(){
		Level level = new Level ();
		level.planetID = 2;
		level.levelID = 1;
		level.ships = new List<int> (new int[]{1,2});
		level.bosses = new List<int> (new int[]{1});
		level.time = 60f;
		return level;
	}
}
