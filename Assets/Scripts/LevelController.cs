using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {
	public static LevelController instance;
	public int currentRace = 6;
	public Dictionary<string, bool> cannonButtonsStatus = new Dictionary<string, bool> ();
	public string currentLeftButtonKey;
	public string currentRightButtonKey;
	public int currentLevelID;
	public Level level;

	public bool levelActive = false;

	public float currentPoints = 0;



	public GameSessionData gameSessionData;



	void Awake(){
		instance = this;
		StartCoroutine(GameController.ActionAfterFewFramesCoroutine(2, () => {
			if(SaveLoadController.GameSessionDataExist()){
				gameSessionData = SaveLoadController.LoadGameSessionData();
			}else{
				NewGameProcess();
			}
			BattleInterface.instance.RedrawAllInfo();
			//StartCoroutine(GameController.ActionAfterFewFramesCoroutine(15, () => {
				//CheckFullVersionGame();
			//}));
		}));
	}


	public void SaveGameSessionData(){
		SaveLoadController.SaveGameSessionData (gameSessionData);
	}



	public void Test(){
		//Time.timeScale = 0;
		//ShipsController.instance.SpawnAbilityBox(2);
		//SpawnEnemy ();
		ShipsController.instance.SpawnShip(1, 6, 2);
		//SetLevel(Levels.GetLevel(1, 1));
		//Debug.Log(progressLine.sizeDelta.y);
		//BackgroundController.instance.SetPlanet(6);
		//PauseGame.instance.StartPauseEnable(); 
		//CannonsPanel.instance.SetLeftCannon(cannongh);
		//cannongh += 1;
	}
	public void Test2(){
		PauseGame.instance.StartPauseDisable();
		//BackgroundController.instance.SetPlanet(1);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.I)) {
			Time.timeScale = 0;
		}
	}

	public void NewGameProcess(){
		gameSessionData = new GameSessionData ();
		gameSessionData.wipeAbilityCount = 2;
		gameSessionData.doubleDamageAbilityCount = 2;
		gameSessionData.freezeAbilityCount = 2;
		gameSessionData.shieldAbilityCount = 2;
		gameSessionData.planetStatus.Add (1, false);
		/*gameSessionData.planetStatus.Add (2, false);
		gameSessionData.planetStatus.Add (3, false);
		gameSessionData.planetStatus.Add (4, false);
		gameSessionData.planetStatus.Add (5, false);
		gameSessionData.planetStatus.Add (6, false);*/

		for(int i = 1; i <= 6; i++){
			LevelSaveData levelSaveData = new LevelSaveData();
			levelSaveData.levelID = 1;
			levelSaveData.planetID = i;
			gameSessionData.levels.Add(i.ToString() + "_1", levelSaveData);
		}

		SaveGameSessionData ();
	}

	public void CheckFullVersionGame(){
		if (IAPController.instance.GetProduct ("full_version").hasReceipt) {
			gameSessionData.fullVersionActive = true;
			Interface.interfaceSt.fullVersionButton.SetActive(false);
			//Advertising.advertising.advertisingActive = false;
		} else {
			gameSessionData.fullVersionActive = false;
			Interface.interfaceSt.fullVersionButton.SetActive(true);
			//Advertising.advertising.advertisingActive = true;
		}

		SaveGameSessionData ();
	}

	public void SetCannonButtonsStatus(bool status){
		List<string> buttonStatusKeysList = new List<string> ();
		foreach (KeyValuePair<string, bool> pair in cannonButtonsStatus) {
			//Debug.Log(pair.Key);
			buttonStatusKeysList.Add(pair.Key);
		}

		foreach (string buttonStatusKey in buttonStatusKeysList) {
			LevelController.instance.cannonButtonsStatus [buttonStatusKey] = status;	
		}
	}


	public void SetLevel(Level newLevel){
		//Debug.Log (newLevel);
		levelActive = true;
		CannonsPanel.instance.SetDefaultCannons ();
		currentPoints = 0;
		level = newLevel;

		ShipSpawner.instance.InstantiateShipSettings ();
		ShipSpawner.instance.spawnShipSettings.Clear ();
		ShipSpawner.instance.nextBossIndex = 0;
		ShipSpawner.instance.StartSpawn ();
		ShipSpawner.instance.maximumHealth = newLevel.healthPool;
		ShipSpawner.instance.AddSpawnShipToSpawner(0, newLevel);
		if (newLevel.bosses.Count != 0) {
			ShipSpawner.instance.AnotherBossSpawn ();
		}
		LevelTimer.instance.StartTimer (newLevel.time);


		level.recoveryBoxes = (int)(level.time / 10f);
		ShipSpawner.instance.StartRecoverySpawnTimer (40f);
		ShipSpawner.instance.StartSpawnCannonBoxTimer (level.time / ShipSpawner.cannonBoxesOnLevel);

		BackgroundController.instance.SetPlanet (level.planetID);
		currentRace = level.planetID;
		currentLevelID = level.levelID;
		//Debug.Log (level.planetID);
		CannonsPanel.instance.RestoreMaximumHealth ();
		CannonsPanel.instance.isDead = false;
		CannonsPanel.instance.shieldPoints = 0;
		CannonsPanel.instance.SetVisualState();

		AudioController.instance.SetSongByPlanet (level.planetID);
	}


	public void LevelComplete(){
		if (gameSessionData.levels.ContainsKey (level.GetLevelKey ())) {
			
			if (gameSessionData.levels [level.GetLevelKey ()].points < currentPoints) {
				Interface.interfaceSt.winWindowOldPoints.text = System.Math.Round (gameSessionData.levels [level.GetLevelKey ()].points, 0).ToString ();
				float plusPoints = currentPoints - gameSessionData.levels [level.GetLevelKey ()].points;
				Interface.interfaceSt.winWindowCurrentPoints.text = "+" + System.Math.Round (plusPoints, 0).ToString ();
				gameSessionData.points += plusPoints;
				gameSessionData.maximumPoints += plusPoints;
				gameSessionData.levels [level.GetLevelKey ()].points = currentPoints;

			} else {
				Interface.interfaceSt.winWindowOldPoints.text = System.Math.Round (gameSessionData.levels [level.GetLevelKey ()].points, 0).ToString ();
				Interface.interfaceSt.winWindowCurrentPoints.text = "+0";
			}
			gameSessionData.levels [level.GetLevelKey ()].isComplete = true;
		} else {
			Interface.interfaceSt.winWindowOldPoints.text = "0";
			LevelSaveData levelSaveData = new LevelSaveData ();
			levelSaveData.planetID = level.planetID;
			levelSaveData.levelID = level.levelID;
			levelSaveData.points = currentPoints;
			levelSaveData.isComplete = true;
			gameSessionData.levels.Add (level.GetLevelKey(), levelSaveData);
			gameSessionData.points += currentPoints;
			gameSessionData.maximumPoints += currentPoints;
		}


		Level nextLevel = Levels.GetNextLevel (level);
		LevelSaveData newLevelSaveData = new LevelSaveData ();
		newLevelSaveData.planetID = nextLevel.planetID;
		newLevelSaveData.levelID = nextLevel.levelID;
		//Debug.Log (nextLevel.GetLevelKey());
		if (!gameSessionData.levels.ContainsKey (nextLevel.GetLevelKey ())) {
			gameSessionData.levels.Add (nextLevel.GetLevelKey (), newLevelSaveData);
		}

		if (level.planetID < nextLevel.planetID) {
			gameSessionData.planetStatus [level.planetID] = true;
			if (!gameSessionData.planetStatus.ContainsKey (nextLevel.planetID)) {
				gameSessionData.planetStatus.Add (nextLevel.planetID, false);
			}
		}

		LevelController.instance.SaveGameSessionData ();
	}
}
