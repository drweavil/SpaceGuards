using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	public static MainMenu instance;
	public GameObject windowObject;

	void Awake(){
		instance = this;
	}

	public void OpenMenu(){
		windowObject.SetActive (true);
	}
}
