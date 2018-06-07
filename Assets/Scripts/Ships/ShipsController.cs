using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsController : MonoBehaviour {
	public static ShipsController instance;
	public List<ExplodeObject> explodeObjects = new List<ExplodeObject>();
	public ParticleSystem freez;
	public static float cd = 0.5f;
	public static float freezeTime = 5f;
	public static float shieldPercent = 25;
	public static float doubleDamageTime = 15f;
	public ParticleSystem wipe;
	public bool doubleDamageActive = false;
	Timer doubleDamageTimer = new Timer();

	public int usedWipe = 0;
	public int usedShield = 0;
	public int usedFreeze = 0;
	public int usedDoubleDamage = 0;


	void Awake(){
		instance = this;
	}

	public float GetAllShipHealth(){
		float allHealth = 0;
		foreach (ExplodeObject expObject in explodeObjects) {
			if(
				expObject.id == "ship_1" ||
				expObject.id == "ship_2" ||
				expObject.id == "ship_3" ||
				expObject.id == "ship_4" ||
				expObject.id == "ship_5" ||
				expObject.id == "ship_6"
			)
			allHealth += expObject.health;
		}
		return allHealth;
	}

	public void SpawnBoss(int race, int type){
		string poolPath = "Prefabs/Airships/bosses/race_" + race.ToString () + "_boss_" + type.ToString (); 
		GameObject newObj = ObjectsPool.PullObject(poolPath);


		ExplodeObject expObj = newObj.GetComponent<ExplodeObject> ();
		expObj.ExplodeObjectAwake();
		expObj.poolPath = poolPath;
		explodeObjects.Add (expObj);
	}


	public ExplodeObject SpawnShip(int race, int type, int expObjType){
		string expObjTypeString = "";
		if (expObjType == Cannon.leftBullet) {
			expObjTypeString = "leftShips";
		} else if (expObjType == Cannon.rightBullet) {
			expObjTypeString = "rightShips";
		} else if (expObjType == Cannon.allBullet) {
			expObjTypeString = "allShips";
		}


		string poolPath = "Prefabs/Airships/" + expObjTypeString + "/race_" + race.ToString () + "/ship_" + type.ToString (); 
		GameObject newObj = ObjectsPool.PullObject(poolPath);


		ExplodeObject expObj = newObj.GetComponent<ExplodeObject> ();
		expObj.ExplodeObjectAwake();
		expObj.poolPath = poolPath;
		explodeObjects.Add (expObj);
		return expObj;
	}

	public void SpawnCannonBox(int cannonType){
		string path = "Prefabs/Boxes/cannonBox_" + cannonType.ToString ();
		GameObject boxObj = ObjectsPool.PullObject (path);
		Box box = boxObj.GetComponent<Box> ();
		box.expObject.poolPath = path;
		box.expObject.ExplodeObjectAwake ();
		explodeObjects.Add (box.expObject);
	}
	public void SpawnAbilityBox(int boxID){
		string path = "Prefabs/Boxes/abilityBox_" + boxID.ToString();
		GameObject boxObj = ObjectsPool.PullObject (path);
		Box box = boxObj.GetComponent<Box> ();
		box.expObject.poolPath = path;
		box.expObject.ExplodeObjectAwake ();
		explodeObjects.Add (box.expObject);
	}

	public void SpawnRandomCannonBox(){
		float random1 = Random.Range (0, 100.00001f);
		if (random1 <= 9f) {
			List<int> cannonsID = new List<int> (new int[]{ 
				Cannon.defaultTrippleCannon, 
				Cannon.triangleTrippleCannon,
				Cannon.mineShatterCannon
			});
			ShipsController.instance.SpawnCannonBox(cannonsID[Random.Range (0, cannonsID.Count)]);
		} else {
			List<int> cannonsID = new List<int> (new int[]{ 
				Cannon.mineCannon, 
				Cannon.triangleCannon,
				Cannon.ballCannon,
				Cannon.laserCannon
			});
			ShipsController.instance.SpawnCannonBox(cannonsID[Random.Range (0, cannonsID.Count)]);
		}
	}




	public void ResetUsedAbilities(bool withResetGameData = false){
		if (withResetGameData) {
			LevelController.instance.gameSessionData.wipeAbilityCount += usedWipe;
			LevelController.instance.gameSessionData.shieldAbilityCount += usedShield;
			LevelController.instance.gameSessionData.freezeAbilityCount += usedFreeze;
			LevelController.instance.gameSessionData.doubleDamageAbilityCount += usedDoubleDamage;
			LevelController.instance.SaveGameSessionData ();
		}
		usedWipe = 0;
		usedDoubleDamage = 0;
		usedShield = 0;
		usedFreeze = 0;
	}

	public void Wipe(){
		wipe.Play ();
		Dictionary<int, ExplodeObject> expObjDict = new Dictionary<int, ExplodeObject> ();
		for (int i =0; i< explodeObjects.Count;i++) {
			expObjDict.Add (i, explodeObjects[i]);
		}

		for (int i = 0; i < expObjDict.Count; i++) {
			if (
				expObjDict [i].id != "standartBox" &&
				expObjDict [i].id != "boss_1" &&
				expObjDict [i].id != "boss_2" &&
				expObjDict [i].id != "boss_3" &&
				expObjDict [i].id != "boss_3_part"
			) {
				expObjDict [i].destroyWithPointsAction.Invoke ();
			}
		}
	}


	public void Freeze(){
		freez.Play ();
		foreach (ExplodeObject expObj in explodeObjects) {
			if (
				//expObj.id != "standartBox" &&
				expObj.id != "boss_1" &&
				expObj.id != "boss_2" &&
				expObj.id != "boss_3" &&
				expObj.id != "boss_3_part") {
				expObj.SetFreeze (freezeTime);
			}
		}
	}

	public void Shield(){
		CannonsPanel.instance.SetShieldPoints (CannonsPanel.instance.maximumHealth * (shieldPercent/100));
	}

	public void DoubleDamage(){
		doubleDamageTimer.SetTimer (ShipsController.doubleDamageTime);
		foreach (ParticleSystem eff in CannonsPanel.instance.overdriveEffects) {
			eff.gameObject.SetActive (true);
		}
		doubleDamageActive = true;
		StartCoroutine (DoubleDamageCoroutine());
		
	}
	IEnumerator DoubleDamageCoroutine(){
		while (doubleDamageActive) {
			if (doubleDamageTimer.TimeIsOver ()) {
				doubleDamageActive = false;
				foreach (ParticleSystem eff in CannonsPanel.instance.overdriveEffects) {
					eff.gameObject.SetActive (false);
				}
				yield break;
			} else {
				yield return null;
			}
		}
	}

}
