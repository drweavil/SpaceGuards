using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
	public ExplodeObject expObject;
	Vector3 rotationVector;
	public MovementController movementController;
	public Transform boxTransform;
	public int lastBulletCannonType;
	Timer existTimer = new Timer();
	public const int regenID = 1, wipeID = 2, shieldID = 3, doubleDamageID = 4, freezeID = 5;

	/*void Awake(){
		BoxAwake ();
	}*/

	void Update(){
		if (existTimer.TimeIsOver ()) {
			

			DestroyEffect ();

			expObject.RemoveFromExpObjList ();
			expObject.DefaultDestroy ();
		}
	}

	public void BoxAwake(){
		existTimer.SetTimer(expObject.damageHealthParam.existTime);
		expObject.DefaultAwake ();
		expObject.explodeTransform.position = SpawnerController.instance.topSpawner.GetRandomPositionInWorld ();
		rotationVector = new Vector3 (Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
		GeneratePath ();
		movementController.PathByPointsRebind ();
	}

	void DestroyEffect(){
		GameObject expEffectObj = ObjectsPool.PullObject ("Prefabs/Effects/explode");
		expEffectObj.transform.position = expObject.explodeTransform.position;
		Effect expEffect = expEffectObj.GetComponent<Effect> ();
		expEffect.main.Play ();
		expEffect.poolPath = "Prefabs/Effects/explode";
		expEffect.DestoyOverTime (expEffect.main.main.duration);
	}

	public void BoxActiveAction(){
		boxTransform.Rotate (rotationVector);
		//boxTransform.rotation = Quaternion.Euler (boxTransform.rotation.eulerAngles + rotationVector);
		movementController.TranslateByPointsPath ();
	}
	void GeneratePath(){
		movementController.pathPoints.Clear ();
		int pointsCount = Random.Range (3, 5);
		bool spawner1Used = false;
		bool spawner2Used = false;
		bool spawner3Used = false;
		bool allSpawnersUsed = false;
		List<int> spawnerIDs = new List<int> (new int[] {1, 2, 3});
		for (int i = 0; i < pointsCount; i++) {
			int spawnerID = spawnerIDs[Random.Range (0, spawnerIDs.Count)];


			if (spawnerID == 1) {
				movementController.pathPoints.Add (SpawnerController.instance.boxPathSpawner1.GetRandomPositionInWorld());
				spawner1Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (1);
				}
			} else if (spawnerID == 2) {
				movementController.pathPoints.Add (SpawnerController.instance.boxPathSpawner2.GetRandomPositionInWorld());
				spawner2Used = true;
				if (!allSpawnersUsed) {
					spawnerIDs.Remove (2);
				}
			} else if (spawnerID == 3) {
				movementController.pathPoints.Add (SpawnerController.instance.boxPathSpawner3.GetRandomPositionInWorld());
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
	public void SetCannonDefault(){
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			DestroyEffect();
			movementController.expObject.DefaultDestroyWithPoints();
			if(lastBulletCannonType == Cannon.leftBullet){
				CannonsPanel.instance.SetLeftCannon(Cannon.defaultCannon);
			}else if(lastBulletCannonType == Cannon.rightBullet){
				CannonsPanel.instance.SetRightCannon(Cannon.defaultCannon);
			}
		}));
	}

	public void SetCannonMine(){
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			DestroyEffect();
			movementController.expObject.DefaultDestroyWithPoints();
			if(lastBulletCannonType == Cannon.leftBullet){
				CannonsPanel.instance.SetLeftCannon(Cannon.mineCannon);
			}else if(lastBulletCannonType == Cannon.rightBullet){
				CannonsPanel.instance.SetRightCannon(Cannon.mineCannon);
			}
		}));
	}

	public void SetCannonTriangle(){
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			DestroyEffect();
			movementController.expObject.DefaultDestroyWithPoints();
			if(lastBulletCannonType == Cannon.leftBullet){
				CannonsPanel.instance.SetLeftCannon(Cannon.triangleCannon);
			}else if(lastBulletCannonType == Cannon.rightBullet){
				CannonsPanel.instance.SetRightCannon(Cannon.triangleCannon);
			}
		}));
	}

	public void SetCannonBall(){
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			DestroyEffect();
			movementController.expObject.DefaultDestroyWithPoints();
			if(lastBulletCannonType == Cannon.leftBullet){
				CannonsPanel.instance.SetLeftCannon(Cannon.ballCannon);
			}else if(lastBulletCannonType == Cannon.rightBullet){
				CannonsPanel.instance.SetRightCannon(Cannon.ballCannon);
			}
		}));
	}

	public void SetCannonDefaultTripple(){
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			DestroyEffect();
			movementController.expObject.DefaultDestroyWithPoints();
			if(lastBulletCannonType == Cannon.leftBullet){
				CannonsPanel.instance.SetLeftCannon(Cannon.defaultTrippleCannon);
			}else if(lastBulletCannonType == Cannon.rightBullet){
				CannonsPanel.instance.SetRightCannon(Cannon.defaultTrippleCannon);
			}
		}));
	}

	public void SetCannonTriangleTripple(){
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			DestroyEffect();
			movementController.expObject.DefaultDestroyWithPoints();
			if(lastBulletCannonType == Cannon.leftBullet){
				CannonsPanel.instance.SetLeftCannon(Cannon.triangleTrippleCannon);
			}else if(lastBulletCannonType == Cannon.rightBullet){
				CannonsPanel.instance.SetRightCannon(Cannon.triangleTrippleCannon);
			}
		}));
	}

	public void SetCannonLaser(){
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			DestroyEffect();
			movementController.expObject.DefaultDestroyWithPoints();
			if(lastBulletCannonType == Cannon.leftBullet){
				CannonsPanel.instance.SetLeftCannon(Cannon.laserCannon);
			}else if(lastBulletCannonType == Cannon.rightBullet){
				CannonsPanel.instance.SetRightCannon(Cannon.laserCannon);
			}
		}));
	}

	public void SetCannonMineShatter(){
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			DestroyEffect();
			movementController.expObject.DefaultDestroyWithPoints();
			if(lastBulletCannonType == Cannon.leftBullet){
				CannonsPanel.instance.SetLeftCannon(Cannon.mineShatterCannon);
			}else if(lastBulletCannonType == Cannon.rightBullet){
				CannonsPanel.instance.SetRightCannon(Cannon.mineShatterCannon);
			}
		}));
	}

	public void Regen(){
		DestroyEffect();
		movementController.expObject.DefaultDestroyWithPoints();
		CannonsPanel.instance.RestoreHealth (CannonsPanel.instance.maximumHealth * 0.25f);
	}

	public void AddWipeAbitity(){
		DestroyEffect();
		movementController.expObject.DefaultDestroyWithPoints();
		LevelController.instance.gameSessionData.wipeAbilityCount += 1;
		BattleInterface.instance.RedrawAllInfo ();
		LevelController.instance.SaveGameSessionData ();
	}
	public void AddDoubleDamageAbitity(){
		DestroyEffect();
		movementController.expObject.DefaultDestroyWithPoints();
		LevelController.instance.gameSessionData.doubleDamageAbilityCount += 1;
		BattleInterface.instance.RedrawAllInfo ();
		LevelController.instance.SaveGameSessionData ();
	}
	public void AddFreezeAbitity(){
		DestroyEffect();
		movementController.expObject.DefaultDestroyWithPoints();
		LevelController.instance.gameSessionData.freezeAbilityCount += 1;
		BattleInterface.instance.RedrawAllInfo ();
		LevelController.instance.SaveGameSessionData ();
	}
	public void AddShieldAbitity(){
		DestroyEffect();
		movementController.expObject.DefaultDestroyWithPoints();
		LevelController.instance.gameSessionData.shieldAbilityCount += 1;
		BattleInterface.instance.RedrawAllInfo ();
		LevelController.instance.SaveGameSessionData ();
	}


	void OnTriggerEnter(Collider col){
		if (col.gameObject.layer == LayerMask.NameToLayer ("Bullet")) {
			lastBulletCannonType = col.gameObject.GetComponent<BulletCollider> ().bullet.bulletType;
		}
	}


}
