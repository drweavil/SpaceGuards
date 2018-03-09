using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonsPanel : MonoBehaviour {
	public static CannonsPanel instance;
	public RectTransform panelPosition;
	public Transform panelTransform;
	public Vector2 standartScreenSize;
	public float standartScaleX;
	public Transform leftCannonTransform;
	public Transform rightCannonTransform;
	public Cannon leftCannon;
	public Cannon rightCannon;
	public float health;
	public float shieldPoints;
	public float maximumHealth = 0;

	public GameObject health100;
	public GameObject health75;
	public GameObject health50;
	public GameObject health25;
	int currentHealthState;

	public List<SpriteRenderer> cannonSprites = new List<SpriteRenderer>();

	public List<ParticleSystem> overdriveEffects = new List<ParticleSystem> ();
	public ParticleSystem shieldEffect;

	void Awake(){
		instance = this;
		RestoreMaximumHealth ();
		SetPanelPosition ();
		SetScale ();
		SetLeftCannon (1);
		SetRightCannon (1);

	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.I)) {
			
			SetLeftCannon (1);
			SetRightCannon (1);
		}

		if (Input.GetKeyDown (KeyCode.U)) {
			MakeDamage (25f);
		}
	}

	public void RestoreMaximumHealth(){
		health = maximumHealth;
		SetVisualState ();
	}

	public void RestoreHealth(float number){
		health += number;
		if (health > maximumHealth) {
			health = maximumHealth;
		}
		SetVisualState ();
	}

	public void SetShieldPoints(float points){
		shieldEffect.gameObject.SetActive (true);
		shieldPoints = points;
	}

	public void MakeDamage(float damage){
		if (shieldPoints <= 0) {
			shieldEffect.gameObject.SetActive (false);
		}
		if (shieldPoints != 0) {

			shieldPoints -= damage;

			if (shieldPoints < 0) {
				damage = shieldPoints * -1f;
				shieldPoints = 0;

			} else if (shieldPoints >= 0) {
				damage = 0;
			}


		}


		if (health <= damage) {
			//ObjectsPool.PushObject(poolPath, gameObject);
		} else {
			health -= damage;
			//StartCoroutine (DamageAnimation());
		}
		//StartCoroutine (DamageAnimation());
		SetVisualState ();
	}

	public void SetVisualState(){
		if (health > (maximumHealth - maximumHealth * 0.25f)) {
			if (currentHealthState != 100) {
				health100.SetActive (true);
				health75.SetActive (false);
				health50.SetActive (false);
				health25.SetActive (false);
				currentHealthState = 100;
			}
		} else if (health <= (maximumHealth - maximumHealth * 0.25f) && health > (maximumHealth - maximumHealth * 0.50f)) {
			if (currentHealthState != 75) {
				health100.SetActive (false);
				health75.SetActive (true);
				health50.SetActive (false);
				health25.SetActive (false);
				currentHealthState = 75;
			}
		} else if (health <= (maximumHealth - maximumHealth * 0.50f) && health > (maximumHealth - maximumHealth * 0.75f)) {
			if (currentHealthState != 50) {
				health100.SetActive (false);
				health75.SetActive (false);
				health50.SetActive (true);
				health25.SetActive (false);
				currentHealthState = 50;
			}
		} else if (health <= (maximumHealth - maximumHealth * 0.75f)) {
			if (currentHealthState != 100) {
				health100.SetActive (false);
				health75.SetActive (false);
				health50.SetActive (false);
				health25.SetActive (true);
				currentHealthState = 25;
			}
		}
	}


	public void SetLeftCannon(int id){
		if (leftCannon != null) {
			ObjectsPool.PushObject(leftCannon.poolPath, leftCannon.gameObject);
		}
		string poolPath = "Prefabs/Cannons/type_" + id.ToString () + "/left";
		GameObject	cannon = ObjectsPool.PullObject (poolPath);
		leftCannon = cannon.GetComponent<Cannon> ();
		leftCannon.poolPath = poolPath;
		cannon.transform.position = leftCannonTransform.position;
	}
	public void SetRightCannon(int id){
		if (rightCannon != null) {
			ObjectsPool.PushObject(rightCannon.poolPath, rightCannon.gameObject);
		}
		string poolPath = "Prefabs/Cannons/type_" + id.ToString () + "/right";
		GameObject	cannon = ObjectsPool.PullObject (poolPath);
		rightCannon = cannon.GetComponent<Cannon> ();
		rightCannon.poolPath = poolPath;
		cannon.transform.position = rightCannonTransform.position;
	}

	public void SetPanelPosition(){
		//Debug.Log (panelPosition);
		panelTransform.position = Camera.main.ScreenToWorldPoint (panelPosition.position);
		panelTransform.position = new Vector3(panelTransform.position.x, panelTransform.position.y, 0);
	}


	public void SetScale(){
		float standartRatio = standartScreenSize.x / standartScreenSize.y;

		panelTransform.localScale = new Vector3((standartScaleX * Camera.main.aspect)/standartRatio, panelPosition.localScale.y, panelPosition.localScale.z);
	}


	IEnumerator DamageAnimation(){
		foreach (SpriteRenderer spr in cannonSprites) {
			spr.color = Color.red;
		}

		for (int i = 0; i < 1; i++) {
			yield return null;
		}

		foreach (SpriteRenderer spr in cannonSprites) {
			spr.color = Color.white;
		}
	}

}
