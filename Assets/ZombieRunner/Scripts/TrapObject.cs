using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class TrapObject : MonoBehaviour
{
    public int damage;
    public bool isMine;
    public ParticleSystem mineExplosionEffect;

    public void OnPlayerCollideMine()
    {
        Instantiate(mineExplosionEffect, transform.position, Quaternion.identity);
        AudioManager.Instance.PlayEffect(SoundID.ExplosionSound1);
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Zombie zombieComponent = other.GetComponent<Zombie>();
            if (zombieComponent != null)
            {
                zombieComponent.Damage(damage);
            }

            if (isMine)
            {
                OnPlayerCollideMine();
            }
        }
    }
}
