using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class UpgradePanelManager : MonoBehaviour
    {
        [SerializeField] private GameObject featurePanel;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject shopPanelBtn;
        [SerializeField] private GameObject featurePanelBtn;
        [SerializeField] private GameObject indi;


        private void Start()
        {
            SetFeaturePanel();
            indi.gameObject.SetActive(DataManager.Instance.Gem > ShopManager.Instance.GetPrice());
        }



        public void SetFeaturePanel()
        {
            featurePanel.SetActive(true);
            shopPanel.SetActive(false);
            featurePanelBtn.GetComponent<Outline>().effectColor = Color.green;
            shopPanelBtn.GetComponent<Outline>().effectColor = Color.white;
        }

        public void SetShopPanel()
        {
            featurePanel.SetActive(false);
            shopPanel.SetActive(true);
            featurePanelBtn.GetComponent<Outline>().effectColor = Color.white;
            shopPanelBtn.GetComponent<Outline>().effectColor = Color.green;
            indi.gameObject.SetActive(false);

        }
       
        public void StartGame()
        {
          
                UIManager.Instance.StartBarAction();
          
            gameObject.SetActive(false);
        }
    }
}
