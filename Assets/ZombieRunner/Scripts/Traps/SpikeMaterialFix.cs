using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpikeMaterialFix : MonoBehaviour
{
    public Material blackMat;
    public MeshRenderer panelMeshRenderer;

    private void Start()
    {
        panelMeshRenderer.material = blackMat;
    }

    [Button]
    public void SetMat()
    {
        panelMeshRenderer.material = blackMat;
    }
}
