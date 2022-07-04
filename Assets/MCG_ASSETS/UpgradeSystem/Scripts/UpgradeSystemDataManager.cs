/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystemDataManager : Manager<UpgradeSystemDataManager>
{
    public int Coin { get { return PlayerPrefs.GetInt("coin"); } set { PlayerPrefs.SetInt("coin", value); } }

    public void AddCoin(int amount)
    {
        Coin += amount;
        UpgradeSystemUIManager.Instance.UpdateCoinText(Coin); 
    }

}
*/