using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Runner;
using UnityEngine;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// The event is triggered when the player loses a level.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(LevelLostEvent),
        menuName = "Runner/" + nameof(LevelLostEvent))]
    public class LevelLostEvent : AbstractGameEvent
    {
        public override void Reset()
        {
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.ResetZombies();            
            }
        }
    }
}