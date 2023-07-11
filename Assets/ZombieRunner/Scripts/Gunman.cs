using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class Gunman : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform firePoint;
    
    public bool isFire;

    public float fireRate;

    public Vector3 direction;
    
    private float fireTimer = 0f;
    
    private void Update()
    {
        fireTimer += Time.deltaTime;
        if (isFire)
        {
            if (fireTimer > fireRate)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(direction * 20f, ForceMode.Impulse);
        AudioManager.Instance.PlayEffect(SoundID.GunSound01);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
