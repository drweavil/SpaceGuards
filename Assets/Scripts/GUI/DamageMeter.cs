using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageMeter : MonoBehaviour {
	public static DamageMeter instance;
	public float damage = 0;
	public float enemyDamage = 0;
	public bool isActive = true;
	float startDpsTime = 0;
	Timer dpsClearTimer = new Timer();
	float dpsClerTime = 5f;

	float startEnemyDpsTime = 0;
	Timer enemyDpsClearTimer = new Timer();
	float enemyDpsClerTime = 5f;
	public float dps = 0;
	public float enemyDps = 0;


	public Text dpsText;
	public Text enemyDPSText;


	void Awake(){
		damage = 0;
		enemyDps = 0;
		startDpsTime = Time.time;
		startEnemyDpsTime = Time.time;
		dpsClearTimer.SetTimer (dpsClerTime);
		enemyDpsClearTimer.SetTimer (enemyDpsClerTime);
		instance = this;
	}

	void Update(){
		if (isActive) {
			if (dpsClearTimer.TimeIsOver ()) {
				damage = 0;
				dpsClearTimer.SetTimer (dpsClerTime);
				startDpsTime = Time.time;
			} else {
				dps = damage / (Time.time - startDpsTime);
			}



			if (enemyDpsClearTimer.TimeIsOver ()) {
				enemyDamage = 0;
				enemyDpsClearTimer.SetTimer (enemyDpsClerTime);
				startEnemyDpsTime = Time.time;
			} else {
				enemyDps = enemyDamage / (Time.time - startEnemyDpsTime);
			}

			dpsText.text = dps.ToString ();
			enemyDPSText.text = enemyDps.ToString ();
		}
	}


}
