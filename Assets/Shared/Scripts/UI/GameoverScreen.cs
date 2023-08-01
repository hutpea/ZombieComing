using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// This View contains Game-over Screen functionalities
    /// </summary>
    public class GameoverScreen : View
    {
        [SerializeField]
        HyperCasualButton m_PlayAgainButton;
        [SerializeField]
        HyperCasualButton m_GoToMainMenuButton;
        [SerializeField]
        AbstractGameEvent m_PlayAgainEvent;
        [SerializeField]
        AbstractGameEvent m_GoToMainMenuEvent;

        public AudioSource audioSource;
        
        void OnEnable()
        {
            m_PlayAgainButton.AddListener(OnPlayAgainButtonClick);
            m_GoToMainMenuButton.AddListener(OnGoToMainMenuButtonClick);
            audioSource.Play();
        }

        void OnDisable()
        {
            m_PlayAgainButton.RemoveListener(OnPlayAgainButtonClick);
            m_GoToMainMenuButton.RemoveListener(OnGoToMainMenuButtonClick);
            audioSource.Stop();
        }

        void OnPlayAgainButtonClick()
        {
            SaveManager.Currency += Inventory.Instance.GetTempGold();
            Debug.Log(Inventory.Instance.GetTempGold());
            Hide();
            m_PlayAgainEvent.Raise();
        }

        void OnGoToMainMenuButtonClick()
        {
            SaveManager.Currency += Inventory.Instance.GetTempGold();

            Hide();
            m_GoToMainMenuEvent.Raise();
        }
    }
}