using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiCanvas : MonoBehaviour
{
    public static ConfettiCanvas Instance;

    public GameObject canvas;
    public ParticleSystem confetti;
    
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void TurnOff()
    {
        confetti.Stop();
        canvas.SetActive(false);
    }

    public void TurnOn(Transform cameraTransform)
    {
        canvas.transform.position = cameraTransform.position + new Vector3(0, 0, 2f);
        canvas.SetActive(true);
        confetti.Play();
    }
}
