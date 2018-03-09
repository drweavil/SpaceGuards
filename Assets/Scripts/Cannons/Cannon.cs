using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cannon : MonoBehaviour {
	public Transform cannonTubeTransform;
	public Transform cannonBulletSpawner;
	public Transform cannonBulletLeftSpawner;
	public Transform cannonBulletRightSpawner;


	public Transform cannonCore;
	public Transform cannonBulletCoreLeft;
	public Transform cannonBulletCoreRight;
	public bool isShatter = false;
	public static int leftBullet = 0, rightBullet = 1, allBullet = 2;
	public static int defaultCannon = 1, mineCannon = 2, triangleCannon = 3, ballCannon = 4, defaultTrippleCannon = 5,
	triangleTrippleCannon = 6, laserCannon = 7, mineShatterCannon = 8; 
	public int cannonType;
	public bool isActive = false;
	public int cannonID;
	public string poolPath;

	Vector2 aimPosition = new Vector2();
	Vector2 aimDirection;


	Timer bulletTimer = new Timer ();
	public float bulletSpawnTime = 0.05f;
	public DamageHealthParam damageHealthParam;
	public UnityEvent awakeAction;
	public UnityEvent isActiveAction;
	public UnityEvent notActiveAction;

	public ParticleSystem spawnEffect;
	public List<ParticleSystem> spawnEffects = new List<ParticleSystem>();


	void Awake(){
		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(1, () => {
			damageHealthParam = DamageHealthParamController.instance.GetCannonParamsById (cannonID);
			awakeAction.Invoke ();
		}));
	}


	void Update(){
		if (damageHealthParam != null) {
			if (cannonType == leftBullet) {
				if (AimTouchController.aimTouchController.leftTouchActive) {
					aimPosition = AimTouchController.aimTouchController.leftTouchPosition;
					isActive = true;
				} else {
					isActive = false;
				}
			} else if (cannonType == rightBullet) {
				if (AimTouchController.aimTouchController.rightTouchActive) {
					aimPosition = AimTouchController.aimTouchController.rightTouchPosition;
					isActive = true;
				} else {
					isActive = false;
				}
			}


			if (isActive) {
				aimDirection = cannonCore.position - Camera.main.ScreenToWorldPoint (aimPosition);
				aimDirection.Normalize ();
				float angle = Vector3.Angle (aimDirection, new Vector3 (0, 1));
				if (aimDirection.x > 0) {
					angle = 360 - angle;
				}
				angle += 180;

				cannonTubeTransform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));

				isActiveAction.Invoke ();
			} else {
				notActiveAction.Invoke ();
			}

		}

	}

	public static bool CheckSuitability(int expType, int bulletType){
		return expType == bulletType || expType == allBullet || bulletType == allBullet;
	}

	public Vector3 GetCannonDirection(){
		Vector3 cannonDirection = cannonBulletSpawner.position - cannonCore.position; 
		cannonDirection.Normalize ();
		return cannonDirection;
	}


	public void DefaultAwake(){
		bulletTimer.SetTimer (damageHealthParam.spawnTime);
	}
	public void DefaultSpawnBullet(){
		if (bulletTimer.TimeIsOver ()) {
			string bulletPath = "";
			if (cannonType == leftBullet) {
				bulletPath = PoolPath.cannonBulletDefaultLeft;
			} else if (cannonType == rightBullet) {
				bulletPath = PoolPath.cannonBulletDefaultRight;
			} 

			GameObject newObj = ObjectsPool.PullObject (bulletPath);

			Vector3 bulletDirection = cannonBulletSpawner.position - cannonCore.position;
			bulletDirection.Normalize ();
			newObj.transform.position = cannonBulletSpawner.position;
			Bullet bullet = newObj.GetComponent<Bullet> ();
			bullet.poolPath = bulletPath;
			bullet.directionVector = bulletDirection;
			bullet.BulletAwake ();
			bullet.bulletType = cannonType;
			//Debug.Log (bulletDirection);
			spawnEffect.Play();

			bulletTimer.SetTimer (damageHealthParam.spawnTime);
		}
	}


	public void BallBulletSpawn(){
		if (bulletTimer.TimeIsOver ()) {
			string bulletPath = "";
			if (cannonType == leftBullet) {
				bulletPath = PoolPath.cannonBulletBallLeft;
			} else if (cannonType == rightBullet) {
				bulletPath = PoolPath.cannonBulletBallRight;
			} 

			GameObject newObj = ObjectsPool.PullObject (bulletPath);

			Vector3 bulletDirection = cannonBulletSpawner.position - cannonCore.position;
			bulletDirection.Normalize ();
			newObj.transform.position = cannonBulletSpawner.position;
			Bullet bullet = newObj.GetComponent<Bullet> ();
			bullet.poolPath = bulletPath;
			bullet.directionVector = bulletDirection;
			bullet.BulletAwake ();
			bullet.bulletType = cannonType;

			bulletTimer.SetTimer (damageHealthParam.spawnTime);
		}
	}



	public void DefaultTrippleAwake(){
		bulletTimer.SetTimer (damageHealthParam.spawnTime);
	}
	public void DefaultTrippleSpawnBullet(){
		if (bulletTimer.TimeIsOver ()) {
			string bulletPath = "";
			if (cannonType == leftBullet) {
				bulletPath = PoolPath.cannonBulletDefaultLeft;
			} else if (cannonType == rightBullet) {
				bulletPath = PoolPath.cannonBulletDefaultRight;
			} 

			GameObject newBullet = ObjectsPool.PullObject (bulletPath);
			Vector3 bulletDirection = cannonBulletSpawner.position - cannonCore.position;
			bulletDirection.Normalize ();
			newBullet.transform.position = cannonBulletSpawner.position;
			Bullet bullet = newBullet.GetComponent<Bullet> ();
			bullet.poolPath = bulletPath;
			bullet.directionVector = bulletDirection;
			bullet.BulletAwake ();
			bullet.bulletType = cannonType;

			GameObject newBulletLeft = ObjectsPool.PullObject (bulletPath);
			Vector3 bulletDirectionLeft = cannonBulletLeftSpawner.position - cannonBulletCoreLeft.position;
			bulletDirectionLeft.Normalize ();
			newBulletLeft.transform.position = cannonBulletLeftSpawner.position;
			Bullet bulletLeft = newBulletLeft.GetComponent<Bullet> ();
			bulletLeft.poolPath = bulletPath;
			bulletLeft.directionVector = bulletDirectionLeft;
			bulletLeft.BulletAwake ();
			bulletLeft.bulletType = cannonType;

			GameObject newBulletRight = ObjectsPool.PullObject (bulletPath);
			Vector3 bulletDirectionRight = cannonBulletRightSpawner.position - cannonBulletCoreRight.position;
			bulletDirectionRight.Normalize ();
			newBulletRight.transform.position = cannonBulletRightSpawner.position;
			Bullet bulletRight = newBulletRight.GetComponent<Bullet> ();
			bulletRight.poolPath = bulletPath;
			bulletRight.directionVector = bulletDirectionRight;
			bulletRight.BulletAwake ();
			bulletRight.bulletType = cannonType;

			foreach (ParticleSystem effect in spawnEffects) {
				effect.Play ();
			}

			bulletTimer.SetTimer (damageHealthParam.spawnTime);
		}
	}



	public void MineSpawnBullet (){
		if (bulletTimer.TimeIsOver ()) {
			string bulletPath = "";
			if (cannonType == leftBullet) {
				if (isShatter) {
					bulletPath = PoolPath.cannonBulletMineShatterLeft;
				} else {
					bulletPath = PoolPath.cannonBulletMineLeft;
				}
			} else if (cannonType == rightBullet) {
				if (isShatter) {
					bulletPath = PoolPath.cannonBulletMineShatterRight;
				} else {
					bulletPath = PoolPath.cannonBulletMineRight;
				}
			} 

			GameObject newObj = ObjectsPool.PullObject (bulletPath);

			Vector3 bulletDirection = cannonBulletSpawner.position - cannonCore.position;
			bulletDirection.Normalize ();
			newObj.transform.position = cannonBulletSpawner.position;
			Bullet bullet = newObj.GetComponent<Bullet> ();
			bullet.poolPath = bulletPath;
			bullet.directionVector = bulletDirection;
			bullet.BulletAwake ();
			bullet.bulletType = cannonType;
			/*string effectPath = "Prefabs/Effects/mineSpawnEffect";
			GameObject effectObj = ObjectsPool.PullObject(effectPath);
			effectObj.transform.position = cannonBulletSpawner.position; 
			Effect effect = effectObj.GetComponent<Effect> ();
			effect.poolPath = effectPath;
			effect.main.Play ();
			effect.DestoyOverTime (effect.main.main.duration);*/
			spawnEffect.Play ();

			bulletTimer.SetTimer (damageHealthParam.spawnTime);
		}
	}

	public void BulletTriangleSpawn(){
		if (bulletTimer.TimeIsOver ()) {
			string bulletPath = "";
			if (cannonType == leftBullet) {
				bulletPath = PoolPath.cannonBulletTriangleLeft;
			} else if (cannonType == rightBullet) {
				bulletPath = PoolPath.cannonBulletTriangleRight;
			} 

			GameObject newObj = ObjectsPool.PullObject (bulletPath);

			Vector3 bulletDirection = cannonBulletSpawner.position - cannonCore.position;
			bulletDirection.Normalize ();
			newObj.transform.position = cannonBulletSpawner.position;
			Bullet bullet = newObj.GetComponent<Bullet> ();
			bullet.poolPath = bulletPath;
			bullet.directionVector = bulletDirection;
			bullet.BulletAwake ();
			bullet.bulletType = cannonType;

			bulletTimer.SetTimer (damageHealthParam.spawnTime);
		}
	}


	public void TriangleTrippleSpawnBullet(){
		if (bulletTimer.TimeIsOver ()) {
			string bulletPath = "";
			if (cannonType == leftBullet) {
				bulletPath = PoolPath.cannonBulletTriangleLeft;
			} else if (cannonType == rightBullet) {
				bulletPath = PoolPath.cannonBulletTriangleRight;
			} 

			GameObject newBullet = ObjectsPool.PullObject (bulletPath);
			Vector3 bulletDirection = cannonBulletSpawner.position - cannonCore.position;
			bulletDirection.Normalize ();
			newBullet.transform.position = cannonBulletSpawner.position;
			Bullet bullet = newBullet.GetComponent<Bullet> ();
			bullet.poolPath = bulletPath;
			bullet.directionVector = bulletDirection;
			bullet.BulletAwake ();
			bullet.bulletType = cannonType;

			GameObject newBulletLeft = ObjectsPool.PullObject (bulletPath);
			Vector3 bulletDirectionLeft = cannonBulletLeftSpawner.position - cannonCore.position;
			bulletDirectionLeft.Normalize ();
			newBulletLeft.transform.position = cannonBulletLeftSpawner.position;
			Bullet bulletLeft = newBulletLeft.GetComponent<Bullet> ();
			bulletLeft.poolPath = bulletPath;
			bulletLeft.directionVector = bulletDirectionLeft;
			bulletLeft.BulletAwake ();
			bulletLeft.bulletType = cannonType;

			GameObject newBulletRight = ObjectsPool.PullObject (bulletPath);
			Vector3 bulletDirectionRight = cannonBulletRightSpawner.position - cannonCore.position;
			bulletDirectionRight.Normalize ();
			newBulletRight.transform.position = cannonBulletRightSpawner.position;
			Bullet bulletRight = newBulletRight.GetComponent<Bullet> ();
			bulletRight.poolPath = bulletPath;
			bulletRight.directionVector = bulletDirectionRight;
			bulletRight.BulletAwake ();
			bulletRight.bulletType = cannonType;

			bulletTimer.SetTimer (damageHealthParam.spawnTime);
		}
	}
}
