using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HyperCasual.Runner;
using MoreMountains.NiceVibrations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Zombie : MonoBehaviour
{
    public Material deathMaterial;
    
    public Vector3 offsetPosToOriginal;

    public int indexInSpawn;
    
    private bool isDied;

    public bool isFinishRun;

    public int currentHealth;

    public Animator animator;
    
    public Rigidbody zombieRigidbody;

    public int wallIndex = 0;
    
    private void Awake()
    {
        isDied = false;
        isFinishRun = false;
        currentHealth = GameData.ZombieMaxHealth;
        animator = GetComponent<Animator>();
        zombieRigidbody = GetComponent<Rigidbody>();

        /*int randomNum = UnityEngine.Random.Range(0, 100);
        if (randomNum >= 50 && randomNum < 75)
        {
            animator.Play("Idle Walk Run Blend 1");
        }
        else if (randomNum >= 75)
        {
            animator.Play("Idle Walk Run Blend 2");
        }*/
    }

    private void Update()
    {
        float offsetX = 6.5f;
        if (isFinishRun) return;
        if (isDied) return;
        if (transform.position.x < -offsetX)
        {
            /*isDied = true;
            PlayerController.Instance.RemoveFromFormation(this, indexInSpawn);*/
            var temp = transform.position;
            temp.x = -offsetX;
            transform.position = temp;
        }

        if (transform.position.x > offsetX)
        {
            var temp = transform.position;
            temp.x = offsetX;
            transform.position = temp;
        }
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            RemoveThisZombie();
        }
        else
        {
            //Instantiate(zombieDamageEffect, this.transform.position, Quaternion.identity, this.transform);
        }
        AudioManager.Instance.PlayEffect(SoundID.ZombieDeath);
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
    }
    
    public void RemoveThisZombie()
    {
        Debug.Log("Remove " + gameObject.name);
        isDied = true;
        Vector3 zOffsetNew = new Vector3(0, 0, 1f);
        var newZom = Instantiate(PlayerController.Instance.selectedZombiePrefab, transform.position + zOffsetNew, Quaternion.identity);
        newZom.tag = "Untagged";
        Quaternion newRot = Quaternion.Euler(0, Random.Range(-45, 45), 0);
        newZom.transform.rotation = newRot;
        //newZom.GetComponent<CapsuleCollider>().enabled = false;
        ChangeMaterial(deathMaterial, newZom);
        Zombie newZomComponent = newZom.GetComponent<Zombie>();
        newZomComponent.animator.Play("Death");
        float pushForce = 5f;
        Rigidbody newZomRb = newZom.GetComponent<Rigidbody>();
        newZomRb.AddForce(new Vector3(0, 1, -1) * pushForce);
        AutoDestroyObj(newZom, 2f);
        PlayerController.Instance.RemoveFromFormation(this, indexInSpawn);
    }

    private void OnCollisionEnter(Collision other)
    {
        RunningMan rmComponent = other.gameObject.GetComponent<RunningMan>();
        if (rmComponent != null)
        {
            if (!rmComponent.isTouch)
            {
                rmComponent.isTouch = true;
                PlayerController.Instance.AddToFormation();
                Inventory.Instance.OnManPicked();
                if (other.gameObject.transform.position.x < transform.position.x)
                {
                    animator.Play("Chay Cao Trai");
                }
                else
                {
                    animator.Play("Chay Cao Phai");
                }
                
                PlayerController.Instance.ToggleZombieScratchEffect(true, other.transform);
                AudioManager.Instance.PlayEffect(SoundID.ZombieScratch);
                
                DOVirtual.DelayedCall(0.5f, delegate
                {
                    animator.Play("Idle Walk Run Blend");
                });
                PlayerController.Instance.PlayDeathZomSound(rmComponent.deathSoundID);
                Destroy(other.gameObject);
            }
            
        }

        if (other.gameObject.CompareTag("Police"))
        {
            PlayerController.Instance.AddToFormation();
            Inventory.Instance.OnManPicked();
            AudioManager.Instance.PlayEffect(SoundID.MaleDie);
            Destroy(other.gameObject);
        }
        /*if (other.gameObject.CompareTag("Man"))
        {
            
        }*/
        if (other.gameObject.CompareTag("Finish"))
        {
            Destroy(gameObject);
        }
    }

    void ChangeMaterial(Material newMat, GameObject parent)
    {
        SkinnedMeshRenderer skinnedMeshRenderer = parent.GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] mats = skinnedMeshRenderer.materials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = newMat;
        }

        skinnedMeshRenderer.materials = mats;
    }

    private IEnumerator AutoDestroyObj(GameObject objToDestroy, float duration)
    {
        yield return new WaitForSeconds(duration);
        objToDestroy.SetActive(false);
    }
}
