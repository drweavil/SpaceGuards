using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMine : MonoBehaviour {
	public ExplodeObject explodeObject;

	public void Destroy(){
		DestroyAction ();
		explodeObject.DefaultDestroy ();
	}
	public void DestroyWithPoints(){
		DestroyAction ();
		explodeObject.DefaultDestroyWithPoints();
	}

	void DestroyAction(){
		string path = "Prefabs/Effects/bullet2ExplodeShatter";
		GameObject expObj = ObjectsPool.PullObject (path);
		Effect exp = expObj.GetComponent<Effect>();
		exp.poolPath = path;
		exp.transform.position = this.transform.position;
		exp.DestoyOverTime(exp.main.main.duration);
	}
}
