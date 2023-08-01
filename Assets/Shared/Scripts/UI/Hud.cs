using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// This View contains head-up-display functionalities
    /// </summary>
    public class Hud : View
    {
        public
        TextMeshProUGUI m_GoldText;
        public Transform m_GoldIconTransform;
        [SerializeField]
        Slider m_XpSlider;
        [SerializeField]
        HyperCasualButton m_PauseButton;
        [SerializeField]
        AbstractGameEvent m_PauseEvent;

        /// <summary>
        /// The slider that displays the XP value 
        /// </summary>
        public Slider XpSlider => m_XpSlider;

        int m_GoldValue;
        
        /// <summary>
        /// The amount of gold to display on the hud.
        /// The setter method also sets the hud text.
        /// </summary>
        public int GoldValue
        {
            get => m_GoldValue;
            set
            {
                if (m_GoldValue != value)
                {
                    m_GoldValue = value;
                    //m_GoldText.text = GoldValue.ToString();
                    m_GoldText.text = SaveManager.Currency.ToString();
                }
            }
        }

        float m_XpValue;
        
        /// <summary>
        /// The amount of XP to display on the hud.
        /// The setter method also sets the hud slider value.
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

        void OnEnable()
        {
            m_GoldText.text = SaveManager.Currency.ToString();
            m_PauseButton.AddListener(OnPauseButtonClick);
            m_PauseButton.gameObject.SetActive(true);
        }

        void OnDisable()
        {
            m_PauseButton.RemoveListener(OnPauseButtonClick);
            m_PauseButton.gameObject.SetActive(false);
        }

        void OnPauseButtonClick()
        {
            m_PauseEvent.Raise();
            UIManager.Instance.Show<PauseMenu>();
        }
    }
}
