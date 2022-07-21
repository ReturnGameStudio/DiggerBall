using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Manager<DataManager>
{
    public int Gem { get { return PlayerPrefs.GetInt("gem"); } set { PlayerPrefs.SetInt("gem", value); } }
    public int MachineLevel { get { return PlayerPrefs.GetInt("machineLevel"); } set { PlayerPrefs.SetInt("machineLevel", value); } }
    public int RollingTime { get { return PlayerPrefs.GetInt("Rolling Time"); } set { PlayerPrefs.SetInt("Rolling Time", value); } }
    private int currentMaxBlock { get { int level = LevelManager.Instance.currentLevel; level = level % 2 == 0 ? level - 1 : level; int max = level * 150; return max; } }
    public int Coin { get { return PlayerPrefs.GetInt("coin"); } set { PlayerPrefs.SetInt("coin", value); } }
    public int BlockCount { get { return PlayerPrefs.GetInt("blockCount"); } set { PlayerPrefs.SetInt("blockCount", value); } }
    private int totalBlocks;// { get { return PlayerPrefs.GetInt("totalBlocks"); } set { PlayerPrefs.SetInt("totalBlocks", value); } }


    private void Start()
    {
        AddGem(0);
        AddBlock(0);
    }



    public void AddGem(int amount)
    {
        Gem += amount;
        UIManager.Instance.UpdateGemText(Gem);
    }

    public int brokeBlock;


    public void AddBlock(int amount)
    {
        Coin += amount;
        BlockCount += amount;
        UIManager.Instance.UpdateCoinText(Coin);

        

        if (BlockCount >= currentMaxBlock)
        {

            if (MachineLevel > 0 && MachineLevel % 7 == 0) // 7
            {
                if (ShopManager.Instance.GetMinimumPrice() > Gem)
                {
                    if (UpgradeManager.Instance.GetMinimumPrice() > Coin) return;
                    UIManager.Instance.UpgradeActivate(true);
                }
                else
                {
                    UIManager.Instance.ShopActivate(true);
                }
            }
            else
            {
                if (UpgradeManager.Instance.GetMinimumPrice() > Coin) return;
                UIManager.Instance.UpgradeActivate(true);
            }

            Bulldozer b = FindObjectOfType<Bulldozer>();
            b.Busy(true);
            b.GoTopOrBottom(true);
        }
    }

    public void AddToTotalBlocks(int amount)
    {
        if (amount <= 0) return;
        totalBlocks += amount;

        if (totalBlocks >= LayerManager.Instance.BlockCountInLevel)
        {
            print("level complete");
            LayerManager.Instance.DestructAllLayers();
            GameManager.Instance.Win();
            totalBlocks = 0;
        }
    }

    public void ResetTotalBlocks()
    {
        BlockCount = 0;
        AddBlock(0);
    }

    public bool CheckAndSpendMoney(MoneyType moneyType, int price)
    {
        bool canPurchased = false;
        switch (moneyType)
        {
            case MoneyType.Gem:
                canPurchased = Gem >= price;
                if (canPurchased) AddGem(-price);
                break;

            case MoneyType.Coin:
                canPurchased = Coin >= price;
                if (canPurchased) AddBlock(-price);
                break;
        }
        return canPurchased;
    }
}
