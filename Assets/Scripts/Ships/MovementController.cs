using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
	public ExplodeObject expObject;
	public Vector3 startPoint;
	public Vector3 startCurve;
	public Vector3 endCurve;
	public Vector3 endPoint;
	//public float time = 3f;
	public float pathProgress = 0;
	//bool onPathPoints = false;
	int currentPathPointIndex = 0;
	public List<Vector3> pathPoints = new List<Vector3> ();
	public List<float> distanceTimeParts = new List<float> ();
	public bool objectOnPathEnd = false;
	bool onFinalPointPath = false;
	public bool withRotation = true;
	public bool objectGetRound = false;
	public delegate void PathEndAction();
	public bool pathEndActionComplete = false;
	public PathEndAction pathEndAction;

	public void InstantiateStandartPath(Vector3 sP, Vector3 eP){
		objectOnPathEnd = false;
		pathEndAction = null;
		pathProgress = 0;
		startPoint = sP;
		endPoint = eP;

		startCurve = GetCurvePoint (sP, eP);
		endCurve = startCurve;
	}
	public void InstantiateShip2Path(Vector3 sP, Vector3 eP){
		objectOnPathEnd = false;
		pathEndAction = null;
		pathProgress = 0;
		startPoint = sP;
		endPoint = eP;

		startCurve = SpawnerController.instance.allScreenSpawner.GetRandomPositionInWorld();
		endCurve = SpawnerController.instance.allScreenSpawner.GetRandomPositionInWorld();
	}

	public Vector3 GetCurvePoint(Vector3 sP, Vector3 eP){
		Vector3 centerPoint = Vector3.Lerp (startPoint, endPoint, 0.5f);
		float a = sP.y - eP.y;
		float b = eP.x - sP.x;
		float c = sP.x * eP.y - eP.x * sP.y;

		if (a == 0) {
			a = 0.0000000000000000000000000000001f;
		}
		if (b == 0) {
			b = 0.0000000000000000000000000000001f;
		}
		if (c == 0) {
			c = 0.0000000000000000000000000000001f;
		}


		Vector3 normal = new Vector3 (a, b, 0);
		normal.Normalize ();
		if (Random.Range (0, 2) == 1) {
			normal = normal * -1f;
		}
		float curveCoeff = Random.Range (2f, 5f);
		float distance = Vector3.Distance (startPoint, centerPoint) / curveCoeff;



		return normal * distance + centerPoint;
	}

	public void StandartAwake(){
		//TestLineScript.instance.Clear ();
		expObject.DefaultAwake ();
		InstantiateStandartPath (expObject.explodeTransform.position, expObject.directionPosition);

	}
	public void Ship2Awake(){
		//TestLineScript.instance.Clear ();
		expObject.DefaultAwake ();
		InstantiateShip2Path (expObject.explodeTransform.position, expObject.directionPosition);

	}
	public void TranslateByStandartPath(){
		//Debug.Log ("lol");
		if (!expObject.isFreeze) {
			pathProgress += Time.deltaTime / expObject.damageHealthParam.pathTime;

			if (pathProgress > 1) {
				pathProgress = 1;
				objectOnPathEnd = true;
				if (pathEndAction != null) {
					pathEndAction.Invoke ();
				}
			} else {
				Vector3 startPosition = expObject.explodeTransform.position;
				expObject.explodeTransform.position = GetBezierPoint (pathProgress, startPoint, startCurve, endCurve, endPoint);

				Vector3 directionVector = expObject.explodeTransform.position - startPosition;

				float angle = Vector3.Angle (directionVector, new Vector3 (0, 1));
				//Debug.Log (directionVector);
				if (directionVector.x > 0) {
					angle = 360 - angle;
				}

				//TestLineScript.instance.AddPosition (expObject.explodeTransform.position);
				//if (withRotation) {
					expObject.explodeTransform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
				//}
			}
		}
	}

	public void TranslateByLerpPath(float pathTime  = -1){
		if (!expObject.isFreeze) {
			if (pathTime == -1) {
				pathProgress += Time.deltaTime / expObject.damageHealthParam.pathTime;
			} else {
				pathProgress += Time.deltaTime / pathTime;
			}
			if (pathProgress > 1) {
				pathProgress = 1;
				objectOnPathEnd = true;
				if (pathEndAction != null) {
					pathEndAction.Invoke ();
				}
			}
			expObject.explodeTransform.position = Vector3.Lerp (startPoint, endPoint, pathProgress);
		}
	}
	public void PathByPointsRebind(){
		onFinalPointPath = false;
		startPoint = expObject.explodeTransform.position;
		endPoint = pathPoints [0];
		objectGetRound = false;
		objectOnPathEnd = false;
		//onPathPoints = false;
		currentPathPointIndex = 0;
		pathProgress = 0;
		SetDistanceParts ();

	}
	void SetDistanceParts(bool withStartPoint = true){
		distanceTimeParts.Clear ();
		if (withStartPoint) {
			distanceTimeParts.Add (Vector3.Distance (startPoint, pathPoints [0]));
		}
		for (int i = 0; i < pathPoints.Count; i++) {
			if (i == pathPoints.Count - 2) {
				distanceTimeParts.Add (Vector3.Distance (pathPoints [pathPoints.Count - 1], pathPoints [i]));
			} else if (i == pathPoints.Count - 1) {
				distanceTimeParts.Add (Vector3.Distance (pathPoints [i - 1], pathPoints [0]));
			} else {
				distanceTimeParts.Add (Vector3.Distance (pathPoints [i], pathPoints [i + 1]));
			}
		}

		float allDistance = 0;
		foreach (float distance in distanceTimeParts) {
			allDistance += distance;
		}




		float allTime = expObject.damageHealthParam.pathTime;

		for (int i = 0; i < distanceTimeParts.Count; i++) {
			distanceTimeParts [i] = (distanceTimeParts [i] * allTime)/allDistance; 
		}

	}
	float GetPathParthTimeByPointIndex(int n){
		if (objectOnPathEnd) {
			if (n == 0) {
				return distanceTimeParts [distanceTimeParts.Count - 1];
			} else {
				return distanceTimeParts [n];
			}
		} else {
			return distanceTimeParts [n];
		}
	}

	public void TranslateByPointsPath(bool withRotationChange = false){
		if (!expObject.isFreeze) {
			if (expObject.damageHealthParam != null) {
				pathProgress += Time.deltaTime / GetPathParthTimeByPointIndex (currentPathPointIndex);

				if (pathProgress >= 1) {
					pathProgress = 0;
					currentPathPointIndex += 1;
					if (currentPathPointIndex > pathPoints.Count - 1) {
						currentPathPointIndex = 0;
						onFinalPointPath = true;
					}

					if (onFinalPointPath) {
						objectGetRound = true;
						if (pathEndAction != null) {
							pathEndAction.Invoke ();
						}
					}

					if (currentPathPointIndex == pathPoints.Count - 1) {
						objectOnPathEnd = true;
					}
					//onPathPoints = true;
					startPoint = endPoint;
					endPoint = pathPoints [currentPathPointIndex];
				}
			
				expObject.explodeTransform.position = Vector3.Lerp (startPoint, endPoint, pathProgress);
			}
		}
	}

	public Vector3 GetBezierPoint(float t, Vector3 sP, Vector3 sC, Vector3 eC, Vector3 eP){
		float u = 1 - t;
		float uu = u * u;
		float tt = t * t;
		float uuu = uu * u;
		float ttt = tt * t;

		Vector3 point = uuu * sP;
		point += 3 * uu * t * sC;
		point += 3 * u * tt * eC;
		point += ttt * eP;
		return point;
	}
}
