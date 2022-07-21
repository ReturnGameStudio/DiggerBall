using UnityEngine;
using UnityEngine.Events;

public class ShopManager : Manager<ShopManager>
{
    [SerializeField] private ShopItem[] items;

    [SerializeField] private UnityEvent SelectActions;
    private void Start()
    {
        UpdateShopItems();
        FindNextItem();
    }

    public void SelectItem(int id)
    {
        PlayerPrefs.SetInt("selected_shop_item", id);
        UpdateShopItems();
        SelectActions?.Invoke();
    }

    private ShopItem nextItem;
    public void BuyItem(int id)
    {
        ShopItem item = items[id];

        if (DataManager.Instance.CheckAndSpendMoney(item.MoneyType, item.Price))
        {
            UpgradeManager.Instance.Reset("Damage");
            item.IsPurchased = true;
            SelectItem(id);
        }
    }

    public float GetPrice()
    {
        if (nextItem == null) return 99999;
        return nextItem.Price;
    }

    private void UpdateShopItems()
    {
        int selected = PlayerPrefs.GetInt("selected_shop_item");
        PlayerCosmetic.Instance.SelectAccessory(selected);
        ShopUIManager.Instance.UpdatePanels(items, selected);

    }

    private void FindNextItem()
    {
        foreach (var item in items)
        {
            if (!item.IsPurchased)
            {
                nextItem = item;
                break;
            }
        }
    }

    public int GetMinimumPrice()
    {
        int price = items[items.Length - 1].Price;
        foreach (var i in items)
        {
            if (i.IsPurchased) continue;
            if (i.Price < price)
                price = i.Price;
        }
        return price;
    }

    //test------
    public void BuyNextItem()
    {
        FindNextItem();
        if (nextItem.Id > items.Length - 1) nextItem = items[0];
        DataManager.Instance.AddGem(nextItem.Price);
        BuyItem(nextItem.Id);
    }
    //------
}
