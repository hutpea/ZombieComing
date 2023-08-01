using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using JetBrains.Annotations;
using UnityEngine;

public class PoliceShooting : Spawnable
{
    public GameObject gunAmmoPrefab;
    public Transform firePoint;
    public Vector3 direction;
    public float delayShot;
    public float minDistanceToActive;
    private Transform playerTransform;
    [SerializeField] private float distanceToPlayer = 100f;
    private float timer = 0f;
    public Animator animator;

    public bool enablePlay = false;
    
    void Start()
    {
        playerTransform = GameObject.FindObjectOfType<PlayerController>().transform;
    }

    protected override void Update()
    {
        base.Update();
        if (!enablePlay) return;
        if (playerTransform.position.z > transform.position.z) return;
        if (playerTransform != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            timer += Time.deltaTime;
            if (distanceToPlayer < minDistanceToActive)
            {
                timer += Time.deltaTime;
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (!enablePlay) return;
        if (distanceToPlayer < minDistanceToActive && timer > delayShot)
        {
            Fire();
            timer = 0f;
        }
    }
    private void Fire()
    {
        if (animator != null)
        {
            animator.Play("Shoot");
        }
        //AudioManager.Instance.PlayEffect();
        Quaternion bulletRot = Quaternion.Euler(0, 180, 0);
        var bullet = Instantiate(gunAmmoPrefab, firePoint.position, bulletRot);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(- direction * 20f, ForceMode.Impulse);
        AudioManager.Instance.PlayEffect(SoundID.GunSound01);
    }
}
