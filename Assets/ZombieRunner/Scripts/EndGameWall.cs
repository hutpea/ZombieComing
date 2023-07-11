using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class EndGameWall : MonoBehaviour
{
    public List<Transform> wallPoints;
    public Transform topPoint;
    public Transform dancePoint;
    public List<MeshRenderer> leftMatLights;
    public List<MeshRenderer> rightMatLights;
    public Material lightOffMaterial;
    public Material lightOnMaterial;

    public int wallLevel;

    private void Awake()
    {
        wallLevel = 0;
    }

    public void SetLight(int lightIndex)
    {
        switch (lightIndex)
        {
            case 1:
            {
                leftMatLights[0].material = lightOnMaterial;
                rightMatLights[0].material = lightOnMaterial;
                wallLevel = 1;
                break;
            }
            case 2:
            {
                leftMatLights[1].material = lightOnMaterial;
                rightMatLights[1].material = lightOnMaterial;
                wallLevel = 2;
                break;
            }
            case 3:
            {
                leftMatLights[2].material = lightOnMaterial;
                rightMatLights[2].material = lightOnMaterial;
                wallLevel = 3;
                break;
            }
            case 4:
            {
                leftMatLights[3].material = lightOnMaterial;
                rightMatLights[3].material = lightOnMaterial;
                wallLevel = 4;
                break;
            }
            case 5:
            {
                leftMatLights[4].material = lightOnMaterial;
                rightMatLights[4].material = lightOnMaterial;
                wallLevel = 5;
                break;
            }
        }
        AudioManager.Instance.PlayEffect(SoundID.ChimeBell);
    }
}
