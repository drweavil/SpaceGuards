using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour {
	public LevelButton button1;
	public LevelButton button2;
	public RectTransform lineRect;
	public GameObject levelGreen;
	public GameObject levelRed;

	public void RedrawLine(){
		Vector2 lineDirection =   button1.transform.position - button2.transform.position;
		lineDirection.Normalize ();

		float angle = Vector3.Angle(lineDirection, new Vector3(1, 0));
		if (lineDirection.y < 0) {
			angle = 360 - angle;
		}

		lineRect.sizeDelta = new Vector2 (Vector2.Distance(button1.rectTransform.localPosition, button2.rectTransform.localPosition), lineRect.sizeDelta.y);
		lineRect.position = button1.transform.position;
		lineRect.rotation = Quaternion.Euler (new Vector3(0, 0, angle));

	}

	public string GetTransitionKey(){
		return button1.planetID.ToString() + "_" + button1.levelID.ToString();
	}
		

	public void Red(){
		levelRed.SetActive (true);
		levelGreen.SetActive (false);
	}

	public void Green(){
		levelRed.SetActive (false);
		levelGreen.SetActive (true);	
	}
}
