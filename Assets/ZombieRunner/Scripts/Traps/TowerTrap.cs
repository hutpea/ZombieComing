using System;
using DG.Tweening;
using HyperCasual.Runner;
using UnityEngine;

public class TowerTrap : Spawnable
{
    public Animator animator;
    public Transform firePoint;
    public GameObject gunAmmoPrefab;
    public float delayShot;
    public float minDistanceToActive;
    [SerializeField] private float distanceToPlayer = 100f;

    private float timer = 0f;
    
    private Transform playerTransform;
    
    void Start()
    {
        playerTransform = GameObject.FindObjectOfType<PlayerController>().transform;
        if (transform.position.x > 0)
        {
            Quaternion q = animator.transform.rotation;
            q.y -= 90f;
            animator.transform.rotation = q;
        }
    }

    protected override void Update()
    {
        base.Update();
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
        if (distanceToPlayer < minDistanceToActive && timer > delayShot)
        {
            Fire();
            timer = 0f;
        }
    }
    
    private void Fire()
    {
        /*if (animator != null)
        {
            animator.Play("Gu");
        }*/
        var bullet = Instantiate(gunAmmoPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.LookAt(playerTransform);
        Debug.Log(playerTransform.position);
        Vector3 bulletDirection = (playerTransform.position + new Vector3(0, 0, 1f)) - transform.position;
        Vector3 targetPos = playerTransform.position + new Vector3(0, 0, 2f);
        bulletDirection.Normalize();
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        //bulletRb.AddForce(bulletDirection * 20f, ForceMode.Impulse);
        bullet.transform.DOMove(targetPos, 1.5f);
        AudioManager.Instance.PlayEffect(SoundID.GunSound01);
    }
}
