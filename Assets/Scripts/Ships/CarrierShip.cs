using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierShip : MonoBehaviour {
	public Ship ship;
	Timer shipTimer = new Timer();



	public void ShipAwake(){
		ship.ShipAwake ();
		shipTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime);
		ship.explodeObject.explodeTransform.rotation = Quaternion.Euler (new Vector3(0, 0, 180));
		ship.explodeObject.explodeTransform.position = SpawnerController.instance.topSpawner.GetRandomPositionInWorld ();
		GeneratePath ();
		ship.movementController.PathByPointsRebind ();
	}


	public void Destroy(){
		DestroyAction ();
		ship.Destroy ();
	}
	public void DestroyWihtPoints(){
		DestroyAction ();
		ship.DestroyWithPoints ();
	}

	void DestroyAction(){
		for (int i = 0; i < 4; i++) {
			SpawnShip ();
		}
	}

	public void ShipActiveAction(){
		if (ship.explodeObject.isActive && !ship.explodeObject.isFreeze) {
			if (shipTimer.TimeIsOver ()) {
				shipTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime);
				SpawnShip ();
			}
		}
			ship.movementController.TranslateByPointsPath ();
	}

	void SpawnShip(){
		Ship spawnShip = ShipsController.instance.SpawnShip (ship.explodeObject.raceType, 1, Cannon.allBullet).GetComponent<Ship>();
		spawnShip.explodeObject.explodeTransform.position = ship.explodeObject.explodeTransform.position;
		spawnShip.explodeObject.DefaultAwake ();
		spawnShip.explodeObject.damageHealthParam = spawnShip.explodeObject.damageHealthParam.Clone ();
		spawnShip.explodeObject.damageHealthParam.pathTime = 3f;
		spawnShip.movementController.StandartAwake ();
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
				ship.movementController.pathPoints.Add (SpawnerController.instance.leftCarrierShipSpawner.GetRandomPositionInWorld());
				spawner1Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (1);
				}
			} else if (spawnerID == 2) {
				ship.movementController.pathPoints.Add (SpawnerController.instance.centerCarrierShipSpawner.GetRandomPositionInWorld());
				spawner2Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (2);
				}
			} else if (spawnerID == 3) {
				ship.movementController.pathPoints.Add (SpawnerController.instance.rightCarrierShipSpawner.GetRandomPositionInWorld());
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
