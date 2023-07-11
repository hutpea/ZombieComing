using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleUI : MonoBehaviour
{
   private const int castle1MaxHourGold = 2;
   private const int castle2MaxHourGold = 3;
   private const int castle3MaxHourGold = 4;
   private const int castle4MaxHourGold = 5;
   public List<Button> claimCastleButtons;
   public List<TextMeshProUGUI> castleCoinTexts;
   public Button combatBtn;
   private int currentCastle1Gold;
   private int currentCastle2Gold;
   private int currentCastle3Gold;
   private int currentCastle4Gold;
   
   private void OnEnable()
   {
      if (GameData.CurrentCastleIndex == 3)
      {
         combatBtn.gameObject.SetActive(false);
      }
      System.TimeSpan timeSpanCastle1 = TimeManager.ParseTimeStartDay(UnbiasedTime.Instance.Now()) - TimeManager.ParseTimeStartDay(GameData.Castle1DateTime);
      System.TimeSpan timeSpanCastle2 = TimeManager.ParseTimeStartDay(UnbiasedTime.Instance.Now()) - TimeManager.ParseTimeStartDay(GameData.Castle2DateTime);
      System.TimeSpan timeSpanCastle3 = TimeManager.ParseTimeStartDay(UnbiasedTime.Instance.Now()) - TimeManager.ParseTimeStartDay(GameData.Castle3DateTime);
      System.TimeSpan timeSpanCastle4 = TimeManager.ParseTimeStartDay(UnbiasedTime.Instance.Now()) - TimeManager.ParseTimeStartDay(GameData.Castle4DateTime);

      Debug.Log(timeSpanCastle1);
      Debug.Log(timeSpanCastle2);
      Debug.Log(timeSpanCastle3);
      Debug.Log(timeSpanCastle4);
      
      if (GameData.CurrentCastleIndex >= 0)
      {
         if (timeSpanCastle1.TotalMinutes > 60)
         {
            currentCastle1Gold = (int)Mathf.Min((int)timeSpanCastle1.TotalHours, 24) * castle1MaxHourGold;
            claimCastleButtons[0].gameObject.SetActive(true);
            castleCoinTexts[0].SetText(currentCastle1Gold.ToString());
            claimCastleButtons[0].onClick.AddListener(delegate
            {
               OnClaimCastleGold(0);
            });
         }
         else
         {
            claimCastleButtons[0].gameObject.SetActive(false);
         }
      }
      else
      {
         claimCastleButtons[0].gameObject.SetActive(false);
      }

      if (GameData.CurrentCastleIndex >= 1)
      {
         if (timeSpanCastle2.TotalMinutes > 60)
         {
            currentCastle1Gold = (int)Mathf.Min((int)timeSpanCastle2.TotalHours, 24) * castle1MaxHourGold;
            claimCastleButtons[1].gameObject.SetActive(true);
            castleCoinTexts[1].SetText(currentCastle2Gold.ToString());
            claimCastleButtons[0].onClick.AddListener(delegate
            {
               OnClaimCastleGold(1);
            });
         }
         else
         {
            claimCastleButtons[1].gameObject.SetActive(false);
         }
      }
      else
      {
         claimCastleButtons[1].gameObject.SetActive(false);
      }
      
      if (GameData.CurrentCastleIndex >= 2)
      {
         if (timeSpanCastle3.TotalMinutes > 60)
         {
            currentCastle1Gold = (int)Mathf.Min((int)timeSpanCastle3.TotalHours, 24) * castle1MaxHourGold;
            claimCastleButtons[2].gameObject.SetActive(true);
            castleCoinTexts[2].SetText(currentCastle3Gold.ToString());
            claimCastleButtons[0].onClick.AddListener(delegate
            {
               OnClaimCastleGold(2);
            });
         }
         else
         {
            claimCastleButtons[2].gameObject.SetActive(false);
         }
      }
      else
      {
         claimCastleButtons[2].gameObject.SetActive(false);
      }
      
      if (GameData.CurrentCastleIndex >= 3)
      {
         if (timeSpanCastle4.TotalMinutes > 60)
         {
            currentCastle1Gold = (int)Mathf.Min((int)timeSpanCastle4.TotalHours, 24) * castle1MaxHourGold;
            claimCastleButtons[3].gameObject.SetActive(true);
            castleCoinTexts[3].SetText(currentCastle4Gold.ToString());
            claimCastleButtons[0].onClick.AddListener(delegate
            {
               OnClaimCastleGold(3);
            });
         }
         else
         {
            claimCastleButtons[3].gameObject.SetActive(false);
         }
      }
      else
      {
         claimCastleButtons[3].gameObject.SetActive(false);
      }
      
      combatBtn.onClick.AddListener(OnCombatBtnClicked);
      AudioManager.Instance.PlayEffect(SoundID.UITap);
   }

   private void OnDisable()
   {
      combatBtn.onClick.RemoveListener(OnCombatBtnClicked);
   }

   private void OnClaimCastleGold(int index)
   {
      switch (index)
      {
         case 0:
         {
            SaveManager.Currency += currentCastle1Gold;
            GameData.Castle1DateTime = UnbiasedTime.Instance.Now();
            break;
         }
         case 1:
         {
            SaveManager.Currency += currentCastle2Gold;
            GameData.Castle2DateTime = UnbiasedTime.Instance.Now();
            break;
         }
         case 2:
         {
            SaveManager.Currency += currentCastle3Gold;
            GameData.Castle3DateTime = UnbiasedTime.Instance.Now();
            break;
         }
         default:
         {
            SaveManager.Currency += currentCastle4Gold;
            GameData.Castle4DateTime = UnbiasedTime.Instance.Now();
            break;
         }
      }

      GameManager.Instance.gameMainMenuUI.currentGoldTxt.text = SaveManager.Currency.ToString();
      
      claimCastleButtons[index].gameObject.SetActive(false);
   }

   private void OnCombatBtnClicked()
   {
      GameManager.Instance.StartCastleMode();
   }
}
