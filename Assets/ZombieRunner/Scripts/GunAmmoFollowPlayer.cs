using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class GunAmmoFollowPlayer : MonoBehaviour
{
    public int damage;
    public float speed;
    
    [SerializeField] private Transform follower;

    private bool enablePlay = false;

    private Vector3 direction;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetFollower(Transform t)
    {
        follower = t;
        enablePlay = true;
    }
    
    private void Update()
    {
        if (!enablePlay) return;
        direction = (follower.position - transform.position).normalized;
        if (Vector3.Distance(follower.position, transform.position) < 0.5f)
        {
            PlayerController.Instance.RemoveFromFormation();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!enablePlay) return;
        rb.velocity = direction * speed;
    }

    /*
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
    }*/
}
