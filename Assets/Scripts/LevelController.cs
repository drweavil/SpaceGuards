using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
	public static LevelController instance;
	public RectTransform spawnRect;

	public Timer spawnTimer = new Timer ();
	public float spawnTime = 1f;

	public GameSessionData gameSessionData;

	void Awake(){
		spawnTimer.SetTimer (0.5f);
		instance = this;

		StartCoroutine(GameController.ActionAfterFewFramesCoroutine (1, () => {
			gameSessionData = new GameSessionData();
			BattleInterface.instance.RedrawAllInfo();
		}));
		//SpawnEnemy ();
		//darkPrefab = (GameObject)Resources.Load ("Prefabs/Airships/leftShips/ship");
		//redPrefab = (GameObject)Resources.Load ("Prefabs/Airships/rightShips/ship");
		//allPrefab = (GameObject)Resources.Load ("Prefabs/Airships/allShips/ship");
	}

	public void SaveGameSessionData(){
		
	}

	public void Test(){
		//ShipsController.instance.SpawnAbilityBox(2);
		//SpawnEnemy ();
		BackgroundController.instance.SetPlanet(6);
	}
	public void Test2(){
		//ShipsController.instance.SpawnAbilityBox(2);
		/*int lol = Random.Range(0, 2);
		if (lol == 0) {
			int tp = Random.Range (1, 6);
			ShipsController.instance.SpawnAbilityBox (tp);
		} else if (lol == 1) {
			int tp2 = Random.Range (1, 9);
			ShipsController.instance.SpawnCannonBox (tp2);
		}*/
		BackgroundController.instance.SetPlanet(1);
		//CannonsPanel.instance.SetRightCannon (7);
		//CannonsPanel.instance.SetLeftCannon (7);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.I)) {
			//Debug.Log ("fdfdhfg");
			SpawnEnemy ();
			//ShipsController.instance.SpawnCannonBox(2);
		}

		if (Input.GetKeyDown (KeyCode.U)) {
			/*for (int i = 0; i < 200; i++) {
				//Debug.Log ("fdfdhfg");
				SpawnEnemy ();
			}*/
			ShipsController.instance.Wipe ();
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			/*for (int i = 0; i < 200; i++) {
				//Debug.Log ("fdfdhfg");
				SpawnEnemy ();
			}*/
			ShipsController.instance.Freeze ();
		}

		/*if (spawnTimer.TimeIsOver ()) {
			SpawnEnemy ();
			spawnTimer.SetTimer (spawnTime);
		}*/
	}

	void SpawnEnemy(){
		
		/*
		int enemyType = Random.Range (0, 3);
		string poolPath = "";
		if (enemyType == Cannon.leftBullet) {
			poolPath = "Prefabs/Airships/leftShips/race_1/ship_1";
		}

		if (enemyType == Cannon.rightBullet) {
			poolPath = "Prefabs/Airships/rightShips/race_1/ship_1";
		}

		if (enemyType == Cannon.allBullet) {
			poolPath = "Prefabs/Airships/allShips/race_1/ship_1";
		}

		GameObject newObj = ObjectsPool.PullObject(poolPath);

		newObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3 (Random.Range(30f, Screen.width - 30f), Screen.height + 40, 0));
		newObj.transform.position = new Vector3(newObj.transform.position.x, newObj.transform.position.y, 0);
		ExplodeObject expObj = newObj.GetComponent<ExplodeObject> ();
		expObj.ExplodeObjectAwake();
		expObj.poolPath = poolPath;*/
		int type = Random.Range (0, 3);

		int ship = Random.Range (1, 7);
		ShipsController.instance.SpawnShip (6, 6, 2);
		//ShipsController.instance.SpawnShip (3, ship, type);
	}
}
