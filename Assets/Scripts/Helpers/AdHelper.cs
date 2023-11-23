using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdHelper : MonoBehaviour
{
	[Header("Time Settings")]
	public int secondsBetweenAds = 90;

#if DEBUG
	private const string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#else
    private const string _adUnitId  = "ca-app-pub-8946455145480386/6700969289";
#endif

	private const string LastAdDateKey = "LastAdDate";

	private InterstitialAd _interstitialAd;

	// Start is called before the first frame update
	void Start()
	{
		PlayerPrefs.SetString(LastAdDateKey, DateTime.UtcNow.ToString());
		MobileAds.Initialize((InitializationStatus initStatus) =>
		{
			Debug.Log("MobileAds Initialized.");
		});
	}

	public void ShowInterstitialAd()
	{
		if (_interstitialAd != null && _interstitialAd.CanShowAd())
		{
			PlayerPrefs.SetString(LastAdDateKey, DateTime.UtcNow.ToString());
			PlayerPrefs.Save();

			Debug.Log("lastAdDate saved. Showing interstitial ad.");
			_interstitialAd.Show();
		}
		else
		{
			Debug.LogError("Interstitial ad is not ready yet.");
		}
	}

	public void LoadInterstitialAd()
	{
		if (Application.platform is not RuntimePlatform.Android)
			return;

		// Clean up the old ad before loading a new one.
		if (_interstitialAd != null)
		{
			_interstitialAd.Destroy();
			_interstitialAd = null;
		}

		Debug.Log("Loading the interstitial ad.");

		// create our request used to load the ad.
		var adRequest = new AdRequest();

		// send the request to load the ad.
		InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
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

				_interstitialAd = ad;

				string lastAdDateString = PlayerPrefs.GetString(LastAdDateKey, DateTime.UtcNow.ToString());
				DateTime lastAdDate;

				if (DateTime.TryParse(lastAdDateString, out lastAdDate))
				{
					if (lastAdDate < DateTime.UtcNow.AddSeconds(-secondsBetweenAds))
					{
						ShowInterstitialAd();
					}
					else
					{
						TimeSpan timeSinceLastAd = DateTime.UtcNow - lastAdDate;
						Debug.Log("Passed time for ads: "+ timeSinceLastAd.TotalSeconds);
						Debug.Log("Required time for ads: " + secondsBetweenAds);
					}
				}
			});
	}
}
