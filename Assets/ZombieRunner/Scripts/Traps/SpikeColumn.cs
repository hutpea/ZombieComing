using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class SpikeColumn : Spawnable
{
    public Transform spikeColumn;
    
    protected override void Update()
    {
        base.Update();
        spikeColumn.Rotate(Vector3.forward, 90f * Time.deltaTime);
    }
}
