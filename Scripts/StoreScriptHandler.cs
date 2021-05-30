using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreScriptHandler : MonoBehaviour {
	
	[Header("Total Coins Earned")]
	[SerializeField]
	int totalEarnedCoins;
	[Header("Rewards Per Levels")]
	public int[] rewardPerLevels;
	[Header("Total Cost of the Product")]
	public int[] costOfProduct;

	// ------------ Store Script Static Ref. Start --------------//
	public static StoreScriptHandler storeScript;
	// ------------ Store Script Static Ref. End --------------//

	void Awake()
	{
		if (storeScript == null) {
			storeScript = this;
		} 
		else {
			Destroy (this.gameObject);	
		}
		DontDestroyOnLoad (gameObject);
	}

	public void firstTimeComing(int temp)
	{
		if (PlayerPrefs.GetInt("FirstTimeComing_1",0) == 0) {
			PlayerPrefs.SetInt ("FirstTimeComing_1", 1);
			setTotalEarnedCoins (temp);
		}
	}

	public void setTotalEarnedCoins(int temp)
	{
		totalEarnedCoins = temp;
		PlayerPrefs.SetInt ("TotalCoinsEarned", totalEarnedCoins);
	}

	public int getTotalEarnedCoins()
	{
		totalEarnedCoins = PlayerPrefs.GetInt ("TotalCoinsEarned",0);
		return totalEarnedCoins;
	}

	public int getRewardOfLevel(int temp)
	{
		return rewardPerLevels [temp];
	}

	public bool buyCurrentProduct(int temp)
	{
		totalEarnedCoins = PlayerPrefs.GetInt ("TotalCoinsEarned",0);
		if (totalEarnedCoins >= costOfProduct[temp]) {
			return true;
		}
		return false;
	}

	public int returnCostOfProduct(int temp)
	{
		return costOfProduct [temp];
	}

}
