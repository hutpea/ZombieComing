using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ShopSkinUI : MonoBehaviour
{
    public List<Button> shopButtons;
    public List<Image> lockImages;
    public List<Image> outlineImages;

    public Button equipBtn;
    public Button buyBtn;
    public Button watchAdBtn;
    public TextMeshProUGUI equipBtnTxt;
    public TextMeshProUGUI buyCostTxt;
    private int index;

    private void OnEnable()
    {
        index = 0;
        equipBtn.onClick.AddListener(OnEquipButtonClicked);
        buyBtn.onClick.AddListener(OnBuyButtonClicked);
        watchAdBtn.onClick.AddListener(OnWatchButtonClicked);
        for (int i = 0; i < shopButtons.Count; i++)
        {
            outlineImages[i].enabled = false;
            SkinItemData currentSelectSkinItemData = GameData.GameSkinData.skinItemDatas[i];
            if (currentSelectSkinItemData.owned)
            {
                lockImages[i].enabled = false;
            }
            else
            {
                lockImages[i].enabled = true;
            }

            int tmpI = i;
            shopButtons[i].onClick.AddListener(() => OnShopButtonClicked(tmpI));
        }

        OnShopButtonClicked(GameData.SelectZombieSkin);
        AudioManager.Instance.PlayEffect(SoundID.UITap);
    }

    public void OnShopButtonClicked(int i)
    {
        foreach (var outlineImg in outlineImages)
        {
            outlineImg.enabled = false;
        }
        index = i;
        outlineImages[index].enabled = true;
        SkinItemData currentSelectSkinItemData = GameData.GameSkinData.skinItemDatas[index];
        if (currentSelectSkinItemData.owned)
        {
            lockImages[index].enabled = false;
            equipBtn.gameObject.SetActive(true);
            buyBtn.gameObject.SetActive(false);
            watchAdBtn.gameObject.SetActive(false);
            if (i != GameData.SelectZombieSkin)
            {
                equipBtn.interactable = true;
                equipBtnTxt.SetText("Equip");
            }
            else
            {
                equipBtn.interactable = false;
                equipBtnTxt.SetText("Equipped");
            }
        }
        else
        {
            lockImages[index].enabled = true;
            equipBtn.gameObject.SetActive(false);
            buyBtn.gameObject.SetActive(true);
            watchAdBtn.gameObject.SetActive(true);
            buyCostTxt.SetText(currentSelectSkinItemData.cost.ToString());
        }
    }

    public void OnEquipButtonClicked()
    {
        equipBtn.interactable = false;
        equipBtnTxt.SetText("Equipped");
        GameData.SelectZombieSkin = index;
        PlayerController.Instance.ChangeZombieSkin();
    }

    public void OnBuyButtonClicked()
    {
        if (GameData.GameSkinData.skinItemDatas[index].owned)
        {
            return;
        }
        else
        {
            if (SaveManager.Currency >= GameData.GameSkinData.skinItemDatas[index].cost)
            {
                GameSkinsData tmpGameData = new GameSkinsData();
                var tmpSkinData = GameData.GameSkinData.skinItemDatas;
                tmpGameData.skinItemDatas = tmpSkinData;
                tmpGameData.skinItemDatas[index].owned = true;
                GameData.GameSkinData = tmpGameData;
                //string convertedData = Newtonsoft.Json.JsonConvert.SerializeObject(defaultData);

                SaveManager.Currency -= GameData.GameSkinData.skinItemDatas[index].cost;
                GameManager.Instance.gameMainMenuUI.currentGoldTxt.text = SaveManager.Currency.ToString();

                lockImages[index].enabled = false;
                equipBtn.gameObject.SetActive(true);
                equipBtn.interactable = false;
                buyBtn.gameObject.SetActive(false);
                watchAdBtn.gameObject.SetActive(false);
                equipBtnTxt.SetText("Equipped");
                GameData.SelectZombieSkin = index;
                PlayerController.Instance.ChangeZombieSkin();
            }
            else
            {
                //Not enough money
            }
        }
    }

    public void OnWatchButtonClicked()
    {
        SaveManager.Currency += 100;
        GameManager.Instance.gameMainMenuUI.currentGoldTxt.text = SaveManager.Currency.ToString();
    }

    [Button]
    public void CreateItem()
    {
        List<SkinItemData> skinItemDatas = new List<SkinItemData>();
        for (int i = 0; i < 4; i++)
        {
            SkinItemData skinItemData = new SkinItemData();
            string name = "Purple Pant Zom";
            int cost = 0;
            switch (i)
            {
                case 1:
                {
                    name = "Red Hat Zom";
                    cost = 100;
                    break;
                }
                case 2:
                {
                    name = "Astronaut Zom";
                    cost = 200;
                    break;
                }
                case 3:
                {
                    name = "Skeleton Zom";
                    cost = 300;
                    break;
                }
            }

            skinItemData.name = name;
            skinItemData.index = i;
            skinItemData.cost = cost;
            skinItemData.owned = false;
            skinItemDatas.Add(skinItemData);
        }

        GameSkinsData gameSkinsData = new GameSkinsData();
        gameSkinsData.skinItemDatas = skinItemDatas;

        GameData.GameSkinData = new GameSkinsData();
        GameData.GameSkinData = gameSkinsData;
    }
}