using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindowController : MonoBehaviour {
	public static DialogWindowController dialogWindowController;
	public GameObject dialogWindow;
	public GameObject acceptButton;
	public GameObject cancelButton;
	public delegate void ButtonAction (object[] actionParams);
	public ButtonAction acceptButtonAction;
	public object[] acceptButtonActionParams;
	public ButtonAction cancelButtonAction;
	public object[] cancelButtonActionParams;
	public Text windowText;

	void Awake(){
		dialogWindowController = this;
	}

	public void OpenDialog(string text, ButtonAction acceptAction = null, object[] acceptButtonParams = null, ButtonAction cancelAction = null, object[] cancelButtonParams = null){
		dialogWindow.SetActive (true);
		acceptButton.SetActive (false);
		if (acceptAction != null) {
			acceptButtonActionParams = acceptButtonParams;
			acceptButton.SetActive (true);
			acceptButtonAction = acceptAction;
		}

		acceptButtonActionParams = acceptButtonParams ?? new object[0];
		cancelButtonActionParams = cancelButtonParams ?? new object[0];

		cancelButtonAction = cancelAction;
		windowText.text = text;
	}


	public void AcceptButton(){
		//AudioController.MenuButtonSound ();
		acceptButtonAction(acceptButtonActionParams);
		dialogWindow.SetActive (false);
	}

	public void CancelButton(){
		//AudioController.CancelSound ();
		if (cancelButtonAction != null) {
			cancelButtonAction (cancelButtonActionParams);
		}
		dialogWindow.SetActive (false);
	}

}
