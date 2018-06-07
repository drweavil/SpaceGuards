using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour {
	public static LevelTimer instance;
	public RectTransform progressPoint;
	public RectTransform progressLine;
	float progressLineHalfHeight;
	float currentLevelTime;
	float allLevelTime;
	public bool timerActive = false;
	int progressRedrawFrame = 10;
	int currentProgressRedrawFrame;
	public delegate void LevelTimerAction();
	public LevelTimerAction levelEndAction;


	void Awake(){
		instance = this;
		levelEndAction = () => {
			DefaultEndAction ();
		};
	}

	public void DefaultEndAction(){
		if (LevelController.instance.levelActive) {
			LevelController.instance.levelActive = false;
			ShipSpawner.instance.StopSpawn ();
			Interface.interfaceSt.OpenWinWindow ();
			Dictionary<int, ExplodeObject> expObjDict = new Dictionary<int, ExplodeObject> ();
			for (int i =0; i< ShipsController.instance.explodeObjects.Count;i++) {
				expObjDict.Add (i, ShipsController.instance.explodeObjects[i]);
			}
			for (int i = 0; i < expObjDict.Count; i++) {
				expObjDict [i].DefaultDestroy ();
			}


			List<Bullet> bulletObjects = new List<Bullet> (FindObjectsOfType<Bullet> ());
			foreach (Bullet bullObj in bulletObjects) {
				ObjectsPool.PushObject (bullObj.poolPath, bullObj.gameObject);
			}

			AudioController.instance.currentSong.Stop ();
			//Advertising.TryShowInterstitial ();
		}
	}

	void Update(){
		if (timerActive) {
			currentLevelTime += Time.deltaTime;
			if (currentProgressRedrawFrame == progressRedrawFrame) {
				if (currentLevelTime >= allLevelTime) {
					currentLevelTime = allLevelTime;
					timerActive = false;
					LevelEndAction ();
				}
				SetCurrentProgress (allLevelTime, currentLevelTime);
			} else {
				currentProgressRedrawFrame += 1;
			}
		}
	}

	void InstantiateProgressBar(){
		progressLineHalfHeight = progressLine.sizeDelta.y / 2f;
	}

	void SetCurrentProgress(float allTime, float currentTime){
		float currentPath = currentTime/allTime; 
		progressPoint.anchoredPosition3D = new Vector3 (0, Mathf.Lerp (progressLineHalfHeight * -1, progressLineHalfHeight, currentPath), 0);
	}

	public void StartTimer(float time){
		InstantiateProgressBar ();
		allLevelTime = time;
		currentLevelTime = 0;
		timerActive = true;
		currentProgressRedrawFrame = 0;
	}



	public void LevelEndAction(){
		levelEndAction.Invoke ();
	}
}
