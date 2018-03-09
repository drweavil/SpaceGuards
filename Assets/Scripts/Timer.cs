﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer {
	private float endTime = 0;
	public delegate void TimerAction ();

	public void SetTimer(float seconds){
		endTime = Time.time + seconds;
	}

	public bool TimeIsOver(){
		bool itIs = false;
		if(endTime <= Time.time){
			itIs = true;
		}
		return itIs;
	}

	public float ResidualTime(){
		float value = 0;
		if (endTime > Time.time) {
			value = endTime - Time.time;
		}
		return value;
	}

	public IEnumerator ActionAfterTimer(TimerAction action){
		while (!TimeIsOver ()) {
			yield return null;
		}
		action();
		yield break;
	}
}