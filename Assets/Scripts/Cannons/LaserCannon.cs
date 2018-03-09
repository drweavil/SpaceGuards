using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : MonoBehaviour {
	public Cannon cannon;
	public GameObject laserHitEffect;
	public GameObject laserEffect;
	public Transform laserMask;
	public Transform laserMaskParent;
	public RaycastHit rayHit;	
	public float maximumDistance = 60f;
	public float maskYMaximumScale = 60f;
	bool laserEffectActive = false;
	bool laserActive = false;
	public float laserDamage = 1;
	public float laserTick = 0.2f;
	Timer laserTickTimer = new Timer();


	public void LaserAwake(){
		laserTickTimer.SetTimer (0);
		laserActive = false;
		laserEffectActive = false;
		laserHitEffect.SetActive (false);
		laserEffect.SetActive (false);
		laserMask.localScale = new Vector3 (laserMask.lossyScale.x, maximumDistance, laserMask.lossyScale.z);
	}

	public void LaserActive(){
		if (!laserActive) {
			laserActive = true;
			laserEffect.SetActive (true);
		}
		if (Physics.Raycast (cannon.cannonBulletSpawner.position, cannon.GetCannonDirection (), out rayHit, maximumDistance, (1 << LayerMask.NameToLayer ("ExplodeObject")))) {
			
			Box box = rayHit.collider.GetComponent<Box> ();
			if (box != null) {
				box.lastBulletCannonType = cannon.cannonType;
			}

			ExplodeObject expObj = rayHit.collider.GetComponent<ExplodeObjectCollider> ().explodeObject;
			if (expObj.isActive) {
				float maskScaleY = Vector3.Distance (cannon.cannonBulletSpawner.position, rayHit.point) * 1.65f;

				laserMask.localScale = new Vector3 (laserMask.lossyScale.x, maskScaleY, laserMask.localScale.z);
				if (!laserEffectActive) {
					laserHitEffect.SetActive (true);
					laserEffectActive = true;
				}
				if (laserTickTimer.TimeIsOver ()) {
				
					if (expObj.objectType == cannon.cannonType || expObj.objectType == Cannon.allBullet) {
						expObj.MakeDamage (cannon.damageHealthParam.damage);
					}
					laserTickTimer.SetTimer (cannon.damageHealthParam.damageTickTime);
				}
			} 
		} else {
			if (laserEffectActive = true) {
				laserEffectActive = false;
				laserHitEffect.SetActive (false);
			}
			laserMask.localScale = new Vector3 (laserMask.lossyScale.x, maximumDistance, laserMask.localScale.z);
		}
	}

	public void LaserDeactive(){
		if (laserActive) {
			laserActive = false;
			laserEffect.SetActive (false);
			laserTickTimer.SetTimer (0);
		}
	}
}
