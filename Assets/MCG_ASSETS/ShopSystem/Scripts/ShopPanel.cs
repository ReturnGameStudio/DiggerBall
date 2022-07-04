using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Text Name;
    [SerializeField] private Image Icon;
   
    [SerializeField] private Text PriceText;
    [SerializeField] private Text SelectText;
    [SerializeField] private GameObject BuyButton;

    private int _id;
    public void InitializeUI(ShopItem item)
    {
        _id = item.Id;
        Name.text = item.Name;
        PriceText.text = item.Price.ToString();
        Icon.sprite = item.Icon;    
        BuyButton.SetActive(!item.IsPurchased);
    }

    public void UpdateSelectText(string text) => SelectText.text = text;

    public void SelectMe() => ShopManager.Instance.SelectItem(_id);
    public void BuyMe() => ShopManager.Instance.BuyItem(_id);

}
