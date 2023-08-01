using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameMainMenuUI : MonoBehaviour
{
    public GameMainMenuUI Instance;
    public ShopSkinUI shopSkinUI;
    public DailyRewardUI dailyRewardUI;
    
    public Button addStartZomBtn;
    public Button addGoldLvZomBtn;
    public Button addZombieHealthBtn;
    public Button changeZomSkinBtn;
    public Button startPlayBtn;
    public TextMeshProUGUI currentStartZomTxt;
    public TextMeshProUGUI currentGoldLvTxt;
    public TextMeshProUGUI currentZombieHealthTxt;
    
    public TextMeshProUGUI currentStartZomValueTxt;
    public TextMeshProUGUI currentGoldLvValueTxt;
    public TextMeshProUGUI currentZombieHealthValueTxt;
    
    public TextMeshProUGUI currentStartZomCostTxt;
    public TextMeshProUGUI currentGoldLvCostTxt;
    public TextMeshProUGUI currentZombieHealthCostTxt;
    
    public TextMeshProUGUI currentGoldTxt;
    public TextMeshProUGUI levelTxt;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        addStartZomBtn.onClick.AddListener(OnStartZomBuy);
        addGoldLvZomBtn.onClick.AddListener(OnGoldLvBuy);
        addZombieHealthBtn.onClick.AddListener(OnHealthZomBuy);
        changeZomSkinBtn.onClick.AddListener(OnZombieSkinChanged);
        startPlayBtn.onClick.AddListener(StartPlayBtn);
        ShowShop();
    }

    private void OnEnable()
    {
        ShowShop();
    }

    public void ShowShop()
    {
        
        currentGoldTxt.text = SaveManager.Currency.ToString();
        
        currentStartZomTxt.text = "LEVEL " + GameData.StartZombieLevel.ToString();
        currentGoldLvTxt.text = "LEVEL " + GameData.LevelCoinMultiplierLevel.ToString();
        currentZombieHealthTxt.text = "LEVEL " + GameData.ZombieMaxHealthLevel.ToString();
        
        currentStartZomValueTxt.text = GameData.StartZombie.ToString();
        currentGoldLvValueTxt.text = GameData.LevelCoinMultiplier.ToString();
        currentZombieHealthValueTxt.text = GameData.ZombieMaxHealth.ToString();
        
        currentStartZomCostTxt.text = GameData.StartZombieCost.ToString();
        currentGoldLvCostTxt.text = GameData.LevelCoinMultiplierCost.ToString();
        currentZombieHealthCostTxt.text = GameData.ZombieMaxHealthCost.ToString();
    }

    public void OnStartZomBuy()
    {
        if (SaveManager.Currency >= GameData.StartZombieCost)
        {
            Debug.Log("buy zom");
            SaveManager.Currency -= GameData.StartZombieCost;
            GameData.StartZombieLevel += 1;
            GameData.StartZombieCost += 150;
            GameData.StartZombie += 1;
            GameData.StartZombie = Mathf.Min(GameData.StartZombie, 50);
            currentStartZomTxt.text = "LEVEL " + GameData.StartZombieLevel.ToString();
            currentStartZomValueTxt.text = GameData.StartZombie.ToString();
            currentStartZomCostTxt.text = GameData.StartZombieCost.ToString();
            currentGoldTxt.text = SaveManager.Currency.ToString();
            PlayerController.Instance.ChangeZombieStat();
            
            AudioManager.Instance.PlayEffect(SoundID.UpgradeSkillSound);
        }
    }

    public void OnGoldLvBuy()
    {
        if (SaveManager.Currency >= GameData.LevelCoinMultiplierCost)
        {
            Debug.Log("buy gold lv");
            SaveManager.Currency -= GameData.LevelCoinMultiplierCost;
            GameData.LevelCoinMultiplierLevel += 1;
            GameData.LevelCoinMultiplierCost += 150;
            GameData.LevelCoinMultiplier += 0.1f;
            currentGoldLvTxt.text = "LEVEL " + GameData.LevelCoinMultiplierLevel.ToString();
            currentGoldLvValueTxt.text = GameData.LevelCoinMultiplier.ToString();
            currentGoldLvCostTxt.text = GameData.LevelCoinMultiplierCost.ToString();
            currentGoldTxt.text = SaveManager.Currency.ToString();
            
            AudioManager.Instance.PlayEffect(SoundID.UpgradeSkillSound);
        }
    }

    public void OnHealthZomBuy()
    {
        if (SaveManager.Currency >= GameData.ZombieMaxHealthCost)
        {
            Debug.Log("buy health zom");
            SaveManager.Currency -= GameData.ZombieMaxHealthCost;
            GameData.ZombieMaxHealthLevel += 1;
            GameData.ZombieMaxHealthCost += 150;
            GameData.ZombieMaxHealth += 2;
            currentZombieHealthTxt.text = "LEVEL " + GameData.ZombieMaxHealthLevel.ToString();
            currentZombieHealthValueTxt.text = GameData.ZombieMaxHealth.ToString();
            currentZombieHealthCostTxt.text = GameData.ZombieMaxHealthCost.ToString();
            currentGoldTxt.text = SaveManager.Currency.ToString();
            PlayerController.Instance.ChangeZombieStat();
            
            AudioManager.Instance.PlayEffect(SoundID.UpgradeSkillSound);
        }
    }

    public void OnZombieSkinChanged()
    {
        shopSkinUI.gameObject.SetActive(true);
    }

    public void StartPlayBtn()
    {
        PlayerController.Instance.EnablePlay();
        this.gameObject.SetActive(false);
    }

    public void OnSettingButtonClicked()
    {
        UIManager.Instance.Show<SettingsMenu>();
    }

    public void OnDailyRewardClicked()
    {
        dailyRewardUI.gameObject.SetActive(true);
    }
}