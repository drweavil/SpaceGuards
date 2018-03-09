using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {
	public Transform centerChecker;
	public Transform controllerTransform;
	public Transform controllerPosition;
	public BackgroundSystem currentSystem;
	public int planetID = 1;
	public static BackgroundController instance;
	public Timer parallaxSpawnTimer  = new Timer();
	public float minimumTime = 0.5f;
	public float maximumTimer = 5f;


	void Awake(){
		parallaxSpawnTimer.SetTimer (Random.Range (minimumTime, maximumTimer));
		instance = this;
		controllerTransform.position = Camera.main.ScreenToWorldPoint (controllerPosition.position);
		controllerTransform.position = new Vector3(controllerTransform.position.x, controllerTransform.position.y, 0);
		centerChecker.transform.position = Camera.main.ScreenToWorldPoint(Interface.interfaceSt.center.transform.position);
	}


	void Update(){
		if (parallaxSpawnTimer.TimeIsOver ()) {
			string path = "Prefabs/Backgrounds/planet_" + planetID + "/Parallax/parallax_1";
			//Debug.Log (path);
			/*for (int i = 0; i < Random.Range (1, 3); i++) {
				GameObject newParallaxObject = ObjectsPool.PullObject (path);
				ParallaxObject parallaxObject = newParallaxObject.GetComponent<ParallaxObject> ();
				parallaxObject.poolPath = path;
				float positionZ = parallaxObject.objectTransform.position.z;
				if (currentSystem.upToDown) {
					parallaxObject.objectTransform.position = SpawnerController.instance.parallaxSpawnerTop.GetRandomPositionInWorld ();
				} else {
					parallaxObject.objectTransform.position = SpawnerController.instance.parallaxSpawnerBottom.GetRandomPositionInWorld ();
				}
				parallaxObject.objectTransform.position = new Vector3 (
					parallaxObject.objectTransform.position.x, 
					parallaxObject.objectTransform.position.y,
					positionZ
				);
				parallaxObject.speed = currentSystem.speed * 1.5f;
				if (currentSystem.upToDown) {
					parallaxObject.speed = parallaxObject.speed * -1f;
				}
				parallaxSpawnTimer.SetTimer (Random.Range (minimumTime, maximumTimer));
			}*/
		}
	}



	public void SetPlanet(int id){
		planetID = id;
		Destroy (currentSystem.gameObject);
		GameObject backgroundObject = (GameObject)Instantiate(Resources.Load ("Prefabs/Backgrounds/planet_" + id.ToString() + "/backgroundSystem"));
		Debug.Log(backgroundObject);
		backgroundObject.transform.SetParent (controllerTransform);
		backgroundObject.transform.position = new Vector3 (0, 0, 1);
		currentSystem = backgroundObject.GetComponent<BackgroundSystem> ();
	}


	
}
