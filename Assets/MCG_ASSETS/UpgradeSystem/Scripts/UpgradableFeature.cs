
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

[System.Serializable]
public class UpgradableFeature
{

    public UnityEvent MyRollingTime;
    [Header("Variables")]
    public string Name;
    public Sprite Icon;
    public int StartLevel;
    public MoneyType _MoneyType;
    public int MaxLevel;

    public int CurrentLevel
    {
        get
        {
            int level = PlayerPrefs.GetInt(Name); return level == 0 ? StartLevel : level;
        }
        set
        {
            PlayerPrefs.SetInt(Name, value);
        }
    }

    public int PriceMultiplier;
    public int CurrentPrice { get { return (CurrentLevel) * PriceMultiplier; } }

    public bool IsMax { get { return CurrentLevel == MaxLevel; } }
    
    
    public void Init()
    {
        if (CurrentLevel == 0) CurrentLevel = StartLevel;
       // Debug.Log(Name + " level: " + CurrentLevel);
    }
    public void Upgrade()
    {
        MyRollingTime?.Invoke();
        if (IsMax) return;
        if (CurrentLevel < MaxLevel)
        {
            if (DataManager.Instance.CheckAndSpendMoney(_MoneyType,CurrentPrice))
            {
                CurrentLevel++;
                UpgradeManager.Instance.CallEvents();
            }
        }
    }
    public void Reset()
    {
        CurrentLevel = 0;
        Init();
    }
}
