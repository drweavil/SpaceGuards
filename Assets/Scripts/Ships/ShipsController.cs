using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsController : MonoBehaviour {
	public static ShipsController instance;
	public List<ExplodeObject> explodeObjects = new List<ExplodeObject>();
	public ParticleSystem freez;
	public static float freezeTime = 50f;
	public static float shieldPercent = 100;
	public static float doubleDamageTime = 15f;
	public ParticleSystem wipe;
	public bool doubleDamageActive = false;
	Timer doubleDamageTimer = new Timer();


	void Awake(){
		instance = this;
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




	public void Wipe(){
		wipe.Play ();
		foreach (ExplodeObject expObj in explodeObjects) {
			if (expObj.id != "standartBox") {
				expObj.destroyWithPointsAction.Invoke ();
			}
		}
		explodeObjects.Clear ();
	}


	public void Freeze(){
		freez.Play ();
		foreach (ExplodeObject expObj in explodeObjects) {
			expObj.SetFreeze(freezeTime);
		}
	}

	public void Shield(){
		CannonsPanel.instance.SetShieldPoints (CannonsPanel.instance.maximumHealth * (shieldPercent/100));
	}

	public void DoubleDamage(){
		doubleDamageTimer.SetTimer (doubleDamageTime);
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
