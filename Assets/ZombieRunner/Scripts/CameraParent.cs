using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class CameraParent : MonoBehaviour
{
    public static CameraParent Instance;
    public PlayerController player;

    public bool isEnable;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        isEnable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEnable) return;
        var temp = transform.position;
        temp.x = player.transform.position.x * 1.8f;
        transform.position = temp;
    }

    public void Setup(PlayerController playerController)
    {
        player = playerController;
        CameraManager.Instance.transform.SetParent(this.transform);
        isEnable = true;
    }
}
