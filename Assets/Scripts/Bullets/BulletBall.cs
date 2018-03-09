using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBall : MonoBehaviour {
	public Bullet bullet;
	public Timer bulletBallDamageTimer = new Timer();
	public float damageTickTime = 0.5f;


	public void BulletBallAwake(){
		bullet.ClearColliders();
		bulletBallDamageTimer.SetTimer (0);
	}
	public void BulletBallTrigger(ExplodeObject expObject){
		if (bulletBallDamageTimer.TimeIsOver ()) {
			//bullet.ClearColliders();
			if (expObject.objectType == bullet.bulletType || expObject.objectType == Cannon.allBullet) {
				expObject.MakeDamage (bullet.damageHealthParams.damage);
			}
			bulletBallDamageTimer.SetTimer (bullet.damageHealthParams.damageTickTime);
			StartCoroutine (bulletBallDamageTimer.ActionAfterTimer (() => {
				if(bullet.gameObject.activeInHierarchy){
					bullet.ClearColliders();
				}
			}));
		}
	}
}
