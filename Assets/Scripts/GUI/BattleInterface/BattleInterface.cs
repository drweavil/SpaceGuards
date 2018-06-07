using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInterface : MonoBehaviour {
	public static BattleInterface instance;
	public GameObject battleInterfaceWindow;
	public AbilityButton wipeButton;
	public AbilityButton freezeButton;
	public AbilityButton doubleDamageButton;
	public AbilityButton shieldButton;

	public Text currentPoints;

	float countdownPointsStack;
	float currentStep;
	int frames = 60;

	void Awake(){
		instance = this;
	}
		
	IEnumerator StartActiveFrameCoroutine(AbilityButton button, float time){
		button.activeFrameObj.SetActive (true);
		float startTime = Time.time;
		float endTime = startTime + time;
		button.activeFrame.fillAmount = 1;
		while (true) {
			float timeProgress = Mathf.InverseLerp (endTime, startTime, Time.time);
			//float timeProgress = button.activeFrame.fillAmount - Time.deltaTime / time;
			if (timeProgress <= 0) {
				button.activeFrameObj.SetActive (false);
				yield break;
			} else {
				button.activeFrame.fillAmount = timeProgress;
				yield return null;
			}
		}
	}

	public void OpenInterface(){
		battleInterfaceWindow.SetActive (true);
		RedrawAllInfo ();
	}

	IEnumerator StartCdCoroutine(AbilityButton button, float time){
		button.cdFrame.gameObject.SetActive (true);
		button.cdFrame.fillAmount = 1;
		button.onCd = true;
		while (true) {
			float timeProgress = button.cdFrame.fillAmount - Time.deltaTime / time;
			if (timeProgress <= 0) {
				button.cdFrame.gameObject.SetActive (false);
				button.onCd = false;
				yield break;
			} else {
				button.cdFrame.fillAmount = timeProgress;
				yield return null;
			}
		}
	}

	public void RedrawAllInfo(){
		wipeButton.textNumber.text = LevelController.instance.gameSessionData.wipeAbilityCount.ToString ();
		shieldButton.textNumber.text = LevelController.instance.gameSessionData.shieldAbilityCount.ToString ();
		doubleDamageButton.textNumber.text = LevelController.instance.gameSessionData.doubleDamageAbilityCount.ToString ();
		freezeButton.textNumber.text = LevelController.instance.gameSessionData.freezeAbilityCount.ToString ();
		currentPoints.text = System.Math.Round (LevelController.instance.currentPoints, 0).ToString();
	}


	public void WipeButton(){
		if (LevelController.instance.gameSessionData.wipeAbilityCount >= 1 && !wipeButton.onCd && !PauseGame.instance.onPause) {
			LevelController.instance.gameSessionData.wipeAbilityCount -= 1;
			ShipsController.instance.Wipe ();
			wipeButton.textNumber.text = LevelController.instance.gameSessionData.wipeAbilityCount.ToString ();
			LevelController.instance.SaveGameSessionData ();
			StartCoroutine (StartCdCoroutine(wipeButton, ShipsController.cd));
			ShipsController.instance.usedWipe += 1;
		}
	}

	public void ShieldButton(){
		if (LevelController.instance.gameSessionData.shieldAbilityCount >= 1 && !shieldButton.onCd && !PauseGame.instance.onPause) {
			LevelController.instance.gameSessionData.shieldAbilityCount -= 1;
			ShipsController.instance.Shield ();
			shieldButton.textNumber.text = LevelController.instance.gameSessionData.shieldAbilityCount.ToString ();
			LevelController.instance.SaveGameSessionData ();
			StartCoroutine (StartCdCoroutine(shieldButton, ShipsController.cd));
			ShipsController.instance.usedShield += 1;
		}
	}

	public void FreezeButton(){
		if (LevelController.instance.gameSessionData.freezeAbilityCount >= 1 && !freezeButton.onCd && !PauseGame.instance.onPause) {
			LevelController.instance.gameSessionData.freezeAbilityCount -= 1;
			ShipsController.instance.Freeze ();
			StartCoroutine (StartActiveFrameCoroutine(freezeButton, ShipsController.freezeTime));
			//freezeButton.StartActiveFrame (ShipsController.freezTime);
			freezeButton.textNumber.text = LevelController.instance.gameSessionData.freezeAbilityCount.ToString ();
			LevelController.instance.SaveGameSessionData ();
			StartCoroutine (StartCdCoroutine(freezeButton, ShipsController.cd));
			ShipsController.instance.usedFreeze += 1;
		}
	}

	public void DoubleDamageButton(){
		if (LevelController.instance.gameSessionData.doubleDamageAbilityCount >= 1 && !doubleDamageButton.onCd && !PauseGame.instance.onPause) {
			LevelController.instance.gameSessionData.doubleDamageAbilityCount -= 1;
			ShipsController.instance.DoubleDamage ();
			StartCoroutine (StartActiveFrameCoroutine(doubleDamageButton, ShipsController.doubleDamageTime));
			doubleDamageButton.StartActiveFrame (ShipsController.doubleDamageTime);
			doubleDamageButton.textNumber.text = LevelController.instance.gameSessionData.doubleDamageAbilityCount.ToString ();
			LevelController.instance.SaveGameSessionData ();
			StartCoroutine (StartCdCoroutine(doubleDamageButton, ShipsController.cd));
			ShipsController.instance.usedDoubleDamage += 1;
		}
	}



	public void AddPointToStack(float plusPoints){
		if (countdownPointsStack == 0) {
			countdownPointsStack += plusPoints;
			currentStep = countdownPointsStack / (float)frames;
			StartCoroutine (StartPointsCountdown ());
		} else {
			countdownPointsStack += plusPoints;
			currentStep = countdownPointsStack / (float)frames;
		}

	}


	IEnumerator StartPointsCountdown(){
		while(countdownPointsStack > 0){
			countdownPointsStack -= currentStep;
			currentPoints.text = System.Math.Round (LevelController.instance.currentPoints - countdownPointsStack, 0).ToString();
			yield return null;
		}
		countdownPointsStack = 0;
		currentPoints.text = System.Math.Round (LevelController.instance.currentPoints, 0).ToString();
		yield break;
	}
}
