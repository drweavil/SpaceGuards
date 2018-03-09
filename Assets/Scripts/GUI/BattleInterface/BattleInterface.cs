using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInterface : MonoBehaviour {
	public static BattleInterface instance;
	public GameObject battleInterfaceWindow;
	public AbilityButton wipeButton;
	public AbilityButton freezeButton;
	public AbilityButton doubleDamageButton;
	public AbilityButton shieldButton;

	void Awake(){
		instance = this;
	}
		
	IEnumerator StartActiveFrameCoroutine(AbilityButton button, float time){
		button.activeFrameObj.SetActive (true);
		button.activeFrame.fillAmount = 1;
		while (true) {
			float timeProgress = button.activeFrame.fillAmount - Time.deltaTime / time;
			if (timeProgress <= 0) {
				button.activeFrameObj.SetActive (false);
				yield break;
			} else {
				button.activeFrame.fillAmount = timeProgress;
				yield return null;
			}
		}
	}

	public void RedrawAllInfo(){
		wipeButton.textNumber.text = LevelController.instance.gameSessionData.wipeAbilityCount.ToString ();
		shieldButton.textNumber.text = LevelController.instance.gameSessionData.shieldAbilityCount.ToString ();
		doubleDamageButton.textNumber.text = LevelController.instance.gameSessionData.doubleDamageAbilityCount.ToString ();
		freezeButton.textNumber.text = LevelController.instance.gameSessionData.freezeAbilityCount.ToString ();
	}


	public void WipeButton(){
		if (LevelController.instance.gameSessionData.wipeAbilityCount >= 1) {
			LevelController.instance.gameSessionData.wipeAbilityCount -= 1;
			ShipsController.instance.Wipe ();
			wipeButton.textNumber.text = LevelController.instance.gameSessionData.wipeAbilityCount.ToString ();
			LevelController.instance.SaveGameSessionData ();
		}
	}

	public void ShieldButton(){
		if (LevelController.instance.gameSessionData.shieldAbilityCount >= 1) {
			LevelController.instance.gameSessionData.shieldAbilityCount -= 1;
			ShipsController.instance.Shield ();
			shieldButton.textNumber.text = LevelController.instance.gameSessionData.shieldAbilityCount.ToString ();
			LevelController.instance.SaveGameSessionData ();
		}
	}

	public void FreezeButton(){
		if (LevelController.instance.gameSessionData.freezeAbilityCount >= 1) {
			LevelController.instance.gameSessionData.freezeAbilityCount -= 1;
			ShipsController.instance.Freeze ();
			StartCoroutine (StartActiveFrameCoroutine(freezeButton, ShipsController.freezeTime));
			//freezeButton.StartActiveFrame (ShipsController.freezTime);
			freezeButton.textNumber.text = LevelController.instance.gameSessionData.freezeAbilityCount.ToString ();
			LevelController.instance.SaveGameSessionData ();
		}
	}

	public void DoubleDamageButton(){
		if (LevelController.instance.gameSessionData.doubleDamageAbilityCount >= 1) {
			LevelController.instance.gameSessionData.doubleDamageAbilityCount -= 1;
			ShipsController.instance.DoubleDamage ();
			StartCoroutine (StartActiveFrameCoroutine(doubleDamageButton, ShipsController.doubleDamageTime));
			doubleDamageButton.StartActiveFrame (ShipsController.doubleDamageTime);
			doubleDamageButton.textNumber.text = LevelController.instance.gameSessionData.doubleDamageAbilityCount.ToString ();
			LevelController.instance.SaveGameSessionData ();
		}
	}
}
