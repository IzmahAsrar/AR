using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using Facebook.Unity;

public class GameplayScript_Handler : MonoBehaviour {

    [Header("Objective Dialoug Screen")]
    public GameObject objDialoug;
    public Text objText;
    [Header("Gameplay Buttons")]
    public GameObject[] gpButtons;
    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public float loadingScreenDisplayTime;
    [Header("Level Pause Screen")]
    public GameObject pauseScreen;
    [Header("Level Failed Screen")]
    public GameObject failedScreen;
    [Header("Level Complete Screen")]
    public GameObject completeScreen;

    [Header("Assign ind. level prefab.")]
    public GameObject[] Levels;

    [Header("Other Varaibles")]
    bool displayAdOnce, primaryObjectives;
    [SerializeField]
    bool levelCompleteTest, levelFailedTest;
    public int selectedLevel, unlockedLevel;
    public GameObject skipCutScene;
    public Text totalCoinsEarned, timerText;
    GameObject spawnedLevel;
    LevelModel_Handler currentLevelModel;
    int totalCoins;
    [Header("Rewarded Ad - Timer")]
    public GameObject rewardedAd;
    public Image spinnerImage;
    public float timeToBeAlotedAfterRewardedVideo;
    // ----------- Static Ref. of GamePlay Script Handler Start------------//
    public static GameplayScript_Handler gsh;
    // ----------- Static Ref. of GamePlay Script Handler End------------//

    void Awake()
    {
        Time.timeScale = 1f;
        if (gsh == null) {
            gsh = this;
        }
        hideButtons();
    }

    void Start()
    {
        selectedLevel = PlayerPrefs.GetInt("SelectedLevel");
#if !UNITY_EDITOR
			selectedLevel = PlayerPrefs.GetInt ("SelectedLevel");
			unlockedLevel = PlayerPrefs.GetInt ("TotalLevelsUnlocked");
#endif
        if (SoundManager._SoundManager) {
            SoundManager._SoundManager.playGameplaySounds(0.65f);
        }
        if (PlayerPrefs.GetInt("ComingFromGP", 0) == 0) {
            PlayerPrefs.SetInt("ComingFromGP", 1);
        }
        spawnLevel(selectedLevel);
        StartCoroutine(loadingScreenHandler());
    }

    void spawnLevel(int temp)
    {
        spawnedLevel = Instantiate(Levels[temp], transform.position, transform.rotation);
        currentLevelModel = spawnedLevel.GetComponent<LevelModel_Handler>();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseGameDialoug();
        }

#if UNITY_EDITOR
        if (levelCompleteTest) {
            levelCompleteTest = false;
            levelCompleteDialoug();
        }
        if (levelFailedTest) {
            levelFailedTest = false;
            levelFailedDialoug();
        }
#endif
    }

    public LevelModel_Handler returnLevelModelHandler()
    {
        return currentLevelModel;
    }

    IEnumerator loadingScreenHandler()
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(loadingScreenDisplayTime);
        loadingScreen.SetActive(false);
        if (currentLevelModel.containsCutScene) {
            hideButtons();
            StartCoroutine(currentLevelModel.cutSceneCalled());
            yield return new WaitForSeconds(currentLevelModel.cutSceneDuration / 2);
            skipCutScene.SetActive(true);
        }
        else {
            ObjectiveDialoug();
        }
    }

    public void ObjectiveDialoug()
    {
        if (!primaryObjectives) {
            primaryObjectives = true;
            objDialoug.SetActive(true);
            objText.text = currentLevelModel.primaryObjective;
        }
    }

    public void playButtonSound()
    {
        if (SoundManager._SoundManager) {
            SoundManager._SoundManager.playButtonClickSound();
        }
    }

    public void skipButtonPressed()
    {
        if (SoundManager._SoundManager) {
            SoundManager._SoundManager.playButtonClickSound();
        }
        currentLevelModel.skipCutScene = true;
    }

    public void hideButtons()
    {
        for (int i = 0; i < gpButtons.Length; i++) {
            gpButtons[i].SetActive(false);
        }
    }

    public void showButtons()
    {
        for (int i = 0; i < gpButtons.Length; i++) {
            gpButtons[i].SetActive(true);
        }
    }

    public void displayTimerScript()
    {
        returnLevelModelHandler().timerScript.gameObject.SetActive(true);
    }

    void HandleonRewardedVideoViewedSuccessfull()
    {
        currentLevelModel.timerScript.time += timeToBeAlotedAfterRewardedVideo;
        currentLevelModel.timerScript.stopTimer = false;
        currentLevelModel.timerScript.stopSpinner = false;
        StartCoroutine(currentLevelModel.timerScript.StartCoundownTimer());
        rewardedAd.SetActive(false);
        spinnerImage.fillAmount = 0;
    }

    public void pauseGameDialoug()
    {
        if (SoundManager._SoundManager) {
            SoundManager._SoundManager.playButtonClickSound();
        }
        pauseScreen.SetActive(true);      
    }

    IEnumerator delayedAdCallingPause()
    {
        yield return new WaitForSeconds(0.01f);       
        Time.timeScale = 0.01f;
    }

    public void resumeGameplay()
    {
        if (SoundManager._SoundManager) {
            SoundManager._SoundManager.playButtonClickSound();
        }
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void restartGame()
    {
        if (SoundManager._SoundManager) {
            SoundManager._SoundManager.playButtonClickSound();
        }
        loadingScreen.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void home()
    {
        if (SoundManager._SoundManager) {
            SoundManager._SoundManager.playButtonClickSound();
        }
        loadingScreen.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void levelFail_CompleteStatusEvent(bool temp)
    {
        StartCoroutine(levelFail_CompleteStatus(temp));
    }

    IEnumerator levelFail_CompleteStatus(bool temp)
    {
        hideButtons();
        yield return new WaitForSeconds(2f);
        if (temp) {
            levelCompleteDialoug();
        }
        else if (!temp) {
            levelFailedDialoug();
        }
    }

     public void levelCompleteDialoug()
    {
        completeScreen.SetActive(true);
        if (PlayerPrefs.GetInt("SelectedLevel") == PlayerPrefs.GetInt("TotalLevelsUnlocked") && PlayerPrefs.GetInt("TotalLevelsUnlocked") < Levels.Length - 1) {
            PlayerPrefs.SetInt("TotalLevelsUnlocked", PlayerPrefs.GetInt("TotalLevelsUnlocked") + 1);
            unlockedLevel = PlayerPrefs.GetInt("TotalLevelsUnlocked");
        }

        if (StoreScriptHandler.storeScript && StoreScriptHandler.storeScript.rewardPerLevels.Length > 0) {
            int temp = StoreScriptHandler.storeScript.getRewardOfLevel(selectedLevel);
            StoreScriptHandler.storeScript.setTotalEarnedCoins(temp + StoreScriptHandler.storeScript.getTotalEarnedCoins());
            totalCoins = StoreScriptHandler.storeScript.getTotalEarnedCoins();
            StartCoroutine(delayedTotalCoinAdder());
        }      
        showAds();
       
    }

    IEnumerator delayedTotalCoinAdder()
    {
        for (int i = 0; i < totalCoins;) {
            yield return new WaitForSeconds(0.01f);
            totalCoinsEarned.text = i.ToString();
            i += 25;
        }
    }

    void levelFailedDialoug()
    {
        failedScreen.SetActive(true);       
        showAds();
    }

    public void next()
    {
        if (SoundManager._SoundManager) {
            SoundManager._SoundManager.playButtonClickSound();
        }
        Time.timeScale = 1f;
        if (PlayerPrefs.GetInt("SelectedLevel") < Levels.Length - 1) {
            PlayerPrefs.SetInt("SelectedLevel", PlayerPrefs.GetInt("SelectedLevel") + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else {
            home();
        }
    }

    void showAds()
    {
        if (!displayAdOnce)
        {
            displayAdOnce = true;
            //if (AdsManagerHandler.adsManager)
            //{
            //    StartCoroutine(delayedShowAdCompleteFail());
            //}
        }
    }

    IEnumerator delayedShowAdCompleteFail()
    {
        yield return new WaitForSeconds(0.01f);
        //AdsManagerHandler.adsManager.showAd(1);
    }

}
