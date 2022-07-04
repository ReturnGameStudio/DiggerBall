using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : Manager<GameLoop>
{
    Bulldozer bulldozer;
    private void Start()
    {
        UpgradeBuldozer();
    }
    public void UpgradeBuldozer()
    {
        if (bulldozer == null) bulldozer = GameManager.Instance.Player.GetComponent<Bulldozer>();
        bulldozer.InitProperties();
    }

    public void DeactivePanels()
    {
        //UIManager.Instance.UpgradeActivate(false);
        //UIManager.Instance.ShopActivate(false);
    }
    public void AddToMachineLevel() => DataManager.Instance.MachineLevel++;
    public void ResetBlocks() => DataManager.Instance.ResetTotalBlocks();

    public void BulldozerNotBusy()
    {
        //UIManager.Instance.StartBarAction();
        //bulldozer.Busy(false); 
       // bulldozer.GoTopOrBottom(false);
    }

    //------ test
#if UNITY_EDITOR
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            DeactivePanels();
            BulldozerNotBusy();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.UpgradeActivate(true);
            bulldozer.Busy(true);
            bulldozer.GoTopOrBottom(true);
        }

    }
    public void LevelUpEffect()
    {
        bulldozer.GetComponent<PlayerFXController>().LevelUp();
    }
#endif
    //------------------
}
