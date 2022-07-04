using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private Text TitleText;
    [SerializeField] private Text LevelText;
    [SerializeField] private Image LevelBar;
    [SerializeField] private Text PriceText;
    [SerializeField] private Image Icon;
    [SerializeField] private Button UpgradeButton;

    private UpgradableFeature myFeature;
    public void Initialize(UpgradableFeature feature)
    {
        myFeature = feature;
        UpdateTexts();
    }
    public void Upgrade()
    {
        myFeature.Upgrade();
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        TitleText.text = myFeature.Name;
        if (LevelText != null) LevelText.text = myFeature.CurrentLevel.ToString();
        if (LevelBar != null) LevelBar.fillAmount = (float)myFeature.CurrentLevel / myFeature.MaxLevel;
        PriceText.text = !myFeature.IsMax ? (myFeature.CurrentPrice > 0 ? myFeature.CurrentPrice.ToString() : "UPGRADE") : "MAX";
        Icon.sprite = myFeature.Icon;
        UpgradeButton.interactable = !myFeature.IsMax;
    }

}
