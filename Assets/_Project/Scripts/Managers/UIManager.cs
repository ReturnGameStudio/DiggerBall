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
    [SerializeField] private GameObject BarPanel;
    [SerializeField] private GameObject CheatPanel;

    [Header("Texts")]
    [SerializeField] private Text LevelText;
    [SerializeField] private Text GemText;
    [SerializeField] private Text BlockText;

    [Header("Images")]
    [SerializeField] private Image BlockBar;
    [SerializeField] private Image BallColorUI;

    [Header("Bar Items")]
    [SerializeField] private GameObject[] barItems;


    public void MenuActivate(bool activate) => MenuPanel.SetActive(activate);
    public void GameActivate(bool activate) => GamePanel.SetActive(activate);
    public void LoseActivate(bool activate) => LosePanel.SetActive(activate);
    public void WinActivate(bool activate) => WinPanel.SetActive(activate);
    public void UpgradeActivate(bool activate) { UpgradePanel.SetActive(activate); }
    public void ShopActivate(bool activate) => ShopPanel.SetActive(activate);
    public void BarActivate(bool activate) => BarPanel.SetActive(activate);
    public void UpdateLevelText(int level) => LevelText.text = "LEVEL  " + level;
    public void UpdateGemText(int value) { if (GemText != null) GemText.text = value.ToString(); }

    public void UpdateBlockText(int current, int max)
    {
        BlockText.text = current + "";
    }
    public void UpdateUpgradeBar(int max)
    {
        foreach (GameObject g in barItems)
            g.SetActive(false);

        for (int i = 0; i < max; i++)
            barItems[i].SetActive(true);
    }

    [SerializeField] private Text text;
    [SerializeField] private Image slide;
    [SerializeField] private Image warning;
    public IEnumerator Fuel()
    {
        UpgradableFeature feature = UpgradeManager.Instance.GetFeature("Rolling Time");
        var time = feature.CurrentLevel * 15.0f;

        
        var constant = slide.fillAmount;
        if (slide.fillAmount >= .9f)
        {
            constant /= 1.5f;
        }
        while (slide.fillAmount > 0)
        {
            slide.fillAmount -= Time.deltaTime / time * constant;
            if (slide.fillAmount < .075f && !warning.gameObject.activeSelf)
            {
                warning.gameObject.SetActive(true);
            }
            yield return null;
        }

        var brokenBlock = DataManager.Instance.MyBlockCount;
        var allBlock = LayerManager.Instance.BlockCountInLevel;
        var result = Mathf.Round(brokenBlock * 100f / allBlock);

        Debug.Log(brokenBlock+"     "+allBlock+"    "+result); 
        text.text = result + "%";

        GameManager.Instance.GameOver();

    }


    private IEnumerator Start()
    {
        yield return new WaitUntil(() => UpgradeManager.Instance != null);
        UpgradeManager.Instance.GetFeature("Rolling Time").MyRollingTime.AddListener(CreaseEnergy);
        slide.fillAmount = UpgradeManager.Instance.GetFeature("Rolling Time").CurrentLevel / 10.0f;
        DataManager.Instance.MyBlockCount = 0;
    }


    //------test
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) CheatPanel.SetActive(!CheatPanel.activeSelf);
    }
    public void AddGem() => DataManager.Instance.AddGem(1000);
    public void AddBlock() => DataManager.Instance.AddBlock(1000);
    public void OpenShop() { UpgradeActivate(true); }*/

    //------

    private void CreaseEnergy()
    {
        StartCoroutine(CreaseEnergyAction());
    }


    private IEnumerator CreaseEnergyAction()
    {
        yield return null;
        var a = UpgradeManager.Instance.GetFeature("Rolling Time").CurrentLevel;
        slide.fillAmount = UpgradeManager.Instance.GetFeature("Rolling Time").CurrentLevel / 10.0f;
    }

    public void StartBarAction()
    {
        MyCameraChange.OnChangeCamera?.Invoke();
        StartCoroutine(BarPanel.GetComponent<StartBar>().StartBarAction());
    }


    public void CloseUpgradeBarBtn()
    {
        UpgradeActivate(false);
    }


    public void ChangeBallColorUI(Color c)
    {
        BallColorUI.color = c;
    }

    public void CloseUpgradeBar()
    {
        GameLoop.Instance.BulldozerNotBusy();
        DataManager.Instance.ResetTotalBlocks();
    }

    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
    }

}
