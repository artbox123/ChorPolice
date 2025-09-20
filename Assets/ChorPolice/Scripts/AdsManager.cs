using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace ArtboxGames
{
    public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public static AdsManager Instance;

        private BannerView bannerView;
        private AdRequest banner_request;

        private InterstitialAd interstitial;
        private AdRequest interstitial_request;

        private RewardedAd rewardedAd;
        private AdRequest rewarded_request;

        private AppOpenAd ad;
        private bool isShowingAd = false;
        private DateTime loadTime;

        private Action getReward;
        private string gameId = "4085737";
        private string interstitialAdUnitId_Unity = "video";
        private string rewardAdUnitId_Unity = "rewardedVideo";
        private int clickCount = 0;

        private VariablesManager vars;

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        // Start is called before the first frame update
        public void Start()
        {
            Advertisement.Initialize(gameId, false, this);  

            // Listen to application foreground and background events.
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

            // Configure TagForChildDirectedTreatment and test device IDs.
            RequestConfiguration requestConfiguration =
                new RequestConfiguration.Builder().build();

            MobileAds.SetRequestConfiguration(requestConfiguration);
            MobileAds.SetiOSAppPauseOnBackground(true);

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(HandleInitCompleteAction);            

            getReward = GetReward;
        }

        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            // Callbacks from GoogleMobileAds are not guaranteed to be called on
            // main thread.
            // In this example we use MobileAdsEventExecutor to schedule these calls on
            // the next Update() loop.
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                RequestBanner();
                RequestInterstitial();
                RequestRewarded();
                LoadAppOpenAd();                
            });
        }

        private void RequestBanner()
        {
            // Create a 320x50 banner at the top of the screen.
            AdSize adSize = new AdSize(340, 50);
            this.bannerView = new BannerView(vars.adMobBannerID, AdSize.Banner, AdPosition.Bottom);

            // Create an empty ad request.
            banner_request = new AdRequest.Builder().Build();

            // Called when an ad request has successfully loaded.
            this.bannerView.OnAdLoaded += this.HandleBannerAdLoaded;
            // Called when an ad request failed to load.
            this.bannerView.OnAdFailedToLoad += this.HandleBannerAdFailedToLoad;
            // Called when an ad is clicked.
            this.bannerView.OnAdOpening += this.HandleBannerAdOpened;
            // Called when the user returned from the app after an ad click.
            this.bannerView.OnAdClosed += this.HandleBannerAdClosed;

            // Load the banner with the request.
            this.bannerView.LoadAd(banner_request);
        }

        private void RequestInterstitial()
        {
            // Initialize an InterstitialAd.
            this.interstitial = new InterstitialAd(vars.adMobInterstitialID);

            // Called when an ad request has successfully loaded.
            this.interstitial.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is shown.
            this.interstitial.OnAdOpening += HandleOnAdOpened;
            // Called when the ad is closed.
            this.interstitial.OnAdClosed += HandleOnAdClosed;

            // Create an empty ad request.
            this.interstitial_request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this.interstitial.LoadAd(interstitial_request);
        }

        private void RequestRewarded()
        {
            this.rewardedAd = new RewardedAd(vars.admobRewardID);
            // Called when an ad request has successfully loaded.
            this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            // Called when the ad is closed.
            this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

            // Create an empty ad request.
            rewarded_request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            this.rewardedAd.LoadAd(rewarded_request);
        }

        private void LoadAppOpenAd()
        {
            AdRequest request = new AdRequest.Builder().Build();

            // Load an app open ad for portrait orientation
            AppOpenAd.LoadAd(vars.admobAppOpenID, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
            {
                if (error != null)
                {
                    // Handle the error.
                    Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());
                    return;
                }

                // App open ad is loaded.
                ad = appOpenAd;
                loadTime = DateTime.UtcNow;
            }));
        }

        private bool IsAdAvailable
        {
            get
            {
                return ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;
            }
        }

        public void ShowAdIfAvailable()
        {
            if (!IsAdAvailable || isShowingAd)
            {
                return;
            }

            ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
            ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
            ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
            ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
            ad.OnPaidEvent += HandlePaidEvent;

            ad.Show();
        }

        // Returns an ad request with custom ad targeting.
        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
                    .AddKeyword("game")
                    .Build();
        }

        private void OnAppStateChanged(AppState state)
        {
            // Display the app open ad when the app is foregrounded.
            //Debug.Log("======= App State is " + state);
            if (state == AppState.Foreground)
            {
                ShowAdIfAvailable();
            }
        }

        public bool ShowRewardVideo()
        {
            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
                return true;
            }
            else
            {
                RequestRewarded();
                return ShowUnityRewardedVideo();
            }
        }

        public void ShowInterstitial()
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
            else
            {
                RequestInterstitial();
                ShowUnityInterstitialAd();
            }
        }

        public void HandleRewardedAdLoaded(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardedAdLoaded event received");
        }

        public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            MonoBehaviour.print(
                "HandleRewardedAdFailedToLoad event received with message: "
                                 + args.LoadAdError);
        }

        public void HandleRewardedAdOpening(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardedAdOpening event received");
        }

        public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
        {
            MonoBehaviour.print(
                "HandleRewardedAdFailedToShow event received with message: "
                                 + args.AdError);
        }

        public void HandleRewardedAdClosed(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardedAdClosed event received");
            RequestRewarded();
        }

        public void HandleUserEarnedReward(object sender, Reward args)
        {
            string type = args.Type;
            double amount = args.Amount;
            MonoBehaviour.print(
                "HandleRewardedAdRewarded event received for "
                            + amount.ToString() + " " + type);
            getReward?.Invoke();
        }

        // interstitial
        public void HandleOnAdLoaded(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdLoaded event received");
        }

        public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                                + args.LoadAdError);
        }

        public void HandleOnAdOpened(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdOpened event received");
        }

        public void HandleOnAdClosed(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdClosed event received");
            RequestInterstitial();
        }

        public void HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdLeavingApplication event received");
        }

        // banner
        public void HandleBannerAdLoaded(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleBannerAdLoaded event received");
            bannerView.Show();
        }

        public void HandleBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            MonoBehaviour.print("HandleBannerAdFailedToLoad event received with message: "
                                + args.LoadAdError);
        }

        public void HandleBannerAdOpened(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleBannerAdOpened event received");
        }

        public void HandleBannerAdClosed(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleBannerAdClosed event received");
        }

        public void HandleBannerAdLeavingApplication(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleBannerAdLeavingApplication event received");
        }
        // app open

        private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
        {
            Debug.Log("Closed app open ad");
            // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
            ad = null;
            isShowingAd = false;
            LoadAppOpenAd();
        }

        private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
        {
            Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
            // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
            ad = null;
            LoadAppOpenAd();
        }

        private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
        {
            Debug.Log("Displayed app open ad");
            isShowingAd = true;
        }

        private void HandleAdDidRecordImpression(object sender, EventArgs args)
        {
            Debug.Log("Recorded ad impression");
        }

        private void HandlePaidEvent(object sender, AdValueEventArgs args)
        {
            Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                    args.AdValue.CurrencyCode, args.AdValue.Value);
        }
        // ------------------------------------------------

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
            LoadUnityInterstitialAd();
            LoadUnityRewaredAd();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        public void LoadUnityInterstitialAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Advertisement.Load(interstitialAdUnitId_Unity);
        }

        // Show the loaded content in the Ad Unit: 
        public void ShowUnityInterstitialAd()
        {
            Advertisement.Show(interstitialAdUnitId_Unity);
        }

        public void LoadUnityRewaredAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Advertisement.Load(rewardAdUnitId_Unity);
        }

        public bool ShowUnityRewardedVideo()
        {
            Advertisement.Load(rewardAdUnitId_Unity);
            return true;
        }

        // Implement Load Listener and Show Listener interface methods:  
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            // Optionally execute code if the Ad Unit successfully loads content.       
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
        }

        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(interstitialAdUnitId_Unity))
            {
                LoadUnityInterstitialAd();
            }
            else if (adUnitId.Equals(rewardAdUnitId_Unity))
            {
                getReward?.Invoke();
                LoadUnityRewaredAd();
            }
        }

        private void GetReward()
        {
            GameManager.instance.coins += 50;
            UIManager.instance.totalStarCount.text = GameManager.instance.coins.ToString();
        }

        public void ShowFullScreen()
        {
            clickCount++;
            if (clickCount >= 5)
            {
                ShowInterstitial();
                clickCount = 0;
            }
        }
    }
}