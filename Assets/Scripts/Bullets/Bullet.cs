using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ExplodeObjectEvent : UnityEvent<ExplodeObject>{}
public class Bullet : MonoBehaviour {
	public Vector3 directionVector;
	public Vector3 directionPosition;
	public int cannonID;
	public DamageHealthParam damageHealthParams;
	public Vector3 speed = new Vector3 (200f, 200f, 0);
	public float damage = 1;
	public float maximumDamage;
	public int bulletType = 1;
	int frame = 0;
	public string poolPath;
	public bool withOverscreenChecker = true;
	public List<Collider> triggeredColliders = new List<Collider>();
	public List<ExplodeObject> triggeredExplodeObject = new List<ExplodeObject> ();
	public UnityEvent awakeEvent;
	public UnityEvent isActiveEvent;
	public ExplodeObjectEvent triggerEvent;
	public delegate void TriggerAction(ExplodeObject expObject);
	public List<ParticleSystem> effects = new List<ParticleSystem> ();


	void Update () {
		isActiveEvent.Invoke ();
		OverscreenChecker ();

	}

	public void BulletAwake(){
		damageHealthParams = DamageHealthParamController.instance.GetCannonParamsById (cannonID);
		awakeEvent.Invoke ();
	}
		

	public void TryTrigger(Collider col){
		if (col.gameObject.layer == LayerMask.NameToLayer ("ExplodeObject")) {
			if (triggeredColliders.FindIndex (c => c == col) == -1) {
				triggeredColliders.Add (col);

				ExplodeObject expObject = (ExplodeObject)col.GetComponent<ExplodeObjectCollider> ().explodeObject;
				if (triggeredExplodeObject.FindIndex (e => e == expObject) == -1) {
					if (expObject.isActive) {
						triggerEvent.Invoke (expObject);
						triggeredExplodeObject.Add (expObject);
					} else {
						triggeredColliders.Remove (col);
						triggeredExplodeObject.Remove (expObject);
					}
				}

			}
		}
	}

	public void ClearColliders(){
		triggeredExplodeObject.Clear ();
		triggeredColliders.Clear ();
	}

	public void DefaultBulletAwake(){
		float angle = Vector3.Angle (directionVector, new Vector3 (1, 0)) - 90;
		if (directionVector.y < 0) {
			angle = 360 - angle;
		}

		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		ClearColliders();
		foreach (ParticleSystem effect in effects) {
			effect.Stop ();
			ParticleSystem.MainModule main = effect.main;
			main.startRotation = (Mathf.Deg2Rad * (angle /*- 90*/)) * -1;
			effect.Play ();
		}

	}
	public void DefaultBulletActiveAction(){
		transform.position = new Vector3 (
			transform.position.x + (directionVector.x * damageHealthParams.bulletSpeed.x * Time.timeScale),
			transform.position.y + (directionVector.y * damageHealthParams.bulletSpeed.y * Time.timeScale),
			transform.position.z + (directionVector.z * damageHealthParams.bulletSpeed.z * Time.timeScale)
		);
	}
	public void DefaultBulletTrigger(ExplodeObject expObject){
		if (Cannon.CheckSuitability(expObject.objectType, bulletType)) {
			expObject.MakeDamage (damageHealthParams.damage);
		} 
		foreach (ParticleSystem effect in effects) {
			effect.Stop ();
		}
		ObjectsPool.PushObject (poolPath, this.gameObject);
	}


	public void MineShatterAwake(){
		directionVector = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
		DefaultBulletAwake ();
	}

	void OverscreenChecker(){
		if (withOverscreenChecker) {
			if (frame == 30) {
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
}
