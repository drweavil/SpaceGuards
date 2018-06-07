using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour {
	public int levelID;
	public int planetID;
	public int status;
	public RectTransform rectTransform;
	public GameObject levelGreen;
	public GameObject levelRed;
	public GameObject levelGray;

	public void ButtonClick(){
		if (status != Levels.locked) {
			Interface.interfaceSt.ClosePlanetsMenu (() => {
				MainMenu.instance.windowObject.SetActive (false);
				BattleInterface.instance.OpenInterface ();
				LevelController.instance.SetLevel(Levels.GetLevel(planetID, levelID));
				CannonsPanel.instance.SetDefaultCannons();
			});
		}
	}

	public string GetButtonKey(){
		return planetID.ToString() + "_" + levelID.ToString();
	}

	public void Open(){
		status = Levels.open;
		levelGray.SetActive (true);
		levelRed.SetActive (false);
		levelGreen.SetActive (false);
	}

	public void Lock(){
		status = Levels.locked;
		levelGray.SetActive (false);
		levelRed.SetActive (true);
		levelGreen.SetActive (false);
	}

	public void Complete(){
		status = Levels.complete;
		levelGray.SetActive (false);
		levelRed.SetActive (false);
		levelGreen.SetActive (true);	
	}
}
