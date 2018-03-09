using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolynomialPart : MonoBehaviour {
	public Polynomial polinomial;


	public void DestroyPart(){
		string path = "Prefabs/Effects/bullet2ExplodeShatter";
		GameObject expObj = ObjectsPool.PullObject (path);
		Effect exp = expObj.GetComponent<Effect>();
		exp.poolPath = path;
		exp.transform.position = this.transform.position;
		exp.DestoyOverTime(exp.main.main.duration);



		this.gameObject.SetActive (false);

		polinomial.TryDestroy ();
	}
}
