using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHealthParamController : MonoBehaviour {
	public static DamageHealthParamController instance;
	public List<DamageHealthParam> cannonParams = new List<DamageHealthParam> ();
	public List<DamageHealthParam> shipParams = new List<DamageHealthParam> ();

	void Awake(){
		instance = this;
	}

	public DamageHealthParam GetCannonParamsById(int id){
		DamageHealthParam lol = cannonParams.Find (p => p.id == id);
		return lol;
	}

	public DamageHealthParam GetExplodeObjectById(string id){
		DamageHealthParam lol = shipParams.Find (p => p.stID == id);
		return lol;
	}
}