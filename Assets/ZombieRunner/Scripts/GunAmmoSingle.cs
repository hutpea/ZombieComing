using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAmmoSingle : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Zombie zombieComponent = other.GetComponent<Zombie>();
            if (zombieComponent != null)
            {
                zombieComponent.Damage(damage);
            }
            
            Destroy(gameObject);
        }
    }
}
