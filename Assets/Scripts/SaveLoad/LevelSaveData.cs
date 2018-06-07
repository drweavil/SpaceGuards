using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSaveData{
	public int levelID;	
	public int planetID;
	public bool isComplete = false;
	public float points = 0; 
	public Level level;
}
