using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer {
	private float endTime = 0;
	public delegate void TimerAction ();
	private float pauseTime;
	private bool pauseActive = false;

	public void SetTimer(float seconds){
		endTime = Time.time + seconds;
		pauseActive = false;
	}

	public void PauseEnable(){
		pauseActive = true;
		pauseTime = ResidualTime ();
	}

	public void PauseDisable(){
		if (pauseActive) {
			SetTimer (pauseTime);
		}
	}

	public bool TimeIsOver(){
		if (pauseActive) {
			return false;
		} else {
			return endTime <= Time.time;
		}
	}

	public float ResidualTime(){
		float value = 0;
		if (endTime > Time.time) {
			value = endTime - Time.time;
		}
		return value;
	}

	public IEnumerator ActionAfterTimer(TimerAction action){
		while (true) {
			if (TimeIsOver ()) {
				if (pauseActive) {
					yield return null;
				} else {
					action ();
					yield break;
				}
			} else {
				yield return null;
			}
		}
	}
}
