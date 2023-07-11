using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RunningMan : Spawnable
{
    public bool isTouch = false;
    public float moveSpeed;

    public Vector3 direction;

    public SoundID deathSoundID;
    
    private void FixedUpdate()
    {
        if (PlayerController.Instance.isInMenu) return;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
