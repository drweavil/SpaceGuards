using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonsPanelCollider : MonoBehaviour {
	public CannonsPanel cannonPanel;

	void OnTriggerEnter(Collider col){
		if (col.gameObject.layer == LayerMask.NameToLayer ("ExplodeObject")) {
			ExplodeObject obj = col.gameObject.GetComponent<ExplodeObjectCollider> ().explodeObject;
			if (obj.isActive) {
				cannonPanel.MakeDamage (col.gameObject.GetComponent<ExplodeObjectCollider> ().explodeObject.damageHealthParam.damage);
				obj.ExplodeObjectDestroy ();
			}
		}
	}
}
