using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElusiveShip : MonoBehaviour {
	public Ship ship;
	Timer shipRelocateTimer = new Timer();
	Timer bulletTimer = new Timer();
	Timer mineTimer = new Timer();


	public void ShipAwake(){
		ship.ShipAwake ();
		SetRelocateTimer ();
		ship.explodeObject.explodeTransform.rotation = Quaternion.Euler (new Vector3(0, 0, 180));
		ship.explodeObject.explodeTransform.position = SpawnerController.instance.topElusiveSpawner.GetRandomPositionInWorld ();
		GeneratePath ();
		ship.movementController.PathByPointsRebind ();
	}


	public void ShipActive(){
		if (ship.explodeObject.isActive) {
			if (shipRelocateTimer.TimeIsOver ()) {
				RelocatePosition ();
				SetRelocateTimer ();
			} else {
				ship.movementController.TranslateByPointsPath ();
			}


			if (bulletTimer.TimeIsOver ()) {
				SpawnBullets ();
			}

			if (mineTimer.TimeIsOver ()) {
				SpawnMines ();
			}
		}
	}

	public void Destroy(){
		ship.DestroyWithPoints ();
	}

	void SpawnBullets(){
		int bulletsCount = 3;
		float bulletLerpPathPart = 1f / (float)bulletsCount;
		float bulletsLerpPosition = 0;

		float minX = Camera.main.ScreenToWorldPoint(SpawnerController.instance.bottomSpawner.leftTop.position).x;
		float maxX = Camera.main.ScreenToWorldPoint(SpawnerController.instance.bottomSpawner.rightTop.position).x;
		float positionY = Camera.main.ScreenToWorldPoint(SpawnerController.instance.bottomSpawner.rightTop.position).y;

		for (int i = 0; i < bulletsCount; i++) {
			bulletTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime);
			string path = "Prefabs/Airships/bullets/race_" + ship.explodeObject.raceType.ToString () + "/defaultArmor";
			GameObject bulletObj = ObjectsPool.PullObject (path);
			Transform spawnerTransform = ship.GetRandomSpawner ();
			ExplodeObject bulletExpObj = bulletObj.GetComponent<ExplodeObject> ();
			bulletExpObj.explodeTransform.position = spawnerTransform.position;
			bulletExpObj.poolPath = path;
			ShipsController.instance.explodeObjects.Add (bulletExpObj);
			bulletExpObj.DefaultAwake ();
			bulletExpObj.directionPosition = Vector3.Lerp(new Vector3(minX, positionY, 0), new Vector3(maxX, positionY, 0), bulletsLerpPosition);
			bulletExpObj.directionVector = bulletExpObj.directionPosition - bulletExpObj.explodeTransform.position;
			bulletExpObj.directionVector.Normalize ();
			bulletExpObj.SetAngle ();

			bulletsLerpPosition += bulletLerpPathPart;
		}
	}

	void SpawnMines(){
		int bulletsCount = 2;
		float bulletLerpPathPart = 1f / (float)bulletsCount;
		float bulletsLerpPosition = 0;

		float minX = Camera.main.ScreenToWorldPoint(SpawnerController.instance.bottomSpawner.leftTop.position).x;
		float maxX = Camera.main.ScreenToWorldPoint(SpawnerController.instance.bottomSpawner.rightTop.position).x;
		float positionY = Camera.main.ScreenToWorldPoint(SpawnerController.instance.bottomSpawner.rightTop.position).y;

		for (int i = 0; i < bulletsCount; i++) {
			mineTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime2);
			string path = "Prefabs/Airships/bullets/race_" + ship.explodeObject.raceType.ToString () + "/mine";
			GameObject bulletObj = ObjectsPool.PullObject (path);
			Transform spawnerTransform = ship.GetRandomSpawner ();
			ExplodeObject bulletExpObj = bulletObj.GetComponent<ExplodeObject> ();
			bulletExpObj.explodeTransform.position = spawnerTransform.position;
			bulletExpObj.poolPath = path;
			ShipsController.instance.explodeObjects.Add (bulletExpObj);
			bulletExpObj.DefaultAwake ();
			bulletExpObj.directionPosition = Vector3.Lerp(new Vector3(minX, positionY, 0), new Vector3(maxX, positionY, 0), bulletsLerpPosition);
			bulletExpObj.directionVector = bulletExpObj.directionPosition - bulletExpObj.explodeTransform.position;
			bulletExpObj.directionVector.Normalize ();
			bulletExpObj.SetAngle ();

			bulletsLerpPosition += bulletLerpPathPart;
		}
	}


	void RelocatePosition(){
		string path = "Prefabs/Effects/warp";
		GameObject expObj = ObjectsPool.PullObject (path);
		Effect exp = expObj.GetComponent<Effect>();
		exp.poolPath = path;
		exp.transform.position = this.transform.position;
		exp.main.Play ();
		exp.DestoyOverTime(exp.main.main.duration);




		int random = Random.Range (0,3);
		if (random == 0) {
			ship.explodeObject.explodeTransform.position = SpawnerController.instance.topElusiveSpawner.GetRandomPositionInWorld ();
		} else if (random == 1) {
			ship.explodeObject.explodeTransform.position = SpawnerController.instance.topLeftElusiveSpawner.GetRandomPositionInWorld ();
		} else if (random == 2) {
			ship.explodeObject.explodeTransform.position = SpawnerController.instance.topRightElusiveSpawner.GetRandomPositionInWorld ();
		}




		GeneratePath ();
		ship.movementController.PathByPointsRebind ();

	}

	void SetRelocateTimer(){
		float time = Random.Range (5f, 10f);
		shipRelocateTimer.SetTimer (time);
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
				ship.movementController.pathPoints.Add (SpawnerController.instance.leftElusiveBossSpawner.GetRandomPositionInWorld());
				spawner1Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (1);
				}
			} else if (spawnerID == 2) {
				ship.movementController.pathPoints.Add (SpawnerController.instance.centerElusiveBossSpawner.GetRandomPositionInWorld());
				spawner2Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (2);
				}
			} else if (spawnerID == 3) {
				ship.movementController.pathPoints.Add (SpawnerController.instance.rightElusiveBossSpawner.GetRandomPositionInWorld());
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
