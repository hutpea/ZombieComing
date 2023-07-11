using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// This View contains shop menu functionalities
    /// </summary>
    public class ShopView : View
    {
        [SerializeField]
        HyperCasualButton m_Button;

        /*public Button addStartZomBtn;
        public Button addGoldLvZomBtn;
        public Button addZombieHealthBtn;
        public Text currentStartZomTxt;
        public Text currentGoldLvTxt;
        public Text currentZombieHealthTxt;
        public Text currentGoldTxt;*/

        void OnEnable()
        {
            m_Button.AddListener(OnButtonClick);
            /*addStartZomBtn.onClick.AddListener(OnStartZomBuy);
            addGoldLvZomBtn.onClick.AddListener(OnGoldLvBuy);
            addZombieHealthBtn.onClick.AddListener(OnHealthZomBuy);
            currentGoldTxt.text = "GOLD: " + SaveManager.Currency.ToString();
            currentStartZomTxt.text = GameData.StartZombie.ToString();
            currentGoldLvTxt.text = GameData.LevelCoinMultiplier.ToString();
            currentZombieHealthTxt.text = GameData.ZombieMaxHealth.ToString();*/
        }

        void OnDisable()
        {
            m_Button.RemoveListener(OnButtonClick);
            /*addStartZomBtn.onClick.RemoveListener(OnStartZomBuy);
            addGoldLvZomBtn.onClick.RemoveListener(OnGoldLvBuy);
            addZombieHealthBtn.onClick.RemoveListener(OnHealthZomBuy);*/
        }

        void OnButtonClick()
        {
            UIManager.Instance.GoBack();
        }

        /*public void OnStartZomBuy()
        {
            if (SaveManager.Currency >= 10)
            {
                SaveManager.Currency -= 10;
                GameData.StartZombie += 1;
                GameData.StartZombie = Mathf.Min(GameData.StartZombie, 6);
                currentStartZomTxt.text = GameData.StartZombie.ToString();
                currentGoldTxt.text = "GOLD: " + SaveManager.Currency.ToString();
            }
        }
        
        public void OnGoldLvBuy()
        {
            if (SaveManager.Currency >= 10)
            {
                SaveManager.Currency -= 10;
                GameData.LevelCoinMultiplier += 0.1f;
                currentGoldLvTxt.text = GameData.LevelCoinMultiplier.ToString();
                currentGoldTxt.text = "GOLD: " + SaveManager.Currency.ToString();
            }
        }
        
        public void OnHealthZomBuy()
        {
            if (SaveManager.Currency >= 10)
            {
                SaveManager.Currency -= 10;
                GameData.ZombieMaxHealth += 2;
                currentZombieHealthTxt.text = GameData.ZombieMaxHealth.ToString();
                currentGoldTxt.text = "GOLD: " + SaveManager.Currency.ToString();
            }
        }*/
    }
}