using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
	public static PauseGame instance;
	float pauseProcessTime = 0.5f;
	public bool onPause = false;
	bool pauseEnableProcess = false;
	bool pauseDisableProcess = false;
	public GameObject pauseWindow;
	public List<ChangeCannonButton> changeCannonButtons = new List<ChangeCannonButton>();

	void Awake(){
		instance = this;
	}


	public void OpenPauseMenu(){
		RedrawCannonButtons ();
		pauseWindow.SetActive (true);
	}

	public void RedrawCannonButtons(){
		LevelController.instance.SetCannonButtonsStatus (false);
		LevelController.instance.cannonButtonsStatus[LevelController.instance.currentRightButtonKey] = true;
		LevelController.instance.cannonButtonsStatus[LevelController.instance.currentLeftButtonKey] = true;
		foreach (ChangeCannonButton button in changeCannonButtons) {
			button.SetButton ();
		}
	}

	public void PauseEnable(){
		onPause = true;
		Time.timeScale = 0;
		AudioController.instance.SetTimeScale (0);
	}

	public void PauseDisable(){
		onPause = false;
		Time.timeScale = 1;
		AudioController.instance.SetTimeScale (1);
	}


	public void StartPauseEnable(){
		onPause = true;
		pauseDisableProcess = false;
		pauseEnableProcess = true;
		StartCoroutine (PauseEnableProcess ());
	}
	public void StartPauseDisable(){
		onPause = false;
		pauseDisableProcess = true;
		pauseEnableProcess = false;
		StartCoroutine (PauseDisableProcess());
	}


	IEnumerator PauseEnableProcess(){
		while (pauseEnableProcess) {
			float scale = Time.timeScale - Time.unscaledDeltaTime / pauseProcessTime;
			if (scale <= 0) {
				scale = 0;
				PauseEnable ();
				yield break;
			}
			Time.timeScale = scale;	
			AudioController.instance.SetTimeScale (Time.timeScale);
			yield return null;
		}
		yield break;
	}

	IEnumerator PauseDisableProcess(){
		while (pauseDisableProcess) {
			float scale = Time.timeScale + Time.unscaledDeltaTime / pauseProcessTime;
			if (scale >= 1) {
				scale = 1;
				PauseDisable ();
				yield break;
			}
			Time.timeScale = scale;	
			AudioController.instance.SetTimeScale (Time.timeScale);
			yield return null;
		}
		yield break;
	}
}
