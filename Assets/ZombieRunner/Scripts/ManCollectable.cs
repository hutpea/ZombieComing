using System.Collections;
using System.Collections.Generic;
using HyperCasual.Gameplay;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class representing a Spawnable object.
    /// If a GameObject tagged "Player" collides
    /// with this object, it will be collected, 
    // incrementing the player's amount of this item.
    /// </summary>
    public class ManCollectable : MonoBehaviour
    {
        [SerializeField]
        SoundID m_Sound = SoundID.None;
        
        const string k_PlayerTag = "Player";

        public ItemPickedEvent m_Event;
        public int m_Count;

        bool m_Collected;

        private void Awake()
        {
        }

        void OnTriggerEnter(Collider col)
        {
            Debug.Log(col.gameObject.tag);
            if (col.CompareTag(k_PlayerTag) && !m_Collected)
            {
                Collect();
            }
        }

        void Collect()
        {
            if (m_Event != null)
            {
                m_Event.Count = m_Count;
                m_Event.Raise();
            }

            m_Collected = true;
            AudioManager.Instance.PlayEffect(m_Sound);
        }
    }
}