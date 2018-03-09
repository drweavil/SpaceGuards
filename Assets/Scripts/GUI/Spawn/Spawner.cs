using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public RectTransform leftTop;
	public RectTransform leftBottom;
	public RectTransform rightTop;
	public RectTransform rightBottom;


	public Vector2 GetRandomPositionInWorld(){
		Vector3 leftTopWorld = Camera.main.ScreenToWorldPoint (leftTop.position);
		Vector3 leftBottomWorld = Camera.main.ScreenToWorldPoint (leftBottom.position);
		Vector3 rightTopWorld = Camera.main.ScreenToWorldPoint (rightTop.position);
		Vector3 rightBottomWorld = Camera.main.ScreenToWorldPoint (rightBottom.position);


		return new Vector2 (
			Random.Range(leftTopWorld.x, rightTopWorld.x), 
			Random.Range(leftTopWorld.y, leftBottomWorld.y)
		);
	}
}
