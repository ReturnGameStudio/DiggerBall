
using UnityEngine;
using UnityEngine.Events;

public enum MoneyType { Gem, Coin }
[System.Serializable]
public class ShopItem
{
    public int Id;
    public string Name;
    public Sprite Icon;
    public MoneyType MoneyType;
    public int Price;
    [SerializeField] private bool IsFree;
    public bool IsPurchased
    {
        get { if (IsFree) return true; else return PlayerPrefs.GetInt("shopItem_" + Id) == 1; }
        set { PlayerPrefs.SetInt("shopItem_" + Id, value ? 1 : 0); }
    }
  
}
