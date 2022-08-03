using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Advertisements;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class InAppPurchasing : MonoBehaviour, IStoreListener
{
	private static IStoreController m_StoreController;
	// The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider;
	// The store-specific Purchasing subsystems.

	// Product identifiers for all products capable of being purchased:
	// "convenience" general identifiers for use with Purchasing, and their store-specific identifier
	// counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers
	// also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

	// General product identifiers for the consumable, non-consumable, and subscription products.
	// Use these handles in the code to reference which product to purchase. Also use these values
	// when defining the Product Identifiers on the store. Except, for illustration purposes, the
	// kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
	// specific mapping to Unity Purchasing's AddProduct, below.
	public static string kProductIDConsumable = "consumable";

	public static string kProductIDSubscription = "subscription";

	[Header("Write your desire Deal's string to be used for further references")]
	public List<string> InappKeys;
	// Apple App Store-specific product identifier for the subscription product.
	//	private static string kProductNameAppleSubscription =  "com.unity3d.subscription.new";

	// Google Play Store-specific product identifier subscription product.
	private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

	public static InAppPurchasing instance { get; private set; }

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
	}

	void Start()
	{
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}
	}

	public void InitializePurchasing()
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		//builder.AddProduct (InappKeys [0], ProductType.NonConsumable);
		//builder.AddProduct (InappKeys [1], ProductType.NonConsumable);

		for (int i = 0; i < InappKeys.Count; i++)
		{
			builder.AddProduct(InappKeys[i], ProductType.Consumable);
		}
		UnityPurchasing.Initialize(this, builder);
	}

	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}



	public void BuyDeals(int _num)
	{
		// Buy the non-consumable product using its general identifier. Expect a response either 
		// through ProcessPurchase or OnPurchaseFailed asynchronously.
		BuyProductID(InappKeys[_num]);
	}


	void BuyProductID(string productId)
	{
		// If Purchasing has been initialized ...
		if (IsInitialized())
		{
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID(productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
			}
			// Otherwise ...
			else
			{
				// ... report the product look-up failure situation  
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else
		{
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}


	// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
	// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
	public void RestorePurchases()
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}
	}

	//
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		//		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		// A consumable product has been purchased by this user.
		PurchaseProcessingResult flag = PurchaseProcessingResult.Pending;
		;
		for (int i = 0; i < InappKeys.Count; i++)
		{
			if (String.Equals(args.purchasedProduct.definition.id, InappKeys[i], StringComparison.Ordinal))
			{
				Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
				InAppSuccessArgs(InappKeys[i]);
				if (i == 1)
				{
					flag = PurchaseProcessingResult.Pending;
				}
				else
				{
					return flag = PurchaseProcessingResult.Complete;
				}
			}
		}
		return flag;

	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

		PopupManager.instance.purchaseFailurePopup.Enable();
	}

	public void OnUnityAdsReady(string placementId)
	{
		throw new NotImplementedException();
	}

	public void OnUnityAdsDidError(string message)
	{
		throw new NotImplementedException();
	}

	public void OnUnityAdsDidStart(string placementId)
	{
		throw new NotImplementedException();
	}

	#region IAP Developer Code
	public void InAppPurchase(int inAppIndex)
	{
		GetComponent<InAppPurchasing>().BuyDeals(inAppIndex);
	}

	public void InAppSuccessArgs(string _name)
	{
		// Rubies_12
		if (_name == GetComponent<InAppPurchasing>().InappKeys[0])
		{
			PopupManager.instance.purchaseIAPCompletePopup.gameObject.SetActive(true);
			PopupManager.instance.purchaseIAPCompletePopup.UpdateValues(12);
		}
		// Rubies_59
		else if (_name == GetComponent<InAppPurchasing>().InappKeys[1])
		{
			PopupManager.instance.purchaseIAPCompletePopup.gameObject.SetActive(true);
			PopupManager.instance.purchaseIAPCompletePopup.UpdateValues(59);
		}
		// Rubies_110
		else if (_name == GetComponent<InAppPurchasing>().InappKeys[2])
		{
			PopupManager.instance.purchaseIAPCompletePopup.gameObject.SetActive(true);
			PopupManager.instance.purchaseIAPCompletePopup.UpdateValues(110);
		}
		// Rubies_220
		else if (_name == GetComponent<InAppPurchasing>().InappKeys[3])
		{
			PopupManager.instance.purchaseIAPCompletePopup.gameObject.SetActive(true);
			PopupManager.instance.purchaseIAPCompletePopup.UpdateValues(220);
		}
		// Rubies_330
		else if (_name == GetComponent<InAppPurchasing>().InappKeys[4])
		{
			PopupManager.instance.purchaseIAPCompletePopup.gameObject.SetActive(true);
			PopupManager.instance.purchaseIAPCompletePopup.UpdateValues(330);
		}
		// Rubies_550
		else if (_name == GetComponent<InAppPurchasing>().InappKeys[5])
		{
			PopupManager.instance.purchaseIAPCompletePopup.gameObject.SetActive(true);
			PopupManager.instance.purchaseIAPCompletePopup.UpdateValues(550);
		}
		// Rubies_990
		else if (_name == GetComponent<InAppPurchasing>().InappKeys[6])
		{
			PopupManager.instance.purchaseIAPCompletePopup.gameObject.SetActive(true);
			PopupManager.instance.purchaseIAPCompletePopup.UpdateValues(990);
		}
	}
	#endregion
}