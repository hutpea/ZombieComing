using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class TrapStaticSaw : Spawnable
{
    public Transform saw;
    
    protected override void Update()
    {
        base.Update();
        saw.Rotate(saw.transform.forward, 90f * Time.deltaTime);
    }
}
