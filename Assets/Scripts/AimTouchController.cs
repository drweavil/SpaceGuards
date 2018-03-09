using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimTouchController : MonoBehaviour {
	public static AimTouchController aimTouchController;
	public RectTransform leftTouchArea;
	public RectTransform rightTouchArea;


	public RectTransform leftAim;
	public RectTransform rightAim;

	public bool leftTouchActive = false;
	private System.Nullable<Touch> leftTouch = null;
	private int leftTouchFinger = -1;
	public Vector2 leftTouchPosition;

	public bool rightTouchActive = false;
	private System.Nullable<Touch> rightTouch = null;
	private int rightTouchFinger = -1;
	public Vector2 rightTouchPosition;

	private Touch[] touches;

	void Awake(){
		aimTouchController = this;
	}

	void Update(){
		touches = Input.touches;
		if (touches.Length != 0) {
		//	Debug.Log (touches[0].GetHashCode());
			//Debug.Log("left: " + leftTouchFinger.ToString() + "; right: " + rightTouchFinger.ToString());
		}
		if (leftTouchActive) {
			if (leftTouchFinger == -1) {
				leftTouchFinger = System.Array.Find (touches, t => RectTransformUtility.RectangleContainsScreenPoint(leftTouchArea, t.position) && t.fingerId != rightTouchFinger).fingerId;
			}
			leftTouchPosition = System.Array.Find (touches, t => t.fingerId == leftTouchFinger).position;
			leftAim.position = leftTouchPosition;
		}

		if (rightTouchActive) {
			if (rightTouchFinger == -1) {
				rightTouchFinger = System.Array.Find (touches, t => RectTransformUtility.RectangleContainsScreenPoint(rightTouchArea, t.position) && t.fingerId != leftTouchFinger).fingerId;
			}
			rightTouchPosition = System.Array.Find (touches, t => t.fingerId == rightTouchFinger).position;
			rightAim.position = rightTouchPosition;
		}
		/*if (leftTouch != null) {
			Debug.Log (leftTouch.Value.position);
		}*/
	}

	public void LeftAreaClick(){
		//Debug.Log ("leftclick");
		leftAim.gameObject.SetActive (true);
		leftTouchActive = true;
		//leftTouch = System.Array.Find (touches, t =>  RectTransformUtility.RectangleContainsScreenPoint(leftTouchArea, t.position));
		//leftTouchFinger = leftTouch.Value.fingerId;
	}

	/*public void LeftAimDrag(){
		Debug.Log ("leftDrag");
		leftAim.position = leftTouchPosition;
	}*/

	public void LeftAimDrop(){
		//Debug.Log ("leftDrop");
		leftTouchActive = false;
		leftTouchFinger = -1;
		leftAim.gameObject.SetActive (false);
	}




	public void RightAreaClick(){
		//Debug.Log ("rightclick");
		rightAim.gameObject.SetActive (true);
		rightTouchActive = true;
		//rightTouch = System.Array.Find (touches, t =>  RectTransformUtility.RectangleContainsScreenPoint(rightTouchArea, t.position));
		//rightTouchFinger = rightTouch.Value.fingerId;
	}
	/*public void RightAimDrag(){
		//Debug.Log ("rightDrag");
		rightAim.position = rightTouchPosition;
	}*/
	public void RightAimDrop(){
		//Debug.Log ("rightDrop");
		rightTouchActive = false;
		rightTouchFinger = -1;
		rightAim.gameObject.SetActive (false);
	}

}
