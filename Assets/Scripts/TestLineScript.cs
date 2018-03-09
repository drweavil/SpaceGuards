using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineScript : MonoBehaviour {
	public static TestLineScript instance;
	public LineRenderer lineRenderer;
	void Awake(){
		instance = this;
	}


	public void Clear(){
		lineRenderer.SetPositions (new List<Vector3>().ToArray());
	}

	public void AddPosition(Vector3 pos){
		/*List<Vector3> lol = new List<Vector3> ();
		Vector3[] lol2 = new Vector3[99999];
		lineRenderer.GetPositions(lol2);
		lol.AddRange (lol2);
		lol.Add (pos);
		lineRenderer.SetPositions (lol.ToArray());*/
		lineRenderer.positionCount += 1;
		lineRenderer.SetPosition (lineRenderer.positionCount - 1, pos);
		/*lineRenderer.position
		lineRenderer.SetPosition (TestLineScript.instance.lineRenderer.positionCount, expObject.explodeTransform.position);*/
	}


}
