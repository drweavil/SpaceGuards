using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPController : MonoBehaviour, IStoreListener
{
	public static IAPController instance;
	public static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
	public delegate void PurchasingAction();
	public PurchasingAction currentPurchasingAction;
	public PurchasingAction initializeAction;

	void Start()
	{
		instance = this;
		Timer connectTimer = new Timer ();
		connectTimer.SetTimer (GameController.connectionTimeout + 0.1f);
		StartCoroutine(connectTimer.ActionAfterTimer (() => {
			if (m_StoreController == null)
			{
				initializeAction = () => {
					StartCoroutine(GameController.ActionAfterFewFramesCoroutine(3, () => {
						LevelController.instance.CheckFullVersionGame ();
					}));
				};
				InitializePurchasing();
				//LevelController.instance.CheckFullVersionGame ();
			}

		}));


		StartCoroutine (GameController.ActionAfterFewFramesCoroutine(10, () => {

		}));
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.I)) {
			Debug.Log (m_StoreController.products.WithID("theme_3").metadata.localizedTitle);
		}
	}

	/*public static string Test(){
		return m_StoreController.products.WithID ("theme_1").metadata.localizedPriceString;
	}*/

	public Product GetProduct(string id){
		Debug.Log (m_StoreController.products);
		return m_StoreController.products.WithID (id);
	}

	public string GetProductPrice(string id){
		try{
			return GetProduct (id).metadata.localizedPriceString;
		}catch{
			return "-";
		}
	}



	public void InitializePurchasing()
	{
		 if (IsInitialized())
        {
            return;
        }

		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		builder.AddProduct ("full_version", ProductType.NonConsumable);
		UnityPurchasing.Initialize(this, builder);
	}


	private bool IsInitialized()
	{
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}


	public void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			Product product = m_StoreController.products.WithID(productId);
			if (product != null && product.availableToPurchase)
			{
				m_StoreController.InitiatePurchase(product);
			}
			else
			{
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		else
		{
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void RestorePurchases()
	{
		if (!IsInitialized())
		{
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		if (Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			Debug.Log("RestorePurchases started ...");
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			apple.RestoreTransactions((result) => {
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		else
		{
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}


	//  
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
		if(initializeAction != null){
			initializeAction.Invoke ();
		}
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		currentPurchasingAction();
		currentPurchasingAction = null;
		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		DialogWindowController.dialogWindowController.OpenDialog (LanguageController.jsonFile["dialogMessages"]["actionCanceled"]);
	}
}
