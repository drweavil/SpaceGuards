using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour {
	public Image activeFrame;
	public Image cdFrame;
	public bool onCd = false;
	public GameObject activeFrameObj;
	public Text textNumber;



	public void StartActiveFrame(float time){
		activeFrameObj.SetActive (true);
		activeFrame.fillAmount = 1;
		StartCoroutine (StartActiveFrameCoroutine(time));
	}
	IEnumerator StartActiveFrameCoroutine(float time){
		while (true) {
			float timeProgress = activeFrame.fillAmount - Time.deltaTime / time;
			if (timeProgress <= 0) {
				activeFrameObj.SetActive (false);
				yield break;
			} else {
				activeFrame.fillAmount = timeProgress;
				yield return null;
			}
		}
	}
}
