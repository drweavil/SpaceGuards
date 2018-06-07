using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCannonButton : MonoBehaviour {
	public GameObject activeFrame;
	public GameObject crossActive;
	public int side;
	public int cannonType;
	public bool isActive = false;


	public void SetButton(){
		if (LevelController.instance.cannonButtonsStatus.ContainsKey (GetKey ())) {
			crossActive.SetActive (false);
			isActive = true;
			if (LevelController.instance.cannonButtonsStatus [GetKey ()]) {
				activeFrame.SetActive (true);
			} else {
				activeFrame.SetActive (false);
			}
		} else {
			isActive = false;
			crossActive.SetActive (true);
		}
	}


	public void ClickButton(){
		if (isActive) {
			activeFrame.SetActive (true);
			if (Cannon.leftBullet == side) {
				CannonsPanel.instance.SetLeftCannon (cannonType);
			} else {
				CannonsPanel.instance.SetRightCannon (cannonType);
			}
			PauseGame.instance.RedrawCannonButtons ();
		}
	}

	public string GetKey(){
		return side.ToString () + "_" + cannonType.ToString ();
	}
}
