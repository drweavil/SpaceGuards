using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipSpawner : MonoBehaviour {
	public static ShipSpawner instance;
	Timer spawnTimer = new Timer();
	public float spawnTime = 0.4f;
	public int nextBossIndex = 0;
	public const float allShipBattleTimePercent = 0.4f;
	public const float cannonBoxesOnLevel = 10f;
	bool timerActive = false;
	bool spawnerOnPause = false;
	List<Level.ShipSpawnerSettings> shipSettings = new List<Level.ShipSpawnerSettings > ();
	public List<Level.ShipSpawnerSettings> spawnShipSettings = new List<Level.ShipSpawnerSettings>();
	public float maximumHealth = 5000;



	Timer shipGateTimer = new Timer ();
	Timer bossTimer = new Timer();
	Timer recoveryBoxGateTimer = new Timer ();
	Timer cannonBoxTimer = new Timer();

	void Awake(){
		instance = this;
	}

	void Update (){
		if (timerActive) {
			if (spawnTimer.TimeIsOver ()) {
				spawnTimer.SetTimer (spawnTime);
				TrySpawnShip ();
			}
		}
	}

	public void AddSpawnShipSettingById(int shipID){
		//Debug.Log (shipID);
		spawnShipSettings.Add(shipSettings.Find (s => s.shipID == shipID));
		spawnShipSettings = spawnShipSettings.OrderBy (s => s.priority).ToList<Level.ShipSpawnerSettings>();
		spawnShipSettings.Reverse ();
	}

	public void TrySpawnShip(){
		float allHealth = ShipsController.instance.GetAllShipHealth ();
		bool shipNotFound = true;
		int i = 0;
		//Debug.Log (shipSettings[0]);
		while(shipNotFound && i < spawnShipSettings.Count){
			//Debug.Log(spawnShipSettings [i]);

			if (allHealth + GetShipHealth (spawnShipSettings [i]) <= maximumHealth) {
				if (Random.Range (0, 2) == 1) {
					int expObjType = Random.Range (0, 3);
					ShipsController.instance.SpawnShip (LevelController.instance.currentRace, spawnShipSettings [i].shipID, expObjType);
					shipNotFound = false;
				}
			}
			i += 1;
		}
	}

	float GetShipHealth(Level.ShipSpawnerSettings shipSetting){
		//Debug.Log ("ship_" + shipSetting.shipID);
		//Debug.Log (DamageHealthParamController.instance.GetExplodeObjectById ("ship_" + shipSetting.shipID));
		return DamageHealthParamController.instance.GetExplodeObjectById ("ship_" + shipSetting.shipID).health;
	}
	public Level.ShipSpawnerSettings GetShipSetting(int id){
		return shipSettings.Find (s => s.shipID == id);
	}


	public void AddSpawnShipToSpawner(int index, Level timeGateLevel){
		if (timerActive) {
			ShipSpawner.instance.AddSpawnShipSettingById (timeGateLevel.ships [index]);
			if (index != timeGateLevel.ships.Count - 1) {
				float allTime = timeGateLevel.time - timeGateLevel.time * ShipSpawner.allShipBattleTimePercent;
				float spawnTimePart = allTime / (timeGateLevel.ships.Count - 1f);

				shipGateTimer.SetTimer (spawnTimePart);
				StartCoroutine (shipGateTimer.ActionAfterTimer (() => {
					if(timerActive){
						AddSpawnShipToSpawner (index + 1, timeGateLevel);
					}else{
						shipGateTimer.SetTimer(0);
					}
				}));
			}
		}
	}

	public void StartSpawnCannonBoxTimer(float cannonBoxPartTime){
		cannonBoxTimer.SetTimer (cannonBoxPartTime);
		StartCoroutine (cannonBoxTimer.ActionAfterTimer(() => {
			if(timerActive){
				StartSpawnCannonBoxTimer(cannonBoxPartTime);
				ShipsController.instance.SpawnRandomCannonBox();
			}
		}));
	}

	public void AnotherBossSpawn(){
		//if (timerActive) {
		if (nextBossIndex < LevelController.instance.level.bosses.Count) {
			//Debug.Log ("nextBossSpawn");
			timerActive = true;
			StartBossTimer (nextBossIndex, LevelController.instance.level);
			nextBossIndex += 1;
			PauseDisable ();
				
		}
		//} 
		LevelTimer.instance.timerActive = true;
	}
	void StartBossTimer(int index, Level timeGateLevel){
		if (timerActive) {
			float allTime = timeGateLevel.time - 5f;
			float spawnTimePart = allTime / (timeGateLevel.bosses.Count);
			bossTimer.SetTimer (spawnTimePart);
			StartCoroutine (bossTimer.ActionAfterTimer (() => {
				if(timerActive){
					ShipsController.instance.SpawnBoss (LevelController.instance.currentRace, timeGateLevel.bosses [index]);
					PauseEnable ();
					timerActive = false;
					LevelTimer.instance.timerActive = false;
				}else{
					bossTimer.SetTimer(0);
				}
			}));
		}
	}


	public void StartRecoverySpawnTimer(float recoveryBoxPartTime){
		recoveryBoxGateTimer.SetTimer (recoveryBoxPartTime);
		StartCoroutine (recoveryBoxGateTimer.ActionAfterTimer(() => {
			if(timerActive){
				StartRecoverySpawnTimer(recoveryBoxPartTime);
				ShipsController.instance.SpawnAbilityBox (1);
			}
		}));
	}

	/*public void StartRecoverySpawnTimer(part){
		if (timerActive) {
			if (boxCount > 0) {
				float partTime = currentLevel.time / (currentLevel.recoveryBoxes + 1);
				//Debug.Log (partTime);

				recoveryBoxGateTimer.SetTimer (partTime);
				StartCoroutine (recoveryBoxGateTimer.ActionAfterTimer (() => {
					if(timerActive){
						ShipsController.instance.SpawnAbilityBox (1);
						StartRecoverySpawnTimer (boxCount - 1, currentLevel);
					}else{
						recoveryBoxGateTimer.SetTimer(0);
					}
				}));
			}
		}
	}*/

	public void InstantiateShipSettings(){
		nextBossIndex = 0;

		shipSettings.Clear ();
		Level.ShipSpawnerSettings ship1 = new Level.ShipSpawnerSettings ();
		ship1.shipID = 1;
		ship1.priority = 1;
		shipSettings.Add (ship1);

		Level.ShipSpawnerSettings ship2 = new Level.ShipSpawnerSettings ();
		ship2.shipID = 2;
		ship2.priority = 2;
		shipSettings.Add (ship2);

		Level.ShipSpawnerSettings ship3 = new Level.ShipSpawnerSettings ();
		ship3.shipID = 3;
		ship3.priority = 3;
		shipSettings.Add (ship3);

		Level.ShipSpawnerSettings ship4 = new Level.ShipSpawnerSettings ();
		ship4.shipID = 4;
		ship4.priority = 4;
		shipSettings.Add (ship4);

		Level.ShipSpawnerSettings ship5 = new Level.ShipSpawnerSettings ();
		ship5.shipID = 5;
		ship5.priority = 5;
		shipSettings.Add (ship5);

		Level.ShipSpawnerSettings ship6 = new Level.ShipSpawnerSettings ();
		ship6.shipID = 6;
		ship6.priority = 6;
		shipSettings.Add (ship6);
	}

	public void StartSpawn(){
		timerActive = true;
		spawnerOnPause = false;
		spawnTimer.SetTimer (spawnTime);
	}

	public void PauseEnable(){
		recoveryBoxGateTimer.PauseEnable ();
		shipGateTimer.PauseEnable ();
		cannonBoxTimer.PauseEnable ();
		spawnerOnPause = false;

	}

	public void PauseDisable(){
		recoveryBoxGateTimer.PauseDisable ();
		shipGateTimer.PauseDisable ();
		cannonBoxTimer.PauseDisable ();
		spawnerOnPause = true;

	}

	public void StopSpawn(){
		timerActive = false;
		shipGateTimer.SetTimer (0);
		bossTimer.SetTimer (0);
		recoveryBoxGateTimer.SetTimer (0);
		cannonBoxTimer.SetTimer (0);
	}
}
