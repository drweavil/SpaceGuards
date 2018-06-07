using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {
	public static ShopMenu instance;
	public GameObject window;

	public const float wipePrice = 300f;
	public const float shieldPrice = 300f;
	public const float doubleDamagePrice = 200f;
	public const float freezePrice = 100f;

	public Text pointsText;

	public Text wipePriceText;
	public Text shieldPriceText;
	public Text doubleDamagePriceText;
	public Text freezePriceText;


	public Text wipeCountText;
	public Text shieldCountText;
	public Text doubleDamageCountText;
	public Text freezeCountText;

	public Text wipeDescriptionText;
	public Text shieldDescriptionText;
	public Text doubleDamageDescriptionText;
	public Text freezeDescriptionText;

	float countdownPointsStack;
	float currentStep;
	int frames = 60;

	void Awake(){
		instance = this;
	}

	public void OpenShopMenu(){
		countdownPointsStack = 0;
		window.SetActive (true);
		RedrawStats ();
		SetPrices ();
		SetDescriptions ();
	}

	public void RedrawStats(){
		wipeCountText.text = LevelController.instance.gameSessionData.wipeAbilityCount.ToString ();
		shieldCountText.text = LevelController.instance.gameSessionData.shieldAbilityCount.ToString ();
		doubleDamageCountText.text = LevelController.instance.gameSessionData.doubleDamageAbilityCount.ToString ();
		freezeCountText.text = LevelController.instance.gameSessionData.freezeAbilityCount.ToString ();
		pointsText.text = System.Math.Round (LevelController.instance.gameSessionData.points, 0).ToString();
	}

	void SetPrices(){
		wipePriceText.text = wipePrice.ToString ();
		shieldPriceText.text = shieldPrice.ToString ();
		doubleDamagePriceText.text = doubleDamagePrice.ToString ();
		freezePriceText.text = freezePrice.ToString ();
	}

	void SetDescriptions(){
		wipeDescriptionText.text = LanguageController.jsonFile ["descriptions"] ["wipe"];
		shieldDescriptionText.text = LanguageController.jsonFile ["descriptions"] ["shield"];
		doubleDamageDescriptionText.text = LanguageController.jsonFile ["descriptions"] ["doubleDamage"];
		freezeDescriptionText.text = LanguageController.jsonFile ["descriptions"] ["freeze"];
	}

	public void BuyWipe(){
		if (LevelController.instance.gameSessionData.points >= wipePrice) {
			LevelController.instance.gameSessionData.wipeAbilityCount += 1;
			BattleInterface.instance.RedrawAllInfo ();
			LevelController.instance.gameSessionData.points -= wipePrice;
			LevelController.instance.SaveGameSessionData ();
			RedrawStats ();
			AddPointToStack (wipePrice);
		}

	}

	public void BuyShield(){
		if (LevelController.instance.gameSessionData.points >= shieldPrice) {
			LevelController.instance.gameSessionData.shieldAbilityCount += 1;
			BattleInterface.instance.RedrawAllInfo ();
			LevelController.instance.gameSessionData.points -= shieldPrice;
			LevelController.instance.SaveGameSessionData ();
			RedrawStats ();
			AddPointToStack (shieldPrice);
		}
	}

	public void BuyDoubleDamage(){
		if (LevelController.instance.gameSessionData.points >= doubleDamagePrice) {
			LevelController.instance.gameSessionData.doubleDamageAbilityCount += 1;
			BattleInterface.instance.RedrawAllInfo ();
			LevelController.instance.gameSessionData.points -= doubleDamagePrice;
			LevelController.instance.SaveGameSessionData ();
			RedrawStats ();
			AddPointToStack (doubleDamagePrice);
		}

	}

	public void BuyFreeze(){
		if (LevelController.instance.gameSessionData.points >= freezePrice) {
			LevelController.instance.gameSessionData.freezeAbilityCount += 1;
			BattleInterface.instance.RedrawAllInfo ();
			LevelController.instance.gameSessionData.points -= freezePrice;
			LevelController.instance.SaveGameSessionData ();
			RedrawStats ();
			AddPointToStack (freezePrice);
		}
	}

	void AddPointToStack(float minusPoints){
		if (countdownPointsStack == 0) {
			countdownPointsStack += minusPoints;
			currentStep = countdownPointsStack / (float)frames;
			StartCoroutine (StartPointsCountdown ());
		} else {
			countdownPointsStack += minusPoints;
			currentStep = countdownPointsStack / (float)frames;
		}

	}

	IEnumerator StartPointsCountdown(){
		while(countdownPointsStack > 0){
			countdownPointsStack -= currentStep;
			pointsText.text = System.Math.Round (LevelController.instance.gameSessionData.points + countdownPointsStack, 0).ToString();
			yield return null;
		}
		countdownPointsStack = 0;
		pointsText.text = System.Math.Round (LevelController.instance.gameSessionData.points, 0).ToString();
		yield break;
	}




}
