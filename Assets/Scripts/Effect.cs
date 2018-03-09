using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
	public ParticleSystem main;
	public string poolPath;

	public void DestoyOverTime(float time){
		Timer lifeTimer = new Timer ();
		lifeTimer.SetTimer (time);
		StartCoroutine (lifeTimer.ActionAfterTimer(() => {
			ObjectsPool.PushObject(poolPath, this.gameObject);
		}));
	}




}
