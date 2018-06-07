using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
	public ExplodeObject explodeObject;
	public MovementController movementController;
	public Animator animator;
	public delegate void AnimationAction();
	public AnimationAction animationStartAction;
	public AnimationAction animationEndAction;
	public List<ParticleSystem> stopEffects = new List<ParticleSystem>();
	public int type = 1;
	Timer spawnBulletTimer = new Timer();
	public List<Transform> bulletSpawners = new List<Transform>();


	/*void Update(){
		if (Input.GetKeyDown (KeyCode.N)) {
			Debug.Log ("asdfsdf");
			animator.Play ("s1_dead");
		}
	}*/

	public void ShipAwake(){
		gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3 (Random.Range(30f, Screen.width - 30f), Screen.height + 40, 0));
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
		explodeObject.DefaultAwake ();
		animator.SetInteger("ship_type", type);
		//animator.Play ("s" + type.ToString() + "_walk");

		foreach (ParticleSystem effect in stopEffects) {
			effect.gameObject.SetActive (false);
		}

		movementController.StandartAwake ();
	}

	public void Ship2Awake(){
		ShipAwake ();
		movementController.Ship2Awake ();
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			spawnBulletTimer.SetTimer(explodeObject.damageHealthParam.spawnTime);
		}));
	}

	public void BossExplodeSound(){
		int bossType = type - 6;

		string audioEffectPoolPath = "Prefabs/AudioEffects/Explodes/race_" + explodeObject.raceType.ToString() + "_boss_" + bossType.ToString();
		GameObject audioEffectObj = ObjectsPool.PullObject (audioEffectPoolPath);
		AudioEffect audioEffect = audioEffectObj.GetComponent<AudioEffect> ();
		audioEffect.poolPath = audioEffectPoolPath;
		audioEffect.StartEffect ();
	}

	public void Tier2ShipActive(){
		movementController.TranslateByStandartPath ();
		if (spawnBulletTimer.TimeIsOver() && explodeObject.isActive && !explodeObject.isFreeze) {
			spawnBulletTimer.SetTimer (explodeObject.damageHealthParam.spawnTime);
			string path = "Prefabs/Airships/bullets/race_" + explodeObject.raceType.ToString() + "/default";
			GameObject bulletObj = ObjectsPool.PullObject (path);
			Transform spawnerTransform = GetRandomSpawner();
			ExplodeObject bulletExpObj = bulletObj.GetComponent<ExplodeObject> ();
			bulletExpObj.explodeTransform.position = spawnerTransform.position;
			bulletExpObj.poolPath = path;
			ShipsController.instance.explodeObjects.Add (bulletExpObj);
			bulletExpObj.DefaultAwake();
		}
	}

	public void Destroy(){
		SetSoundEffect ();
		explodeObject.isActive = false;


		animationStartAction = () => {
			explodeObject.withOverscreenChecker = false;
			foreach (ParticleSystem effect in stopEffects) {
				effect.gameObject.SetActive (true);
				effect.Stop ();
				effect.Play ();

			}	
		};

		animator.Play ("s" + type.ToString() + "_dead", 0, 0);

		animationEndAction = () => {
			foreach (ParticleSystem effect in stopEffects) {
				effect.Stop ();
				effect.Play ();
				effect.gameObject.SetActive (false);
			}
			explodeObject.DefaultDestroy ();
		};
	}

	public void DestroyWithPoints(){
		SetSoundEffect ();
		explodeObject.isActive = false;
		animationStartAction = () => {
			foreach (ParticleSystem effect in stopEffects) {
				effect.Stop ();
				effect.Play ();
				effect.gameObject.SetActive (true);
			}	
		};

		animator.Play ("s" + type.ToString() + "_dead", 0, 0);
		//Debug.Log ("destr");
		animationEndAction = () => {
			foreach (ParticleSystem effect in stopEffects) {
				effect.Stop ();
				effect.Play ();
				effect.gameObject.SetActive (false);
			}
			explodeObject.DefaultDestroyWithPoints ();
		};
	}


	public void SetSoundEffect(){
		if (type != 7 && type != 8 && type != 9) {
			string audioEffectPoolPath = "";
			if (
				type == 1 ||
				type == 2 ||
				type == 3 ||
				type == 6) {
				int expIndex = Random.Range (0, 5);
				audioEffectPoolPath = "Prefabs/AudioEffects/Explodes/exp" + expIndex.ToString();
			} else if(type == 4 || type == 5){
				int expIndex = Random.Range (0, 4);
				audioEffectPoolPath = "Prefabs/AudioEffects/Explodes/texp" + expIndex.ToString();	
			}
			GameObject audioEffectObj = ObjectsPool.PullObject (audioEffectPoolPath);
			AudioEffect audioEffect = audioEffectObj.GetComponent<AudioEffect> ();
			audioEffect.poolPath = audioEffectPoolPath;
			audioEffect.StartEffect ();
		}
	}

	public Transform GetRandomSpawner(){
		return bulletSpawners [Random.Range (0, bulletSpawners.Count)];
	}

	public void AnimationEnd(){
		animationEndAction.Invoke ();
	}

	public void AnimationStart(){
		animationStartAction.Invoke ();
	}
}
