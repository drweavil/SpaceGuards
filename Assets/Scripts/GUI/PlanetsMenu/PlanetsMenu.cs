using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsMenu : MonoBehaviour {
	public static PlanetsMenu instance;
	public GameObject planetsMenuGameObj;
	public Animator menuAnimator;
	public PlanetButton currentPlanetButton;
	public int selectedPlanet;
	public Interface.InterfaceAction closeButtonAction;
	public List<PlanetButton> planetButtons = new List<PlanetButton> ();
	public List<LevelButton> levelButtons = new List<LevelButton> ();
	public List<LevelTransition> levelTransitions = new List<LevelTransition>();

	void Awake(){
		instance = this;
	}

	public void RestatusPlanetsInfo(){

		foreach (PlanetButton button in planetButtons) {
			if (LevelController.instance.gameSessionData.planetStatus.ContainsKey (button.planetID)) {
				if (LevelController.instance.gameSessionData.planetStatus [button.planetID]) {
					button.anim.Play ("green");
					button.status = Levels.complete;
				} else {
					button.anim.Play ("gray");
					button.status = Levels.open;
				}
			} else {
				button.anim.Play ("red");
				button.status = Levels.locked;
			}
			button.button.enabled = true;
		}



		foreach (LevelButton button in levelButtons) {
			if (LevelController.instance.gameSessionData.levels.ContainsKey (button.GetButtonKey())) {
				if (LevelController.instance.gameSessionData.levels [button.GetButtonKey()].isComplete) {
					button.Complete();
				} else {
					button.Open ();
				}
			} else {
				button.Lock ();
			}
		}

		foreach (LevelTransition trans in levelTransitions) {
			trans.RedrawLine ();
			if (LevelController.instance.gameSessionData.levels.ContainsKey (trans.GetTransitionKey())) {
				if (LevelController.instance.gameSessionData.levels [trans.GetTransitionKey()].isComplete) {
					trans.Green();
				} else {
					trans.Red ();
				}
			} else {
				trans.Red ();
			}
		}
	}


	public void CloseButtonAction(){
		closeButtonAction.Invoke ();
	}

	public void BackToPlanetsButton(){
		closeButtonAction = () => {
			PlanetsMenu.instance.menuAnimator.Play("standart");
			Interface.interfaceSt.ClosePlanetsMenu (() => {
			});
		};
		currentPlanetButton.button.enabled = true;
		menuAnimator.Play ("backToPlanets_" + selectedPlanet.ToString());

	}
}
