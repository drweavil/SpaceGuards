using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShip : MonoBehaviour {
	//public Cannon cannon;
	public Ship ship;
	public GameObject laserHitEffect;
	public GameObject laserEffect;
	public Transform laserMask;
	public Transform laserMaskParent;
	public RaycastHit rayHit;	
	public float maximumDistance = 60f;
	public float maskYMaximumScale = 60f;
	public Transform spawnerTransform;
	Vector3 laserPosition;
	Vector3 direction;
	CannonsPanel cannonsPanel;
	bool laserEffectActive = false;
	bool laserActive = false;
	bool rotationActive = false;
	bool isRotated = false;
	float rotationPath = 0;
	float rotationX;
	float rotationY;
	public float laserDamage = 1;
	public float laserTick = 0.2f;
	Timer laserTickTimer = new Timer();


	public void ShipAwake(){
		LaserAwake ();
		ship.ShipAwake ();
		ship.movementController.InstantiateStandartPath (SpawnerController.instance.topSpawner.GetRandomPositionInWorld (), SpawnerController.instance.laserShipPositionSpawner.GetRandomPositionInWorld ());
	}


	public void ShipAction(){
		
		if (ship.movementController.objectOnPathEnd && !laserActive) {
			SetActivePhase ();
		} else if (!laserActive) {
			ship.movementController.TranslateByStandartPath ();
		} else {
			
		}
		if (laserActive && ship.explodeObject.isActive) {
			if (!ship.explodeObject.isFreeze) {
				LaserActive ();
			} else {
				LaserDeactive ();
			}
		}

		if (laserActive && !ship.explodeObject.isActive) {
			laserEffect.SetActive (false);
		}
	}


	public void SetActivePhase(){
		Vector2 tmp = SpawnerController.instance.bottomSpawner.GetRandomPositionInWorld ();
		laserPosition = new Vector3 (tmp.x, tmp.y, 0);
		direction = laserPosition - ship.movementController.endPoint;
		direction.Normalize ();


		float angle = Vector3.Angle (direction, new Vector3 (0, 1));
		//Debug.Log (directionVector);
		if (direction.x > 0) {
			angle = 360 - angle;
		}

		ship.explodeObject.explodeTransform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		laserActive = true;
		laserEffect.SetActive (true);

	}

	public void LaserAwake(){
		cannonsPanel = null;
		laserTickTimer.SetTimer (0);
		laserActive = false;
		laserEffectActive = false;
		laserHitEffect.SetActive (false);
		laserEffect.SetActive (false);
		laserMask.localScale = new Vector3 (laserMask.lossyScale.x, maximumDistance, laserMask.lossyScale.z);
	}

	public void LaserActive(){
		if (Physics.Raycast (spawnerTransform.position, direction, out rayHit, maximumDistance, (1 << LayerMask.NameToLayer ("CannonPanel")))) {
			if (cannonsPanel == null) {
				cannonsPanel = rayHit.collider.GetComponent<CannonsPanelCollider> ().cannonPanel;
			}
			float maskScaleY = Vector3.Distance (spawnerTransform.position, rayHit.point) * 1.65f;

			laserMask.localScale = new Vector3 (laserMask.lossyScale.x, maskScaleY, laserMask.localScale.z);
			if (!laserEffectActive) {
				laserHitEffect.SetActive (true);
				laserEffect.SetActive (true);
				laserEffectActive = true;
			}
			if (laserTickTimer.TimeIsOver ()) {
				cannonsPanel.MakeDamage (ship.explodeObject.damageHealthParam.damage);
				laserTickTimer.SetTimer (ship.explodeObject.damageHealthParam.damageTickTime);
			}
		} 
	}

	public void LaserDeactive(){
		if (laserEffectActive) {
			laserHitEffect.SetActive (false);
			laserEffectActive = false;
			laserEffect.SetActive (false);
		}
	}
}
