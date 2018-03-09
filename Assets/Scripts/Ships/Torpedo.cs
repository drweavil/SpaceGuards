using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour {
	public ExplodeObject explodeObject;

	public void DestroyTorpedo(){
		DestroyAction ();
		explodeObject.DefaultDestroy ();
	}
	public void DestroyTorpedoWithPoints(){
		DestroyAction ();
		explodeObject.DefaultDestroyWithPoints();
	}

	void DestroyAction(){
		string path = "Prefabs/Effects/explode";
		GameObject expObj = ObjectsPool.PullObject (path);
		Effect exp = expObj.GetComponent<Effect>();
		exp.poolPath = path;
		exp.transform.position = this.transform.position;
		exp.DestoyOverTime(exp.main.main.duration);
	}
}
