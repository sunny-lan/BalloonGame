using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.Events;

public class InterstitialAdG : MonoBehaviour
{
	// These ad units are configured to always serve test ads.
	[SerializeField]
#if UNITY_ANDROID
	private string _adUnitId;
#elif UNITY_IPHONE
  private string _adUnitId;
#else
  private string _adUnitId = "unused";
#endif

	private InterstitialAd _interstitialAd;

	/// <summary>
	/// Loads the interstitial ad.
	/// </summary>
	public void LoadInterstitialAd()
	{
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

				_interstitialAd = ad;

				RegisterEventHandlers(_interstitialAd);
				ShowInterstitialAd();
			});
	}

	/// <summary>
	/// Shows the interstitial ad.
	/// </summary>
	public void ShowInterstitialAd()
	{
		if (_interstitialAd != null && _interstitialAd.CanShowAd())
		{
			Debug.Log("Showing interstitial ad.");
			_interstitialAd.Show();
		}
		else
		{
			Debug.LogError("Interstitial ad is not ready yet.");
		}
	}

	[SerializeField] public UnityEvent OnAdClosed;

	private void RegisterEventHandlers(InterstitialAd interstitialAd)
	{
		// Raised when the ad is estimated to have earned money.
		interstitialAd.OnAdPaid += (AdValue adValue) =>
		{
			Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
				adValue.Value,
				adValue.CurrencyCode));
		};
		// Raised when an impression is recorded for an ad.
		interstitialAd.OnAdImpressionRecorded += () =>
		{
			Debug.Log("Interstitial ad recorded an impression.");
		};
		// Raised when a click is recorded for an ad.
		interstitialAd.OnAdClicked += () =>
		{
			Debug.Log("Interstitial ad was clicked.");
		};
		// Raised when an ad opened full screen content.
		interstitialAd.OnAdFullScreenContentOpened += () =>
		{
			Debug.Log("Interstitial ad full screen content opened.");
		};
		// Raised when the ad closed full screen content.
		interstitialAd.OnAdFullScreenContentClosed += () =>
		{
			Debug.Log("Interstitial ad full screen content closed.");
			OnAdClosed?.Invoke();
		};
		// Raised when the ad failed to open full screen content.
		interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
		{
			Debug.LogError("Interstitial ad failed to open full screen content " +
						   "with error : " + error);
		};
	}



	private void OnDisable()
	{
		_interstitialAd.Destroy();
	}

	private void OnEnable()
	{
		LoadInterstitialAd();
	}
}