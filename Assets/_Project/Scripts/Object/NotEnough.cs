using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotEnough : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    private void Update()
    {
        image1.gameObject.SetActive(UpgradeManager.Instance.GetFeature("Speed").CurrentPrice>DataManager.Instance.Block);
        image2.gameObject.SetActive(UpgradeManager.Instance.GetFeature("Size").CurrentPrice>DataManager.Instance.Block);
        image3.gameObject.SetActive(UpgradeManager.Instance.GetFeature("Damage").CurrentPrice>DataManager.Instance.Block);
        image4.gameObject.SetActive(UpgradeManager.Instance.GetFeature("Rolling Time").CurrentPrice>DataManager.Instance.Block);
    }
}
