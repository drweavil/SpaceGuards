using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollider : MonoBehaviour {
	public Bullet bullet;
	public delegate void ColliderExitAction(Collider col);
	public ColliderExitAction colliderExitAction;


	void OnTriggerEnter(Collider col){
		bullet.TryTrigger (col);
	}

	void OnTriggerStay(Collider col){
		bullet.TryTrigger (col);
	}

	void OnTriggerExit(Collider col){
		if (colliderExitAction != null) {
			colliderExitAction.Invoke (col);
		}
	}
}
