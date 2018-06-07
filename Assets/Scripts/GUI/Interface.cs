using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour {
	public RectTransform screenArea;
	public RectTransform center;
	public RectTransform panelPosition;
	public Animator interfaceMenu;
	public delegate void InterfaceAction ();
	public InterfaceAction animationEndAction;
	public static Interface interfaceSt;
	public GameObject loseWindow;
	public GameObject winWindow;
	public Text winWindowOldPoints;
	public Text winWindowCurrentPoints;

	public GameObject fullVersionButton;
	public GameObject fullVersionBanner;
	public Text fullVersionBannerButtonBuyText;

	public GameObject clickShield;

	void Awake(){
		interfaceSt = this;
		Timer clickShieldTimer = new Timer ();
		clickShieldTimer.SetTimer (1.5f);
		StartCoroutine (clickShieldTimer.ActionAfterTimer(() => {
			clickShield.SetActive(false);
		}));
	}


	public void OpenMonitorAnimation(InterfaceAction endAction){
		animationEndAction = endAction;
		interfaceMenu.Play ("openMonitorMenu");
	}

	public void CloseMonitorAnimation(InterfaceAction endAction){
		animationEndAction = endAction;
		interfaceMenu.Play ("closeMonitorMenu");
	}

	public void AnimationEndAction(){
		animationEndAction.Invoke ();
	}
		

	public void OpenPlanetsMenu(){
		OpenMonitorAnimation (() => {
			PlanetsMenu.instance.planetsMenuGameObj.SetActive(true);
			PlanetsMenu.instance.menuAnimator.Play("standart");
			PlanetsMenu.instance.RestatusPlanetsInfo();
			//Advertising.TryShowInterstitial ();
			PlanetsMenu.instance.closeButtonAction = () => {
				Interface.interfaceSt.ClosePlanetsMenu (() => {
					//Advertising.TryShowInterstitial ();
				});
			};
		});
	}

	public void ClosePlanetsMenu(InterfaceAction action){
		PlanetsMenu.instance.planetsMenuGameObj.SetActive(false);
		CloseMonitorAnimation (action);
	}


	public void OpenShopMenu(){
		OpenMonitorAnimation (() => {
			//Advertising.TryShowInterstitial ();
			ShopMenu.instance.OpenShopMenu();
		});
	}

	public void CloseShopMenu(){
		//Advertising.TryShowInterstitial ();
		ShopMenu.instance.window.SetActive (false);
		CloseMonitorAnimation (() => {});
	}

	public void NewGameButton(){
		DialogWindowController.dialogWindowController.OpenDialog (LanguageController.jsonFile ["dialogMessages"] ["newGameMessage"], 
			(object[] paramsOBJ) => {
				PlanetsMenu.instance.closeButtonAction = () => {
					Interface.interfaceSt.ClosePlanetsMenu (() => {

					});
				};
				LevelController.instance.NewGameProcess ();
				OpenPlanetsMenu ();	
			});
	}

	public void OpenLoseWindow(){
		loseWindow.SetActive (true);
		ShipsController.instance.ResetUsedAbilities (true);
	}

	public void OpenWinWindow(){
		LevelController.instance.LevelComplete ();
		ShipsController.instance.ResetUsedAbilities ();
		winWindow.SetActive (true);
	}


	public void OpenMainMenuButton(){
		loseWindow.SetActive (false);
		winWindow.SetActive (false);
		MainMenu.instance.windowObject.SetActive (true);
	}

	public void TryAgainButton(){
		loseWindow.SetActive (false);
		winWindow.SetActive (false);
		LevelController.instance.SetLevel (LevelController.instance.level);
		BattleInterface.instance.OpenInterface ();
	}

	public void NextLevelButton(){
		if (LevelController.instance.level.levelID == 12 && LevelController.instance.level.planetID == 6) {
			DialogWindowController.dialogWindowController.OpenDialog (LanguageController.jsonFile ["dialogMessages"] ["gameComplete"]);
		} else {
			CannonsPanel.instance.SetDefaultCannons ();
			winWindow.SetActive (false);
			LevelController.instance.SetLevel (Levels.GetNextLevel (LevelController.instance.level));
			BattleInterface.instance.OpenInterface ();
		}
	}


	public void OpenMainMenu(){
		DialogWindowController.dialogWindowController.OpenDialog (LanguageController.jsonFile ["dialogMessages"] ["exitLevelMessage"], 
			(object[] paramsOBJ) => {
			CannonsPanel.instance.SetInactive ();
			MainMenu.instance.OpenMenu ();	
			ShipsController.instance.ResetUsedAbilities (true);
		});
	}


	public void OpenFullVersionWindow(){
		fullVersionBannerButtonBuyText.text = IAPController.instance.GetProductPrice ("full_version");
		fullVersionBanner.SetActive (true);
	}

	public void BuyFullVersion(){
		IAPController.instance.currentPurchasingAction = () => {
			//Advertising.advertising.advertisingActive = false;
			fullVersionBanner.SetActive(false);
			LevelController.instance.gameSessionData.fullVersionActive = true;
			LevelController.instance.SaveGameSessionData();
			Interface.interfaceSt.fullVersionButton.SetActive(false);
			//OpenPlanetsMenu();
		};
		Debug.Log(IAPController.m_StoreController.products);
		IAPController.instance.BuyProductID ("full_version");
	}

	public void CloseFullVersionBanner(){
		fullVersionBanner.SetActive (false);
	}


	public void ExitButton(){
		DialogWindowController.dialogWindowController.OpenDialog (LanguageController.jsonFile ["dialogMessages"] ["exitWindow"], (object[] actParams) => {
			Application.Quit ();
		});
	}


	public void Test1(){
		Timer loltime = new Timer ();
		loltime.SetTimer (5);
		StartCoroutine (loltime.ActionAfterTimer(() => {
			Debug.Log("lol");
		}));
	}

	public void Test2(){
		float lol = 0;
		for (int i = 0; i < 8; i++) {
			lol += Random.Range (120f, 180f);
		}

		Debug.Log (lol/60f);
	}

	public void Test3(){
		Debug.Log(Levels.GetHealthPool(1, 1));
	}
}
