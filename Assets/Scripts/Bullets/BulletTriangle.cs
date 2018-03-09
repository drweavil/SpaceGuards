using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTriangle : MonoBehaviour {
	public Transform triangle1Transform;
	public Transform triangle2Transform;
	public Bullet bullet;
	public Vector3 maximumSize;
	public int currentState = 0;


	public void BulletAwake(){
		bullet.ClearColliders ();
		triangle1Transform.localScale = maximumSize;
		triangle2Transform.localScale = maximumSize;
		currentState = 0;
		bullet.DefaultBulletAwake ();
	}

	public void BulletTrigger(ExplodeObject expObj){
		if (Cannon.CheckSuitability (expObj.objectType, bullet.bulletType)) {
			expObj.MakeDamage (GetValueByState(bullet.damageHealthParams.damage, currentState));
			currentState += 1;
			if (currentState == 3) {
				ObjectsPool.PushObject (bullet.poolPath, bullet.gameObject);
			} else {
				triangle1Transform.localScale = new Vector3 (
					GetValueByState (triangle1Transform.localScale.x, currentState),
					GetValueByState (triangle1Transform.localScale.y, currentState),
					GetValueByState (triangle1Transform.localScale.z, currentState)
				);
				triangle2Transform.localScale = new Vector3 (
					GetValueByState (triangle2Transform.localScale.x, currentState),
					GetValueByState (triangle2Transform.localScale.y, currentState),
					GetValueByState (triangle2Transform.localScale.z, currentState)
				);
			}
		}
	}


	float GetValueByState(float value, int state){
		float finalVal = value;
		for (int i = 0; i < state; i++) {
			finalVal = finalVal / 2f;
		}
		return finalVal;
	}
}
