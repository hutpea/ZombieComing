using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;

public class NumberSpawnable : Spawnable
{
    public MeshRenderer meshRenderer;
    public List<Material> greenMats;
    public List<Material> redMats;
    public TextMeshPro textMesh;
    public int number;
    public NumberSign numberSign;
    
    private bool isActive = false;

    private void Awake()
    {
        SetNumber(number, numberSign);
    }

    public void SetNumber(int _number, NumberSign _sign)
    {
        switch (_sign)
        {
            case NumberSign.Plus:
            {
                textMesh.SetText("+" + _number);
                Material[] tempMats = meshRenderer.sharedMaterials;
                tempMats[0] = greenMats[0];
                tempMats[1] = greenMats[1];
                meshRenderer.sharedMaterials = tempMats;
                textMesh.color = Color.green;
                break;
            }
            case NumberSign.Minus:
            {
                textMesh.SetText("-" + _number);
                Material[] tempMats = meshRenderer.sharedMaterials;
                tempMats[0] = redMats[0];
                tempMats[1] = redMats[1];
                meshRenderer.sharedMaterials = tempMats;
                textMesh.color = Color.red;
                break;
            }
            case NumberSign.Multiply:
            {
                textMesh.SetText("x" + _number);
                Material[] tempMats = meshRenderer.sharedMaterials;
                tempMats[0] = greenMats[0];
                tempMats[1] = greenMats[1];
                meshRenderer.sharedMaterials = tempMats;
                textMesh.color = Color.green;
                break;
            }
            case NumberSign.Divide:
            {
                textMesh.SetText("÷" + _number);
                Material[] tempMats = meshRenderer.sharedMaterials;
                tempMats[0] = redMats[0];
                tempMats[1] = redMats[1];
                meshRenderer.sharedMaterials = tempMats;
                textMesh.color = Color.red;
                break;
            }
        }

        this.number = _number;
        this.numberSign = _sign;
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
    //None = 0,
    Plus = 1,
    Minus = 2,
    Multiply = 3,
    Divide = 4
}
