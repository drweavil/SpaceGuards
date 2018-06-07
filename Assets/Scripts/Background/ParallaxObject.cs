using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxObject : MonoBehaviour {
	public Transform objectTransform;
	//public bool upToDown = false;
	public Vector3 speed;
	public string poolPath;
	public int frameRate = 0;


	void Update(){

		if (frameRate == 150) {
			frameRate = 0;
			if (!RectTransformUtility.RectangleContainsScreenPoint (
				Interface.interfaceSt.screenArea, 
				Camera.main.WorldToScreenPoint (new Vector2(objectTransform.position.x, objectTransform.position.y)))
			) {
				//Timer destroyTimer = new Timer ();
				//destroyTimer.SetTimer (5f);

				//StartCoroutine (destroyTimer.ActionAfterTimer (() => {
				//	Debug.Log("aziz");
					ObjectsPool.PushObject (poolPath, this.gameObject);

				//}));
			}
		} else {
			frameRate += 1;
		}


		objectTransform.Translate (speed * Time.timeScale);
	}
}
