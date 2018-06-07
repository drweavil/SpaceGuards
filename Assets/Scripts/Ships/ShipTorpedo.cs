using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTorpedo : MonoBehaviour {
	public Ship ship;
	Vector3 startPoint;
	Vector3 startCurve;
	Vector3 torpedoLaunchPoint;
	Vector3 endCurve;
	Vector3 endPoint;
	int currentPhase = 0;
	bool readyToTranslate = false;
	float halfTime;


	public void ShipAwake(){
		
		//readyToTranslate = false;
		/*GameController.ActionAfterFewFramesCoroutine (1, () => {
			halfTime = ship.explodeObject.damageHealthParam;
			readyToTranslate = true;
		});*/

		if (Random.Range (0, 2) == 1) {
			startPoint = SpawnerController.instance.leftTorpedoShipSpawner.GetRandomPositionInWorld ();
			endPoint = SpawnerController.instance.rightTorpedoShipSpawner.GetRandomPositionInWorld ();
		} else {
			endPoint = SpawnerController.instance.leftTorpedoShipSpawner.GetRandomPositionInWorld ();
			startPoint = SpawnerController.instance.rightTorpedoShipSpawner.GetRandomPositionInWorld ();
		}

		torpedoLaunchPoint = SpawnerController.instance.torpedoLaunchShipSpawner.GetRandomPositionInWorld ();

		startCurve = Vector3.Lerp(startPoint, torpedoLaunchPoint, 0.5f);//ship.movementController.GetCurvePoint (startPoint, torpedoLaunchPoint);
		endCurve = Vector3.Lerp(torpedoLaunchPoint, endPoint, 0.5f);//ship.movementController.GetCurvePoint (torpedoLaunchPoint, endPoint);

		ship.explodeObject.explodeTransform.position = startPoint;

		currentPhase = 0;
		readyToTranslate = false;
		ship.explodeObject.DefaultAwake ();
		ship.animator.SetInteger("ship_type", ship.type);

		Phase0 ();
	}


	void Phase0(){
		ship.movementController.startPoint = startPoint;
		ship.movementController.startCurve = startCurve;
		ship.movementController.endCurve = startCurve;
		ship.movementController.endPoint = torpedoLaunchPoint;
		ship.movementController.objectOnPathEnd = false;
		ship.movementController.pathProgress = 0;
	}
	void Phase1(){
		ship.movementController.startPoint = torpedoLaunchPoint;
		ship.movementController.startCurve = endCurve;
		ship.movementController.endCurve = endCurve;
		ship.movementController.endPoint = endPoint;
		ship.movementController.objectOnPathEnd = false;
		ship.movementController.pathProgress = 0;
	}
	void Phase2(){
		ship.movementController.startPoint = endPoint;
		ship.movementController.startCurve = Vector3.Lerp(startPoint, endPoint, 0.5f);
		ship.movementController.endCurve = Vector3.Lerp(startPoint, endPoint, 0.5f);
		ship.movementController.endPoint = startPoint;
		ship.movementController.objectOnPathEnd = false;
		ship.movementController.pathProgress = 0;
	}

	public void ShipActiveAction(){
		//if (readyToTranslate) {
			if (ship.movementController.objectOnPathEnd) {
				currentPhase += 1;
			if (currentPhase == 1) {
				Phase1 ();
				if (ship.explodeObject.isActive) {
					string path = "Prefabs/Airships/bullets/race_" + ship.explodeObject.raceType.ToString () + "/torpedo";
					GameObject torpedoObj = ObjectsPool.PullObject (path);
					ExplodeObject torpedo = torpedoObj.GetComponent<ExplodeObject> ();
					torpedo.explodeTransform.position = ship.GetRandomSpawner ().position;
					torpedo.poolPath = path;
					torpedo.ExplodeObjectAwake ();
					ShipsController.instance.explodeObjects.Add (torpedo);
				}
			} else if (currentPhase == 2) {
				Phase2 ();
			} else if (currentPhase == 3) {
				Phase0 ();
				currentPhase = 0;
			}
			}else{
			ship.movementController.TranslateByStandartPath ();
			}
		//}
	}
}
