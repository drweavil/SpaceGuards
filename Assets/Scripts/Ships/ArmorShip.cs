using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorShip : MonoBehaviour {
	public Ship ship;
	bool chargeMode = false;
	Timer bulletTimer = new Timer();
	Timer mineTimer = new Timer();



	public void ShipAwake(){
		chargeMode = false;
		ship.ShipAwake ();
		bulletTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime);
		mineTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime2);
		ship.explodeObject.explodeTransform.rotation = Quaternion.Euler (new Vector3(0, 0, 180));
		ship.explodeObject.explodeTransform.position = SpawnerController.instance.topSpawner.GetRandomPositionInWorld ();
		GeneratePath ();
		ship.movementController.PathByPointsRebind ();
	}


	public void ShipActiveAction(){
		if (ship.movementController.objectGetRound && !chargeMode) {
			SetChargeMode();
		}


		if (!chargeMode && ship.explodeObject.isActive && !ship.explodeObject.isFreeze) {
			if (bulletTimer.TimeIsOver ()) {
				bulletTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime);
				string path = "Prefabs/Airships/bullets/race_" + ship.explodeObject.raceType.ToString () + "/defaultArmor";
				GameObject bulletObj = ObjectsPool.PullObject (path);
				Transform spawnerTransform = ship.GetRandomSpawner ();
				ExplodeObject bulletExpObj = bulletObj.GetComponent<ExplodeObject> ();
				bulletExpObj.explodeTransform.position = spawnerTransform.position;
				bulletExpObj.poolPath = path;
				ShipsController.instance.explodeObjects.Add (bulletExpObj);
				bulletExpObj.DefaultAwake ();
			}

			if (mineTimer.TimeIsOver ()) {
				mineTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime2);
				string path = "Prefabs/Airships/bullets/race_" + ship.explodeObject.raceType.ToString () + "/mine";
				GameObject mineObj = ObjectsPool.PullObject (path);
				Transform spawnerTransform = ship.GetRandomSpawner ();
				ExplodeObject mineExpObj = mineObj.GetComponent<ExplodeObject> ();
				mineExpObj.explodeTransform.position = spawnerTransform.position;
				mineExpObj.poolPath = path;
				ShipsController.instance.explodeObjects.Add (mineExpObj);
				mineExpObj.DefaultAwake ();
			}
		}



		if(chargeMode){
			ship.movementController.TranslateByLerpPath(ship.explodeObject.damageHealthParam.chargePathTime);	
		}else{
			ship.movementController.TranslateByPointsPath ();
		}
	}

	void SetChargeMode(){
		ship.movementController.startPoint = ship.movementController.pathPoints [ship.movementController.pathPoints.Count - 1];
		ship.movementController.endPoint = SpawnerController.instance.bottomSpawner.GetRandomPositionInWorld (); 


		Vector3 directionVector = ship.movementController.endPoint - ship.movementController.startPoint;
		directionVector.Normalize ();
		float angle = Vector3.Angle (directionVector, new Vector3 (0, 1));
		//Debug.Log (directionVector);
		if (directionVector.x > 0) {
			angle = 360 - angle;
		}
		ship.explodeObject.explodeTransform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));

		chargeMode = true;


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
				ship.movementController.pathPoints.Add (SpawnerController.instance.leftArmorShipSpawner.GetRandomPositionInWorld());
				spawner1Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (1);
				}
			} else if (spawnerID == 2) {
				ship.movementController.pathPoints.Add (SpawnerController.instance.centerArmorShipSpawner.GetRandomPositionInWorld());
				spawner2Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (2);
				}
			} else if (spawnerID == 3) {
				ship.movementController.pathPoints.Add (SpawnerController.instance.rightArmorShipSpawner.GetRandomPositionInWorld());
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
