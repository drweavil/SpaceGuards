using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static GameController startProcessController;
	public delegate void Action();

	public static IEnumerator ActionAfterFewFramesCoroutine(int frames, Action action){
		for(int i = 0; i < frames; i++){
			yield return null;
		}
		action ();		
	}
}
