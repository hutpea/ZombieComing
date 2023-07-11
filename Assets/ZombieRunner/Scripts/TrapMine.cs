using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class TrapMine : Spawnable
{
    public MeshRenderer meshRenderer;
    public Material matRed;
    public Material matGreen;
    public float lightSpeed;
    
    private void Start()
    {
        StartCoroutine(MineLightCoroutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(MineLightCoroutine());
    }

    private IEnumerator MineLightCoroutine()
    {
        bool isGreen = true;
        while (true)
        {
            //Debug.Log("Mine change materials");
            if (meshRenderer != null)
            {
                Material[] tempMats = meshRenderer.sharedMaterials;
                if(isGreen)
                {
                    tempMats[1] = matGreen;
                }
                else
                {
                    tempMats[1] = matRed;
                }

                meshRenderer.sharedMaterials = tempMats;
                isGreen = !isGreen;
            }
            yield return new WaitForSeconds(lightSpeed);
        }
    }
    
    public ParticleSystem explosionEffect;
    public Vector3 offsetExplosePosition;
    public float range;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            List<Zombie> exlodedZombieList = new List<Zombie>();
            foreach (var zom in PlayerController.Instance.zombieList)
            {
                float distance = Vector3.Distance(zom.transform.position, this.transform.position);
                if (distance <= range)
                {
                    exlodedZombieList.Add(zom);
                }
            }

            foreach (var zom in exlodedZombieList)
            {
                zom.Damage(damage);
            }
            
            Instantiate(explosionEffect, other.transform.position + offsetExplosePosition, Quaternion.identity);
            AudioManager.Instance.PlayEffect(SoundID.ExplosionSound1);
            PlayerController.Instance.PlayDeathZomSound(SoundID.CrowdDie);
            Destroy(this.gameObject);
        }
    }
}