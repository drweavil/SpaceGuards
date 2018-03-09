using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polynomial : MonoBehaviour {
	public Ship ship;
	public List<ExplodeObject> shipParts = new List<ExplodeObject>();
	int activeParts = 0;
	Timer bulletTimer = new Timer();
	Timer mineTimer = new Timer();

	public void ShipAwake(){
		activeParts = shipParts.Count;	
		StartCoroutine(GameController.ActionAfterFewFramesCoroutine (1, () => {
			foreach (ExplodeObject expObj in shipParts) {
				expObj.gameObject.SetActive (true);
				expObj.RestoreHealth ();
				expObj.isActive = true;
				expObj.RestoreSourceColor();
				Debug.Log(expObj.gameObject.name);
			}
		}));
		ship.ShipAwake ();
		bulletTimer.SetTimer (ship.explodeObject.damageHealthParam.spawnTime);
		ship.explodeObject.explodeTransform.rotation = Quaternion.Euler (new Vector3(0, 0, 180));
		ship.explodeObject.explodeTransform.position = SpawnerController.instance.topSpawner.GetRandomPositionInWorld ();
		GeneratePath ();
		ship.movementController.PathByPointsRebind ();
	}

	public void ShipActive(){
		
		if (ship.explodeObject.isActive) {
			/*if (bulletTimer.TimeIsOver ()) {
				SpawnBullets ();
			}*/
			ship.movementController.TranslateByPointsPath ();
			if (mineTimer.TimeIsOver ()) {
				SpawnMines ();
			}
		}
	}

	void SpawnBullets(){
		int bulletsCount = 1;
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
		int bulletsCount = 8;
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


	public void TryDestroy(){
		activeParts -= 1;
		if (activeParts <= 0) {
			ship.DestroyWithPoints ();
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
