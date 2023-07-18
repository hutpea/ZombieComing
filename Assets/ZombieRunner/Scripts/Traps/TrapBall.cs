using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class TrapBall : Spawnable
{
    public bool enablePlay;
    public Vector3 ballDirection;
    public float spinSpeed;
    public Transform ball;
    protected override void Update()
    {
        base.Update();
        if (enablePlay)
        {
            transform.Translate(spinSpeed * ballDirection * Time.deltaTime);
            ball.Rotate(new Vector3(1, 0, 0), - 100 * Time.deltaTime);
        }
    }
}
