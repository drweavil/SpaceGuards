using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplodeObject : MonoBehaviour {
	public Vector3 directionVector;
	public Vector3 directionPosition;
	public string id;
	//public Vector3 speed = new Vector3(0.05f, 0.05f, 0.02f);
	public Transform explodeTransform;
	//public Collider collider;
	public float health = 0;
	public List<SpriteRenderer> objectSprites;
	public List<MeshRenderer> meshRenderers;
	List<Color> sourceColors = new List<Color>();
	bool inDamageAnimation = false;
	public int objectType;
	public int frame = 0;
	public string poolPath;
	public bool withOverscreenChecker = true;
	public bool overscreenCheckerEnable = true;
	public UnityEvent awakeAction;
	public UnityEvent isActiveAction;
	public UnityEvent destroyAction;
	public UnityEvent destroyWithPointsAction;
	public DamageHealthParam damageHealthParam;
	public bool isActive = true;
	Timer startOverscreenCheckerTimer = new Timer();
	float startOverscreenCheckerTime = 5f;
	public bool isFreeze = false;
	Timer freezeTimer = new Timer();
	public int raceType = 1;




	void Awake(){
		//Debug.Log (frame);
		damageHealthParam = DamageHealthParamController.instance.GetExplodeObjectById (id);
	}

	void Update(){
		isActiveAction.Invoke ();
		OverscreenChecker ();
	}

	public void ExplodeObjectAwake(){
		awakeAction.Invoke ();
	}
	public void ExplodeObjectDestroy (){
		destroyAction.Invoke ();
		RemoveFromExpObjList ();
	}

	public void RemoveFromExpObjList(){
		int expObjIndex = ShipsController.instance.explodeObjects.FindIndex (e => e == this);
		if (expObjIndex != -1) {
			ShipsController.instance.explodeObjects.RemoveAt (expObjIndex);
		}
	}

	public void DefaultDestroy(){
		isActive = false;
		ObjectsPool.PushObject (poolPath, this.gameObject);
		RemoveFromExpObjList ();
	}
	public void DefaultDestroyWithPoints(){
		isActive = false;
		LevelController.instance.currentPoints += damageHealthParam.points;
		BattleInterface.instance.AddPointToStack (damageHealthParam.points);
		ObjectsPool.PushObject (poolPath, this.gameObject);
		RemoveFromExpObjList ();
	}

	public void DefaultAwake(){
		inDamageAnimation = false;
		withOverscreenChecker = true;
		isFreeze = false;
		if (overscreenCheckerEnable) {
			startOverscreenCheckerTimer.SetTimer (startOverscreenCheckerTime);
		}
		frame = 0;
		GetDirectionVector ();
		SetAngle ();
		RestoreSourceColor ();
		RestoreHealth ();
		isActive = true;
	}

	public void DefaultActiveAction(){
		if (!isFreeze) {
			/*explodeTransform.Translate (new Vector3 (
				directionVector.x * damageHealthParam.bulletSpeed.x,
				directionVector.y * damageHealthParam.bulletSpeed.y,
				directionVector.z * damageHealthParam.bulletSpeed.z)
				* Time.timeScale
			);*/
			explodeTransform.position = new Vector3 (
				explodeTransform.position.x + (directionVector.x * damageHealthParam.bulletSpeed.x * Time.timeScale),
				explodeTransform.position.y + (directionVector.y * damageHealthParam.bulletSpeed.y * Time.timeScale),
				explodeTransform.position.z + (directionVector.z * damageHealthParam.bulletSpeed.z * Time.timeScale)
			);
		}
	}

	public void RestoreHealth(){
		health = damageHealthParam.health;
	}

	public void SetFreeze(float freezeTime){
		freezeTimer.SetTimer (freezeTime);
		isFreeze = true;

		if (!isFreeze) {
			StartCoroutine (FreezeCoroutine());
		}
		
	}
	IEnumerator FreezeCoroutine(){
		if (freezeTimer.TimeIsOver ()) {
			isFreeze = false;
			yield break;
		} else {
			yield return null;
		}
	}
		


	public void GetDirectionVector(){
		directionPosition = Camera.main.ScreenToWorldPoint(new Vector3 (Random.Range(30f, Screen.width - 30f), -10, 0));
		directionPosition.z = 0;

		//Debug.Log (directionPosition);
		//Debug.Log (directionPosition);

		directionVector = directionPosition - explodeTransform.position;
		directionVector.Normalize ();



		/*GameObject prefab = (GameObject)Resources.Load ("Prefabs/Airships/ship");
		GameObject newObj = Instantiate (prefab);
		newObj.transform.position = directionPosition;*/
	}

	public void SetAngle(){

		float angle = Vector3.Angle (directionVector, new Vector3 (0, 1));
		//Debug.Log (directionVector);
		if (directionVector.x > 0) {
			angle = 360 - angle;
		}

		explodeTransform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
	}




	public void MakeDamage(float damage){
		if (DamageMeter.instance.isActive) {
			DamageMeter.instance.damage += damage;
		}

		if (ShipsController.instance.doubleDamageActive) {
			damage = damage * 2f;
		}
		if (health <= damage) {
			if (isActive) {
				isActive = false;
				destroyWithPointsAction.Invoke ();
				RemoveFromExpObjList ();
			}
		} else {
			health -= damage;
			StartCoroutine (DamageAnimation());

			string audioEffectPoolPath = "Prefabs/AudioEffects/hitEffect";
			GameObject audioEffectObj = ObjectsPool.PullObject (audioEffectPoolPath);
			AudioEffect audioEffect = audioEffectObj.GetComponent<AudioEffect> ();
			audioEffect.poolPath = audioEffectPoolPath;
			audioEffect.StartEffect ();
		}




	}

	IEnumerator DamageAnimation(){
		if (!inDamageAnimation) {
			inDamageAnimation = true;
			sourceColors.Clear ();
			foreach (SpriteRenderer spr in objectSprites) {
				sourceColors.Add (spr.color);
				spr.color = Color.red;

			}

			foreach (MeshRenderer rend in meshRenderers) {
				rend.material.color = Color.red;
			}

			for (int i = 0; i < 2; i++) {
				yield return null;
			}

			RestoreSourceColor ();
			inDamageAnimation = false;
		}
	}
	public void RestoreSourceColor(){
		for (int i = 0; i < sourceColors.Count; i++) {
			objectSprites[i].color = sourceColors[i];
		}

		foreach (MeshRenderer rend in meshRenderers) {
			rend.material.color = Color.white;
		}
	}


	void OverscreenChecker(){
		if (withOverscreenChecker && startOverscreenCheckerTimer.TimeIsOver() && overscreenCheckerEnable) {
			if (frame == 60) {
				if (!RectTransformUtility.RectangleContainsScreenPoint (Interface.interfaceSt.screenArea, 
					Camera.main.WorldToScreenPoint (gameObject.transform.position))) {
					ObjectsPool.PushObject (poolPath, gameObject);
				}
				frame = 0;
			} else {
				frame += 1;
			}
		}
	}

	public void ForeverActive(){
		isActive = true;
	}
}
