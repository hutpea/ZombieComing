#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class MyEditorTool : MonoBehaviour
{
    public GameObject egyptMan;
    public GameObject egyptWoman;
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
    
    [Button("Replace All To Egypt")]
    public void ReplaceEgypt()
    {
        var allMen = GameObject.FindObjectsOfType<RunningMan>();
        for (var index = 0; index < allMen.Length; index++)
        {
            var rman = allMen[index];
            var rPosition = rman.transform.position;
            var parentT = rman.transform.parent;
            DestroyImmediate(rman.gameObject);
            int numRand = UnityEngine.Random.Range(0, 100);
            if (numRand <= 50)
            {
                var obj = PrefabUtility.InstantiatePrefab(egyptMan, parentT) as GameObject;
                obj.transform.position = new Vector3(rPosition.x, -0.12f, rPosition.z);
                obj.transform.rotation = Quaternion.identity;
            }
            else
            {
                var obj = PrefabUtility.InstantiatePrefab(egyptWoman, parentT) as GameObject;
                obj.transform.position = new Vector3(rPosition.x, -0.12f, rPosition.z);
                obj.transform.rotation = Quaternion.identity;
            }
        }
    }
}
#endif