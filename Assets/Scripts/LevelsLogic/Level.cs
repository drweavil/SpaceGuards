using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level {
	public float time = 0;
	public int planetID = -1;
	public int levelID = 0;
	public List<int> ships = new List<int>();
	public List<int> bosses = new List<int>();
	public int recoveryBoxes = 0;
	public float healthPool = 0;

	public string GetLevelKey(){
		return planetID.ToString () + "_" + levelID.ToString ();
	}

	public class ShipSpawnerSettings{
		public int priority;
		public int shipID;
	}
}


