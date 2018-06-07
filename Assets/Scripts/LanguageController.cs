using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class LanguageController : MonoBehaviour {
	public static LanguageController languageController;
	public Text newGameButton;
	public Text shopButton;
	public Text mapButton;
	public Text fullVersion;
	public Text exit;
	public Text mainMenu;
	public Text mainMenu2;
	public Text mainMenu3;
	public Text buyConsumable;
	public Text buyConsumable2;
	public Text buyConsumable3;
	public Text buyConsumable4;
	public Text tryAgainButton;
	public Text tryAgainButton2;
	public Text nextLevel;
	public Text buyFullVersion;



	public Text levelComplete;
	public Text levelFault;
	public Text aimPauseText;
	public Text aimPauseText2;
	public Text fullVersionBannerTitle;
	public Text fullVersionBannerPlanets;
	public Text fullVersionBannerNoAds;
	public Text fullVersionBannerNewGameplay;

	public static JSONNode jsonFile;


	void Awake(){
		languageController = this;
		StartCoroutine(GameController.ActionAfterFewFramesCoroutine(10, () =>{
			//if(GameController.gameController.currentSettings.language == ""){
			if(Application.systemLanguage == SystemLanguage.Russian || 
				Application.systemLanguage == SystemLanguage.Ukrainian || 
				Application.systemLanguage == SystemLanguage.Belarusian
			){
				SetRussian();
			} else{
				SetEnglish();
			}
			/*}else{
				TextAsset jsonAsset = (TextAsset)Resources.Load("Text/" + GameController.gameController.currentSettings.language);
				string jsonString = jsonAsset.text;
				jsonFile = JSON.Parse(jsonString);
				languageController.SetButtons();
			}*/
		}));
	}



	public static void SetRussian(){
		//GameController.gameController.currentSettings.language = "ru";
		TextAsset jsonAsset = (TextAsset)Resources.Load ("Text/ru");
		string jsonString = jsonAsset.text;
		jsonFile = JSON.Parse(jsonString);
		languageController.SetButtons();
		//SaveLoad.SaveGameSettings (GameController.gameController.currentSettings);
	}

	public static void SetEnglish(){
		//GameController.gameController.currentSettings.language = "en";
		TextAsset jsonAsset = (TextAsset)Resources.Load ("Text/en");
		string jsonString = jsonAsset.text;
		jsonFile = JSON.Parse(jsonString);
		languageController.SetButtons();
		//SaveLoad.SaveGameSettings (GameController.gameController.currentSettings);
	}

	public void SetButtons(){
		

		mapButton.text = jsonFile ["menuButtons"] ["map"];
		newGameButton.text = jsonFile ["menuButtons"] ["newGame"];
		shopButton.text = jsonFile ["menuButtons"] ["shop"];

		fullVersion.text = jsonFile ["menuButtons"] ["fullVersion"];
		exit.text = jsonFile ["menuButtons"] ["exit"];
		mainMenu.text = jsonFile ["menuButtons"] ["mainMenu"];
		mainMenu2.text = jsonFile ["menuButtons"] ["mainMenu"];
		mainMenu3.text = jsonFile ["menuButtons"] ["mainMenu"];
		buyConsumable.text = jsonFile ["menuButtons"] ["buyConsumable"];
		buyConsumable2.text = jsonFile ["menuButtons"] ["buyConsumable"];
		buyConsumable3.text = jsonFile ["menuButtons"] ["buyConsumable"];
		buyConsumable4.text = jsonFile ["menuButtons"] ["buyConsumable"];
		tryAgainButton.text = jsonFile ["menuButtons"] ["tryAgainButton"];
		tryAgainButton2.text = jsonFile ["menuButtons"] ["tryAgainButton"];
		nextLevel.text = jsonFile ["menuButtons"] ["nextLevel"];
		buyFullVersion.text = jsonFile ["menuButtons"] ["buyFullVersion"];

		levelComplete.text = jsonFile ["labels"] ["levelComplete"];
		levelFault.text = jsonFile ["labels"] ["levelFault"];
		aimPauseText.text = jsonFile ["labels"] ["aimPauseText"];
		aimPauseText2.text = jsonFile ["labels"] ["aimPauseText"];
		fullVersionBannerTitle.text = jsonFile ["labels"] ["fullVersionBannerTitle"];
		fullVersionBannerPlanets.text = jsonFile ["labels"] ["fullVersionBannerPlanets"];
		fullVersionBannerNoAds.text = jsonFile ["labels"] ["fullVersionBannerNoAds"];
		fullVersionBannerNewGameplay.text = jsonFile ["labels"] ["fullVersionBannerNewGameplay"];
	}
}
