using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour {
	public RectTransform screenArea;
	public RectTransform center;
	public RectTransform panelPosition;

	public static Interface interfaceSt;


	void Awake(){
		interfaceSt = this;
	}
}
