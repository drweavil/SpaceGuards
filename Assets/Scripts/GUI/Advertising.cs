/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class Advertising : MonoBehaviour, IInterstitialAdListener {

	public static Advertising advertising;
	public bool advertisingActive = true;
	public static float advertisingTime = 120f;
	Timer advertisingTimer = new Timer();

	void Start(){
		advertising = this;
		Timer connectTimer = new Timer ();
		connectTimer.SetTimer (GameController.connectionTimeout + 0.1f);
		StartCoroutine(connectTimer.ActionAfterTimer (() => {
			Initialize();
			advertisingTimer.SetTimer(advertisingTime);
		}));


	}

	public void Initialize(){
		string appKey = "345353ffab6721301180c706bd6f0e81f1072f313d2103a1";
		Appodeal.disableLocationPermissionCheck();
		Appodeal.muteVideosIfCallsMuted(true);
		Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.BANNER);
		Appodeal.setInterstitialCallbacks (this);
	}

	public static void HideAllBanners(){
		Appodeal.hide (Appodeal.BANNER);
	}

	public static void ShowBannerLeft(){
		if (advertising.advertisingActive) {
			Appodeal.show (Appodeal.BANNER_HORIZONTAL_LEFT);
		}
	}

	public static void ShowBannerTop(){
		if (advertising.advertisingActive) {
			Appodeal.hide (Appodeal.BANNER);
			Appodeal.show (Appodeal.BANNER_TOP);
		}
	}

	public static void ShowInterstitial(){
		if (advertising.advertisingActive) {
			if(Appodeal.isLoaded(Appodeal.INTERSTITIAL) && !Appodeal.isPrecache(Appodeal.INTERSTITIAL)) {
				Appodeal.show (Appodeal.INTERSTITIAL);

			} 
		}
	}

	public static void TryShowInterstitial(){
		if (advertising.advertisingActive) {
			if (advertising.advertisingTimer.TimeIsOver ()) {
				if (Appodeal.isLoaded (Appodeal.INTERSTITIAL) && !Appodeal.isPrecache (Appodeal.INTERSTITIAL)) {
					Appodeal.show (Appodeal.INTERSTITIAL);
					advertising.advertisingTimer.SetTimer (advertisingTime);
				}
			}
		}
	}



	public void onInterstitialLoaded(bool isPrecache) { }
	public void onInterstitialFailedToLoad() {}
	public void onInterstitialShown() {}
	public void onInterstitialClicked() {}

	public void onInterstitialClosed() {}
}*/
