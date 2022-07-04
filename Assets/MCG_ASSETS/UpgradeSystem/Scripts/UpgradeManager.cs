
using System;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : Manager<UpgradeManager>
{

    [Header("Upgradable Features")]
    public UpgradableFeature[] Features;

    [Space(10)]
    [Header("Events")]
    public UnityEvent UpgradeAction;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < Features.Length; i++)
        {
            Features[i].Init();

            if (UpgradeSystemUIManager.Instance.Panels[i] != null)
                UpgradeSystemUIManager.Instance.Panels[i].Initialize(Features[i]);
        }
    }

    public UpgradableFeature GetFeature(string name)
    {
        UpgradableFeature uf = new UpgradableFeature();
        for (int i = 0; i < Features.Length; i++)
        {
            if (Features[i].Name == name)
            {
                uf = Features[i];
                break;
            }
        }
        return uf;
    }

    bool _gameStarted;
    public void CallEvents()
    {
        if (!_gameStarted)
        {
        UIManager.Instance.BarActivate(true);
            _gameStarted = true;
        }

        UpgradeAction?.Invoke();
    }

    public void Reset(string featureName)
    {
        GetFeature(featureName).Reset();
        Initialize();
    }

}
