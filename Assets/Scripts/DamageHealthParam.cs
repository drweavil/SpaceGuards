using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class DamageHealthParam : MonoBehaviour {
	public int id;
	public string stID;
	public float damage;
	public float damageTickTime;
	public float health;
	public float spawnTime;
	public float spawnTime2;
	public float spawnTime3;
	public float pathTime = 1;
	public float chargePathTime = 1;
	public Vector3 bulletSpeed;
	public float existTime = 1;


	public DamageHealthParam Clone(){
		DamageHealthParam damageHealthParam = new DamageHealthParam ();
		foreach (FieldInfo field in this.GetType().GetFields()) {
			FieldInfo thisField = damageHealthParam.GetType ().GetField (field.Name);
			if(thisField != null){
				thisField.SetValue(damageHealthParam, field.GetValue(this));
			}
		}
		return damageHealthParam;
	}
}
