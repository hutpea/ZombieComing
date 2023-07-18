using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class Bombman : Spawnable
{
    public bool isTouch = false;
    public float moveSpeed;

    public Vector3 direction;

    public ParticleSystem explosionEffect;
    public Vector3 offsetExplosePosition;
    public float range;
    public int damage;
    
    private void FixedUpdate()
    {
        if (PlayerController.Instance.isInMenu) return;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    
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
