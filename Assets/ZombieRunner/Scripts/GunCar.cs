using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasual.Runner;
using UnityEngine;

public class GunCar : Spawnable
{
    public GameObject gunAmmoPrefab;
    public Transform firePoint;
    public float moveSpeed;
    public Vector3 direction;
    public float fireInterval;
    
    private float timeSinceStart = 0f;
    private float timeFire = 0f;
    private bool isAccelerateFirst = false;
    private bool isAccelerateSecond = false;
    private bool isAllowToFire = true;
    
    public bool enablePlay;
    
    public float minDistanceToActive;
    private Transform playerTransform;
    [SerializeField] private float distanceToPlayer = 100f;

    private bool startCar = false;
    private void Start()
    {
        playerTransform = GameObject.FindObjectOfType<PlayerController>().transform;
        
        moveSpeed = 15f;
        timeSinceStart = 0f;
        timeFire = 0f;
        isAccelerateFirst = false;
        isAccelerateSecond = false;
        isAllowToFire = true;
    }

    public void StartCar()
    {
        if (!startCar)
        {
            startCar = true;
            enablePlay = true;
            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.5f);
            s.Append(transform.DOMoveX(-4f, 1f));
            s.AppendInterval(0.5f);
            s.Append(transform.DOMoveX(4f, 1.75f));
            s.Append(transform.DOMoveX(0F, 1f));
        }
    }

    protected override void Update()
    {
        
        base.Update();
        if (playerTransform != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer < minDistanceToActive)
            {
                StartCar();
            }
        }
        if (enablePlay)
        {
            timeSinceStart += Time.deltaTime;
            timeFire += Time.deltaTime;
        }
    }
    
    private void FixedUpdate()
    {
        if (!enablePlay) return;
        transform.position += direction * moveSpeed * Time.deltaTime;
        if (timeSinceStart > 5.5f && !isAccelerateFirst)
        {
            isAccelerateFirst = true;
            moveSpeed = 25f;
            isAllowToFire = false;
        }
        if (timeSinceStart > 7f && !isAccelerateSecond)
        {
            isAccelerateSecond = true;
            moveSpeed = 55f;
        }
        if (timeFire > fireInterval && isAllowToFire)
        {
            Fire();
            timeFire = 0f;
        }
    }

    private void Fire()
    {
        //AudioManager.Instance.PlayEffect();
        var bullet = Instantiate(gunAmmoPrefab, firePoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(- direction * 20f, ForceMode.Impulse);
        AudioManager.Instance.PlayEffect(SoundID.GunSound01);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Destroy(this.gameObject);
        }
    }
}
