using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DailyRewardUI : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public RectTransform lastDayRect;
    public List<Button> dailyRewardBtns;
    public List<Image> bgImages;
    public List<Image> iconImages;
    public List<Image> checkImages;
    public List<Image> notifyImages;
    public List<Image> outlineImages;
    public List<TextMeshProUGUI> btnTxt;

    public Material grayscaleMaterial;
    public Sprite greenBtnSprite;
    public Sprite grayBtnSprite;
    //public Sprite coinSprite;
    //public Sprite checkSprite;
    private void Awake()
    {
        float realDailyUIWidth = GetComponent<RectTransform>().rect.width;
        Debug.Log(realDailyUIWidth);
        float cellWidth = realDailyUIWidth / 5.25f;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellWidth);
        Rect rect = lastDayRect.rect;
        rect.width = cellWidth * 1.35f;
        for(int i = 0; i < dailyRewardBtns.Count; i++)
        {
            int index = i;
            dailyRewardBtns[i].onClick.AddListener(() => OnDailyRewardBtnClicked(index));
        }
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable dailyreward");
        System.TimeSpan timeSpanDaily = TimeManager.ParseTimeStartDay(UnbiasedTime.Instance.Now()) - TimeManager.ParseTimeStartDay(GameData.GetDateTimeDailyReward());
        Debug.Log(timeSpanDaily.Days);
        if (timeSpanDaily.TotalDays >= 1)
        {
            GameData.DailyDayIndex = Mathf.Min(GameData.DailyDayIndex + 1, 6);
            for(int i = 0; i < dailyRewardBtns.Count; i++)
            {
                if (i == GameData.DailyDayIndex - 1)
                {
                    dailyRewardBtns[i].interactable = true;
                    bgImages[i].material = null;
                    iconImages[i].material = null;
                    checkImages[i].enabled = false;
                    notifyImages[i].enabled = true;
                    outlineImages[i].enabled = true;
                    btnTxt[i].SetText("TO CLAIM");
                }
                else if(i < GameData.DailyDayIndex - 1)
                {
                    dailyRewardBtns[i].interactable = false;
                    bgImages[i].material = grayscaleMaterial;
                    iconImages[i].material = grayscaleMaterial;
                    checkImages[i].enabled = true;
                    notifyImages[i].enabled = false;
                    outlineImages[i].enabled = false;
                    btnTxt[i].SetText("CLAIMED");
                }
                else if(i > GameData.DailyDayIndex - 1)
                {
                    dailyRewardBtns[i].interactable = false;
                    bgImages[i].material = null;
                    iconImages[i].material = null;
                    checkImages[i].enabled = false;
                    notifyImages[i].enabled = false;
                    outlineImages[i].enabled = false;
                    btnTxt[i].SetText("DAY " + (i+1).ToString());
                }
            }
        }
        else
        {
            for(int i = 0; i < dailyRewardBtns.Count; i++)
            {
                if (i == GameData.DailyDayIndex - 1)
                {
                    dailyRewardBtns[i].interactable = false;
                    bgImages[i].material = grayscaleMaterial;
                    iconImages[i].material = grayscaleMaterial;
                    checkImages[i].enabled = true;
                    notifyImages[i].enabled = false;
                    outlineImages[i].enabled = true;
                    btnTxt[i].SetText("CLAIMED");
                }
                else if(i < GameData.DailyDayIndex - 1)
                {
                    dailyRewardBtns[i].interactable = false;
                    bgImages[i].material = grayscaleMaterial;
                    iconImages[i].material = grayscaleMaterial;
                    checkImages[i].enabled = true;
                    notifyImages[i].enabled = false;
                    outlineImages[i].enabled = false;
                    btnTxt[i].SetText("CLAIMED");
                }
                else if(i > GameData.DailyDayIndex - 1)
                {
                    dailyRewardBtns[i].interactable = false;
                    bgImages[i].material = null;
                    iconImages[i].material = null;
                    checkImages[i].enabled = false;
                    notifyImages[i].enabled = false;
                    outlineImages[i].enabled = false;
                    btnTxt[i].SetText("DAY " + (i+1).ToString());
                }
            }
        }
        
        AudioManager.Instance.PlayEffect(SoundID.UITap);
    }

    public void OnDailyRewardBtnClicked(int index)
    {
        dailyRewardBtns[index].interactable = false;
        bgImages[index].material = grayscaleMaterial;
        iconImages[index].material = grayscaleMaterial;
        checkImages[index].enabled = true;
        notifyImages[index].enabled = false;
        outlineImages[index].enabled = true;
        btnTxt[index].SetText("CLAIMED");
        
        SaveManager.Currency += 100;
        GameManager.Instance.gameMainMenuUI.currentGoldTxt.text = SaveManager.Currency.ToString();
        GameData.SetDateTimeDailyQuest(UnbiasedTime.Instance.Now());
        
        AudioManager.Instance.PlayEffect(SoundID.ScoreCollect);
    }
}
