using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// This View contains celebration screen functionalities
    /// </summary>
    public class LevelCompleteScreen : View
    {
        public TextMeshProUGUI m_LevelText;
        [SerializeField]
        HyperCasualButton m_NextButton;
        [SerializeField]
        Image[] m_Stars;
        [SerializeField]
        AbstractGameEvent m_NextLevelEvent;
        [SerializeField]
        RectTransform m_GoldPanel;
        [SerializeField]
        TextMeshProUGUI m_GoldText;
        [SerializeField]
        Slider m_XpSlider;

        [SerializeField] private Button bonusBrainBtn;
        [SerializeField] private BonusPointer bonusPointer;
        [SerializeField] private ParticleSystem confettiFX;

        //[SerializeField] private GameObject bonusWindowPanel;
        //[SerializeField] private TextMeshProUGUI bonusWindowPanel_YouGotTxt;
        //[SerializeField] private TextMeshProUGUI bonusWindowPanel_ValueTxt;
        //[SerializeField] private Button bonusWindowPanel_OKBtn;
        [SerializeField] private Button continueAfterGetBonusBtn;
        public List<Transform> brainCoins;
        public RectTransform targetPos;
        
        /// <summary>
        /// The slider that displays the XP value 
        /// </summary>
        public Slider XpSlider => m_XpSlider;

        int m_GoldValue;
        
        /// <summary>
        /// The amount of gold to display on the celebration screen.
        /// The setter method also sets the celebration screen text.
        /// </summary>
        public int GoldValue
        {
            get => m_GoldValue;
            set
            {
                if (m_GoldValue != value)
                {
                    m_GoldValue = value;
                    m_GoldText.text = GoldValue.ToString();
                }
            }
        }

        float m_XpValue;
        
        /// <summary>
        /// The amount of XP to display on the celebration screen.
        /// The setter method also sets the celebration screen slider value.
        /// </summary>
        public float XpValue
        {
            get => m_XpValue;
            set
            {
                if (!Mathf.Approximately(m_XpValue, value))
                {
                    m_XpValue = value;
                    m_XpSlider.value = m_XpValue;
                }
            }
        }

        int m_StarCount = -1;
        
        /// <summary>
        /// The number of stars to display on the celebration screen.
        /// </summary>
        public int StarCount
        {
            get => m_StarCount;
            set
            {
                if (m_StarCount != value)
                {
                    m_StarCount = value;
                    DisplayStars(m_StarCount);
                }
            }
        }

        private void WaitDisplay()
        {
            m_NextButton.gameObject.SetActive(true);
            Debug.Log("m_NextButton Show");
        }

        private IEnumerator DelayGoldTitle()
        {
            yield return new WaitForSeconds(0.75f);
            m_GoldPanel.DOScale(1f, 0.5f).SetEase(Ease.InOutQuad).OnComplete(delegate
            {
                AudioManager.Instance.PlayEffect(SoundID.ScoreCollect);
            });
        }
        
        private IEnumerator Confetti()
        {
            if (ConfettiCanvas.Instance != null)
            {
                ConfettiCanvas.Instance.TurnOn(CameraManager.Instance.transform);
                yield return new WaitForSeconds(2f);
                ConfettiCanvas.Instance.TurnOff();
            }
        }

        void OnEnable()
        {
            Debug.Log("Level Complete Show");
            m_NextButton.gameObject.SetActive(false);
            continueAfterGetBonusBtn.gameObject.SetActive(false);
            m_NextButton.AddListener(OnNextButtonClicked);
            bonusBrainBtn.onClick.AddListener(OnBonusButtonClicked);
            //bonusWindowPanel_OKBtn.onClick.AddListener(OnOKBtnClicked);
            continueAfterGetBonusBtn.onClick.AddListener(OnNextButtonClicked);
            ToggleBonusButton(true);
            Invoke("WaitDisplay", 2F);
            //Instantiate(confettiFX);
            m_GoldPanel.localScale = Vector3.zero;
            StartCoroutine(DelayGoldTitle());
            StartCoroutine(Confetti());
            //bonusWindowPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
            m_LevelText.SetText("LEVEL " + GameManager.Instance.m_CurrentLevel.LevelIndex);
            enableUpdateTextBasedOnPointer = true;
            foreach (var bCoin in brainCoins)
            {
                bCoin.transform.localPosition = Vector3.zero;
                bCoin.gameObject.SetActive(false);
            }
        }

        void OnDisable()
        {
            m_NextButton.RemoveListener(OnNextButtonClicked);
            bonusBrainBtn.onClick.RemoveListener(OnBonusButtonClicked);
            //bonusWindowPanel_OKBtn.onClick.RemoveListener(OnOKBtnClicked);
            continueAfterGetBonusBtn.onClick.RemoveListener(OnNextButtonClicked);
        }

        void OnNextButtonClicked()
        {
            m_NextLevelEvent.Raise();
            UIManager.Instance.GetView<LevelCompleteScreen>().gameObject.SetActive(false);
        }

        void OnBonusButtonClicked()
        {
            enableUpdateTextBasedOnPointer = false;
            
            //bonusWindowPanel.GetComponent<RectTransform>().DOScale(1f, 0.5f).SetEase(Ease.InOutQuad);
            int bonusValue = (int)(bonusPointer.GetLevelMultiplier() * (float)GoldValue - (float)GoldValue);
            Debug.Log("Bonus: " + bonusValue);
            int calculatedValue = SaveManager.Currency + bonusValue;
            SaveManager.Currency += bonusValue;
            GameManager.Instance.gameMainMenuUI.currentGoldTxt.text = SaveManager.Currency.ToString();
            ToggleBonusButton(false);
            m_GoldText.SetText((bonusValue + GoldValue).ToString());
            //bonusWindowPanel.SetActive(true);
            //bonusWindowPanel_YouGotTxt.SetText("You got " + bonusPointer.GetLevelMultiplier().ToString("F1") + " bonuses!");
            //bonusWindowPanel_ValueTxt.SetText(bonusValue.ToString());

            AudioManager.Instance.PlayEffect(SoundID.ScoreCollect);
            StartCoroutine(BrainCoinsEffect(calculatedValue));
        }

        private void ContinueButtonAppear()
        {
            continueAfterGetBonusBtn.GetComponent<RectTransform>().localScale = Vector3.zero;
            continueAfterGetBonusBtn.gameObject.SetActive(true);
            continueAfterGetBonusBtn.GetComponent<RectTransform>().DOScale(1, 0.5f);
        }

        void OnOKBtnClicked()
        {
            //bonusWindowPanel.SetActive(false);
            OnNextButtonClicked();
        }
        
        public void ToggleBonusButton(bool value)
        {
            bonusBrainBtn.gameObject.SetActive(value);
        }

        void DisplayStars(int count)
        {
            count = Mathf.Clamp(count, 0, m_Stars.Length);

            if (m_Stars.Length > 0 && count >= 0 && count <= m_Stars.Length)
            {
                for (int i = 0; i < m_Stars.Length; i++)
                {
                    m_Stars[i].gameObject.SetActive(i < count);
                }
            }
        }

        private IEnumerator BrainCoinsEffect(int value)
        {
            brainCoins[0].gameObject.SetActive(true);
            brainCoins[0].DOJump(Inventory.Instance.m_Hud.m_GoldIconTransform.position, 1f, 1, 0.75f).OnComplete(delegate
            {
                Inventory.Instance.m_Hud.m_GoldText.SetText(value.ToString());
                brainCoins[0].gameObject.SetActive(false);
            });
            yield return new WaitForSeconds(0.33f);
            brainCoins[1].gameObject.SetActive(true);
            brainCoins[1].DOJump(Inventory.Instance.m_Hud.m_GoldIconTransform.position, 1f, 1, 0.75f).OnComplete(delegate
            {
                brainCoins[1].gameObject.SetActive(false);
            });
            yield return new WaitForSeconds(0.33f);
            brainCoins[2].gameObject.SetActive(true);
            brainCoins[2].DOJump(Inventory.Instance.m_Hud.m_GoldIconTransform.position, 1f, 1, 0.75f).OnComplete(delegate
            {
                brainCoins[2].gameObject.SetActive(false);
            });
            yield return new WaitForSeconds(0.42f);
            ContinueButtonAppear();
        }
        
        private bool enableUpdateTextBasedOnPointer = true;
        private void Update()
        {
            if (!enableUpdateTextBasedOnPointer) return;
            m_GoldText.SetText(((int)(bonusPointer.GetLevelMultiplier() * (float)GoldValue)).ToString());
        }
    }
}
