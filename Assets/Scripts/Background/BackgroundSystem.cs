using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSystem : MonoBehaviour {
	public BackgroundPart part1;
	public BackgroundPart part2;
	public Transform systemTransform;
	public Vector3 speed = new Vector3(0, 0.1f);
	public int frameCheckRate = 100;
	public bool upToDown = true;
	bool part1Active = true;
	public Vector2 standartScreenSize = new Vector2(1920, 1080);
	public Vector3 standartScale = new Vector3 (1, 1, 1);

	public void Awake(){
		SetPixelToPixel ();
		SetScale ();
	}

	public void ChangePartPositions(){
		if (part1Active) {
			
		}
	}


	void SetPixelToPixel(){
		part2.SetPositionByStart (part1.endTransform.position);
	}

	void SetScale(){
		float standartRatio = standartScreenSize.x / standartScreenSize.y;
		systemTransform.localScale = new Vector3(
			(standartScale.x * Camera.main.aspect)/standartRatio, 
			(standartScale.y * Camera.main.aspect)/standartRatio, 
		    (standartScale.z * Camera.main.aspect)/standartRatio
		);
	}
}
