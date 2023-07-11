using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MyEditorTool : MonoBehaviour
{
    [Button("Replace Position All Running Man")]
    public void ReplacePositionAllRunningMan()
    {
        var allMen = GameObject.FindObjectsOfType<RunningMan>();
        foreach (var rman in allMen)
        {
            rman.transform.position = new Vector3(rman.transform.position.x, -.05f, rman.transform.position.z);
        }
        var allPolices = GameObject.FindObjectsOfType<PoliceShooting>();
        foreach (var plc in allPolices)
        {
            plc.transform.position = new Vector3(plc.transform.position.x, -.34f, plc.transform.position.z);
        }
    }
}
