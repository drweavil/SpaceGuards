using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPart : MonoBehaviour {
	public BackgroundSystem systemController;
	public BackgroundPart anotherPart;
	public Transform startTransform;
	public Transform endTransform;
	public Transform partTransform;
	public Transform centerTransform;
	//public Vector3 speed = new Vector3(0, 0.2f, 0);
	public int frame = 0;

	public void SetPositionByStart(Vector3 position){
		partTransform.position = position;
		float distanceY = Vector3.Distance (startTransform.position, partTransform.position);
		partTransform.position = new Vector3 (partTransform.position.x, partTransform.position.y + distanceY, partTransform.position.z);
	}

	public void SetPositionByEnd(Vector3 position){
		partTransform.position = position;
		float distanceY = Vector3.Distance (endTransform.position, partTransform.position);
		partTransform.position = new Vector3 (partTransform.position.x, partTransform.position.y - distanceY, partTransform.position.z);
	}

	void Update(){
		if (frame == systemController.frameCheckRate) {
			frame = 0;
			if (systemController.upToDown) {
				if (IsUnderCenter ()) {
					SetPositionByStart (anotherPart.endTransform.position);
				}
			} else {
				if (IsAboveCenter ()) {
					SetPositionByEnd (anotherPart.startTransform.position);
				}
			}
		} else {
			frame += 1;
		}
		if (systemController.upToDown) {
			partTransform.Translate (systemController.speed * -1 * Time.timeScale);
		} else {
			partTransform.Translate (systemController.speed * Time.timeScale);
		}
	}

	bool IsUnderCenter(){
		return BackgroundController.instance.centerChecker.position.y >= anotherPart.centerTransform.position.y;
	}

	bool IsAboveCenter(){
		return BackgroundController.instance.centerChecker.position.y <= anotherPart.centerTransform.position.y;
	}
}
