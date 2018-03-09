using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FewFrameBulletCollider : MonoBehaviour {
	List<ExplodeObject> triggeredExplodeObjects = new List<ExplodeObject>();
	List<Collider> colliders = new List<Collider>();
	public delegate void Action();
	public Action colliderAction;

	void Trigger(){
		
	}
}
