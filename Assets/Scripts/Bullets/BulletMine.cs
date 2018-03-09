using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMine : MonoBehaviour {
	public Bullet bullet;
	public BulletCollider bulletCollider;
	Timer explodeTimer = new Timer();
	float explodeTime = 0.2f;
	bool explodeTimerActive = false;
	//float bulletDamage = 1;
	public bool isShutterMine = false;
	public int shattersCount = 5;

	public void BulletMineTrigger(ExplodeObject expObj){
		if (Cannon.CheckSuitability(expObj.objectType, bullet.bulletType)) {
			if (!explodeTimerActive) {
				explodeTimer.SetTimer (explodeTime);
				explodeTimerActive = true;
				StartCoroutine (explodeTimer.ActionAfterTimer (() => {
					foreach (ExplodeObject obj in bullet.triggeredExplodeObject) {
						if (obj.gameObject.activeInHierarchy) {
							if(Cannon.CheckSuitability(obj.objectType, bullet.bulletType)){
								obj.MakeDamage (bullet.damageHealthParams.damage);
							}
						}
					}

					string effectPath = "";
					if(isShutterMine){
						effectPath = "Prefabs/Effects/bullet2ExplodeShatter";
					}else{
						effectPath = "Prefabs/Effects/bullet2Explode";
					}
					Vector3 bulletPosition = bullet.transform.position;
					GameObject effectObj = ObjectsPool.PullObject (effectPath);
					effectObj.transform.position = bulletPosition;
					Effect effect = effectObj.GetComponent<Effect> ();
					effect.poolPath = effectPath;
					effect.main.Play ();
					effect.DestoyOverTime (effect.main.main.duration);

					if(isShutterMine){
						string shatterPath = "Prefabs/Bullets/bullet2ShatterPart";
						for(int i = 0; i < shattersCount; i++){
							GameObject shatter = ObjectsPool.PullObject (shatterPath);
							//Debug.Log(bullet.transform.position);
							shatter.transform.position = bulletPosition;
							Bullet sbullet = shatter.GetComponent<Bullet> ();
							sbullet.poolPath = shatterPath;
							sbullet.BulletAwake ();
							sbullet.bulletType = bullet.bulletType;
						}
					}

					ObjectsPool.PushObject (bullet.poolPath, this.gameObject);
				}));
			}
		}
	}

	public void BulletMineAwake(){
		bullet.DefaultBulletAwake ();
		explodeTimerActive = false;
		bulletCollider.colliderExitAction = (Collider col) => {
			if(col.gameObject.layer == LayerMask.NameToLayer("ExplodeObject")){
				int colliderIndex = bullet.triggeredColliders.FindIndex(c => col == c);
				bullet.triggeredColliders.Remove(col);

				if(bullet.triggeredColliders.FindIndex(
					c => c.gameObject.GetComponent<ExplodeObjectCollider>().explodeObject == 
					col.gameObject.GetComponent<ExplodeObjectCollider>().explodeObject) == -1
				){
					bullet.triggeredExplodeObject.Remove(col.gameObject.GetComponent<ExplodeObjectCollider>().explodeObject);
				}	
			}
		};
	}
}
