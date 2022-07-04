
using UnityEngine;

public class ShopUIManager : Manager<ShopUIManager>
{   
    [Header("Panels")]
    [SerializeField] private ShopPanel[] Panels;
    public void UpdatePanels(ShopItem[] items, int selectedId)
    {
        if (Panels.Length != items.Length) return;
        for (int i = 0; i < items.Length; i++)
        {
            Panels[i].UpdateSelectText(i == selectedId ? "SELECTED" : "CHANGE");
            Panels[i].InitializeUI(items[i]);
        }
    }
  
}
