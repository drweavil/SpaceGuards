using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetButton : MonoBehaviour {
	public int status;
	public Animator anim;
	public int planetID;
	public Button button;


	public void PlanetButtonClick(){
		if (status != Levels.locked) {
			if (!LevelController.instance.gameSessionData.fullVersionActive && !(planetID == 1 | planetID == 2)) {
				Interface.interfaceSt.OpenFullVersionWindow ();
			} else {
				PlanetsMenu.instance.menuAnimator.speed = 1;
				PlanetsMenu.instance.menuAnimator.Play ("zoomPlanet_" + planetID.ToString ());
				PlanetsMenu.instance.selectedPlanet = planetID;
				PlanetsMenu.instance.currentPlanetButton = this;
				PlanetsMenu.instance.closeButtonAction = () => {
					PlanetsMenu.instance.BackToPlanetsButton ();
					//Advertising.TryShowInterstitial ();
				};
				button.enabled = false;
				//Advertising.TryShowInterstitial ();
			}
		}
	}
}
