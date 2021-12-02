using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GoogleAdMob : MonoBehaviour
{
    [SerializeField]
    PlaySpeed playSpeed;

    [SerializeField]
    TextMeshProUGUI textTest;
    private List<string> deviceIDs = new List<string>();

    private BannerView bannerView;
    //private const string bannerTestId = "ca-app-pub-3940256099942544/630097811";
    private const string bannerId = "ca-app-pub-7422891816852048/4218110678";

    private RewardedAd rewardedAd;
    private const string rewardTestId = "ca-app-pub-3940256099942544/5224354917";

    private RewardedInterstitialAd rewardedInterstitialAd;
    private const string rewardedInterstitialId = "ca-app-pub-7422891816852048/2381095450";
    //private const string rewardedInterstitialTestId = "ca-app-pub-3940256099942544/5354046379";

    public RewardedAd RewardedAd => rewardedAd;
    public RewardedInterstitialAd RewardedInterstitialAd => rewardedInterstitialAd;
    private void Start()
    {

        RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize(HandleInitComplete);
    }
    private void HandleInitComplete(InitializationStatus initStatus)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            textTest.text = "Initialization complete";
            RequestBannerAd();
            CreateRewardedAd();
            RequestAndLoadRewardedInterstitialAd();
        });
    }
    #region Banner ADS
    private void RequestBannerAd()
    {
        textTest.text = "Requesting Banner Ad";
        if(bannerView != null)
        {
            bannerView.Destroy();
        }
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.BottomLeft);
        // Test
        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

        bannerView.LoadAd(CreateAdRequest());
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        textTest.text = "HandleAdLoaded event received";
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        textTest.text = "HandleFailedToReceiveAd event received with message: " + args.Message;
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        textTest.text = "HandleAdOpened event received";
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        textTest.text = "HandleAdClosed event received";
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        textTest.text = "HandleAdLeavingApplication event received";
    }
    #endregion

    #region Rewarded Ads
    public void CreateRewardedAd()
    {
        rewardedAd = new RewardedAd(rewardTestId);

        rewardedAd.OnUserEarnedReward += HandleOnUserEarnedReward;
        rewardedAd.OnAdClosed += HandleOnRewardedAdClosed;

        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd()
    {
        if(rewardedAd != null)
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet");
        }
    }

    public void HandleOnUserEarnedReward(object sender, EventArgs args)
    {
        playSpeed.watchedAd = true;
    }

    public void HandleOnRewardedAdClosed(object sender, EventArgs args)
    {
        CreateRewardedAd();
    }
    #endregion

    #region Rewarded interstitial Ads
    public void RequestAndLoadRewardedInterstitialAd()
    {
        RewardedInterstitialAd.LoadAd(rewardedInterstitialId, CreateAdRequest(), (rewardedInterstitialAd, error) =>
        {
            if(error != null)
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Debug.Log("RewardedInterstitialAd load failed, error: " + error);
                });
                return;
            }
            else
            {
                this.rewardedInterstitialAd = rewardedInterstitialAd;
            }

            this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    Debug.Log("Rewarded Interstitial dismissed.");
                });
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    Debug.Log("Rewarded Interstitial failed to present.");
                });
                this.rewardedInterstitialAd = null;
            };
        });
    }

    public void ShowRewardedInterstitialAd()
    {
        if(rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show((reward) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    playSpeed.OnWatchedAd();
                });
            });
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }
    #endregion

    #region Helper Methods
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            //.AddTestDevice(AdRequest.TestDeviceSimulator)
            .Build();
    }
    #endregion
}
