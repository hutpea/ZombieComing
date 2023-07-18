using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;

public class NumberSpawnable : Spawnable
{
    public TextMeshPro textMesh;
    public int number;
    public NumberSign numberSign;
    
    private bool isActive = false;

    private void Awake()
    {
        number = UnityEngine.Random.Range(2, 4);
        numberSign = (NumberSign) UnityEngine.Random.Range(0, 4);
        switch (numberSign)
        {
            case NumberSign.Plus:
            {
                textMesh.SetText("+" + number);
                break;
            }
            case NumberSign.Minus:
            {
                textMesh.SetText("-" + number);
                break;
            }
            case NumberSign.Multiply:
            {
                textMesh.SetText("x" + number);
                break;
            }
            case NumberSign.Divide:
            {
                textMesh.SetText("÷" + number);
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            isActive = true;
            switch (numberSign)
            {
                case NumberSign.Plus:
                {
                    for (int i = 1; i <= number; i++)
                    {
                        PlayerController.Instance.AddToFormation();
                    }
                    AudioManager.Instance.PlayEffect(SoundID.ChimeBell);

                    break;
                }
                case NumberSign.Minus:
                {
                    int zombieCountToDestroy = Mathf.Min(number, PlayerController.Instance.zombieList.Count);
                    for (int i = 1; i <= zombieCountToDestroy; i++)
                    {
                        PlayerController.Instance.RemoveFromFormation();
                    }
                    break;
                }
                case NumberSign.Multiply:
                {
                    int currentZombieCount = PlayerController.Instance.zombieList.Count;
                    int zombieCountToAdd = currentZombieCount * number - currentZombieCount;
                    for (int i = 1; i <= zombieCountToAdd; i++)
                    {
                        PlayerController.Instance.AddToFormation();
                    }
                    AudioManager.Instance.PlayEffect(SoundID.ChimeBell);

                    break;
                }
                case NumberSign.Divide:
                {
                    int currentZombieCount = PlayerController.Instance.zombieList.Count;
                    int zombieCountToDestroy = currentZombieCount - currentZombieCount / number;
                    for (int i = 1; i <= zombieCountToDestroy; i++)
                    {
                        PlayerController.Instance.RemoveFromFormation();
                    }
                    break;
                }
            }
            Destroy(this.gameObject);
        }
    }
    
    
}

public enum NumberSign
{
    Plus,
    Minus,
    Multiply,
    Divide
}
