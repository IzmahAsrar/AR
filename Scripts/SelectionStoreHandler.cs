using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionStoreHandler : MonoBehaviour {

	[Header("Objects to be Selected")]
	public GameObject[] objectSelected;
	public int currentSelected;
	[Header("Assign Camera Looking At player")]
	public GameObject cameraLookingAtPlayer;
	[Header("Left Right Button")]
	public GameObject leftButton;
	public GameObject rightButton;
	[Header("Select Player")]
	public GameObject selectPlayer;
	[Header("Buy Player")]
	public GameObject buyPlayer;
	public GameObject costOfPlayer,notEnoughCoins;
	public Text costOfPlayerText;
    bool crosspromotion;
    int temp;

	void Start()
	{
		if (PlayerPrefs.GetInt ("UnlockedPlayer0",0) == 0) {
			PlayerPrefs.SetInt ("UnlockedPlayer0", 1);
		}
		delayedLeftRightState();
	}

	void delayedLeftRightState()
	{
		if (currentSelected < objectSelected.Length - 1) {
			leftButton.SetActive (true);
			rightButton.SetActive (true);
		}
		if (currentSelected == 0) {
			leftButton.SetActive (false);
		}
		if (currentSelected == objectSelected.Length - 1) {
			rightButton.SetActive (false);
		}
		checkUnlockedState ();
	}

	void checkUnlockedState()
	{
		if (PlayerPrefs.GetInt ("UnlockedPlayer"+currentSelected,0) == 1) {
			costOfPlayer.SetActive (false);
			buyPlayer.SetActive (false);
			selectPlayer.SetActive (true);
		} 
		else {
			costOfPlayer.SetActive (true);
			buyPlayer.SetActive (true);
			selectPlayer.SetActive (false);
			costOfPlayerText.text = StoreScriptHandler.storeScript.returnCostOfProduct (currentSelected).ToString ();
		}
	}

	public void leftRightState(bool temp)
	{
		objectSelected [currentSelected].SetActive (false);
		if (temp) {
			currentSelected++;
		}
		else if (!temp) {
			currentSelected--;
		}
        if (!crosspromotion)
        {
            crosspromotion = true;
            //AdsManagerHandler.adsManager.showCrosspromotionAd();
        }
		delayedLeftRightState();
		objectSelected [currentSelected].SetActive (true);
        iTween.MoveTo(cameraLookingAtPlayer, new Vector3(objectSelected[currentSelected].transform.position.x, 1, -3), 3f);
    }
	
	public void buyProduct()
	{
		if (StoreScriptHandler.storeScript.buyCurrentProduct(currentSelected)) {
			temp = StoreScriptHandler.storeScript.getTotalEarnedCoins () - StoreScriptHandler.storeScript.returnCostOfProduct (currentSelected);
			StoreScriptHandler.storeScript.setTotalEarnedCoins (temp);
			costOfPlayer.SetActive (false);
			buyPlayer.SetActive (false);
			selectPlayer.SetActive (true);
			PlayerPrefs.SetInt ("UnlockedPlayer"+currentSelected,1);

			MainMenuScriptHandler.mmsh.updateCoins ();
		} 
		else {
			notEnoughCoins.SetActive (true);
		}
	}

	public void selectedPlayer()
	{
		PlayerPrefs.SetInt ("currentSelectedPlayer", currentSelected);
		MainMenuScriptHandler.mmsh.showLevelSelection ();
	}
}
