using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : MonoBehaviour {
	public string poolPath;
	public AudioSource effect;


	public void StartEffect(){
		effect.Play ();
		Timer effectTimer = new Timer ();
		effectTimer.SetTimer (effect.clip.length);
		StartCoroutine(effectTimer.ActionAfterTimer (() => {
			effect.Stop ();
			ObjectsPool.PushObject(poolPath, this.gameObject);
		}));
	}
}
