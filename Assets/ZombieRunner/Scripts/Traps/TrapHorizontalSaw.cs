using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class TrapHorizontalSaw : Spawnable
{
    public float moveSpeed;
    public Transform saw;

    private bool isLeft;
    
    private void Start()
    {
        if(transform.position.x <= 0)
        {
            isLeft = true;
        }
        else
        {
            isLeft = false;
        }
    }

    protected override void Update()
    {
        base.Update();
        saw.Rotate(-saw.forward * 100 * Time.deltaTime);
    }

    void FixedUpdate()
    {
        var targetPosition = transform.position;
        if (isLeft)
        {
            targetPosition.x = Mathf.PingPong(Time.time * moveSpeed, 6) - 6f;
        }
        else
        {
            targetPosition.x = Mathf.PingPong(Time.time * moveSpeed, 6);
        }
        transform.position = targetPosition;
    }
}
