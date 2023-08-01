using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// This View contains pause menu functionalities
    /// </summary>
    public class PauseMenu : View
    {
        [SerializeField]
        HyperCasualButton m_ContinueButton;

        [SerializeField]
        HyperCasualButton m_QuitButton;

        [SerializeField]
        AbstractGameEvent m_ContinueEvent;

        [SerializeField]
        AbstractGameEvent m_QuitEvent;

        void OnEnable()
        {
            Time.timeScale = 0f;
            m_ContinueButton.AddListener(OnContinueClicked);
            m_QuitButton.AddListener(OnQuitClicked);
            GameManager.Instance.gameMainMenuUI.gameObject.SetActive(false);
        }

        void OnDisable()
        {
            Time.timeScale = 1f;
            m_ContinueButton.RemoveListener(OnContinueClicked);
            m_QuitButton.RemoveListener(OnQuitClicked);
            if (PlayerController.Instance.isInMenu)
            {
                if (GameManager.Instance.gameMainMenuUI != null)
                {
                    GameManager.Instance.gameMainMenuUI.gameObject.SetActive(true);
                }
            }
            else
            {
                UIManager.Instance.Show<Hud>();
            }
        }

        void OnContinueClicked()
        {
            Time.timeScale = 1f;
            m_ContinueEvent.Raise();
            Hide();
        }

        void OnQuitClicked()
        {
            Time.timeScale = 1f;
            m_QuitEvent.Raise();
            Hide();
            Application.Quit();
        }
    }
}
