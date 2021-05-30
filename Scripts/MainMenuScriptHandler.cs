using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using Facebook.Unity;

public class MainMenuScriptHandler : MonoBehaviour {

	[Header("Loading Screen")]
	public GameObject loadingImage;
	public float loadingDelayTime;

	[Header("Main Menu Stuff")]
	public GameObject mainMenu;
	public GameObject settingsDialoug,removeAdsButton,unlockAllButton,restore;
	public Image soundButton;
	public Sprite soundOn,SoundOff;
	bool soundState = true;

	[Header("Level Selection Stuff")]
	public GameObject levelSelection;
	bool inLevelSelection;
	[Header("Levels Parent")]
	[Tooltip("Just Assign the parent the levels array will be populated automatically")]
	public GameObject levelsParent;
	GameObject[] Levels;
	[SerializeField]
	int totalLevelsUnlocked;
	[Header("Player Selection Stuff")]
	public GameObject selectionScene;
	bool inSelectionScene;

	[Header("Text Fields to Display User Coins")]
	public Text[] userCoins;

    static bool crossPromotion = false;
    static bool backToMainMenu = false;

    [Header("Clear Saved Prefs - Usable only on Editor")]
	public bool clearData;
	// ----------- Static Ref. of Main Menu Handler Start------------//
	public static MainMenuScriptHandler mmsh;
	// ----------- Static Ref. of Main Menu Handler End------------//


	void Start()
	{
		if (mmsh == null) {
			mmsh = this;
		}
		#if UNITY_EDITOR
		if (clearData) {
			PlayerPrefs.DeleteAll();
		}
		#endif
		Time.timeScale = 1f;
		Levels = new GameObject[levelsParent.transform.childCount];
		for (int i = 0; i < Levels.Length; i++) {
			Levels [i] = levelsParent.transform.GetChild (i).gameObject;
		}

		SoundManager._SoundManager.playMainMenuSounds (1f);
		//if (AdsManagerHandler.adsManager.areAdsRemoved())
  //      {
  //          removeAdsButton.SetActive(false);
  //      }
#if UNITY_ANDROID
        restore.SetActive(false);
#elif UNITY_IPHONE
        restore.SetActive(true);
#endif
        if (PlayerPrefs.GetInt ("TotalLevelsUnlocked") >= Levels.Length - 1) {
			unlockAllButton.SetActive (false);
		}
		StartCoroutine (showLoadingImage());
	}

    public void ShowPrivacyPolicy()
    {
        //Application.OpenURL(AdsManagerHandler.adsManager.privacyPolicy());
    }

    public void restorePurchases()
    {
        //InAppManager.inAppManager.RestorePurchases();
    }

	IEnumerator showLoadingImage()
	{
		loadingImage.SetActive (true);
		yield return new WaitForSeconds (loadingDelayTime);
		loadingImage.SetActive (false);
		if (backToMainMenu && !crossPromotion) {
            crossPromotion = true;
            backToMainMenu = false;
            //AdsManagerHandler.adsManager.showAd(0);
        }
        else
        {
            crossPromotion = false;
            //AdsManagerHandler.adsManager.showCrosspromotionAd();
        }
		updateCoins ();
	}

	public void updateCoins()
	{
        for (int i = 0; i < userCoins.Length; i++)
        {
            userCoins[i].text = StoreScriptHandler.storeScript.getTotalEarnedCoins().ToString();
        }
	}

	void OnEnable()
	{
        //InAppManager.onPurchaseWasSucessFull += InAppPurchaseManager_PurchaseSucessFull;
    }

	void OnDisable()
	{
       // InAppManager.onPurchaseWasSucessFull -= InAppPurchaseManager_PurchaseSucessFull;
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && (!inLevelSelection || !inSelectionScene)) {
            //AdsManagerHandler.adsManager.showExitMenu();
        }
		if (Input.GetKeyDown(KeyCode.Escape) && (inLevelSelection || inSelectionScene)) {
			// In Main Menu Now exit the game
			backFromLevelSelection();
		}
	}

	public void showLevelSelection()
	{
		inLevelSelection = true;
		inSelectionScene = false;
		initLevelSelection ();
		mainMenu.SetActive (false);
		levelSelection.SetActive (true);
		selectionScene.SetActive (false);
        //AdsManagerHandler.adsManager.showAd(0);
	}

	public void showSelectionScene()
	{
		inLevelSelection = false;
		inSelectionScene = true;
		selectionScene.SetActive (true);
		mainMenu.SetActive (false);
		levelSelection.SetActive (false);
	}

	public void PlayButtonClickSound()
	{
		SoundManager._SoundManager.playButtonClickSound ();
	}

	public void moreAppsButtonClicked()
	{
        //AdsManagerHandler.adsManager.openMoreApps();
    }

	public void rateUsApp()
	{
        //AdsManagerHandler.adsManager.showRateUsDialoug();
    }

	public void backFromLevelSelection()
	{
		if (inLevelSelection)
        {
			inLevelSelection = false;
			showSelectionScene ();
		}
		else if (inSelectionScene)
        {
			inSelectionScene = false;
			selectionScene.SetActive (false);
			mainMenu.SetActive (true);
		}
        //AdsManagerHandler.adsManager.showCrosspromotionAd();
    }

	void initLevelSelection()
	{
		totalLevelsUnlocked = PlayerPrefs.GetInt ("TotalLevelsUnlocked");
		for (int i = 0; i <= totalLevelsUnlocked; i++) {
			Levels [i].transform.GetChild (0).gameObject.SetActive (false);
		}
	}

	public void selectLevel(int temp)
	{
		if (!Levels [temp].transform.GetChild (0).gameObject.activeSelf) {
            backToMainMenu = true;
            loadingImage.SetActive (true);
			PlayerPrefs.SetInt ("SelectedLevel", temp);
			SceneManager.LoadScene (1);
		}
	}
		
	void InAppPurchaseManager_PurchaseSucessFull (string temp)
	{
        //if (InAppManager.inAppManager.inApps[0].id.Equals(temp))
        {
			//AdsManagerHandler.adsManager.removeAllAds();
            removeAdsButton.SetActive(false);
            //AdsManagerHandler.adsManager.recordUnityLogs("User Purchased InApp","Remove All Ads",0);
        }
      //  if (InAppManager.inAppManager.inApps[1].id.Equals(temp))
        {
            PlayerPrefs.SetInt("TotalLevelsUnlocked", Levels.Length - 1);
            initLevelSelection();
            unlockAllButton.SetActive(false);
            //AdsManagerHandler.adsManager.recordUnityLogs("User Purchased InApp", "Unlock All Levels", 0);
        }
    }

	public void removeAds()
	{
        //InAppManager.inAppManager.BuyInApp(InAppManager.inAppManager.inApps[0].id);
    }

	public void unlockLevels()
	{
        //InAppManager.inAppManager.BuyInApp(InAppManager.inAppManager.inApps[1].id);
    }
    
}
