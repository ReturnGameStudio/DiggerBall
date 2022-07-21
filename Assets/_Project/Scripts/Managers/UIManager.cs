using System;
using System.Collections;
using _Project.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject LosePanel;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject UpgradePanel;
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private GameObject CheatPanel;

    [Header("Texts")]
    [SerializeField] private Text LevelText;
    [SerializeField] private Text GemText;
    [SerializeField] private Text CoinText;

    public void MenuActivate(bool activate) => MenuPanel.SetActive(activate);
    public void GameActivate(bool activate) => GamePanel.SetActive(activate);
    public void LoseActivate(bool activate) => LosePanel.SetActive(activate);
    public void WinActivate(bool activate) => WinPanel.SetActive(activate);
    public void UpgradeActivate(bool activate) { UpgradePanel.SetActive(activate); }
    public void ShopActivate(bool activate) => ShopPanel.SetActive(activate);

    public void UpdateLevelText(int level) => LevelText.text = "LEVEL  " + level;
    public void UpdateGemText(int value) { if (GemText != null) GemText.text = value.ToString(); }

    public void UpdateCoinText(int current)
    {
        CoinText.text = current + "";
    }
  

    //------test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) CheatPanel.SetActive(!CheatPanel.activeSelf);
    }
    public void AddGem() => DataManager.Instance.AddGem(1000);
    public void AddBlock() => DataManager.Instance.AddBlock(1000);
    public void OpenShop() { UpgradeActivate(true); }

    //------


    public void CloseUpgradePanels()
    {
        GameLoop.Instance.ResetBlocks();
        GameLoop.Instance.DeactivePanels();
        GameLoop.Instance.UpgradeBuldozer();
        GameLoop.Instance.BulldozerNotBusy();
    }



    public void Play() => GameManager.Instance.Play();
    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
    }

}
