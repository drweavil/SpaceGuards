using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierBoss : MonoBehaviour {
	public Ship ship;
	int currentPhase = 0;
	Timer spawnShip = new Timer();
	Timer spawnGroupTimer = new Timer();
	float shipPath0Time = 3f;
	float shipPath1Time = 3.5f;
	float standTime = 2f;



	public void BossAwake(){
		spawnShip.SetTimer (ship.explodeObject.damageHealthParam.spawnTime);
		spawnGroupTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime2);
		currentPhase = 0;
		ship.ShipAwake ();
		ship.explodeObject.explodeTransform.rotation = Quaternion.Euler (new Vector3(0, 0, 180));
		ship.explodeObject.explodeTransform.position = SpawnerController.instance.topSpawner.GetRandomPositionInWorld ();
		GeneratePath ();
		ship.movementController.PathByPointsRebind ();
	}

	public void BossActive(){
		if (ship.explodeObject.isActive) {
			if (spawnGroupTimer.TimeIsOver () && currentPhase != 1 && currentPhase != 2) {
				SetPhase1 ();
			}

			if (spawnShip.TimeIsOver () && currentPhase == 0) {
				SpawnShip ();
				spawnShip.SetTimer (ship.explodeObject.damageHealthParam.spawnTime);
			}

			if (currentPhase == 0) {
				ship.movementController.TranslateByPointsPath ();
			} else if (currentPhase == 1) {
				if (ship.movementController.objectOnPathEnd && currentPhase != 2) {
					SetPhase2 ();
				} else {
					ship.movementController.TranslateByLerpPath (1);
				}
			}
		}

	}

	public void Destroy(){
		ship.DestroyWithPoints ();
	}

	void SetPhase0(){
		GeneratePath ();
		ship.movementController.PathByPointsRebind ();
		currentPhase = 0;
		spawnGroupTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime2);
	}


	void SetPhase1(){
		ship.movementController.startPoint = ship.explodeObject.explodeTransform.position;
		ship.movementController.endPoint = SpawnerController.instance.carrierBossShipSpawnPosition.GetRandomPositionInWorld();
		ship.movementController.pathProgress = 0;
		ship.movementController.objectOnPathEnd = false;
		currentPhase = 1;
	}

	void SetPhase2(){
		SpawnGroup ();
		currentPhase = 2;
		Timer startMovingTimer = new Timer ();
		startMovingTimer.SetTimer (standTime + shipPath0Time);
		StartCoroutine (startMovingTimer.ActionAfterTimer(() => {
			SetPhase0();
		}));
	}

	void SpawnShip(){
		int expType = Random.Range (0, 3);
		Ship spawnShip = ShipsController.instance.SpawnShip (ship.explodeObject.raceType, 2, expType).GetComponent<Ship>();
		spawnShip.explodeObject.explodeTransform.position = ship.explodeObject.explodeTransform.position;
		spawnShip.explodeObject.DefaultAwake ();
		spawnShip.explodeObject.damageHealthParam = spawnShip.explodeObject.damageHealthParam.Clone ();
		spawnShip.explodeObject.damageHealthParam.pathTime = 3f;
		spawnShip.movementController.Ship2Awake ();
		ShipsController.instance.explodeObjects.Add (spawnShip.explodeObject);
	}

	void SpawnGroup(){
		List<SpawnerGroup> groups = new List<SpawnerGroup> ();
		groups.Add (SpawnerController.instance.firstCarrirerBossGroup);
		groups.Add (SpawnerController.instance.secondCarrirerBossGroup);
		groups.Add (SpawnerController.instance.thirdCarrirerBossGroup);
		SpawnerGroup spawnGroup = groups [Random.Range (0, groups.Count)];
		foreach (Spawner spawner in spawnGroup.spawners) {
			int explodeType = Random.Range (0, 3);
			Ship spawnShip = ShipsController.instance.SpawnShip (ship.explodeObject.raceType, 1, explodeType).GetComponent<Ship>();
			spawnShip.explodeObject.explodeTransform.position = ship.explodeObject.explodeTransform.position;
			spawnShip.explodeObject.DefaultAwake ();
			spawnShip.explodeObject.damageHealthParam = spawnShip.explodeObject.damageHealthParam.Clone ();
			spawnShip.explodeObject.damageHealthParam.pathTime = shipPath0Time;
			spawnShip.explodeObject.DefaultAwake ();
			spawnShip.movementController.InstantiateStandartPath (spawnShip.explodeObject.explodeTransform.position, spawner.GetRandomPositionInWorld ());
			spawnShip.movementController.pathEndActionComplete = false;
			ShipsController.instance.explodeObjects.Add (spawnShip.explodeObject);
			spawnShip.movementController.pathEndAction = () => {
					
				if(spawnShip.movementController.pathEndActionComplete == false){
					spawnShip.explodeObject.explodeTransform.rotation = Quaternion.Euler (new Vector3(0, 0, 180));
					Timer stopTimer = new Timer();
					stopTimer.SetTimer(standTime);
					StartCoroutine(stopTimer.ActionAfterTimer(() => {
						spawnShip.explodeObject.damageHealthParam.pathTime = shipPath1Time;
						spawnShip.movementController.InstantiateStandartPath (spawnShip.explodeObject.explodeTransform.position, SpawnerController.instance.bottomSpawner.GetRandomPositionInWorld());
					}));
				}
				spawnShip.movementController.pathEndActionComplete = true;
			};
		}
	}

	void GeneratePath(){
		ship.movementController.pathPoints.Clear ();
		int pointsCount = Random.Range (3, 5);
		bool spawner1Used = false;
		bool spawner2Used = false;
		bool spawner3Used = false;
		bool allSpawnersUsed = false;
		List<int> spawnerIDs = new List<int> (new int[] {1, 2, 3});
		for (int i = 0; i < pointsCount; i++) {
			int spawnerID = spawnerIDs[Random.Range (0, spawnerIDs.Count)];


			if (spawnerID == 1) {
				ship.movementController.pathPoints.Add (SpawnerController.instance.leftPolynomialBossSpawner.GetRandomPositionInWorld());
				spawner1Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (1);
				}
			} else if (spawnerID == 2) {
				ship.movementController.pathPoints.Add (SpawnerController.instance.centerPolynomialBossSpawner.GetRandomPositionInWorld());
				spawner2Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (2);
				}
			} else if (spawnerID == 3) {
				ship.movementController.pathPoints.Add (SpawnerController.instance.rightPolynomialBossSpawner.GetRandomPositionInWorld());
				spawner3Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (3);
				}
			}

			if (spawner1Used && spawner2Used && spawner3Used) {
				allSpawnersUsed = true;
				spawnerIDs = new List<int> (new int[] {1, 2, 3});
			}
		}
	}
}
