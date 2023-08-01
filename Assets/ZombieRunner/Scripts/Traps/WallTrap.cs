using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class WallTrap : Spawnable
{
    public bool isActive = false;
    public Transform topPointA;
    public Transform topPointB;
    public List<Transform> points;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isActive)
            {
                isActive = true;
                PlayerController.Instance.PlayClimbWallTrapState(this);
            }
        }
    }
}
