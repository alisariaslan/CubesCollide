using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdController : MonoBehaviour
{

#if UNITY_ANDROID
	private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif PLATFORM_ANDROID
  private string _adUnitId = "ca-app-pub-8946455145480386/5127207475";
#endif

	private InterstitialAd interstitialAd;

	void Start()
    {
		Manager.Game.General.AdController = this;

#if PLATFORM_ANDROID || UNITY_ANDROID
		MobileAds.RaiseAdEventsOnUnityMainThread = true;
		MobileAds.Initialize((InitializationStatus initStatus) =>{});
#endif
	}


	float everySecond = 60;
	bool readyToShowAd = false;
	void Update()
	{
		everySecond -= Time.deltaTime;
		if (everySecond < 0)
		{
			readyToShowAd = true;
			everySecond = 60;
		}
	}

	public void LoadInterstitialAd()
	{
		if (readyToShowAd)
			everySecond = 60;
		else return;

#if false

		// Clean up the old ad before loading a new one.
		if (interstitialAd != null)
		{
			interstitialAd.Destroy();
			interstitialAd = null;
		}

		Debug.Log("Loading the interstitial ad.");

		// create our request used to load the ad.
		var adRequest = new AdRequest();
		adRequest.Keywords.Add("unity-admob-sample");

		// send the request to load the ad.
		InterstitialAd.Load(_adUnitId, adRequest,
			(InterstitialAd ad, LoadAdError error) =>
			{
				// if error is not null, the load request failed.
				if (error != null || ad == null)
				{
					Debug.LogError("interstitial ad failed to load an ad " +
								   "with error : " + error);
					return;
				}

				Debug.Log("Interstitial ad loaded with response : "
						  + ad.GetResponseInfo());

				interstitialAd = ad;
			});

#endif
	}

	public void ShowAd()
	{
		if (readyToShowAd)
			everySecond = 60;
		else return;

#if false
		if (interstitialAd != null && interstitialAd.CanShowAd())
		{
			Debug.Log("Showing interstitial ad.");
			//register events
			interstitialAd.Show();
			RegisterEventHandlers(interstitialAd);
			readyToShowAd = false;
		}
		else
		{
			Debug.LogError("Interstitial ad is not ready yet.");
		}

#endif
	}

	private void RegisterEventHandlers(InterstitialAd ad)
	{
		// Raised when the ad is estimated to have earned money.
		ad.OnAdPaid += (AdValue adValue) =>
		{
			Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
				adValue.Value,
				adValue.CurrencyCode));
		};
		// Raised when an impression is recorded for an ad.
		ad.OnAdImpressionRecorded += () =>
		{
			Debug.Log("Interstitial ad recorded an impression.");
		};
		// Raised when a click is recorded for an ad.
		ad.OnAdClicked += () =>
		{
			Debug.Log("Interstitial ad was clicked.");
		};
		// Raised when an ad opened full screen content.
		ad.OnAdFullScreenContentOpened += () =>
		{
			Debug.Log("Interstitial ad full screen content opened.");
		};
		// Raised when the ad closed full screen content.
		ad.OnAdFullScreenContentClosed += () =>
		{
			Debug.Log("Interstitial ad full screen content closed.");
			interstitialAd.Destroy();
			interstitialAd = null;
		};
		// Raised when the ad failed to open full screen content.
		ad.OnAdFullScreenContentFailed += (AdError error) =>
		{
			Debug.LogError("Interstitial ad failed to open full screen content " +
						   "with error : " + error);
			interstitialAd.Destroy();
			interstitialAd = null;
		};
	}
}
