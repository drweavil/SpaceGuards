using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSessionData{
	public float points = 0;
	public float maximumPoints = 0;
	public int doubleDamageAbilityCount = 1;
	public int freezeAbilityCount = 1;
	public int shieldAbilityCount = 1;
	public int wipeAbilityCount = 1;
	public Dictionary<int, bool> planetStatus = new Dictionary<int, bool> (); 
	public Dictionary<string, LevelSaveData> levels = new Dictionary<string, LevelSaveData>();
	public bool fullVersionActive = false;




}
